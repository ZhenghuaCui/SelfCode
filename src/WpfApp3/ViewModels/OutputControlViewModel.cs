using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;

namespace WpfApp3.ViewModels
{
	/// <summary>
	/// 输出的弹窗shell
	/// </summary>
    public class OutputControlViewModel:BindableBase
	{
		public RolesInfoControlViewModel rolesVm;

		private string damageResult = "";
		public string DamageResult
		{
			get { return damageResult; }
			set
			{
				SetProperty(ref damageResult, value);
			}
		}
		private IEventAggregator _aggregator;
		public OutputControlViewModel(IContainerProvider containerProvider, IEventAggregator aggregator)
		{
			_aggregator = aggregator;
			
		}
	}
}
