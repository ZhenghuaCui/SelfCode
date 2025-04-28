using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using WpfApp3.Data;
using Wuhua.Model;

namespace WpfApp3.Common
{
    public static partial class CommonStaticSource
	{
		#region dic resource
		public static Dictionary<Occupation, string> OccupationDic = new Dictionary<Occupation, string>()
		{
			{ Occupation.Solder,"轻锐" },
			{ Occupation.Shooter,"远击" },
			{ Occupation.Tank,"宿卫" },
			{ Occupation.Witch,"构术" },
			{ Occupation.Guard,"战略" },
		};
		public static Dictionary<RoleProEnm, string> RoleProDic = new Dictionary<RoleProEnm, string>()
		{
			{ RoleProEnm.Deep,"深造" },
			{ RoleProEnm.Life,"致知" },
			{ RoleProEnm.RoleType,"角色类型" },
			{ RoleProEnm.Weapon,"武器" },
			{ RoleProEnm.IncreEf,"加成效果" },
			{ RoleProEnm.CoreLearn,"考核" },
			{ RoleProEnm.CommonLearn,"共研" },
			{ RoleProEnm.Learn,"博物研学" }
		};
		public static Dictionary<RoleTypeEnum, string> RoleTypeDic = new Dictionary<RoleTypeEnum, string>()
		{
			{ RoleTypeEnum.AD,"输出" },
			{ RoleTypeEnum.Worker,"辅助" },
		};
		public static Dictionary<DamageType, string> DamageTypeDic = new Dictionary<DamageType, string>()
		{
			{ DamageType.Authentic,"真实伤害" },
			{ DamageType.Physical,"物理伤害" },
			{ DamageType.Magic,"构术伤害" },
            { DamageType.Burn,"灼伤" },
            { DamageType.LostBlood,"流失" },
			{ DamageType.Melt,"熔伤" }
        };
		public static Dictionary<AtkType, string> AtkTypeDic = new Dictionary<AtkType, string>()
		{
			{ AtkType.Alert,"警戒" },
			{ AtkType.Frequent,"常击" },
			{ AtkType.Batter,"连击" },
			{ AtkType.Stunk,"绝技" },
			{ AtkType.Counterattack,"回击" },
			{ AtkType.Extra,"额外" },
			{ AtkType.Profession,"职业" },
            { AtkType.Contential,"持续" }
        };
        #endregion
        #region

        #endregion
        #region funcs

        public static string SerializeIncreInfo(List<ShowIncreInfo> showList)
		{
			string increListring = "";
			foreach (var item in showList)
			{
				if (item.SelectedIncre == null|| item.IncreNum.Equals("0")) continue;
				var str = "";
				str = item.Title + ","
					+ item.SelectedIncre.IncreName + ","
					+ item.SelectedIncre.IncreClass + ","
					+ item.SelectedIncre.IncreType + ","
					+ item.SelectedIncre.IncreDetail + ","
					+ item.IncreNum.ToString() + '&';
				increListring += str;
			}
			return increListring;
		}
		public static List<ShowIncreInfo> DeSerializeIncreInfo(string increEf, ObservableCollection<IncreInfo> increInfoList=null,Occupation occupation=Occupation.Solder)
		{
			List<ShowIncreInfo> resultList = new List<ShowIncreInfo>();
			string[] tempLi = increEf.Split('&');
			foreach (string item in tempLi)
			{
				if (string.IsNullOrEmpty(item)) break;
				var result = item.Split(',');
				var temp = new IncreInfo()
				{
					IncreName = result[1],
					IncreClass = int.Parse(result[2]),
					IncreType = int.Parse(result[3]),
					IncreDetail = int.Parse(result[4]),
                    IncreNum =  int.Parse(result[5])

                };
                var newInfo = new ShowIncreInfo()
				{
					Title = result[0],
					IncreNum = result[5],
					IncreInfoList = increInfoList,
					SelectedOccupation = occupation
				};
				if (increInfoList != null)
				{
					newInfo.SelectedIncre = increInfoList.FirstOrDefault(i => i.IncreName.Equals(temp.IncreName)
							&& i.IncreClass.Equals(temp.IncreClass)&& i.IncreType.Equals(temp.IncreType));
				}
				else
				{
					newInfo.SelectedIncre = temp;
				}
				resultList.Add(newInfo);
			}
			return resultList;
		}
		public static List<SkillItem> DeSerializeSkillInfo(string increEf)
		{
			List<SkillItem> resultList = new List<SkillItem>();
			string[] tempLi = increEf.Split('&');
			foreach (string item in tempLi)
			{
				if (string.IsNullOrEmpty(item)) break;
				var result = item.Split(',');
				resultList.Add(new SkillItem()
				{
					AtkType = (AtkType)Enum.Parse(typeof(AtkType), result[0], true),
					DamageType = (DamageType)Enum.Parse(typeof(DamageType), result[1], true),
					DamageTimes = result[2],
					SkillNum = result[3]
				});
			}
			return resultList;
		}

		public static string SerializeSkillInfo(List<SkillItem> showList)
		{
			string increListring = "";
			foreach (var item in showList)
			{
				if (string.IsNullOrEmpty(item.SkillNum)) continue;
				var str = "";
				str = $"{item.AtkType},{item.DamageType},{item.DamageTimes},{item.SkillNum}&";
				increListring += str;
			}
			return increListring;
		}
        #endregion

        public static T DeepCopy<T>(T obj)
        {
            if (obj == null)
            {
                return obj;
            }
            var type = obj.GetType();
            if (obj is string || type.IsValueType)
            {
                return obj;
            }

            var result = Activator.CreateInstance(type);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                field.SetValue(result, field.GetValue(obj));
            }
            return (T)result;
        }
    }
}
