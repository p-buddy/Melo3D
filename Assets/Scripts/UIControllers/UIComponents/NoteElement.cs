using NUnit.Framework;
using UnityEngine;

namespace UIControllers
{
    public class NoteElement : MonoBehaviour, IUIComponent
    {
        public Vector2 OriginalSize { get; private set; }
        public Vector2 CurrentSize => rectTransform.rect.size;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            Assert.IsNotNull(rectTransform);
            
            OriginalSize = CurrentSize;
        }

        public void SetHeight(float height)
        {
            rectTransform.sizeDelta = new Vector2(CurrentSize.x, height);
        }
    }
}