using MykroFramework.Runtime.Controls;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MykroFramework.Runtime.UI
{
    public class WindowControl : SerializedMonoBehaviour
    {
        [SerializeField] private WindowControlSO _windowControlSO;
        [SerializeField] public bool BlockNavigation;
        [SerializeField, ReadOnly] private Window _currentWindow;
        [SerializeField, ReadOnly] private Window _previousWindow;
        private Dictionary<Window, Vector2Int> _rememberedIndices = new Dictionary<Window, Vector2Int>(8);
        private bool _canMove;

        [SerializeField, ReadOnly] private Selectable[,] _selectables;
        private Vector2Int _currentIndex;

        public UnityEvent SelectionChanged;
        public UnityEvent SelectionPressed;

        private void OnEnable()
        {
            _windowControlSO.ReturnToPreviousWindowRequested += OnPreviousWindowRequested;
            _windowControlSO.BlockChangeRequested += OnBlockChangeRequested;
            _windowControlSO.WindowChangeRequested += OnWindowChangeRequested;
            _windowControlSO.OpenWithoutClosingRequested += OnOpenWithoutClose;
        }

        private void OnDisable()
        {
            _windowControlSO.ReturnToPreviousWindowRequested -= OnPreviousWindowRequested;
            _windowControlSO.BlockChangeRequested -= OnBlockChangeRequested;
            _windowControlSO.WindowChangeRequested -= OnWindowChangeRequested;
            _windowControlSO.OpenWithoutClosingRequested -= OnOpenWithoutClose;
        }

        private void OnOpenWithoutClose(Window obj)
        {
            SwitchWindow(obj, false);
        }

        private void OnPreviousWindowRequested()
        {
            SwitchWindow(_previousWindow, true, false);
        }

        private void OnBlockChangeRequested(bool obj)
        {
            BlockNavigation = obj;
        }

        public void Cancel()
        {
            if (BlockNavigation)
                return;
            if (_currentWindow == null)
                return;
            if (_currentWindow.PreviousWindow != null)
                SwitchWindow(_currentWindow.PreviousWindow);
        }

        public void Apply()
        {
            if (BlockNavigation)
                return;
            if (_selectables != null)
            {
                _selectables[_currentIndex.x, _currentIndex.y].Press();
                SelectionPressed?.Invoke();
            }
        }

        public void Move(Vector2 movement)
        {
            Move((int)movement.x, (int)movement.y);
        }

        public void Move(int x, int y)
        {
            if (BlockNavigation)
                return;
            if (_canMove)
            {
                OffsetIndex(new Vector2Int(x, -y));
                SelectionChanged?.Invoke();
            }
        }
        
        private void OnWindowChangeRequested(Window obj)
        {
            SwitchWindow(obj);
        }

        private void OffsetIndex(Vector2Int offset)
        {
            var newIndex = new Vector2Int();
            newIndex.x = Mathf.Clamp(_currentIndex.x + offset.x, 0, _selectables.GetLength(0) - 1);
            newIndex.y = Mathf.Clamp(_currentIndex.y + offset.y, 0, _selectables.GetLength(1) - 1);
            if (_selectables[newIndex.x, newIndex.y] == null)
                return;
            if (newIndex != _currentIndex)
            {
                _selectables[_currentIndex.x, _currentIndex.y].Deselect();
            }
            _currentIndex = newIndex;
            var currentlySelected = _selectables[_currentIndex.x, _currentIndex.y];
            currentlySelected.Select();
        }

        public void SwitchWindow(Window newWindow, bool close = true, bool rememberPreviousWindow = true)
        {
            if (_rememberedIndices.ContainsKey(newWindow))
            {
                _rememberedIndices[newWindow] = _currentIndex;
            }
            if (close)
            {
                if (_selectables != null)
                {
                    bool whenToNOTDoit = _selectables.GetLength(0) <= _currentIndex.x || _selectables.GetLength(1) <= _currentIndex.y;
                    if (!whenToNOTDoit)
                    {
                        _selectables[_currentIndex.x, _currentIndex.y].Deselect();
                    }
                        
                }
                _currentWindow?.Close();
            }
            _canMove = true;
            _currentIndex = Vector2Int.zero;
            if (newWindow == null)
                return;
            if (rememberPreviousWindow)
                _previousWindow = _currentWindow;
            _currentWindow = newWindow;
            _currentWindow.Open();

            var navRoot = _currentWindow.NavigationRoot;
            if (navRoot == null)
                navRoot = _currentWindow.GetComponent<LayoutGroup>();
            if (navRoot is VerticalLayoutGroup)
            {
                var verticalLayoutGroup = navRoot as VerticalLayoutGroup;
                var selectables = verticalLayoutGroup.GetComponentsInChildren<Selectable>();
                _selectables = new Selectable[1, selectables.Length];

                for (int i = 0; i < _selectables.Length; i++)
                {
                    _selectables[0, i] = selectables[i];
                    selectables[i].Deselect();
                }
            }

            else if (navRoot is HorizontalLayoutGroup)
            {
                var horizontalLayoutGroup = navRoot as HorizontalLayoutGroup;
                var selectables = horizontalLayoutGroup.GetComponentsInChildren<Selectable>();
                _selectables = new Selectable[selectables.Length, 1];
                for (int i = 0; i < selectables.Length; i++)
                {
                    _selectables[i, 0] = selectables[i];
                }
            }

            else if (navRoot is GridLayoutGroup)
            {
                var gridLayoutGroup = navRoot as GridLayoutGroup;
                var selectables = gridLayoutGroup.GetComponentsInChildren<Selectable>();
                var x = selectables.Length;
                var y = selectables.Length;

                switch (gridLayoutGroup.constraint)
                {
                    case GridLayoutGroup.Constraint.Flexible:
                        break;
                    case GridLayoutGroup.Constraint.FixedColumnCount:
                        x = gridLayoutGroup.constraintCount;
                        var rest = selectables.Length % gridLayoutGroup.constraintCount;
                        y = selectables.Length / gridLayoutGroup.constraintCount;
                        if (rest != 0)
                            y++;
                        break;
                    case GridLayoutGroup.Constraint.FixedRowCount:
                        y = gridLayoutGroup.constraintCount;
                        x /= gridLayoutGroup.constraintCount;
                        break;
                }

                #region OldMethod

                _selectables = new Selectable[x, y];
                switch (gridLayoutGroup.constraint)
                {
                    case GridLayoutGroup.Constraint.Flexible:
                        break;
                    case GridLayoutGroup.Constraint.FixedColumnCount:
                        for (int ix = 0; ix < x; ix++)
                        {
                            for (int iy = 0; iy < y; iy++)
                            {
                                var index = ix + (iy * x);
                                if (index >= selectables.Length)
                                    continue;
                                _selectables[ix, iy] = selectables[index];
                            }
                        }
                        //0 - 1 - 2       0 - 1
                        //3 - 4 - 5       2 - 3  
                        //4 - 6 - 7       4 - 5
                        //8 - 9 - 10      6 - 7
                        //all even in ys
                        break;
                    case GridLayoutGroup.Constraint.FixedRowCount:
                        for (int ix = 0; ix < x; ix++)
                        {
                            for (int iy = 0; iy < y; iy++)
                            {
                                var newidx = Mathf.Clamp(ix + iy, 0, selectables.Length);
                                _selectables[ix, iy] = selectables[ix + iy];
                            }
                        }
                        break;
                }
                #endregion

            }
            if (_selectables.Length == 0)
            {
                _canMove = false;
                return;
            }
            Move(0, 0);
        }

        private void ResetSelected(Selectable selected)
        {
            selected.Deselect();
        }

        private void GetColumnAndRow(GridLayoutGroup glg, out int column, out int row)
        {
            column = 0;
            row = 0;

            if (glg.transform.childCount == 0)
                return;

            //Column and row are now 1
            column = 1;
            row = 1;

            //Get the first child GameObject of the GridLayoutGroup
            RectTransform firstChildObj = glg.transform.
                GetChild(0).GetComponent<RectTransform>();

            Vector2 firstChildPos = firstChildObj.anchoredPosition;
            bool stopCountingRow = false;

            //Loop through the rest of the child object
            for (int i = 1; i < glg.transform.childCount; i++)
            {
                //Get the next child
                RectTransform currentChildObj = glg.transform.
               GetChild(i).GetComponent<RectTransform>();

                Vector2 currentChildPos = currentChildObj.anchoredPosition;

                //if first child.x == otherchild.x, it is a column, ele it's a row
                if (firstChildPos.x == currentChildPos.x)
                {
                    column++;
                    //Stop couting row once we find column
                    stopCountingRow = true;
                }
                else
                {
                    if (!stopCountingRow)
                        row++;
                }
            }
        }
    }

}

