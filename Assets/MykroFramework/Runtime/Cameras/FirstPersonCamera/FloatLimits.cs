namespace MykroFramework.Runtime.Cameras
{
    [System.Serializable]
    public class FloatLimits : Limits<float>
    {
        public FloatLimits(Limits<float> limits) : base(limits)
        {
        }

        public FloatLimits(float min, float max) : base(min, max)
        {
        }
    }
}
