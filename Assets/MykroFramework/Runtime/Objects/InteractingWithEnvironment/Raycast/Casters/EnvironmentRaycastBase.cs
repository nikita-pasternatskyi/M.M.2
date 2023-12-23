namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast
{
    public abstract class EnvironmentRaycastBase<T>
    {
        public abstract bool FindHit(out T environmentQueryResult, float length = -1);
        public abstract void DrawDebug();
    }
}