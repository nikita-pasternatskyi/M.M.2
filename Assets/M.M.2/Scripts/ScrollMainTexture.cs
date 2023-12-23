using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Misc
{
    public class ScrollMainTexture : MonoBehaviour
    {        
        [SerializeField] private Material _material;
        [SerializeField] private Vector2 _speed;

        private void Start()
        {
            Timing.RunCoroutine(Scroll());
        }

        private IEnumerator<float> Scroll()
        {
            Vector2 scroll = Vector2.zero;
            while (true)
            {
                scroll += _speed * Time.deltaTime;
                _material.SetTextureOffset("_MainTex", scroll);
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}
