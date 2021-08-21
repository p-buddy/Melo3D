using UIControllers;

namespace UIControllers
{
    public interface INoteActionUI<TData> : IUIComponent, 
                                            IDeletableUI,
                                            IUserEditableUI<TData>,
                                            ISettableFromCodeUI<TData>,
                                            IRetrievableUI<TData> where TData : struct
    {
        
    }
}