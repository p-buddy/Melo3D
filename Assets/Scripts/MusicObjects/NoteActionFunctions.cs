using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace MusicObjects
{
    [BurstCompile]
    public static class NoteActionFunctions
    {
        public delegate float2 NoteAction<TActionData>(float2 a, TActionData b, float time = 1.0f) where TActionData : struct;

        [BurstCompile]
        public static float2 MatrixMultiply(float2 a, float2x2 b, float time = 1f)
        {
            return math.mul(a, b);
        }
        
        [BurstCompile]
        public static float2 RotationMatrixMultiply(float2 a, float2x2 b, float time = 1f)
        {
            float2x2 matrix = new float2x2(math.cos(math.radians(b.c0.x)),
                math.sin(math.radians(b.c1.x)),
                -math.sin(math.radians(b.c0.y)),
                math.cos(math.radians(b.c1.y)));
            return math.mul(a, matrix);
        }
    }
}