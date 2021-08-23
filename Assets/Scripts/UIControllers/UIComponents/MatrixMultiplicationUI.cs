using System.Linq;
using UIControllers;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class MatrixMultiplicationUI : MonoBehaviour, INoteActionUI<float2x2>
{
    public enum Type
    {
        None,
        Rotation,
        Scale,
    }
    [field: SerializeField]
    public Type MatrixType { get; set; }
    
    private const int RowCount = 2;
    [SerializeField]
    private TMP_InputField[] column0InputFields = new TMP_InputField[RowCount];
    [SerializeField]
    private TMP_InputField[] column1InputFields = new TMP_InputField[RowCount];

    [SerializeField] 
    private TMP_InputField durationInputField;

    private float2x2? current = null;

    #region MonoBehaviour
    void Awake()
    {
        for (var index = 0; index < RowCount; index++)
        {
            TMP_InputField inputField = column0InputFields[index];
            Assert.IsNotNull(inputField, $"{nameof(MatrixMultiplicationUI)} exception: column 0, index {index} is null");
            inputField = column1InputFields[index];
            Assert.IsNotNull(inputField, $"{nameof(MatrixMultiplicationUI)} exception: column 0, index {index} is null");
        }
        SetupOnChangedListeners();

        Assert.IsNotNull(durationInputField);
        durationInputField.onValueChanged.AddListener(ProcessDurationInput);
    }
    #endregion MonoBehaviour

    public event OnUIDelete OnUIDelete;
    public void Delete()
    {
        OnUIDelete?.Invoke();
        Destroy(gameObject);
    }

    public event OnUIEdit<float2x2> OnUIEdit;
    
    public void Set(float2x2? value)
    {
        if (!value.HasValue)
        {
            return;
        }
        RemoveOnChangedListeners();
        column0InputFields[0].text = $"{value.Value.c0.x}";
        column0InputFields[1].text = $"{value.Value.c0.y}";
        column1InputFields[0].text = $"{value.Value.c1.x}";
        column1InputFields[1].text = $"{value.Value.c1.y}";
        SetupOnChangedListeners();
    }

    public virtual float2x2? GetData()
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

    protected virtual void ProcessMatrixInputs(string value)
    {
        float2x2? previous = current;
        current = GetData();
        
        if (current.HasValue)
        {
            float2x2 mat = current.Value;
            float2 a = mat.c0;
            float2 b = mat.c1;
        }
        
        OnUIEdit?.Invoke(previous, current);
    }

    private void ProcessDurationInput(string value)
    {
        GetDuration();
    }

    private void SetupOnChangedListeners()
    {
        foreach (var field in column0InputFields.Concat(column1InputFields))
        {
            field.onValueChanged.AddListener(ProcessMatrixInputs);
        }
    }

    private void RemoveOnChangedListeners()
    {
        foreach (var field in column0InputFields.Concat(column1InputFields))
        {
            field.onValueChanged.RemoveListener(ProcessMatrixInputs);
        }
    }
}
