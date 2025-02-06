using System;
using System.IO;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using SqlSugar.IOC;
using WpfApp3.Views;
using Wuhua.IRepository;
using Wuhua.IService;
using Wuhua.Repository;
using Wuhua.Service;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
	{
		protected override Window CreateShell()
		{
			var w = Container.Resolve<MainWindow>();
			return w;
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			//containerRegistry.RegisterSingleton<SqlSugarClient>(() => SqlSugarConfig.GetInstance());

			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource/Wuhua.db");

            SugarIocServices.AddSqlSugar(new IocConfig()
			{
				
				ConnectionString = @"Data Source="+path+";",
				DbType = IocDbType.Sqlite,

                IsAutoCloseConnection = true//自动释放
			});

			containerRegistry.Register<IRoleInfoRepository, RoleInfoRepository>();
			containerRegistry.Register<IWeaponInfoRepository, WeaponInfoRepository>();
			containerRegistry.Register<IIncreInfoRepository, IncreInfoRepository>();
			containerRegistry.Register<IDeepInfoRepository, DeepInfoRepository>();
			containerRegistry.Register<ICustomWeaponRepository, CustomWeaponRepository>();
			containerRegistry.Register<ILearnInfoRepository, LearnInfoRepository>();

			containerRegistry.Register<IRoleInfoService, RoleInfoService>();
			containerRegistry.Register<IWeaponInfoService, WeaponInfoService>();
			containerRegistry.Register<IIncreInfoService, IncreInfoService>();
			containerRegistry.Register<IDeepInfoService, DeepInfoService>();
			containerRegistry.Register<ICustomWeaponService, CustomWeaponService>();
			containerRegistry.Register<ILearnInfoService, LearnInfoService>();

		}
		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			
		}
	}
}
