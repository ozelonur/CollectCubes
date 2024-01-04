using UnityEngine;

namespace _GAME_.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Collector Movement Settings", menuName = "Settings/Collector Movement Settings", order = 1)]
    public class CollectorSettingsScriptableObject : ScriptableObject
    {
        public float speed;
        public float rotationSpeed;
        public float responseThreshold = .15f;
        public float movementDelay;
    }
}