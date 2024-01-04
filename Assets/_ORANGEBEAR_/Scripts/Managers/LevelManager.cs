#region Header

// Developed by Onur ÖZEL

#endregion

using _GAME_.Scripts.Bears;
using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.GameVariables;
using _ORANGEBEAR_.Scripts.ScriptableObjects;
using UnityEngine;

namespace _ORANGEBEAR_.Scripts.Managers
{
    public class LevelManager : Bear
    {
        #region SerializeFields

        [SerializeField] private Level[] levels;

        #endregion

        #region Private Variables

        private Level _level;
        private GameObject _tempLevel;
        private int _levelCount;

        private int LevelIndex
        {
            get => PlayerPrefs.GetInt(GameStrings.LevelIndex, 0);
            set => PlayerPrefs.SetInt(GameStrings.LevelIndex, value);
        }

        private int LevelCount
        {
            get => PlayerPrefs.GetInt(GameStrings.LevelCount, 1);
            set => PlayerPrefs.SetInt(GameStrings.LevelCount, value);
        }

        #endregion

        #region MonoBehaviour Methods

        private void Start()
        {
            CreateLevel();
        }

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(GameEvents.OnGameComplete, OnGameComplete);
                Register(GameEvents.InitLevel, InitLevel);
                Register(GameEvents.NextLevel, NextLevel);
            }

            else
            {
                UnRegister(GameEvents.OnGameComplete, OnGameComplete);
                UnRegister(GameEvents.InitLevel, InitLevel);
                UnRegister(GameEvents.NextLevel, NextLevel);
            }
        }

        private void InitLevel(object[] args)
        {
            Roar(GameEvents.GetLevelNumber, LevelCount);
        }

        private void NextLevel(object[] args)
        {
            CreateLevel();
        }

        private void OnGameComplete(object[] obj)
        {
            bool status = (bool)obj[0];

            if (!status) return;

            LevelCount++;
            LevelIndex++;
        }

        #endregion

        #region Private Methods

        private void CreateLevel()
        {
            if (_tempLevel != null)
            {
                Destroy(_tempLevel);
            }

            if (LevelIndex >= levels.Length)
            {
                LevelIndex = 0;
            }

            InstantiateLevel();
        }

        private void InstantiateLevel()
        {
            _level = levels[LevelIndex];
            _tempLevel = Instantiate(_level.LevelPrefab);
            _tempLevel.GetComponent<GameLevelBear>().InitLevel();
        }

        #endregion
    }
}