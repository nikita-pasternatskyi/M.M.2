using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player2D.Input
{
    public interface IMovementInputProvider2D 
    {
        Vector2 AbsoluteInput { get; }
    }
}
