using Project.Configs;
using Project.Cubes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Project.Player 
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private PlayerInputsConfig _inputsConfigs;

        [Header("Movement Limits")]
        [SerializeField] private Vector2 _maxCubeOffset;

        [Header("Events")]
        public UnityEvent OnCubeReleased;

        private Camera _mainCamera;
        private CubeBase _cube;

        private InputActionReference _dragAction;
        private InputActionReference _positionAction;

        private Vector2 _touchPosition;
        private bool _isDragging;

        #region Unity Lifecycle
        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null) 
            {
                Debug.LogWarning("[PlayerController] Main Camera not found");
            }
        }

        private void OnEnable()
        {
            SetupActions();
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
        #endregion

        #region Draging
        private void OnDragStarted(InputAction.CallbackContext context) 
        {
            _isDragging = true;

            if (_cube != null) 
            {
                _touchPosition = _positionAction.action.ReadValue<Vector2>();
                MoveCube(_touchPosition);
            }
        }

        private void OnDragEnded(InputAction.CallbackContext context) 
        {
            if (!_isDragging) return;

            _isDragging=false;
            if (_cube != null)
            {
                _cube.Push();
                OnCubeReleased?.Invoke();
                _cube = null;
            }
        }

        private void OnPosition(InputAction.CallbackContext context) 
        {
            if (_isDragging && _cube != null) 
            {
                _touchPosition = context.ReadValue<Vector2>();

                MoveCube(_touchPosition);
            }
        }

        private void MoveCube(Vector2 screenPosition) 
        {
            Vector3 touchPosition = new Vector3(screenPosition.x, screenPosition.y, _mainCamera.transform.position.y);
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);

            float clampedX = Mathf.Clamp(worldPosition.x, _maxCubeOffset.x, _maxCubeOffset.y);
            _cube.Transform.position = new Vector3(clampedX, _cube.Transform.position.y, _cube.Transform.position.z);
        }
        #endregion

        #region Inputs
        private void SetupActions() 
        {
            _dragAction = _inputsConfigs.dragAction;
            _positionAction = _inputsConfigs.positionAction;
        }
        
        private void Subscribe() 
        {
            if (_dragAction != null) 
            {
                _dragAction.action.Enable();
                _dragAction.action.performed += OnDragStarted;
                _dragAction.action.canceled += OnDragEnded;
            }
            else 
            {
                Debug.LogWarning("[PlayerController] _dragAction not assigned");
            }

            if (_positionAction != null)
            {
                _positionAction.action.Enable();
                _positionAction.action.performed += OnPosition;
            }
            else
            {
                Debug.LogWarning("[PlayerController] _positionAction not assigned");
            }

        }

        private void Unsubscribe()
        {
            if (_dragAction != null)
            {
                _dragAction.action.performed -= OnDragStarted;
                _dragAction.action.canceled -= OnDragEnded;

                _dragAction.action.Disable();
            }

            if (_positionAction != null)
            {
                _positionAction.action.performed -= OnPosition;
                _positionAction.action.Disable();
            }

        }
        #endregion

        #region Api
        public void SetCube(CubeBase cube) 
        {
            _cube = cube;
        }
        #endregion
    }
}
