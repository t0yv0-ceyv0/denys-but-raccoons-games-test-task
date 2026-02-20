using UnityEngine;
using UnityEngine.Events;

namespace Project.Cubes
{
    [RequireComponent(typeof(CubeBase))]
    public class CubeMerge : MonoBehaviour
    {
        [Header("Merge Settings")]
        [SerializeField] private float _minMergeImpulse = 1.5f;
        [SerializeField] private float _mergeForce = 2;

        [Header("Events")]
        public UnityEvent<int> OnMergedNewValue;
        public UnityEvent<Vector3> OnMergeAtPosition;

        private CubeBase _cube;

        #region Unity Lifecycle
        private void Awake()
        {
            _cube = GetComponent<CubeBase>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent<CubeBase>(out CubeBase cubeCollision)) return;

            if (cubeCollision.Value != _cube.Value) return;

            if (collision.impulse.magnitude < _minMergeImpulse) return;

            if (gameObject.GetInstanceID() > collision.gameObject.GetInstanceID()) return;

            MergeImpact();

            int newValue = _cube.Value * 2;
            _cube.ApplyMergeResult(newValue);

            OnMergedNewValue?.Invoke(newValue);
            OnMergeAtPosition?.Invoke(transform.position);

            Destroy(collision.gameObject);

        }
        #endregion

        #region Internals
        private void MergeImpact() 
        {
            if (_cube._rigidBody == null) return;

            _cube._rigidBody.linearVelocity = Vector3.zero;
            _cube._rigidBody.angularVelocity = Vector3.zero;

            _cube._rigidBody.AddForce(Vector3.up * _mergeForce, ForceMode.Impulse);

        }
        #endregion

    }
}