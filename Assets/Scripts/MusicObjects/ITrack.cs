using DefaultNamespace;
using Unity.Mathematics;

namespace MusicObjects
{
    public interface ITrack
    {
        float2? GetInput(int myIndex);
        float2? GetResult();
        void StartingCoordinateChange(float2? value);
        void ActionDataChange<TData>(TData? value, int myIndex) where TData : struct;
        void RemoveAtIndex(int myIndex);
        void AddAtIndex(ControllerUIBundle<INoteActionResult> actionResultBundle, int index);

        void Add(ControllerUIBundle<INoteActionResult> actionResultBundle);
        int ActionsCount { get; }
        
    }
}