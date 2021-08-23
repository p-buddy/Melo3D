using Unity.Mathematics;

namespace NoteElements
{
    public interface INoteActionResult : INoteElement
    {
        void SetIndexWithinTrack(int index);

        float2? CurrentResult { get; }
        
        public float? CurrentActionDuration { get; }
        public float? CurrentResultDuration { get; }
        void RefreshResult();
    }
}