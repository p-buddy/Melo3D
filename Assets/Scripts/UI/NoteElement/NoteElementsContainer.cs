using System.Linq;
using UnityEngine;

namespace UI
{
    public class NoteElementsContainer : MonoBehaviour, IUIComponent
    {
        public void Append(UIComponentsContainer components)
        {
            foreach (Transform uiTransform in components.GameObjects.Select(x => x.transform))
            {
                uiTransform.SetParent(transform, false);
                uiTransform.SetAsLastSibling();
            }
        }

        public void Insert(UIComponentsContainer components, int index)
        {
            int siblingIndex = index;
            foreach (Transform uiTransform in components.GameObjects.Select(x => x.transform))
            {
                uiTransform.SetParent(transform, false);
                uiTransform.SetSiblingIndex(siblingIndex);
                siblingIndex++;
            }
        }
    }
}