using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Managers
{
    public class GameLoopManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private CubeSpawner _spawner;
        [SerializeField] private GameFieldManager _fieldManager;
        [SerializeField] private ScoreManager _scoreManager;

        [Header("Inputs")]
        [SerializeField] private InputActionAsset _actionAsset;

        #region Unity Lifecycle
        public void Start()
        {
            StartNewGame();
        }
        #endregion

        #region Api

        public void StartNewGame() 
        {
            _fieldManager.ClearGameField();
            _spawner.StartSpawnTimer();
            _scoreManager.ResetScore();
            _actionAsset?.Enable();
        }

        public void GameOver() 
        {
            _actionAsset?.Disable();
        }

        #endregion

    }
}