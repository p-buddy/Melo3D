using Unity.Mathematics;

namespace UIControllers
{
    public class RotationMatrixMultiplicationUI : MatrixMultiplicationUI
    {
        public override float2x2? GetData()
        {
            float2x2? data = base.GetData();
            if (data == null)
            {
                return null;
            }
            
            return new float2x2(math.cos(math.radians(data.Value.c0.x)),
                math.sin(math.radians(data.Value.c1.x)),
                -math.sin(math.radians(data.Value.c0.y)),
                math.cos(math.radians(data.Value.c1.y)));
        }

        protected override void ProcessMatrixInputs(string value)
        {
            if (UIUtility.TryGetFloat(value, out float angle))
            {
                Set(new float2x2(angle));
            }
            base.ProcessMatrixInputs(value);
        }
    }
}