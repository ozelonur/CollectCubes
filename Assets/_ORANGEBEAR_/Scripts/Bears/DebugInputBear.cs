#region Header

// Developed by Onur ÖZEL

#endregion

using _ORANGEBEAR_.EventSystem;
using UnityEngine;

namespace _ORANGEBEAR_.Scripts.Bears
{
    public class DebugInputBear : Bear
    {
        #region MonoBehaviour Methods

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Roar(GameEvents.OnGameComplete, true);
            }
        }

        #endregion
    }
}