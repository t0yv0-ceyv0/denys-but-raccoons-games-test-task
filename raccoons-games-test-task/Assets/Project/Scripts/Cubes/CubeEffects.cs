using UnityEngine;

namespace Project.Cubes
{
    [RequireComponent(typeof(CubeMerge))]
    public class CubeEffects : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private ParticleSystem _mergeEffectPrefab;

        [Header("SFX")]
        [SerializeField] private AudioClip _mergeClip;
        [SerializeField] private AudioSource _audioSource;

        private CubeMerge _merge;
        private CubeView _view;

        #region Unity Lifecycle
        private void Awake()
        {
            _merge = GetComponent<CubeMerge>();
            _view = GetComponent<CubeView>();
        }

        private void OnEnable()
        {
            _merge.OnMergeAtPosition.AddListener(PlayMergeEffects);
        }

        private void OnDisable()
        {
            _merge.OnMergeAtPosition.RemoveListener(PlayMergeEffects);
        }
        #endregion

        #region Effects
        private void PlayMergeEffects(Vector3 position)
        {
            PlayVfx(position);
            PlaySfx();
        }

        private void PlayVfx(Vector3 position)
        {
            if (_mergeEffectPrefab == null) return;

            var effect = Instantiate(_mergeEffectPrefab, position, Quaternion.identity);

            if (_view != null)
            {
                var main = effect.main;
                main.startColor = _view.CurrentColor;
            }

            effect.Play();

            var mainModule = effect.main;
            Destroy(effect.gameObject, mainModule.duration + mainModule.startLifetime.constantMax);
        }

        private void PlaySfx()
        {
            if (_audioSource == null) return;

            _audioSource.PlayOneShot(_mergeClip);
        }
        #endregion

    }
}
