using _ORANGEBEAR_.Scripts.Entity;

namespace _GAME_.Scripts.Entities
{
    public class MoneyData : Entity<MoneyData>
    {
        #region Public Variables

        public int Money;

        #endregion
        protected override bool Init() => true;

        #region Public Methods

        public void AddMoney(int amount)
        {
            Money += amount;
            Save();
        }
        
        public void SubtractMoney(int amount)
        {
            Money -= amount;
            Save();
        }

        #endregion
    }
}