#region Header

// Developed by Onur ÖZEL

#endregion

using _ORANGEBEAR_.Scripts.Managers;
using UnityEngine;

namespace _ORANGEBEAR_.EventSystem
{
    public abstract class Bear : MonoBehaviour
    {
        #region Properties

        public BearManager Manager => BearManager.Instance;

        #endregion

        #region MonoBehaviour Methods

        protected virtual void OnEnable()
        {
            CheckRoarings(true);
        }

        protected virtual void OnDisable()
        {
            CheckRoarings(false);
        }

        #endregion

        #region Protected Methods

        protected virtual void CheckRoarings(bool status)
        {
        }

        protected virtual void Roar(string roarName, params object[] args)
        {
            Manager.Roar(roarName, args);
        }

        protected virtual void Register(string roarName, Roaring roaring)
        {
            Manager.Register(roarName, roaring);
        }

        protected virtual void UnRegister(string roarName, Roaring roaring)
        {
            Manager.Unregister(roarName, roaring);
        }

        #endregion
    }
}