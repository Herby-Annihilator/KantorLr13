using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr13.ViewModels
{
	public class ViewModelLocator
	{
		public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
	}
}
