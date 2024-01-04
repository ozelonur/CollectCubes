#region Header

// Developed by Onur ÖZEL

#endregion

using _GAME_.Scripts.GlobalVariables;
using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.Bears;
using TMPro;
using UnityEngine;

namespace _GAME_.Scripts.Bears
{
    public class GameUIBear : UIBear
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private TMP_Text timeText;

        [SerializeField] private TMP_Text cubeCountText;

        #endregion

        #region Private Variables

        private GameObject _timerParent;
        private GameObject _counterParent;

        #endregion

        #region Event Methods

        protected override void InitLevel(object[] args)
        {
            base.InitLevel(args);
            _timerParent = timeText.transform.parent.gameObject;
            _counterParent = cubeCountText.transform.parent.gameObject;

            _timerParent.SetActive(false);
            _counterParent.SetActive(false);
        }

        protected override void CheckRoarings(bool status)
        {
            base.CheckRoarings(status);
            if (status)
            {
                Register(CustomEvents.UpdateCollectedCubeCount, UpdateCubeCount);
                Register(CustomEvents.UpdateTimer, UpdateTimer);
                Register(GameEvents.OnGameComplete, OnGameComplete);
            }

            else
            {
                UnRegister(CustomEvents.UpdateCollectedCubeCount, UpdateCubeCount);
                UnRegister(CustomEvents.UpdateTimer, UpdateTimer);
                Register(GameEvents.OnGameComplete, OnGameComplete);
            }
        }

        private void OnGameComplete(object[] args)
        {
            _timerParent.SetActive(false);
            _counterParent.SetActive(false);
        }

        private void UpdateTimer(object[] args)
        {
            if (!_timerParent.activeSelf)
            {
                _timerParent.SetActive(true);
            }

            float time = (float)args[0];
            timeText.text = time.ToString("0.00");
        }

        private void UpdateCubeCount(object[] args)
        {
            if (!_counterParent.activeSelf)
            {
                _counterParent.SetActive(true);
            }

            int count = (int)args[0];

            cubeCountText.text = count.ToString();
        }

        #endregion
    }
}