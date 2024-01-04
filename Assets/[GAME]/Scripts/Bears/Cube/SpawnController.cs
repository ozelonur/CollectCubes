using System.Collections.Generic;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Models;
using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.Enums;
using _ORANGEBEAR_.Scripts.Managers;

namespace _GAME_.Scripts.Bears.Cube
{
    public class SpawnController : Bear
    {
        #region Public Variables

        public List<CubeData> cubes = new List<CubeData>();

        #endregion

        #region Private Variables

        private int _cubeCount;

        #endregion

        #region MonoBehaviour Methods

        private void Start()
        {
            _cubeCount = cubes.Count;

            foreach (CubeData cube in cubes)
            {
                cube.cube.InitCube(cube.color);
            }
        }

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(CustomEvents.DecreaseCubeCount, DecreaseCubeCount);
                Register(CustomEvents.ResetCubeCounts, ResetCubeCounts);
            }

            else
            {
                UnRegister(CustomEvents.DecreaseCubeCount, DecreaseCubeCount);
                UnRegister(CustomEvents.ResetCubeCounts, ResetCubeCounts);
            }
        }

        private void ResetCubeCounts(object[] args)
        {
            _cubeCount = (int)args[0];
        }

        private void DecreaseCubeCount(object[] args)
        {
            LevelType levelType = ((GameLevelBear)GameManager.Instance.currentLevel).levelType;

            _cubeCount--;
            if (levelType == LevelType.Time)
            {
                if (_cubeCount == 0)
                {
                    ResetAllCube();
                    Roar(CustomEvents.RespawnCubes);
                }
            }

            else
            {
                if (_cubeCount > 0) return;

                ResetAllCube();

                Roar(GameEvents.OnGameComplete, true);
            }
        }

        #endregion

        #region Private Methods

        private void ResetAllCube()
        {
            foreach (CubeData cube in cubes)
            {
                cube.cube.ResetCube();
            }
        }

        #endregion
    }
}