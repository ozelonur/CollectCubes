using UnityEngine.EventSystems;

namespace _GAME_.Scripts.Bears.CustomInput
{
    public class VariableJoystick : Joystick
    {
        #region Event System Methods

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            base.OnPointerDown(eventData);
        }

        #endregion
    }
}