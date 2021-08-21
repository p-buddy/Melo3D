using UI;
using Unity.Mathematics;

namespace DataUIInterface
{
    /// <summary>
    /// Interface for UI Controllers
    /// </summary>
    public interface INoteStartUI : IUIComponent,
                                    ISettableFromCodeUI<float2>,
                                    IRetrievableUI<float2>,
                                    IUserEditableUI<float2>
    {
        
    }
}