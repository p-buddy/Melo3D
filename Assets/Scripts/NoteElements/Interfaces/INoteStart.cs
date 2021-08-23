using Unity.Mathematics;

namespace NoteElements
{
    public interface INoteStart : INoteElement
    {
        float2? Current { get; }
        public float? CurrentDuration { get; }
    }
}