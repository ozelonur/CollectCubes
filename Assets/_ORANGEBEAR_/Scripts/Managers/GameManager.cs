#region Header

// Developed by Onur ÖZEL

#endregion

using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.Bears;
using _ORANGEBEAR_.Scripts.Enums;
using UnityEngine;

namespace _ORANGEBEAR_.Scripts.Managers
{
    public class GameManager : Manager<GameManager>
    {
        // [SerializeField] private ParticleSystem confetti;

        #region Public Variables
        
        public bool isGameStarted;
        public bool isGameRestarted;
        public bool isGameEnded;
        
        [HideInInspector] public LevelBear currentLevel;

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(GameEvents.OnGameComplete, OnGameComplete);
                Register(GameEvents.OnGameStart, OnGameStarted);
            }

            else
            {
                UnRegister(GameEvents.OnGameComplete, OnGameComplete);
                UnRegister(GameEvents.OnGameStart, OnGameStarted);
            }
        }

        private void OnGameStarted(object[] args)
        {
            isGameStarted = true;
            isGameEnded = false;
        }

        private void OnGameComplete(object[] obj)
        {
            isGameEnded = true;
            
            bool status = (bool)obj[0];

            if (status)
            {
                // confetti.Play();
            }

            Roar(GameEvents.ActivatePanel, status ? PanelsEnums.GameWin : PanelsEnums.GameOver);
        }

        #endregion
    }
}