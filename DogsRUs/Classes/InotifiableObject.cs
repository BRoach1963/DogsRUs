using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows.Threading;

namespace DogsRUs.Classes
{
	public class NotifiableObject : INotifyPropertyChanged
	{

		#region INotifyPropertyChanged Methods

		/// <summary>
		/// Raised when a property on this object has a new value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises this object's PropertyChanged event for each of the properties.
		/// </summary>
		/// <param name="propertyNames">The properties that have a new value.</param>
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Method used to raise an event")]
		protected void RaisePropertyChanged(params string[] propertyNames)
		{
			if (propertyNames == null)
			{
				throw new ArgumentNullException(nameof(propertyNames));
			}

			foreach (string name in propertyNames)
			{
				OnNotifyPropertyChanged(name);
			}
		}

		/// <summary>
		/// Raises this object's PropertyChanged event.
		/// </summary>
		/// <typeparam name="T">The type of the property that has a new value</typeparam>
		/// <param name="propertyExpression">A Lambda expression representing the property that has a new value.</param>
		protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
			OnNotifyPropertyChanged(propertyName);
		}

		/// <summary>
		/// Raises this object's PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The property that has a new value.</param>
		public void OnNotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				Delegate[] delegates = PropertyChanged.GetInvocationList();
				//delegates MUST be called in the order that they are returned by GetInvocationList (per MSDN)

				var e = new PropertyChangedEventArgs(propertyName);

				foreach (var @delegate in delegates)
				{
					var handler = (PropertyChangedEventHandler) @delegate;

					// If the subscriber is a DispatcherObject on a different thread
					if (handler.Target is DispatcherObject dispatcherObject && dispatcherObject.CheckAccess() == false)
					{
						dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind, handler, this, e);
					}
					else
					{
						handler(this, e);
					}
				}
			}
		}

		#endregion
	}
}
