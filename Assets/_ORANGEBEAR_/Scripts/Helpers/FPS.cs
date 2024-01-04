#region Header
// Developed by Onur ÖZEL
#endregion

using UnityEngine;

namespace _ORANGEBEAR_.Scripts.Helpers
{
    public class FPS : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private bool showFPS = true;

        #endregion

        #region Private Variables

        private float _deltaTime;

        #endregion

        #region MonoBehaviour Methods

        private void Update() => _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * .1f;

        #endregion

        #region GUI Methods

        private void OnGUI()
        {
            if (!showFPS)
            {
                return;
            }
            
            int w = Screen.width, h = Screen.height;
            
            GUIStyle style = new GUIStyle();
            
            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float mSec = _deltaTime * 1000.0f;
            float fps = 1.0f / _deltaTime;
            string text = $"{mSec:0.0} ms ({fps:0.} fps)";
            GUI.Label(rect, text, style);
        }

        #endregion
    }
}