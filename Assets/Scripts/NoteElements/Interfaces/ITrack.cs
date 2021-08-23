using DefaultNamespace;
using DefaultNamespace.Audio;
using Unity.Mathematics;

namespace NoteElements
{
    /// <summary>
    /// Something responsible for coordinating the
    /// </summary>
    public interface ITrack
    {
        float2? GetInput(int myIndex);
        float2? GetResult();
        void StartingCoordinateChange(float2? value);
        void ActionDataChange<TData>(TData? value, int myIndex) where TData : struct;
        void RemoveAtIndex(int myIndex);
        void AddAtIndex(in NoteUIBundle<INoteActionResult> actionResultBundle, int index);

        void Add(in NoteUIBundle<INoteActionResult> actionResultBundle);
        int ActionsCount { get; }
        AudioEvent[] GetEvents();
    }
}