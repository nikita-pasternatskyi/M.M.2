using System;
using UnityEngine;

namespace MykroFramework.Runtime.UI
{
    [CreateAssetMenu(menuName = "Game/UI/Window Control SO")]
    public class WindowControlSO : ScriptableObject
    {
        public event Action<Window> OpenWithoutClosingRequested;
        public event Action ReturnToPreviousWindowRequested;
        public event Action<Window> WindowChangeRequested;
        public event Action<bool> BlockChangeRequested;

        public void BlockNavigation(bool block)
        {
            BlockChangeRequested?.Invoke(block);
        }

        public void ChangeWindow(Window newWindow)
        {
            WindowChangeRequested?.Invoke(newWindow);
        }

        public void OpenWindowWithoutClosing(Window window)
        {
            OpenWithoutClosingRequested?.Invoke(window);
        }

        public void ReturnToPreviousWindow()
        {
            ReturnToPreviousWindowRequested?.Invoke();
        }
    }

}

