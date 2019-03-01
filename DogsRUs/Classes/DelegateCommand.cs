using System;
using System.Windows.Input;

namespace DogsRUs.Classes
{
	public class DelegateCommand : ICommand
	{
		#region Fields

		private readonly Func<bool> m_canExecuteMethod;
		private readonly Action m_executeMethod;

		#endregion

		#region Constructors

		public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
		{
			m_executeMethod = executeMethod;
			m_canExecuteMethod = canExecuteMethod;
		}

		public DelegateCommand(Action executeMethod)
		{
			m_executeMethod = executeMethod;
			m_canExecuteMethod = null;
		}

		#endregion

		#region Events

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Methods

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				m_executeMethod();
			}
		}

		public bool CanExecute(object parameter)
		{
			return m_canExecuteMethod == null || m_canExecuteMethod();
		}

		public virtual void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;

			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion
	}
}
