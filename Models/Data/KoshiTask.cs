using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KantorLr13.Models.Data
{
	public class KoshiTask : INotifyPropertyChanged
	{
		public string DerivativeName { get; set; }
		public string Expression { get; set; }

		private double _startXCondition;
		public double StartXCondition
		{
			get => _startXCondition;
			set
			{
				if (!Equals(_startXCondition, value))
				{
					_startXCondition = value;
					OnPropertyChanged(nameof(StartXCondition));
				}
			}
		}
		public double StartYCondition { get; set; }

		public KoshiTask(string derivativeName, string expression, double startXCondition, double startYCondition)
		{
			DerivativeName = derivativeName;
			Expression = expression;
			StartXCondition = startXCondition;
			StartYCondition = startYCondition;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
