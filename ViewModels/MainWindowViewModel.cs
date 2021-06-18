using KantorLr13.Infrastructure.Commands;
using KantorLr13.Models.Data;
using KantorLr13.Models.RungeKutt;
using KantorLr13.Models.Vectors;
using KantorLr13.ViewModels.Base;
using org.mariuszgromada.math.mxparser;
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
		private StepMode _stepMode;
		public MainWindowViewModel()
		{
			_stepMode = StepMode.Fixed;
			ClearSystemOfDifferentialEquationsCommand = new LambdaCommand(OnClearSystemOfDifferentialEquationsCommandExecuted, CanClearSystemOfDifferentialEquationsCommandExecute);
			AddKoshiTaskToSystemCommand = new LambdaCommand(OnAddKoshiTaskToSystemCommandExecuted, CanAddKoshiTaskToSystemCommandExecute);
			DeleteSelectedKoshiTaskCommand = new LambdaCommand(OnDeleteSelectedKoshiTaskCommandExecuted, CanDeleteSelectedKoshiTaskCommandExecute);
			SelectStepModeCommand = new LambdaCommand(OnSelectStepModeCommandExecuted, CanSelectStepModeCommandExecute);
			SolveCommand = new LambdaCommand(OnSolveCommandExecuted, CanSolveCommandExecute);
			PaintSelectedKoshiTaskCommand = new LambdaCommand(OnPaintSelectedKoshiTaskCommandExecuted, CanPaintSelectedKoshiTaskCommandExecute);
			ClearPaintedSelectedKoshiTaskCommand = new LambdaCommand(OnClearPaintedSelectedKoshiTaskCommandExecuted, CanClearPaintedSelectedKoshiTaskCommandExecute);
			ShowSelectedTaskValuesCommand = new LambdaCommand(OnShowSelectedTaskValuesCommandExecuted, CanShowSelectedTaskValuesCommandExecute);
			DrawRealFunctionCommand = new LambdaCommand(OnDrawRealFunctionCommandExecuted, CanDrawRealFunctionCommandExecute);
			ClearRealFunctionGraphCommand = new LambdaCommand(OnClearRealFunctionGraphCommandExecuted, CanClearRealFunctionGraphCommandExecute);
			ShowRealFunctionCommand = new LambdaCommand(OnShowRealFunctionCommandExecuted, CanShowRealFunctionCommandExecute);
			ClearRealFunctionTableCommand = new LambdaCommand(OnClearRealFunctionTableCommandExecuted, CanClearRealFunctionTableCommandExecute);
		}
		#region Properties
		private bool _isFixedStepMode = true;
		public bool IsFixedStepMode { get => _isFixedStepMode; set => Set(ref _isFixedStepMode, value); }

		private bool _isAutoStepMode = false;
		public bool IsAutoStepMode { get => _isAutoStepMode; set => Set(ref _isAutoStepMode, value); }

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
		public ObservableCollection<Point> SelectedFunctionPoints { get; private set; } = new ObservableCollection<Point>();

		private List<Point>[] _functionsPoints;


		private string _realFunctionExpression;
		public string RealFunctionExpression { get => _realFunctionExpression; set => Set(ref _realFunctionExpression, value); }

		public ObservableCollection<Point> RealFunctionPointsForGraph { get; private set; } = new ObservableCollection<Point>();
		public ObservableCollection<Point> RealFunctionPointsForTable { get; private set; } = new ObservableCollection<Point>();

		private double _left;
		public double Left { get => _left; set => Set(ref _left, value); }

		private double _right;
		public double Right { get => _right; set => Set(ref _right, value); }

		private double _step;
		public double Step { get => _step; set => Set(ref _step, value); }
		#endregion

		#region Commands
		public ICommand ClearSystemOfDifferentialEquationsCommand { get; }
		private void OnClearSystemOfDifferentialEquationsCommandExecuted(object p)
		{
			SystemOfDifferentialEquations.Clear();
			SelectedTask = null;
			Status = "Таблица очищена";
		}
		private bool CanClearSystemOfDifferentialEquationsCommandExecute(object p) => SystemOfDifferentialEquations.Count > 0;

		public ICommand AddKoshiTaskToSystemCommand { get; }
		private void OnAddKoshiTaskToSystemCommandExecuted(object p)
		{
			SystemOfDifferentialEquations.Add(new KoshiTask("", "", StartX, default));
			Status = "Добавлено";
		}
		private bool CanAddKoshiTaskToSystemCommandExecute(object p) => true;

		public ICommand DeleteSelectedKoshiTaskCommand { get; }
		private void OnDeleteSelectedKoshiTaskCommandExecuted(object p)
		{
			SystemOfDifferentialEquations.Remove(SelectedTask);
			SelectedTask = null;
			SelectedFunctionPoints.Clear();
			Status = "Выбранное уравнение удалено";
		}
		private bool CanDeleteSelectedKoshiTaskCommandExecute(object p) => SelectedTask != null;

		public ICommand SelectStepModeCommand { get; }
		private void OnSelectStepModeCommandExecuted(object p)
		{
			if ((string)p == "Fixed")
			{
				_stepMode = StepMode.Fixed;
				IsFixedStepMode = true;
				IsAutoStepMode = false;
			}
			else
			{
				_stepMode = StepMode.Auto;
				IsFixedStepMode = false;
				IsAutoStepMode = true;
			}
		}
		private bool CanSelectStepModeCommandExecute(object p) => true;

		public ICommand SolveCommand { get; }
		private void OnSolveCommandExecuted(object p)
		{
			try
			{
				DerivativeFunction[] derivatives = new DerivativeFunction[SystemOfDifferentialEquations.Count];
				Vector functionsStartConditions = new Vector(SystemOfDifferentialEquations.Count);
				for (int i = 0; i < derivatives.Length; i++)
				{
					derivatives[i] = TaskToDerivativeFunction(SystemOfDifferentialEquations[i]);
					functionsStartConditions[i] = SystemOfDifferentialEquations[i].StartYCondition;
				}
				GlobalVectorDerivativeFunction f = new GlobalVectorDerivativeFunction()
				{
					DerivativeFunctions = derivatives
				};
				if (_stepMode == StepMode.Fixed)
				{					
					RungeKuttMethod method = new RungeKuttMethod();
					_functionsPoints = method.GetSystemSolution(f, StartX, EndX, functionsStartConditions, StepsCount);
				}
				else
				{
					RungeKuttaFehlberMethod method = new RungeKuttaFehlberMethod();
					_functionsPoints = method.GetSystemSolution(f, StartX, EndX, functionsStartConditions, Precision);
				}
				Status = "Успешное решение";
			}
			catch(Exception e)
			{
				Status = $"Неудача. Причина: {e.Message}";
			}
		}
		private bool CanSolveCommandExecute(object p)
		{
			bool ok = false;
			if (_stepMode == StepMode.Fixed)
			{
				if (StepsCount > 0)
					ok = true;
			}
			else
			{
				if (Precision > 0)
					ok = true;
			}
			return ok && (SystemOfDifferentialEquations.Count > 0);
		}

		public ICommand PaintSelectedKoshiTaskCommand { get; }
		private void OnPaintSelectedKoshiTaskCommandExecuted(object p)
		{
			try
			{
				int index = SystemOfDifferentialEquations.IndexOf(SelectedTask);
				if (index > -1)
				{
					SelectedFunctionPoints.Clear();
					for (int i = 0; i < _functionsPoints[index].Count; i++)
					{
						SelectedFunctionPoints.Add(_functionsPoints[index][i]);
					}
				}
				Status = "Выбранная функция нарисована";
			}
			catch(Exception e)
			{
				Status = $"Неудача. Причина: {e.Message}";
			}
		}
		private bool CanPaintSelectedKoshiTaskCommandExecute(object p) => SelectedTask != null && _functionsPoints != null && _functionsPoints.Length > 0;

		public ICommand ClearPaintedSelectedKoshiTaskCommand { get; }
		private void OnClearPaintedSelectedKoshiTaskCommandExecuted(object p)
		{
			SelectedFunctionPoints.Clear();
			SelectedTask = null;
			Status = "Выбранный график стерт";
		}
		private bool CanClearPaintedSelectedKoshiTaskCommandExecute(object p) => SelectedFunctionPoints.Count > 0;

		public ICommand ShowSelectedTaskValuesCommand { get; }
		private void OnShowSelectedTaskValuesCommandExecuted(object p)
		{
			try
			{
				int index = SystemOfDifferentialEquations.IndexOf(SelectedTask);
				if (index > -1)
				{
					SelectedFunctionPoints.Clear();
					for (int i = 0; i < _functionsPoints[index].Count; i++)
					{
						SelectedFunctionPoints.Add(_functionsPoints[index][i]);
					}
				}
				Status = "Выбранная функция отображена";
			}
			catch(Exception e)
			{
				Status = $"Неудача. Причина: {e.Message}";
			}
		}
		private bool CanShowSelectedTaskValuesCommandExecute(object p) => SelectedTask != null &&
			_functionsPoints != null && _functionsPoints.Length > 0;


		public ICommand DrawRealFunctionCommand { get; }
		private void OnDrawRealFunctionCommandExecuted(object p)
		{
			try
			{
				Function f = new Function(RealFunctionExpression);
				RealFunctionPointsForGraph.Clear();
				for (double i = Left; i < Right; i += Step)
				{
					RealFunctionPointsForGraph.Add(new Point(i, f.calculate(i)));
				}
				Status = "График нарисован";
			}
			catch(Exception e)
			{
				Status = $"Неудача. Причина: {e.Message}";
			}
		}
		private bool CanDrawRealFunctionCommandExecute(object p) => Left < Right && Step > 0;

		public ICommand ClearRealFunctionGraphCommand { get; }
		private void OnClearRealFunctionGraphCommandExecuted(object p)
		{
			RealFunctionPointsForGraph.Clear();
			Status = "График стерт";
		}
		private bool CanClearRealFunctionGraphCommandExecute(object p) =>RealFunctionPointsForGraph.Count > 0;

		public ICommand ShowRealFunctionCommand { get; }
		private void OnShowRealFunctionCommandExecuted(object p)
		{
			try
			{
				Function f = new Function(RealFunctionExpression);
				RealFunctionPointsForTable.Clear();
				for (double i = Left; i < Right; i += Step)
				{
					RealFunctionPointsForTable.Add(new Point(i, f.calculate(i)));
				}
				Status = "Таблица получена";
			}
			catch(Exception e)
			{
				Status = $"Неудача. Причина: {e.Message}";
			}
		}
		private bool CanShowRealFunctionCommandExecute(object p) => Left < Right && Step > 0;

		public ICommand ClearRealFunctionTableCommand { get; }
		private void OnClearRealFunctionTableCommandExecuted(object p)
		{
			RealFunctionPointsForTable.Clear();
			Status = "Таблица очищена";
		}
		private bool CanClearRealFunctionTableCommandExecute(object p) => RealFunctionPointsForTable.Count > 0;
		#endregion

		private DerivativeFunction TaskToDerivativeFunction(KoshiTask task)
		{
			string functionArgs = "";
			string lower;
			char[] potencialArgs = { 'a', 'b', 'c', 'd', /*'e',*/ 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
			for (int i = 0; i < task.Expression.Length; i++)
			{
				lower = task.Expression.ToLower();
				for (int j = 0; j < potencialArgs.Length; j++)
				{
					if (lower[i] == potencialArgs[j])
					{
						functionArgs += potencialArgs[j] + ", ";
						break;
					}
				}
			}
			functionArgs = functionArgs.Remove(functionArgs.Length - 2);
			DerivativeFunction function = new DerivativeFunction($"f({functionArgs})", task.Expression);
			return function;
		}
	}
}
