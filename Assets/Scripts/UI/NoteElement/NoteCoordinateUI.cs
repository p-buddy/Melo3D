using System;
using DataUIInterface;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace UI
{
    public abstract class NoteCoordinateUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_InputField[] coordinateInputFields = new TMP_InputField[2];

        [SerializeField] 
        private TMP_InputField durationInputField;

        private float2? current;

        private void Awake()
        {
            foreach (TMP_InputField inputField in coordinateInputFields)
            {
                Assert.IsNotNull(inputField);
                inputField.onValueChanged.AddListener(ProcessInputs);
            }
            Assert.IsNotNull(durationInputField);
            durationInputField.onValueChanged.AddListener(ProcessInputs);
        }

        public virtual void Set(float2? value)
        {
            if (value.HasValue)
            {
                coordinateInputFields[0].text = $"{value.Value.x}";
                coordinateInputFields[1].text = $"{value.Value.y}";
            }
        }

        public float2? GetData()
        {
            if (UIUtility.ProcessAndValidateFloatInput(coordinateInputFields[0], out float x) &&
                UIUtility.ProcessAndValidateFloatInput(coordinateInputFields[1], out float y))
            {
                return new float2(x, y);
            }

            return null;
        }

        public float? GetDuration()
        {
            if (UIUtility.ProcessAndValidateFloatInput(durationInputField, out float duration))
            {
                return duration;
            }

            return null;
        }

        public event OnUIEdit<float2> OnUIEdit;
        
        private void ProcessInputs(string value)
        {
            float2? previous = current;
            current = GetData();
            GetDuration();
            OnUIEdit?.Invoke(previous, current);
        }

        public event OnUIDelete OnUIDelete;
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}