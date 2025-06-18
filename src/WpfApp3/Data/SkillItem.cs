using Prism.Mvvm;
using WpfApp3.Common;

namespace WpfApp3.Data
{
    public class SkillItem : BindableBase
    {
        private AtkType atkType;
        public AtkType AtkType
        {
            get { return atkType; }
            set
            {
                SetProperty(ref atkType, value);
            }
        }
        private DamageType damageType;
        public DamageType DamageType
        {
            get { return damageType; }
            set
            {
                SetProperty(ref damageType, value);
            }
        }
        private string targets = "1";
        public string Targets
        {
            get { return targets; }
            set
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    SetProperty(ref targets, number.ToString());
                }
                else
                {
                    SetProperty(ref targets, "0");
                }

            }
        }

        private string damageTimes = "1";
        public string DamageTimes
        {
            get { return damageTimes; }
            set
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    SetProperty(ref damageTimes, number.ToString());
                }
                else
                {
                    SetProperty(ref damageTimes, "0");
                }

            }
        }
        private string skillNum = "0";
        public string SkillNum
        {
            get { return skillNum; }
            set
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    SetProperty(ref skillNum, number.ToString());
                }
                else
                {
                    SetProperty(ref skillNum, "0");
                }
            }
        }

        private bool bindLastDamage = false;
        public bool BindLastDamage
        {
            get { return bindLastDamage; }
            set
            {
                SetProperty(ref bindLastDamage, value);
            }
        }
        private bool notCritable = false;
        public bool NotCritable
        {
            get { return notCritable; }
            set
            {
                SetProperty(ref notCritable, value);
            }
        }

    }
}
