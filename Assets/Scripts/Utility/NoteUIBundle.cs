using UIControllers;

namespace DefaultNamespace
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNoteElement"></typeparam>
    public readonly struct NoteUIBundle<TNoteElement>
    {
        
        public TNoteElement NoteElement { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public UIComponentsContainer UIContainer { get; }
        
        public NoteUIBundle(TNoteElement noteElement, UIComponentsContainer uiContainer)
        {
            NoteElement = noteElement;
            UIContainer = uiContainer;
        }
    }
}