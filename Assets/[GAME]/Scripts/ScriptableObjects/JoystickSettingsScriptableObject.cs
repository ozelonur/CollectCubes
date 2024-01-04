using _GAME_.Scripts.Enums;
using UnityEngine;

namespace _GAME_.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Joystick Settings", menuName = "Settings/Joystick Settings", order = 1)]
    public class JoystickSettingsScriptableObject : ScriptableObject
    {
        #region Serialized Fields

        [Header("Configurations")] public float handleRange = 1f;
        
        public float deadZone;

        public AxisOptions axisOptions = AxisOptions.Both;

        #endregion
    }
}