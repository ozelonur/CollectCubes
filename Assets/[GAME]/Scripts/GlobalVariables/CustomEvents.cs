#region Header

// Developed by Onur ÖZEL

#endregion

namespace _GAME_.Scripts.GlobalVariables
{
    /// <summary>
    /// This class is used to store Custom Events that you create.
    /// </summary>
    public class CustomEvents
    {
        public const string CollectorCanMove = nameof(CollectorCanMove);
        public const string ActivateAllCubes = nameof(ActivateAllCubes);
        public const string DecreaseCubeCount = nameof(DecreaseCubeCount);
        public const string RespawnCubes = nameof(RespawnCubes);
        public const string UpdateCollectedCubeCount = nameof(UpdateCollectedCubeCount);
        public const string UpdateTimer = nameof(UpdateTimer);
        public const string ResetCubeCounts = nameof(ResetCubeCounts);
    }
}