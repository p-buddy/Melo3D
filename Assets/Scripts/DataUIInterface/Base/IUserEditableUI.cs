namespace DataUIInterface
{
    public interface IUserEditableUI<TData> where TData : struct
    {
        event OnUIEdit<TData> OnUIEdit;
    }
}