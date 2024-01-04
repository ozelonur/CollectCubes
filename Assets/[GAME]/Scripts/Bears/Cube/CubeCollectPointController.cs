using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.Enums;
using _ORANGEBEAR_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Bears.Cube
{
    public class CubeCollectPointController : Bear
    {
        #region Private Variables

        private int _collectedCubeCount;

        #endregion

        #region MonoBehaviour Methods

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ICube cube))
            {
                return;
            }

            LevelType levelType = ((GameLevelBear)GameManager.Instance.currentLevel).levelType;

            if (levelType == LevelType.Time)
            {
                _collectedCubeCount++;
                Roar(CustomEvents.UpdateCollectedCubeCount, _collectedCubeCount);
            }

            cube.CubeInteractedWithTheCollectPoint(transform.position);
        }

        #endregion
    }
}