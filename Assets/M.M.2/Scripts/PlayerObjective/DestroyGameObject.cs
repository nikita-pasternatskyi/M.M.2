using UnityEngine;

namespace MM2
{

    public class DestroyGameObject : MonoBehaviour
    {
        [SerializeField] private bool _destroyOnAwake;

        public void DestroyObject(GameObject go)
        {
            Destroy(go);
        }

        public void DestroySelf() => Destroy(gameObject);
    }
}
