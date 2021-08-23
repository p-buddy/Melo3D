using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace NoteElements
{
    [BurstCompile]
    public static class NoteActionFunctions
    {
        public delegate float2 NoteAction<TActionData>(float2 a, TActionData b, float interpolationPoint = 1.0f) where TActionData : struct;

        [BurstCompile]
        public static float2 MatrixMultiply(float2 a, float2x2 b, float interpolationPoint = 1f)
        {
            float2 scale = new float2(math.sign(b.c0.x) * math.length(b.c0), math.sign(b.c1.y) * math.length(b.c1));
            scale = math.lerp(new float2(1f), scale, interpolationPoint);
            float angle = math.atan2(b.c1.x, b.c1.y);
            angle = math.lerp(0f, angle, interpolationPoint);
            float2x2 rotateThenScale = new float2x2(new float2(math.cos(angle), -math.sin(angle)) * scale.x,
                new float2(math.sin(angle), math.cos(angle)) * scale.y);
            return math.mul(a, rotateThenScale);
        }
    }
}