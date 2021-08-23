using Unity.Burst;
using Unity.Mathematics;

namespace DefaultNamespace
{
    [BurstCompile]
    public static class CoordinateTo
    {
        private const int OctaveDistance = 10;

        [BurstCompile]
        public static float Frequency(float2 coordinate)
        {
            float angleDeg = math.degrees(GetAngle(coordinate));
            float delta = angleDeg / 30f;
            int start = (int) delta;
            float weight = 30f * (delta - start);
            int octave = (int) math.length(coordinate) / OctaveDistance;
            return math.lerp(Notes.Frequencies[start + 12 * octave], Notes.Frequencies[start + 1 + 12 * octave], weight);
        }
        
        [BurstCompile]
        public static float Gain(float2 coordinate)
        {
            return math.length(coordinate) % OctaveDistance / OctaveDistance;;
        }

        [BurstCompile]
        public static float GetAngle(float2 coordinate)
        {
            float angle = math.atan2(coordinate.y, coordinate.x);
            angle += (angle < 0) ? 2 * math.PI : 0f;
            return angle;
        }
    }
}