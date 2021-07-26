namespace DataUIInterface
{
    public delegate void OnUIDelete();

    public delegate void OnUIEdit<TData>(TData? previous, TData? current) where TData : struct;
}