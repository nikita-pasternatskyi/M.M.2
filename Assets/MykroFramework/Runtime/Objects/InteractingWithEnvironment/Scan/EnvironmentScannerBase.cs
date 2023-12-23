using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Scan
{
    public abstract class EnvironmentScannerBase<T>
    {
        public Transform Transform;
        public abstract T[] GetHits(Vector3 position);
        public abstract void DrawDebug();
    }
}
