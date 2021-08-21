namespace UIControllers
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void OnUIDelete();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="previous"></param>
    /// <param name="current"></param>
    /// <typeparam name="TData"></typeparam>
    public delegate void OnUIEdit<TData>(TData? previous, TData? current) where TData : struct;
}