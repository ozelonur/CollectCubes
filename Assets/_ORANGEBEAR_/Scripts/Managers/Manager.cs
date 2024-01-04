using _ORANGEBEAR_.EventSystem;
using UnityEngine;

namespace _ORANGEBEAR_.Scripts.Managers
{
    public class Manager<T> : Bear where T : Bear
    {
        #region Private Variables

        private static T _instance;

        #endregion

        #region Properties

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError("There is no " + typeof(T) + " in the scene!");
                }

                return _instance;
            }
        }

        #endregion
    }
}