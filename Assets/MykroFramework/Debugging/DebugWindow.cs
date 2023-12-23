using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MykroFramework.Debugging
{
    public class DebugWindow : MonoBehaviour
    {
        [SerializeField] private float _width;
        [SerializeField] private Vector2Int _offset;
        [SerializeField] private int _blockOffset;

        private Dictionary<string, IDebuggable> _debuggables = new Dictionary<string, IDebuggable>();
        private List<DebugTextBlock> _blocks = new List<DebugTextBlock>();
        private Dictionary<DebugTextBlock, int> _textBlockIndices = new Dictionary<DebugTextBlock, int>();

        private LinkedList<IDebuggable> _currentDebuggers = new LinkedList<IDebuggable>();
        
        private static DebugWindow _instance;
        private GUIStyle _currentStyle = null;

        private bool _rolledOut = false;
        private string _text = "hello";

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this);

            var commands = GetAll().ToList();
            foreach (var command in commands)
            {
                _debuggables.Add(command.Name, command);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _rolledOut = !_rolledOut;
            }

            foreach (var item in _currentDebuggers)
            {
                item.DoDebug();
            }
        }

        IEnumerable<IDebuggable> GetAll()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterface("IDebuggable") != null && type.IsAssignableFrom(typeof(MonoBehaviour)) == false)
                .Select(type => Activator.CreateInstance(type) as IDebuggable);
        }

        private void OnGUI()
        {   
            if (!_rolledOut)
                return;
            InitStyles();

            if (Event.current.Equals(Event.KeyboardEvent("return")))
            {
                if (_debuggables.TryGetValue(_text, out IDebuggable debuggable))
                {
                    if (_currentDebuggers.Contains(debuggable))
                    {
                        debuggable.Close();
                        _currentDebuggers.Remove(debuggable);
                    }
                    else
                    {
                        debuggable.Open(CreateTextBlock());
                        _currentDebuggers.AddLast(debuggable);
                    }
                }
                _text = "";
            }

            int textFieldHeight = 20;

            _text = GUI.TextField(new Rect(_offset.x, _offset.y, _width, textFieldHeight), _text, (int)(_width / 10));

            if (_blocks.Count != 0)
            {
                float previousHeight = 0;
                for (int i = 0; i < _blocks.Count; i++)
                {
                    var text = _blocks[i].DrawText();
                    var guiContent = new GUIContent(text);
                    float height = _currentStyle.CalcHeight(guiContent, _width);
                    GUI.Box(new Rect(_offset.x, _offset.y + previousHeight + textFieldHeight, _width, height), text, _currentStyle);
                    previousHeight = height;
                }
            }
            
        }

        private void InitStyles()
        {
            if (_currentStyle == null)
            {
                _currentStyle = new GUIStyle(GUI.skin.box);
                _currentStyle.richText = true;
                _currentStyle.alignment = TextAnchor.UpperLeft;
                _currentStyle.wordWrap = true;
                _currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1f));
            }
        }

        public static DebugTextBlock CreateTextBlock()
        {
            DebugTextBlock textBlock = new DebugTextBlock();
            _instance._blocks.Add(textBlock);
            _instance._textBlockIndices.Add(textBlock, _instance._blocks.Count);
            return textBlock;
        }

        public static void Register(IDebuggable debugabble, DebuggerDescription description)
        {
            _instance._debuggables.Add(description.Name, debugabble);
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
