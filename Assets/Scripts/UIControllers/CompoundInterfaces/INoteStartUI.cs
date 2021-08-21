using UIControllers;
using Unity.Mathematics;

namespace UIControllers
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