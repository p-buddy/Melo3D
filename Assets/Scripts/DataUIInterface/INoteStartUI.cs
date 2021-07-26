using UI;
using Unity.Mathematics;

namespace DataUIInterface
{
    public interface INoteStartUI : IUIComponent, ISettableFromCodeUI<float2>, IRetrievableUI<float2>, IUserEditableUI<float2>
    {
        
    }
}