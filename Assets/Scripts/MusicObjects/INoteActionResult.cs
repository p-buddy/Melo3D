using Unity.Mathematics;

namespace MusicObjects
{
    public interface INoteActionResult
    {
        void SetIndexWithinTrack(int index);

        float2? CurrentResult { get; }
        void RefreshResult();
    }
}