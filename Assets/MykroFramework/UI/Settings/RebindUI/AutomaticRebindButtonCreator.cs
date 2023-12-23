using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace MykroFramework.Runtime.Controls.RebindUI
{
    public class AutomaticRebindButtonCreator : MonoBehaviour
    {
        [SerializeField] [Required] private ButtonRebindUI _buttonRebindUIPrefab;
        [SerializeField] [Required] private ButtonMap _map;
        [SerializeField] private Controller _controller;
        
        private void Awake()
        {
            foreach (var item in _map.ButtonsArray)
            {
                var btn = Instantiate(_buttonRebindUIPrefab, transform);
                btn.Map = _map;
                btn.Button = item;
                btn.Controller = _controller;
                btn.UpdateUI();
            }
        }
    }
}
