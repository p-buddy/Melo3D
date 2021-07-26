using DataUIInterface;
using NUnit.Framework;
using TMPro;
using UI;
using Unity.Mathematics;
using UnityEngine;

public class MatrixMultiplicationUI : MonoBehaviour, INoteActionUI<float2x2>
{
    private const int RowCount = 2;
    [SerializeField]
    private TMP_InputField[] column0InputFields = new TMP_InputField[RowCount];
    [SerializeField]
    private TMP_InputField[] column1InputFields = new TMP_InputField[RowCount];

    [SerializeField] 
    private TMP_InputField durationInputField;

    private float2x2? current = null;

    void Awake()
    {
        for (var index = 0; index < column0InputFields.Length; index++)
        {
            TMP_InputField inputField = column0InputFields[index];
            Assert.IsNotNull(inputField, $"{nameof(MatrixMultiplicationUI)} exception: column 0, index {index} is null");
            inputField.onValueChanged.AddListener(ProcessInputs);
                
            inputField = column1InputFields[index];
            Assert.IsNotNull(inputField, $"{nameof(MatrixMultiplicationUI)} exception: column 0, index {index} is null");
            inputField.onValueChanged.AddListener(ProcessInputs);
        }
        Assert.IsNotNull(durationInputField);
        durationInputField.onValueChanged.AddListener(ProcessInputs);
    }
    

    public event OnUIDelete OnUIDelete;
    
    public void Delete()
    {
        OnUIDelete?.Invoke();
        Destroy(gameObject);
    }

    public event OnUIEdit<float2x2> OnUIEdit;
    
    public void Set(float2x2? value)
    {
        throw new System.NotImplementedException();
    }

    public float2x2? GetData()
    {
        if (UIUtility.ProcessAndValidateFloatInput(column0InputFields[0], out float c0r0) &&
            UIUtility.ProcessAndValidateFloatInput(column0InputFields[1], out float c0r1) &&
            UIUtility.ProcessAndValidateFloatInput(column1InputFields[0], out float c1r0) &&
            UIUtility.ProcessAndValidateFloatInput(column1InputFields[1], out float c1r1))
        {
            return new float2x2(new float2(c0r0, c0r1), new float2(c1r0, c1r1));
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

    private void ProcessInputs(string value)
    {
        float2x2? previous = current;
        current = GetData();
        
        if (current.HasValue)
        {
            float2x2 mat = current.Value;
            float2 a = mat.c0;
            float2 b = mat.c1;
        }
        
        GetDuration();
        OnUIEdit?.Invoke(previous, current);
    }
}
