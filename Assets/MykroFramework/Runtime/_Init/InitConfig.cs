using UnityEngine;

namespace MykroFramework.Runtime._Init
{
    public class InitConfig : ScriptableObject
    {
        public GameObject[] PrefabsToKeep;
        public bool UseInit;

        public void Initialize()
        {
            foreach (var item in PrefabsToKeep)
            {
                var obj = Instantiate(item);
                DontDestroyOnLoad(obj);
            }
        }
    }
}
