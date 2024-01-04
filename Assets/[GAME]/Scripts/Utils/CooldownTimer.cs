using _GAME_.Scripts.GlobalVariables;
using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Utils
{
    public class CooldownTimer : Bear
    {
        #region Public Variables

        [HideInInspector] public float cooldownTime;

        #endregion

        #region Private Variables

        private bool _timeIsUp;

        #endregion

        #region MonoBehaviour Methods

        private void Update()
        {
            if (GameManager.Instance.isGameEnded || !GameManager.Instance.isGameStarted)
            {
                return;
            }

            if (_timeIsUp)
            {
                return;
            }

            if (cooldownTime > 0)
            {
                cooldownTime -= Time.deltaTime;
                Roar(CustomEvents.UpdateTimer, cooldownTime);
            }

            else
            {
                _timeIsUp = true;
                Roar(GameEvents.OnGameComplete, true);
            }
        }

        #endregion
    }
}