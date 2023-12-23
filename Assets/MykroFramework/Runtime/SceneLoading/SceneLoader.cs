using System.Collections.Generic;
using UnityEngine;
using Udar.SceneManager;
using UnityEngine.SceneManagement;

namespace MykroFramework.Runtime.SceneLoading
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;
        private int _keptScene;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void ReloadScene()
        {
            if (_keptScene != -1)
                SceneManager.LoadSceneAsync(_keptScene, LoadSceneMode.Single);
            else
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        public void LoadScene(SceneField scene)
        {
            SceneManager.LoadSceneAsync(scene.BuildIndex, LoadSceneMode.Single);
        }

        public void LoadSceneAdditively(SceneField scene, bool keepForRestart)
        {
            SceneManager.LoadSceneAsync(scene.BuildIndex, LoadSceneMode.Additive);
            _keptScene = keepForRestart ? scene.BuildIndex : -1;
        }

    }
}
