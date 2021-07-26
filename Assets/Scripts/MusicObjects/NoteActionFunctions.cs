using Unity.Burst;
using Unity.Mathematics;

namespace MusicObjects
{
    [BurstCompile]
    public static class NoteActionFunctions
    {
        public delegate float2 NoteAction<TActionData>(float2 a, TActionData b, float time = 1.0f) where TActionData : struct;

        [BurstCompile]
        public static float2 MatrixMultiply(float2 a, float2x2 b, float time = 1f)
        {
            return new float2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
        }
    }
}