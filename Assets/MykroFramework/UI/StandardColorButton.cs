using UnityEngine;
using UnityEngine.UI;

namespace MykroFramework.Runtime.UI
{
    public class StandardColorButton : MonoBehaviour
    {
        [SerializeField] private Selectable _selectable;
        [SerializeField] private Color32 _selectedColor;
        [SerializeField] private Color32 _deselectedColor;
        [SerializeField] private Color32 _pressedColor;
        [SerializeField] private Graphic _target;

        private void OnEnable()
        {
            _selectable.Selected += OnSelected;
            _selectable.Deselected += OnDeselected;
            _selectable.Pressed += OnPressed;
        }
      
        private void OnDisable()
        {
            _selectable.Selected -= OnSelected;
            _selectable.Deselected -= OnDeselected;
            _selectable.Pressed -= OnPressed;
        }

        private void Awake()
        {
            _target.color = _deselectedColor;
        }

        private void OnSelected()
        {
            _target.color = _selectedColor;
        }

        private void OnDeselected()
        {
            _target.color = _deselectedColor;
        }

        private void OnPressed()
        {
            _target.color = _pressedColor;
        }
    }

}

