namespace UIControllers
{
    public interface ISettableFromCodeUI<TData> where TData : struct
    {
        void Set(TData? value);
    }
}