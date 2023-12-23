using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MykroFramework.Runtime.Controls.Editor
{
    public class ButtonMapCreatorEditorWindow : OdinEditorWindow
    {
        [SerializeField, TabGroup("Editing")] private MonoScript _script;
        [SerializeField, TabGroup("Editing")] private ButtonMap _editingScriptableObject;

        [Button, TabGroup("Editing"), ShowIf("@_script != null && _editingScriptableObject != null")]
        private void StopEditing()
        {
            _script = null;
            _buttonMapTemplate = _editingScriptableObject = null;
        }

        [Space]
        
        [SerializeField, Tooltip("This is used for when your mouse needs to be locked and hidden")]
        private bool _lockCursor;
        [SerializeField, Tooltip("This is used for checking all KEYS on your keyboard and gamepad every frame. Avoid for gameplay, use for UI")] 
        private bool _constantlyCheckAllKeys;
        
        [SerializeField] [ListDrawerSettings(AlwaysAddDefaultValue = true), DictionaryDrawerSettings(KeyLabel = "Button Name", ValueLabel = "Input Keys", DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)] private Dictionary<string, InputButton> _buttons = new Dictionary<string, InputButton>();
        [SerializeField, ShowIf("@_script == null && _editingScriptableObject == null")] private string _buttonMapName;
        private ButtonMap _buttonMapTemplate;

        private static bool _justRecompiled = true;
        private bool _waitingForRecompiling;
        private string _savePath = "Assets/";
        private string _lastFilePath;

        private static string BOILERPLATE_START = "using UnityEngine; using MykroFramework.Runtime.Controls; namespace Assets._Game.Controls{ public class CLASS_NAME : ButtonMap { ";

        private static string STRING_DECLARATION = "public readonly string {0} = {1};";

        private static string GETTER_DECLARATION = "public ButtonState Get{0}State()";

        private static string RETURN_FROM_VALUES_ARRAY = "return Values[{0}];";

        private static string BOILERPLATE_END = "};}";


        [MenuItem("MykroFramework/Button Map Editor Window")]
        public static void ShowWindow()
        {
            GetWindow<ButtonMapCreatorEditorWindow>("Button Map Creator").Show();
        }

        static ButtonMapCreatorEditorWindow()
        {
            _justRecompiled = true;
        }

        [Button(ButtonSizes.Large), HideIf("@_script != null && _editingScriptableObject != null")]
        public void CreateButtonMap()
        {
            CreateScript($"Assets/{_buttonMapName}.cs");
        }

        [Button(ButtonSizes.Large), ShowIf("@_script != null && _editingScriptableObject != null")]
        public void OverrideButtonMap()
        {
            CreateScript(AssetDatabase.GetAssetPath(_script), false);

            _editingScriptableObject.ButtonsArray = new string[_buttons.Count];
            _buttons.Keys.CopyTo(_editingScriptableObject.ButtonsArray, 0);
            _editingScriptableObject.Reset();
            _editingScriptableObject.LockCursor = _lockCursor;
            _editingScriptableObject.ConstantlyCheckAllKeys = _constantlyCheckAllKeys;
            _editingScriptableObject.DefaultButtons = new Dictionary<string, InputButton>(_buttons);
            _editingScriptableObject.CurrentButtons = new Dictionary<string, InputButton>(_buttons);
            EditorUtility.SetDirty(_editingScriptableObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void CreateScript(string path, bool recreateScriptableObject = true)
        {
            bool validArray = true;
            foreach (var button in _buttons.Keys)
            {
                if (String.IsNullOrEmpty(button))
                {
                    validArray = false;
                    break;
                }
            }

            if (!validArray)
                return;

            if (File.Exists(path))
                File.Delete(path);

            var className = _buttonMapName.Replace(' ', '_');
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(BOILERPLATE_START.Replace("CLASS_NAME", className));

                //creating string constants
                foreach (var button in _buttons.Keys)
                {
                    if (String.IsNullOrEmpty(button))
                        continue;
                    string name = button;
                    name = name.Replace(' ', '_');
                    string nameValue = $"\"{button}\"";
                    sw.Write(STRING_DECLARATION, name.ToUpper(), nameValue);
                }
                //creating getters
                foreach (var button in _buttons.Keys)
                {
                    if (String.IsNullOrEmpty(button))
                        continue;
                    string name = button;
                    name = name.Replace(' ', '_');
                    string nameValue = $"\"{button}\"";
                    sw.Write(GETTER_DECLARATION, name);
                    sw.Write("{");
                    sw.Write(RETURN_FROM_VALUES_ARRAY, name.ToUpper());
                    sw.Write("}");
                }
                sw.Write(BOILERPLATE_END);
            }
            _waitingForRecompiling = recreateScriptableObject;
            AssetDatabase.Refresh();
        }

        public void Update()
        {
            if (_justRecompiled && _waitingForRecompiling)
            {
                _waitingForRecompiling = false;
                OnRecompile();
            }
            _justRecompiled = false;
        }

        private void OnRecompile()
        {
            ScriptableObject obj = ScriptableObject.CreateInstance(_buttonMapName);
            var buttonMap = obj as ButtonMap;
            var path = "Assets" + $@"\{_buttonMapName}.asset";
            if (File.Exists(path))
                File.Delete(path);
            AssetDatabase.CreateAsset(obj, path);
            buttonMap.ButtonsArray = new string[_buttons.Count];
            _buttons.Keys.CopyTo(buttonMap.ButtonsArray, 0);
            buttonMap.LockCursor = _lockCursor;
            buttonMap.ConstantlyCheckAllKeys = _constantlyCheckAllKeys;
            buttonMap.DefaultButtons = new Dictionary<string, InputButton>(_buttons);
            buttonMap.CurrentButtons = new Dictionary<string, InputButton>(_buttons);
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = obj;

        }

        private void OnInspectorUpdate()
        {
            if (_editingScriptableObject != null && _buttonMapTemplate != _editingScriptableObject)
            {
                _buttons = _editingScriptableObject.DefaultButtons;
                _buttonMapName = _editingScriptableObject.name;
                _buttonMapTemplate = _editingScriptableObject;
                _lockCursor = _buttonMapTemplate.LockCursor;
                _constantlyCheckAllKeys = _buttonMapTemplate.ConstantlyCheckAllKeys;
            }

            //if (Selection.activeObject == null)
            //{
            //    _buttonMapTemplate = null;
            //    return;
            //}
            //var type = Selection.activeObject.GetType();
            //if (typeof(ButtonMap).IsAssignableFrom(type))
            //{
            //    var selectedTemplate = Selection.activeObject as ButtonMap;
            //    if (_buttonMapTemplate != selectedTemplate || selectedTemplate.name != _buttonMapName)
            //    {
            //        _buttonMapTemplate = selectedTemplate;
            //        _buttonMapName = _buttonMapTemplate.name;
            //        _buttons = _buttonMapTemplate.DefaultButtons;
            //        _savePath = AssetDatabase.GetAssetPath(_buttonMapTemplate);
            //    }
            //}
        }
    }
}
