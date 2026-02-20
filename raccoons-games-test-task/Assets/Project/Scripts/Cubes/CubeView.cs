using TMPro;
using UnityEngine;

namespace Project.Cubes
{
    [RequireComponent(typeof(CubeBase))]
    [RequireComponent(typeof(Renderer))]
    public class CubeView : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TextMeshPro[] _texts;

        [Header("Color")]
        [SerializeField] private float _saturation = 0.7f;
        [SerializeField] private float _value = 0.9f;

        public Color CurrentColor { get; private set; }

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private CubeBase _cube;
        private Renderer _renderer;
        private MaterialPropertyBlock _mpb;

        #region Unity Lifecycle
        private void Awake()
        {
            _cube = GetComponent<CubeBase>();
            _renderer = GetComponent<Renderer>();
            _mpb = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            _cube.OnValueChanged.AddListener(Refresh);
            Refresh(_cube.Value);
        }

        private void OnDisable()
        {
            _cube.OnValueChanged.RemoveListener(Refresh);
        }

        #endregion

        #region Visuals
        private void Refresh(int newValue)
        {
            SetText(newValue);
            SetColor(newValue);
        }

        private void SetText(int value)
        {
            if (_texts == null) return;

            string stringValue = value.ToString();

            foreach (var tmp in _texts)
                if (tmp != null) tmp.text = stringValue;
        }

        private void SetColor(int value)
        {
            CurrentColor = GetColorForValue(value);

            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(BaseColorId, CurrentColor);
            _renderer.SetPropertyBlock(_mpb);
        }

        private Color GetColorForValue(int value)
        {
            UnityEngine.Random.InitState(value);

            float randomHue = UnityEngine.Random.value;
            return Color.HSVToRGB(randomHue, _saturation, _value);
        }

        #endregion

    }
}