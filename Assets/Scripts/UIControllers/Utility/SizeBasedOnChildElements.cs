using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace UIControllers
{
    
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class SizeBasedOnChildElements : MonoBehaviour
    {
        [SerializeField] 
        private ControlDimension controlDimension = ControlDimension.None;

        [SerializeField] 
        private Vector2 size;

        private readonly EditModeRefresher refresher = new EditModeRefresher(10f);
        private RectTransform rectTransform;

#if UNITY_EDITOR
        private void Update()
        {
            if (!refresher.IsTimeToRefresh())
            {
                return;
            }

            size = UIUtility.GetSizeOfActiveChildren(transform);
            ResizeToChildren(size);
        }
#endif

        private void ResizeToChildren(Vector2 sizeOfChildren)
        {
            rectTransform = rectTransform ? rectTransform : transform.GetComponent<RectTransform>();
            Rect rect = rectTransform.rect;
            float width = (controlDimension.HasFlag(ControlDimension.Width)) ? sizeOfChildren.x : rect.width;
            float height = (controlDimension.HasFlag(ControlDimension.Height)) ? sizeOfChildren.y : rect.height;
            rectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}
