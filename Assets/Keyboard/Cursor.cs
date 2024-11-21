using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard
{
    public class Cursor : UIBehaviour
    {
        [SerializeField]
        private Canvas m_Canvas;
        private readonly List<UIBehaviour> m_behaviours = new();
        private UIBehaviour m_HoveredBehaviour;
        private UIBehaviour m_PressedBehaviour;
        private void RefreshCanvas()
        {
            if (m_Canvas == null)
            {
                m_Canvas = GetComponentInParent<Canvas>();
            }
            m_behaviours.Clear();
            foreach (var uIBehaviour in GetComponentsInChildren<UIBehaviour>())
            {
                m_behaviours.Add(uIBehaviour);
            }
        }

        private void UpdatePosition(Vector2 position)
        {
            float lowestZ = int.MaxValue;
            UIBehaviour targetBehaviour = null;
            foreach (var uIBehaviour in m_behaviours)
            {
                RectTransform rect = uIBehaviour.transform as RectTransform;
                if (rect.rect.Contains(position))
                {
                    float zValue = m_Canvas.transform.InverseTransformPoint(rect.position).z;
                    if (zValue < lowestZ)
                    {
                        lowestZ = zValue;
                        targetBehaviour = uIBehaviour;
                    }
                }
            }

            if (targetBehaviour != null)
            {
                if (m_HoveredBehaviour is ICursorExitHandler exitHandler)
                {
                    m_HoveredBehaviour = null;
                    exitHandler.OnCursorExit();
                }
            }

            if (m_HoveredBehaviour != targetBehaviour)
            {
                if (m_HoveredBehaviour is ICursorExitHandler exitHandler)
                {
                    exitHandler.OnCursorExit();
                }
                m_HoveredBehaviour = targetBehaviour;
                if (m_HoveredBehaviour is ICursorEnterHandler enterHandler)
                {
                    enterHandler.OnCursorEnter();
                }
                if (m_HoveredBehaviour is Selectable selectable)
                {
                    selectable.Select();
                }
            }
        }

        public void UpdateButtonState(bool pressed)
        {
            if (pressed)
            {
                if (m_HoveredBehaviour is ICursorDownHandler downHandler)
                {
                    m_PressedBehaviour = m_HoveredBehaviour;
                    downHandler.OnCursorDown();
                }
            }
            else
            {
                if (m_PressedBehaviour is ICursorUpHandler upHandler)
                {
                    upHandler.OnCursorUp();
                }
                if (m_PressedBehaviour == m_HoveredBehaviour && m_PressedBehaviour is ICursorClickHandler clickHandler)
                {
                    clickHandler.OnCursorClick();
                }
                m_PressedBehaviour = null;
            }
        }

    }

    public interface ICursorClickHandler
    {
        void OnCursorClick();
    }

    public interface ICursorDownHandler
    {
        void OnCursorDown();
    }

    public interface ICursorUpHandler
    {
        void OnCursorUp();
    }

    public interface ICursorEnterHandler
    {
        void OnCursorEnter();
    }

    public interface ICursorExitHandler
    {
        void OnCursorExit();
    }
}