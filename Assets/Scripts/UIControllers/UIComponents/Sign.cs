using UnityEngine;

namespace UIControllers
{
    public class Sign : MonoBehaviour, IUIComponent
    {
        public enum Type
        {
            Multiplication,
            Equals,
            Addition
        }
        [field: SerializeField]
        public Type SignType { get; set; }
    }
}