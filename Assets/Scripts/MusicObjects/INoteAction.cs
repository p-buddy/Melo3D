using Unity.Mathematics;

namespace MusicObjects
{
    public interface INoteAction<TActionData>
    {
        float2 Calculate(float2 input, TActionData actionData);
    }
}