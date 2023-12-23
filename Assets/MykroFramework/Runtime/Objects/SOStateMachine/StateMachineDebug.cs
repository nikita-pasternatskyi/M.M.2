#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [RequireComponent(typeof(SOStateMachine))]
    public class StateMachineDebug : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Vector3 _offset;
        [SerializeField] private string _text;

        [SerializeField] private SOStateMachine _stateMachine;

        private static GUIStyle style;
        private static GUIStyle Style
        {
            get
            {
                if (style == null)
                {
                    style = new GUIStyle(EditorStyles.largeLabel);
                    style.alignment = TextAnchor.MiddleCenter;
                    style.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
                    style.fontSize = 32;
                }
                return style;
            }
        }

        void OnDrawGizmos()
        {
            Style.fontSize = 16;

            Vector3 wPos = transform.position + _offset;

            Vector3 scPos = Camera.current.WorldToScreenPoint(wPos);
            if (scPos.z <= 0)
            {
                return;
            }

            Handles.BeginGUI();


            scPos.y = Screen.height - scPos.y; // Flip Y

            Vector2 strSize = Style.CalcSize(new GUIContent(_text));

            Rect rect = new Rect(0f, 0f, strSize.x + 6, strSize.y + 4);
            rect.center = scPos - Vector3.up * rect.height * 0.5f;
            GUI.color = new Color(0f, 0f, 0f, 1);
            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1);
            GUI.Label(rect, _text, Style);
            GUI.color = Color.white;

            Handles.EndGUI();
        }

        private void Awake()
        {
            if(_stateMachine == null)
                _stateMachine = GetComponent<SOStateMachine>();

        }

        private void OnEnable()
        {
            _stateMachine.NewStateEntered += OnStateChanged;
        }

        private void OnDisable()
        {
            _stateMachine.NewStateEntered -= OnStateChanged;
        }

        private void OnStateChanged(StateContainer obj)
        {
            _text = obj.ToString();
        }
#endif
    }
}