#region Header

// Developed by Onur ÖZEL

#endregion

using System.Collections;
using _ORANGEBEAR_.EventSystem;
using _ORANGEBEAR_.Scripts.Managers;
using UnityEngine;

namespace _ORANGEBEAR_.Scripts.Bears
{
    public class LevelBear : Bear
    {
        #region Serialized Fields

        [Header("Skybox")] [SerializeField] private Material skybox;

        #endregion

        #region Public Methods

        public virtual void InitLevel()
        {
            RenderSettings.skybox = skybox;
            StartCoroutine(Delay());
        }

        #endregion

        #region Private Methods

        private IEnumerator Delay()
        {
            yield return null;
            Roar(GameEvents.InitLevel);
            GameManager.Instance.currentLevel = this;
        }

        #endregion
    }
}