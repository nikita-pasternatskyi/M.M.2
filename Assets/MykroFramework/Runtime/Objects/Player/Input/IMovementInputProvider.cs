using MykroFramework.Runtime.Controls;
using System.Collections;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Input
{
    public interface IMovementInputProvider
    {
        public Vector3 RelativeInput { get; }
        public Vector2 AbsoluteInput { get; }
    }
}