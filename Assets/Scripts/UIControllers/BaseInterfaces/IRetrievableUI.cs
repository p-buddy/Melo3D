namespace UIControllers
{
    public interface IRetrievableUI<TData> where TData : struct
    {
        TData? GetData();
        float? GetDuration();
    }
}