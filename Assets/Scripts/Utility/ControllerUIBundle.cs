using UI;

namespace DefaultNamespace
{
    public readonly struct ControllerUIBundle<TController>
    {
        public TController Controller { get; }
        public UIComponentsContainer UIContainer { get; }
        
        public ControllerUIBundle(TController controller, UIComponentsContainer uiContainer)
        {
            Controller = controller;
            UIContainer = uiContainer;
        }
    }
}