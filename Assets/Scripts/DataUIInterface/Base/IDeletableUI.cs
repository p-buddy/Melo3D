namespace DataUIInterface
{
    public interface IDeletableUI
    {
        event OnUIDelete OnUIDelete;
        void Delete();
    }
}