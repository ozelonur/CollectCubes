#region Header
// Developed by Onur ÖZEL
#endregion

using _ORANGEBEAR_.Scripts.Bears;
using _ORANGEBEAR_.Scripts.Enums;
using UnityEngine;

namespace _GAME_.Scripts.Bears
{
    public class GameLevelBear : LevelBear
    {
        [HideInInspector] public LevelType levelType;
        [HideInInspector] public float levelTime;
    }
}