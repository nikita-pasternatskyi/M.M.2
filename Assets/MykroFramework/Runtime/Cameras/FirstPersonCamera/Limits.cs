namespace MykroFramework.Runtime.Cameras
{
    public class Limits<T>
    {
        public T Min;
        public T Max;

        public Limits(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public Limits(Limits<T> limits)
        {
            Min = limits.Min;
            Max = limits.Max;
        }
    }
}
