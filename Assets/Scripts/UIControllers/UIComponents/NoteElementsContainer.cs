using System.Linq;
using UnityEngine;

namespace UIControllers
{
    public class NoteElementsContainer : MonoBehaviour, IUIComponent
    {
        public void Append(UIComponentsContainer uiContainer)
        {
            foreach (Transform uiTransform in uiContainer.GameObjects.Select(x => x.transform))
            {
                uiTransform.SetParent(transform, false);
                uiTransform.SetAsLastSibling();
            }
        }

        public void Insert(UIComponentsContainer uiContainer, int index)
        {
            int siblingIndex = index;
            foreach (Transform uiTransform in uiContainer.GameObjects.Select(x => x.transform))
            {
                uiTransform.SetParent(transform, false);
                uiTransform.SetSiblingIndex(siblingIndex);
                siblingIndex++;
            }
        }
    }
}