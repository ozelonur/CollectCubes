using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace _GAME_.Scripts.Structs
{
    [BurstCompile]
    public struct CalculateIsObjectMovingJob : IJob
    {
        public Vector3 previousPosition;
        public Vector3 currentPosition;
        
        public NativeArray<bool> isMoving;
        
        public void Execute()
        {
            if (math.abs(previousPosition.x - currentPosition.x) > .01f ||
                math.abs(previousPosition.y - currentPosition.y) > .01f ||
                math.abs(previousPosition.z - currentPosition.z) > .01f)
            {
                isMoving[0] = true;
            }

            else
            {
                isMoving[0] = false;
            }
        }
    }
}