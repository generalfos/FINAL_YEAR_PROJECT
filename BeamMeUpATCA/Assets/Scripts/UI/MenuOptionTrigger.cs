using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeamMeUpATCA
{
    public class MenuOptionTrigger : EventTrigger
    {
        public Button Button { get; set; }
        public TextMeshProUGUI Text { get; set; }

        private static readonly Color HoverFontColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        private const float HoverSpacing = 0f;
        private const float HoverFont = 0.8f;
        private const float AnimationSpeed = 1.5f;

        private float _initialFontSize;
        private float _fontSize;

        private float _initialCharacterSpacing;
        private float _characterSpacing;

        private float _initialButtonSize;
        private float _buttonSize;

        private Color _initalColor;
        private Color _color;

        private bool _setup;

        private void Awake()
        {
            if (!Text || !Button) return;
            _setup = true;

            _initialCharacterSpacing = Text.characterSpacing;
            _initialFontSize = Text.fontSize;
            _initalColor = Text.color;
        }

        public override void OnPointerEnter(PointerEventData eventData) => PointerEnter();
        public void PointerEnter()
        {
            if (!Text || !Button) return;

            _characterSpacing = HoverSpacing;
            _fontSize = _initialFontSize * HoverFont;
            _color = HoverFontColor;
            Text.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);


            _interpolationValue = 0.3f;
        }

        public override void OnPointerExit(PointerEventData eventData) => PointerExit();
        public void PointerExit()
        {
            if (!Text || !Button) return;

            _characterSpacing = _initialCharacterSpacing;
            _fontSize = _initialFontSize;
            _color = _initalColor;
            Text.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 1f);

            _interpolationValue = 0.3f;
        }

        private float _interpolationValue = 1f;

        private void Update()
        {
            if (!_setup) Awake();

            if (_interpolationValue < 1f)
            {
                _interpolationValue += Time.deltaTime * AnimationSpeed;
                // Smooth out transitions
                Text.characterSpacing = Mathf.Lerp(Text.characterSpacing, _characterSpacing, _interpolationValue);
                Text.fontSize = Mathf.Lerp(Text.fontSize, _fontSize, _interpolationValue);
                Text.color = Color.Lerp(Text.color, _color, _interpolationValue);
            }
        }
    }
}