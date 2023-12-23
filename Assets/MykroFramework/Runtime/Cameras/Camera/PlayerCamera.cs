using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Camera
{
    public class PlayerCamera : MonoBehaviour
    {
        public PlayerCharacter SpiderMan;
        public Transform FollowTarget;
        public UnityEngine.Camera Camera;
        public SpringArm.SpringArm SpringArm;
    }
}