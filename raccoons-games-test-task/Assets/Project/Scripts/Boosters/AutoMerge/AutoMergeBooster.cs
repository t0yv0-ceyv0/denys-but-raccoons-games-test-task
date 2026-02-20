using DG.Tweening;
using Project.Cubes;
using Project.Managers;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Boosters 
{
    [RequireComponent(typeof(AutoMergeEffects))]
    public class AutoMergeBooster : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameFieldManager _fieldManager;

        [Header("Settings")]
        [SerializeField] private float _riseHeight = 5f;

        private AutoMergeEffects _effects;

        #region Unity Lifecycle
        private void Awake()
        {
            _effects = GetComponent<AutoMergeEffects>();
        }
        #endregion

        #region Api

        public async Task ExecuteAutoMerge() 
        {
            var cubes = _fieldManager.CubesContainer.GetComponentsInChildren<CubeBase>().Where(cube => cube.CurrentState == CubeState.OnBoard).GroupBy(cube => cube.Value).FirstOrDefault(group => group.Count() >= 2);

            if (cubes == null) return;

            var cubeA = cubes.ElementAt(0);
            var cubeB = cubes.ElementAt(1);

            PrepareForAnimation(cubeA, cubeB);

            Vector3 midPoint = (cubeA.transform.position + cubeB.transform.position) / 2f + Vector3.up * _riseHeight;

            var riseSequence = DOTween.Sequence();
            riseSequence.Join(cubeA.transform.DOMove(cubeA.transform.position + Vector3.up * _riseHeight, 0.5f).SetEase(Ease.OutBack));
            riseSequence.Join(cubeB.transform.DOMove(cubeB.transform.position + Vector3.up * _riseHeight, 0.5f).SetEase(Ease.OutBack));

            _effects.PlayPrepareSFX();

            await riseSequence.AsyncWaitForCompletion();


            Vector3 backA = cubeA.transform.position + (cubeA.transform.position - midPoint).normalized * 0.8f;
            Vector3 backB = cubeB.transform.position + (cubeB.transform.position - midPoint).normalized * 0.8f;

            var swingSequence = DOTween.Sequence();
            swingSequence.Join(cubeA.transform.DOMove(backA, 0.3f).SetEase(Ease.OutQuad));
            swingSequence.Join(cubeB.transform.DOMove(backB, 0.3f).SetEase(Ease.OutQuad));

            _effects.StartCameraShake();

            await swingSequence.AsyncWaitForCompletion();

            var hitSequence = DOTween.Sequence();
            hitSequence.Join(cubeA.transform.DOMove(midPoint, 0.2f).SetEase(Ease.InQuint));
            hitSequence.Join(cubeB.transform.DOMove(midPoint, 0.2f).SetEase(Ease.InQuint));

            await hitSequence.AsyncWaitForCompletion();

            FinalizeMerge(cubeA, cubeB, midPoint);

        }

        #endregion

        #region Internals

        private void FinalizeMerge(CubeBase a, CubeBase b, Vector3 position)
        {
            int newValue = a.Value * 2;
            a.transform.position = position;

            a._rigidBody.isKinematic = false;
            a._rigidBody.linearVelocity = Vector3.zero;

            if (a.TryGetComponent<Collider>(out var col)) col.enabled = true;

            a.ApplyMergeResult(newValue);
            if (a.TryGetComponent<CubeMerge>(out CubeMerge merge)) merge.OnMergedNewValue?.Invoke(newValue);
            a.SetState(CubeState.OnBoard);

            Color mergeColor = Color.red;

            if (a.TryGetComponent<CubeView>(out CubeView view)) 
            {
                mergeColor = view.CurrentColor;
            }

            _effects.EndCameraShake();
            _effects.PlayVFX(position, mergeColor);
            _effects.PlayMergeSFX();

            Destroy(b.gameObject);
        }

        private void PrepareForAnimation(params CubeBase[] cubes) 
        {
            foreach (var cube in cubes)
            {
                cube.SetState(CubeState.Launching);

                cube._rigidBody.isKinematic = true;
                if (cube.TryGetComponent<Collider>(out Collider collider)) collider.enabled = false;
            }
        }
        #endregion
    }
}

