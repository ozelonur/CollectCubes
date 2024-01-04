using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;

namespace _GAME_.Scripts.Structs
{
    [BurstCompile]
    public struct CalculateSpawnPositionsJob : IJobParallelFor
    {
        [ReadOnly] public int width;
        [ReadOnly] public int height;
        [ReadOnly] public float scale;

        public NativeArray<Color> pixels;

        public NativeArray<Vector3> positions;
        public NativeArray<Color> colors;

        public void Execute(int index)
        {
            int x = index % width;
            int y = index / width;

            Color pixel = pixels[index];

            if (!(pixel.a > 0))
            {
                return;
            }

            Vector3 position;

            position.x = (x - width / 2) * scale;
            position.y = 0.075f;
            position.z = (y - height / 2) * scale;

            positions[index] = position;
            colors[index] = pixel;
        }
    }
}