using UI;

namespace DataUIInterface
{
    public interface INoteActionUI<TData> : IUIComponent, 
                                            IDeletableUI,
                                            IUserEditableUI<TData>,
                                            ISettableFromCodeUI<TData>,
                                            IRetrievableUI<TData> where TData : struct
    {
        
    }
}