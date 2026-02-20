using DG.Tweening;
using Project.Cubes;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Project.Boosters 
{
    public class AutoMergeEffects : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private ParticleSystem _superMergeEffectPrefab;

        [Header("SFX")]
        [SerializeField] private AudioClip _superMergeClip;
        [SerializeField] private AudioClip _superMargePrepareClip;
        [SerializeField] private AudioSource _audioSource;

        [Header("Camera Shake Settings")]
        [SerializeField] private float _shakeDuration = 99f;
        [SerializeField] private float _shakeStrength = 0.2f;
        [SerializeField] private int _vibrato = 10;

        private Camera _mainCamera;
        private Tween _shakeTween;

        #region Unity Lifecycle
        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        #endregion

        #region Api
        public void PlayVFX(Vector3 position, Color color) 
        {
            if (_superMergeEffectPrefab == null) return;
            
            var effect = Instantiate(_superMergeEffectPrefab, position, Quaternion.identity);

            var main = effect.main;
            main.startColor = color;

            effect.Play();

            Destroy(effect.gameObject, main.duration + main.startLifetime.constantMax);

        }

        public void PlayPrepareSFX()
        {
            if (_audioSource == null || _superMargePrepareClip == null) return;

            _audioSource.PlayOneShot(_superMargePrepareClip);
        }

        public void PlayMergeSFX()
        {
            if (_audioSource == null || _superMergeClip == null) return;

            _audioSource.Stop();
            _audioSource.PlayOneShot(_superMergeClip);
        }

        public void StartCameraShake() 
        {
            if (_mainCamera == null) return;

            _shakeTween?.Kill(true);

            _shakeTween = _mainCamera.transform.DOShakePosition(_shakeDuration, _shakeStrength, _vibrato).SetAutoKill(false);
        }

        public void EndCameraShake() 
        {
            if (_mainCamera == null || _shakeTween == null) return;

            _shakeTween.Kill(true);
            _shakeTween = null;
        }

        #endregion
    }
}
