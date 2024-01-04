using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace _GAME_.Scripts.Structs
{
    [BurstCompile]
    public struct CalculateDestinationJob : IJobParallelForTransform
    {
        public float speed;
        public float rotationSpeed;
        public float inputX;
        public float inputZ;
        public float joystickMagnitude;
        public float deltaTime;

        public NativeArray<Vector3> destination; 

        public void Execute(int i, TransformAccess transform)
        {
            destination[0] = math.normalizesafe(new float3(inputX, 0, inputZ)) *
                             (speed * 6 * deltaTime * joystickMagnitude);

            // transform.position += destination[0];
            
            Vector3 lookDirection = new Vector3(inputX, 0, inputZ);
            
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * deltaTime);

            transform.position = new Vector3(math.clamp(transform.position.x, -3.4f, 3.4f),
                math.clamp(transform.position.y, 0, 0), math.clamp(transform.position.z, -5.4f, 5.6f));
        }
    }
}
