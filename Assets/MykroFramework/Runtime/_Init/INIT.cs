using UnityEngine;

namespace MykroFramework.Runtime._Init
{
    public static class INIT
    {
        private static InitConfig _initConfig;
        private static string _path = "MykroConfig/config";
        private static string _name = "INITTED GAME 18052005";
        private static GameObject _flagGO;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoadRuntimeMethod()
        {
            if (_flagGO)
                return;
            _initConfig = (Resources.Load(_path) as InitConfig);
            if (_initConfig.UseInit == false)
            {
                return;
            }
            _flagGO = new GameObject(_name);
            GameObject.DontDestroyOnLoad(_flagGO);
            _initConfig.Initialize();
        }
    }
}