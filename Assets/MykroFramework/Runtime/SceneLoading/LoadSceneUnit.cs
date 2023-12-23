using Udar.SceneManager;
using UnityEngine;

namespace MykroFramework.Runtime.SceneLoading
{
    [CreateAssetMenu(menuName = "Game/Load Scene Unit")]
    public class LoadSceneUnit : ScriptableObject
    {
        public SceneField SceneToLoad;

        public void Load()
        {
            SceneLoader.Instance.LoadScene(SceneToLoad);
        }
    }
}
