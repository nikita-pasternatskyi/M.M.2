using System;
using System.Collections;
using UnityEngine;

namespace MykroFramework.Runtime.GameProgress
{

    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private GameGlobals _gameGlobals;
        [SerializeField] private Vector3 _size = new Vector3(1, 1, 0.5f);
        [SerializeField] private bool _spawnOnAwake = true;
        private Color _redColor = new Color(1, 0, 0, 0.25f);
        private Color _greenColor = new Color(0, 1, 0, 0.25f);

        public event Action<GameObject> PlayerSpawned; 

        private void Awake()
        {
            if (_spawnOnAwake)
                Spawn();
        }

        public void Spawn()
        {
            PlayerSpawned?.Invoke(Instantiate(_gameGlobals.PlayerPrefab, transform.position, transform.rotation));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _greenColor;
            if (Physics.CheckBox(transform.position, _size * 0.5f))
                Gizmos.color = _redColor;
            Gizmos.DrawCube(transform.position, _size);
        }
    }
}