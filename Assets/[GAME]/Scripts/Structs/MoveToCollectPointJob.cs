using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace _GAME_.Scripts.Structs
{
    [BurstCompile]
    public struct MoveToCollectPointJob : IJob
    {
        public Vector3 target;
        public Vector3 position;

        public NativeArray<Vector3> destination;


        public void Execute()
        {
            destination[0] = target - position;
        }
    }
}