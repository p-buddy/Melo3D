namespace UIControllers
{
    /// <summary>
    /// Interface for a UI that can be deleted (from screen)
    /// </summary>
    public interface IDeletableUI
    {
        event OnUIDelete OnUIDelete;
        void Delete();
    }
}