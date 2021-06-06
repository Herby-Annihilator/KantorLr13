﻿using KantorLr13.Infrastructure.Commands;
using KantorLr13.Models.Data;
using KantorLr13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Markup;

namespace KantorLr13.ViewModels
{
	[MarkupExtensionReturnType(typeof(MainWindowViewModel))]
	public class MainWindowViewModel : ViewModel
	{
		public MainWindowViewModel()
		{
			ClearSystemOfDifferentialEquationsCommand = new LambdaCommand(OnClearSystemOfDifferentialEquationsCommandExecuted, CanClearSystemOfDifferentialEquationsCommandExecute);
			AddKoshiTaskToSystemCommand = new LambdaCommand(OnAddKoshiTaskToSystemCommandExecuted, CanAddKoshiTaskToSystemCommandExecute);
			DeleteSelectedKoshiTaskCommand = new LambdaCommand(OnDeleteSelectedKoshiTaskCommandExecuted, CanDeleteSelectedKoshiTaskCommandExecute);
		}
		#region Properties
		private string _title = "Title";
		public string Title { get => _title; set => Set(ref _title, value); }

		private string _status = "Status";
		public string Status { get => _status; set => Set(ref _status, value); }

		private double _startX;
		public double StartX
		{
			get => _startX;
			set
			{
				Set(ref _startX, value);
				foreach (var task in SystemOfDifferentialEquations)
				{
					task.StartXCondition = value;
				}
			}
		}

		private double _endX;
		public double EndX { get => _endX; set => Set(ref _endX, value); }

		private int _stepsCount;
		public int StepsCount { get => _stepsCount; set => Set(ref _stepsCount, value); }

		private double _precision;
		public double Precision { get => _precision; set => Set(ref _precision, value); }

		private KoshiTask _selectedTask;
		public KoshiTask SelectedTask { get => _selectedTask; set => Set(ref _selectedTask, value); }

		public ObservableCollection<KoshiTask> SystemOfDifferentialEquations { get; private set; } = new ObservableCollection<KoshiTask>();
		#endregion

		#region Commands
		public ICommand ClearSystemOfDifferentialEquationsCommand { get; }
		private void OnClearSystemOfDifferentialEquationsCommandExecuted(object p)
		{
			SystemOfDifferentialEquations.Clear();
			SelectedTask = null;
		}
		private bool CanClearSystemOfDifferentialEquationsCommandExecute(object p) => SystemOfDifferentialEquations.Count > 0;

		public ICommand AddKoshiTaskToSystemCommand { get; }
		private void OnAddKoshiTaskToSystemCommandExecuted(object p)
		{
			SystemOfDifferentialEquations.Add(new KoshiTask("", "", StartX, default));
		}
		private bool CanAddKoshiTaskToSystemCommandExecute(object p) => true;

		public ICommand DeleteSelectedKoshiTaskCommand { get; }
		private void OnDeleteSelectedKoshiTaskCommandExecuted(object p)
		{
			SystemOfDifferentialEquations.Remove(SelectedTask);
			SelectedTask = null;
		}
		private bool CanDeleteSelectedKoshiTaskCommandExecute(object p) => SelectedTask != null;
		#endregion
	}
}