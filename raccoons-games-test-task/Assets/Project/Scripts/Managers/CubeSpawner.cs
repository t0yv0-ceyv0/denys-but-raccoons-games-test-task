using UnityEngine;
using Project.Cubes;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Project.Managers 
{
    public class CubeSpawner : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private ScoreManager _scoreManager;

        [Header("Spawn Settings")]
        [SerializeField] private Transform _cubeSpawnPosition;
        [SerializeField] private float _timeToSpawnCube = 1f;
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private List<CubesSpawnConfig> _spawnPresets = new List<CubesSpawnConfig>();

        [Header("Events")]
        public UnityEvent<CubeBase> OnCubeSpawned;

        private Coroutine _spawnCoroutine;

        #region Api
        public void StartSpawnTimer()
        {
            if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = StartCoroutine(SpawnTimerCoroutine());
        }
        #endregion

        #region Spawning
        private IEnumerator SpawnTimerCoroutine()
        {
            yield return new WaitForSeconds(_timeToSpawnCube);
            SpawnCube();
        }

        private void SpawnCube()
        {
            if (_spawnPresets == null || _spawnPresets.Count == 0)
            {
                return;
            }

            CubesSpawnConfig spawnPreset = SpawnPresetSelector.GetRandomPreset(_spawnPresets);

            var cubeInstance = Instantiate(cubePrefab, _cubeSpawnPosition.position, Quaternion.identity);
            
            if (cubeInstance.TryGetComponent<CubeBase>(out CubeBase cubeBase))
            {
                if (cubeInstance.TryGetComponent<CubeMerge>(out CubeMerge mergeLogic))
                {
                    if (_scoreManager == null) return;

                    mergeLogic.OnMergedNewValue.AddListener(_scoreManager.AddScore);
                }

                cubeBase.Init(spawnPreset.cubeValue);
                OnCubeSpawned?.Invoke(cubeBase);
            }
        }
        #endregion

    }

    [Serializable]
    public struct CubesSpawnConfig 
    {
        [Range(1f, 100f)]
        public float spawnChance;

        public int cubeValue; 
    }
}
