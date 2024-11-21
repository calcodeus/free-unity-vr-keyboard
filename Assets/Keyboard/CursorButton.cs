#region Assembly UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// location unknown
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard
{

    [AddComponentMenu("UI/CursorButton", 31)]
    public class CursorButton : CursorSelectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
    {
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

        public Button.ButtonClickedEvent onClick
        {
            get
            {
                return m_OnClick;
            }
            set
            {
                m_OnClick = value;
            }
        }

        protected CursorButton()
        {
        }

        private void Press()
        {
            if (IsActive() && IsInteractable())
            {
                UISystemProfilerApi.AddMarker("Button.onClick", this);
                m_OnClick.Invoke();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Press();
            }
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
            if (IsActive() && IsInteractable())
            {
                DoStateTransition(SelectionState.Pressed, instant: false);
                StartCoroutine(OnFinishSubmit());
            }
        }

        private IEnumerator OnFinishSubmit()
        {
            float fadeTime = base.colors.fadeDuration;
            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(base.currentSelectionState, instant: false);
        }
    }
}