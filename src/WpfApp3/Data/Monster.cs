using Prism.Mvvm;

namespace WpfApp3.Data
{
    /// <summary>
    /// 怪物信息显示
    /// </summary>
    public class Monster : BindableBase
    {
        private string def = "0";
        public string Def
        {
            get { return def; }
            set
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    SetProperty(ref def, number.ToString());
                }
                else
                {
                    SetProperty(ref def, "0");
                }
            }
        }
        private string blood = "250000";
        public string Blood
        {
            get { return blood; }
            set
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    SetProperty(ref blood, number.ToString());
                }
                else
                {
                    SetProperty(ref blood, "0");
                }
            }
        }
        private string level = "90";
        public string Level
        {
            get { return level; }
            set
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    SetProperty(ref level, number.ToString());
                }
                else
                {
                    SetProperty(ref level, "90");
                }
            }
        }
    }
}
