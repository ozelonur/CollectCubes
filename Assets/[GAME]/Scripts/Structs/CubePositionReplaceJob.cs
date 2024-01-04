using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace _GAME_.Scripts.Structs
{
    public struct CubePositionReplaceJob : IJobParallelForTransform
    {
        public NativeArray<Vector3> positions;
        
        public void Execute(int index, TransformAccess transform)
        {
            transform.position = positions[index];
        }
    }
}