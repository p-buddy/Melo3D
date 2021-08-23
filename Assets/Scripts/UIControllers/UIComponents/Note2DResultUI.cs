using System;
using TMPro;
using UIControllers;
using Unity.Mathematics;

namespace UIControllers
{
    public class Note2DResultUI : Note2DCoordinateUI, INoteResultUI
    {
        protected override void Initialize()
        {
            foreach (TMP_InputField inputField in coordinateInputFields)
            {
                inputField.interactable = false;
            }
        }
    }
}