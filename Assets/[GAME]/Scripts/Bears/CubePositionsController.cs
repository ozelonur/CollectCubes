using System.Collections.Generic;
using _GAME_.Scripts.Bears.Cube;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Manager;
using _GAME_.Scripts.Models;
using _GAME_.Scripts.Structs;
using _ORANGEBEAR_.EventSystem;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace _GAME_.Scripts.Bears
{
    public class CubePositionsController : Bear
    {
        #region Public Variables

        public List<CalculatedLevelDataListStorer> calculatedLevelDatas = new List<CalculatedLevelDataListStorer>();

        [HideInInspector] public Transform parent;

        #endregion

        #region Private Variables

        private int waveCount;

        private PoolManager _poolManager;

        private readonly List<Transform> _transforms = new List<Transform>();
        private NativeArray<Vector3> _positions;
        private TransformAccessArray _transformAccessArray;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _poolManager = PoolManager.Instance;
        }

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(CustomEvents.RespawnCubes, RespawnCubes);
            }

            else
            {
                UnRegister(CustomEvents.RespawnCubes, RespawnCubes);
            }
        }

        private void RespawnCubes(object[] args)
        {
            if (waveCount >= calculatedLevelDatas.Count)
            {
                Roar(GameEvents.OnGameComplete, true);
                return;
            }

            List<CalculatedLevelData> calculatedLevelData = calculatedLevelDatas[waveCount].calculatedPositions;

            int count = calculatedLevelData.Count;
            Roar(CustomEvents.ResetCubeCounts, count);

            _positions = new NativeArray<Vector3>(count, Allocator.TempJob);
            int index = 0;

            foreach (CalculatedLevelData data in calculatedLevelData)
            {
                CubeController cube = _poolManager.cubePool.Get();

                Transform cubeTransform = cube.transform;
                cubeTransform.position = data.position;
                cubeTransform.localScale = Vector3.one * data.scale;
                cubeTransform.parent = parent;

                _transforms.Add(cubeTransform);
                _positions[index] = data.position;
                index++;

                cube.InitCube(data.color);
            }

            _transformAccessArray = new TransformAccessArray(_transforms.ToArray());

            CubePositionReplaceJob cubePositionReplaceJob = new CubePositionReplaceJob
            {
                positions = _positions
            };

            JobHandle jobHandle = cubePositionReplaceJob.Schedule(_transformAccessArray);
            jobHandle.Complete();

            _transformAccessArray.Dispose();
            _positions.Dispose();

            _transforms.Clear();


            waveCount++;
        }

        #endregion
    }
}