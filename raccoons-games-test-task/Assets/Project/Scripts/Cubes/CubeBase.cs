using Project.Interfaces;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cubes 
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeBase : MonoBehaviour, IInteractable
    {
        [Header("State settings")]
        [SerializeField] private float _setOnBoardDelay = 1.5f;

        [Header("Physics")]
        [SerializeField] private float _pushForce = 20f;

        [Header("Events")]
        public UnityEvent<int> OnValueChanged;
        public UnityEvent<CubeState> OnStateChanged;
        public UnityEvent OnPushed;

        public CubeState CurrentState { get; private set; } = CubeState.Launching;
        public int Value { get; private set; }

        public Rigidbody _rigidBody { get; private set; }

        public Transform Transform => transform;

        private Coroutine _stateCoroutine;

        #region Unity Lifecycle
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        #endregion

        #region Api
        public void Init(int cubeValue) 
        {
            SetValue(cubeValue);
        }

        public void Push()
        {
            _rigidBody.AddForce(Vector3.forward * _pushForce, ForceMode.Impulse);
            OnPushed?.Invoke();

            StartSetOnBoardTimer();
        }

        public void ApplyMergeResult(int newValue)
        {
            SetValue(newValue);
        }

        public void SetState(CubeState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
        }

        #endregion

        #region Internals
        private void SetValue(int newValue)
        {
            if (Value == newValue) return;

            Value = newValue;
            OnValueChanged?.Invoke(Value);
        }

        private void StartSetOnBoardTimer()
        {
            if (_stateCoroutine != null)
                StopCoroutine(_stateCoroutine);

            _stateCoroutine = StartCoroutine(SetOnBoardAfterDelay());
        }

        private IEnumerator SetOnBoardAfterDelay()
        {
            yield return new WaitForSeconds(_setOnBoardDelay);
            SetState(CubeState.OnBoard);
            _stateCoroutine = null;
        }
        #endregion

    }

    public enum CubeState{ Launching, OnBoard }
}
