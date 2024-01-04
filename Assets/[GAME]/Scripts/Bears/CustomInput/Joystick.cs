using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _GAME_.Scripts.Bears.CustomInput
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        protected RectTransform background;

        [SerializeField] private RectTransform handle;

        #endregion

        #region Private Variables

        protected JoystickSettingsScriptableObject _joystickSettings;

        private RectTransform _rectTransform;

        private Canvas _canvas;
        private Camera _camera;
        private Vector2 _input;

        #endregion

        #region Properties

        public float Horizontal => _input.x;
        public float Vertical => _input.y;

        public Vector2 Direction => new Vector2(Horizontal, Vertical);

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _joystickSettings = Resources.Load<JoystickSettingsScriptableObject>(FolderPaths.JoystickSettings);

            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        protected virtual void Start()
        {
            if (_canvas == null)
            {
                Debug.LogError("Joystick must be placed on a canvas!!");
            }

            Vector2 center = new Vector2(0.5f, 0.5f);

            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

        #endregion

        #region Event System Methods

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _camera = null;

            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _camera = _canvas.worldCamera;
            }

            Vector2 position = RectTransformUtility.WorldToScreenPoint(_camera, background.position);
            Vector2 radius = background.sizeDelta / 2;

            _input = (eventData.position - position) / (radius * _canvas.scaleFactor);

            FormatInput();
            HandleInput(_input.magnitude, _input.normalized);

            handle.anchoredPosition = _input * radius * _joystickSettings.handleRange;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        #endregion

        #region Private Methods

        private void FormatInput()
        {
            _input = _joystickSettings.axisOptions switch
            {
                AxisOptions.Horizontal => new Vector2(_input.x, 0f),
                AxisOptions.Vertical => new Vector2(0f, _input.y),
                _ => _input
            };
        }

        protected void HandleInput(float magnitude, Vector2 normalized)
        {
            if (magnitude > _joystickSettings.deadZone)
            {
                if (magnitude > 1)
                {
                    _input = normalized;
                }
            }

            else
            {
                _input = Vector2.zero;
            }
        }

        #endregion

        #region Protected Methods

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPosition, _camera,
                    out Vector2 localPoint)) return Vector2.zero;

            Vector2 sizeDelta;
            Vector2 pivotOffset = _rectTransform.pivot * (sizeDelta = _rectTransform.sizeDelta);

            return localPoint - background.anchorMax * sizeDelta + pivotOffset;
        }

        #endregion
    }
}