using _GAME_.Scripts.Models;
using _ORANGEBEAR_.Scripts.Enums;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ORANGEBEAR_.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Custom Level Data", menuName = "Level/Custom Level Data", order = 1)]
    public class CustomLevelData : ScriptableObject
    {
        #region Serialized Fields

        [BoxGroup("Level Data")] [SerializeField]
        private LevelType levelType;

        [ShowIf("IsNormalLevel")] [BoxGroup("Level Data")] [SerializeField]
        private Texture2D levelReference;

        [ShowIf("IsNormalLevel")] [BoxGroup("Level Data")] [SerializeField]
        private float scale = .15f;

        [ShowIf("IsTimeLevel")] [BoxGroup("Level Data")] [SerializeField]
        private float levelTime;
        
        [ShowIf("IsTimeLevel")] [BoxGroup("Level Data")] [SerializeField]
        private TextureAndScaleData[] textureAndScaleDatas;

        #endregion

        #region Properties

        public LevelType LevelType => levelType;
        public Texture2D LevelReference => levelReference;
        public TextureAndScaleData[] TextureAndScaleDatas => textureAndScaleDatas;
        public float LevelTime => levelTime;
        public float Scale => scale;

        #endregion

        #region Check for Visibility Methods

        [UsedImplicitly]
        private bool IsTimeLevel()
        {
            return levelType == LevelType.Time;
        }

        [UsedImplicitly]
        private bool IsNormalLevel()
        {
            return levelType == LevelType.Normal;
        }

        #endregion
    }
}