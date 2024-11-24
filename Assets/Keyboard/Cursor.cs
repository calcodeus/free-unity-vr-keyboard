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

        [SerializeField]
        private float m_CursorSpeed = 1.0f;

        public float CursorSpeed { get => m_CursorSpeed; set => m_CursorSpeed = value; }

        public void PressDown()
        {
            UpdateButtonState(true);
        }

        public void PressUp()
        {
            UpdateButtonState(false);
        }

        public void MoveCursor(Vector2 direction)
        {
            if (m_Canvas == null)
            {
                RefreshCanvas();
            }
            transform.localPosition = (Vector2)transform.localPosition + m_CursorSpeed * Time.deltaTime * direction;
            UpdatePosition(transform.position);
        }

        // public void MoveCursorTo(Vector2 position)
        // {
        //     if (m_Canvas == null)
        //     {
        //         RefreshCanvas();
        //     }
        //     UpdatePosition(position);
        //     transform.position = position;
        // }

        protected override void Start()
        {
            RefreshCanvas();
            base.Start();
        }


        private readonly List<GameObject> m_behaviours = new();
        private GameObject m_HoveredBehaviour;
        private GameObject m_PressedBehaviour;
        private readonly List<Component> m_ComponentsCache = new();
        private void RefreshCanvas()
        {
            if (m_Canvas == null)
            {
                m_Canvas = GetComponentInParent<Canvas>();
            }
            m_behaviours.Clear();
            foreach (var uIBehaviour in m_Canvas.GetComponentsInChildren<ICursorInteractable>())
            {
                m_behaviours.Add(((Component)uIBehaviour).gameObject);
            }
        }

        private void UpdatePosition(Vector2 position)
        {
            float lowestZ = int.MaxValue;
            GameObject targetBehaviour = null;
            foreach (var uIBehaviour in m_behaviours)
            {
                if (uIBehaviour.gameObject == this.gameObject)
                {
                    continue;
                }
                RectTransform rectTransform = uIBehaviour.transform as RectTransform;
                if (rectTransform.rect.Contains(position))
                {
                    float zValue = m_Canvas.transform.InverseTransformPoint(rectTransform.position).z;
                    if (zValue < lowestZ)
                    {
                        lowestZ = zValue;
                        targetBehaviour = uIBehaviour;
                    }
                }
            }

            if (m_HoveredBehaviour != targetBehaviour)
            {
                if (m_HoveredBehaviour != null)
                {
                    m_HoveredBehaviour.GetComponents(m_ComponentsCache);
                    foreach (var component in m_ComponentsCache)
                    {
                        if (component is ICursorExitHandler exitHandler)
                        {
                            exitHandler.OnCursorExit();
                        }
                    }
                }

                m_HoveredBehaviour = targetBehaviour;

                if (m_HoveredBehaviour != null)
                {
                    m_HoveredBehaviour.GetComponents(m_ComponentsCache);
                    foreach (var component in m_ComponentsCache)
                    {
                        if (component is ICursorEnterHandler enterHandler)
                        {
                            enterHandler.OnCursorEnter();
                        }
                    }
                }
                // if (m_HoveredBehaviour is Selectable selectable)
                // {
                //     selectable.Select();
                // }
            }
        }

        public void UpdateButtonState(bool pressed)
        {
            if (pressed)
            {
                m_HoveredBehaviour.GetComponents(m_ComponentsCache);
                foreach (var component in m_ComponentsCache)
                {
                    if (component is ICursorDownHandler downHandler)
                    {
                        m_PressedBehaviour = m_HoveredBehaviour;
                        downHandler.OnCursorDown();
                    }
                }
            }
            else
            {
                m_HoveredBehaviour.GetComponents(m_ComponentsCache);
                foreach (var component in m_ComponentsCache)
                {
                    if (component is ICursorUpHandler upHandler)
                    {
                        upHandler.OnCursorUp();
                    }
                }
                if (m_PressedBehaviour == m_HoveredBehaviour)
                {
                    m_PressedBehaviour.GetComponents(m_ComponentsCache);
                    foreach (var component in m_ComponentsCache)
                    {
                        if (component is ICursorClickHandler clickHandler)
                        {
                            clickHandler.OnCursorClick();
                        }
                    }
                }
                m_PressedBehaviour = null;
            }
        }

    }

    public interface ICursorInteractable
    {

    }

    public interface ICursorClickHandler : ICursorInteractable
    {
        void OnCursorClick();
    }

    public interface ICursorDownHandler : ICursorInteractable
    {
        void OnCursorDown();
    }

    public interface ICursorUpHandler : ICursorInteractable
    {
        void OnCursorUp();
    }

    public interface ICursorEnterHandler : ICursorInteractable
    {
        void OnCursorEnter();
    }

    public interface ICursorExitHandler : ICursorInteractable
    {
        void OnCursorExit();
    }
}