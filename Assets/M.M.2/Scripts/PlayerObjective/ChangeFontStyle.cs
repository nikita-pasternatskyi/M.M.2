using TMPro;
using UnityEngine;

namespace MM2
{
    public class ChangeFontStyle : MonoBehaviour
    {
        [SerializeField] private FontStyles _targetFontStyle;
        [SerializeField] private TextMeshProUGUI _text;

        public void Change()
        {
            _text.fontStyle = _targetFontStyle;
        }
    }
}
