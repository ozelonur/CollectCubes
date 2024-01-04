using _GAME_.Scripts.Bears.CustomInput;
using _ORANGEBEAR_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Manager
{
    public class JoystickManager : Manager<JoystickManager>
    {
        #region Properties

        public Joystick Joystick => joystick;

        #endregion

        #region Serialized Fields

        [Header("Components")]
        [SerializeField] private Joystick joystick;

        #endregion
    }
}