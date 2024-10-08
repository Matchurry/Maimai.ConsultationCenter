using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zhaoxi.CourseManagement.Common;
using Zhaoxi.CourseManagement.Model;
using static MaimaiConsulationCenter.Common.Interfaces;

namespace Zhaoxi.CourseManagement.ViewModel
{
    public class MainViewModel:NotifyBase
    {
        public UserModel UserInfo { get; set; } = new UserModel();
		private string _searchText;

		public string SearchText
		{
			get { return _searchText; }
			set { _searchText = value; DoNotify(); }
		}

		private FrameworkElement _mainContent;

		public FrameworkElement MainContent
		{
			get { return _mainContent; }
			set { _mainContent = value; DoNotify(); }
		}

		public CommandBase NavChangedCommand { get; set; }

		public MainViewModel() {
			NavChangedCommand = new CommandBase();
			NavChangedCommand.DoExecute = new Action<object>(DoNavChanged);
			NavChangedCommand.DoCanExecute = new Func<object, bool>((o) => true);
			DoNavChanged("FirstPageView");
		}

		private async void DoNavChanged(object o)
		{
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MainContent = new MaimaiConsulationCenter.View.LoadingView(); // 加载过渡页面
            }));

            //await Task.Run(async () =>
			//{
                
				await Application.Current.Dispatcher.BeginInvoke(new Action(async() =>
				{
					Type type = Type.GetType("Zhaoxi.CourseManagement.View." + o.ToString());
					ConstructorInfo cti = type.GetConstructor(System.Type.EmptyTypes);
					var tar = (FrameworkElement)cti.Invoke(null);
					if(tar is IDataLoadable dataLoadablePage)
					{
                        await dataLoadablePage.InitializeDataAsync(); //这边等待检测其实也执行了一次 就不需要在初始化的时候再执行了
                    }
					MainContent = tar;

                }));
			//});
		}
	}
}
