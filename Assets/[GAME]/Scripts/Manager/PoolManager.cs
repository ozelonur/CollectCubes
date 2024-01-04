using _GAME_.Scripts.Bears.Cube;
using _GAME_.Scripts.Utils;
using _ORANGEBEAR_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Manager
{
    public class PoolManager : Manager<PoolManager>
    {
        #region Serialized Fields

        [Header("Prefabs")]
        [SerializeField] private CubeController cubePrefab;
        
        [Header("Parents")]
        [SerializeField] private Transform cubeParent;

        #endregion

        #region Public Variables

        [HideInInspector] public CustomObjectPool<CubeController> cubePool;

        #endregion

        #region MonoBehaviour Methods

        private void Start()
        {
            cubePool = new CustomObjectPool<CubeController>(cubePrefab, 1000, cubeParent);
        }

        #endregion
    }
}