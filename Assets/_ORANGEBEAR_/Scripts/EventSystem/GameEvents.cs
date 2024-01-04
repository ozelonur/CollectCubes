#region Header

// Developed by Onur ÖZEL

#endregion

namespace _ORANGEBEAR_.EventSystem
{
    /// <summary>
    /// This class is used to store general Game Events.
    /// </summary>
    public static class GameEvents
    {
        #region UI Events

        public const string ActivatePanel = nameof(ActivatePanel);

        #endregion

        #region General Game Events

        public const string InitLevel = nameof(InitLevel);
        public const string OnGameStart = nameof(OnGameStart);
        public const string OnGameComplete = nameof(OnGameComplete);
        public const string GetLevelNumber = nameof(GetLevelNumber);
        public const string NextLevel = nameof(NextLevel);

        #endregion
    }
}