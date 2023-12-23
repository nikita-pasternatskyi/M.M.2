using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using MykroFramework.Runtime._Init;

namespace MykroFramework.Editor._Init
{

    public class InitEditorWindow : OdinEditorWindow
    {
        [SerializeField][InlineEditor] private InitConfig _initConfig;
        private const string _pathToInitSettings = "Assets/Resources/MykroConfig/";
        private const string _fileName = "config";
        private string _filePath = _pathToInitSettings + _fileName;

        [MenuItem("MykroFramework/Init Editor Window")]
        public static void ShowWindow()
        {
            GetWindow<InitEditorWindow>("Init Editor Window").Show();
        }

        protected override void Initialize()
        {
            base.Initialize(); 
            var path = _pathToInitSettings + $@"\{_fileName}.asset";
            if (File.Exists(path))
            {
                _initConfig = AssetDatabase.LoadAssetAtPath(path, typeof(InitConfig)) as InitConfig;
                return;
            }
            if (!Directory.Exists(_pathToInitSettings))
            {
                var info = Directory.CreateDirectory(_pathToInitSettings);
            }
            ScriptableObject obj = ScriptableObject.CreateInstance(typeof(InitConfig));
            var config = obj as InitConfig;
            AssetDatabase.CreateAsset(obj, path);
            _initConfig = config;
        }
    }
}
