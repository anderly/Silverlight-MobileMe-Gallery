using System;

namespace Gallery.Core
{
	/// <summary>
	/// Stores an Action without causing a hard reference
	/// to be created to the Action's owner. The owner can be garbage collected at any time.
	/// </summary>
	/// <typeparam name="T">The type of the Action's parameter.</typeparam>
	////[ClassInfo(typeof(Messenger))]
	public class WeakAction<T> : WeakAction, IExecuteWithObject
	{
		private readonly Action<T> _action;

		public WeakAction(object target, Action<T> action)
			: base(target, null)
		{
			_action = action;
		}

		public new Action<T> Action
		{
			get
			{
				return _action;
			}
		}

		public new void Execute()
		{
			if (_action != null
				&& IsAlive)
			{
				_action(default(T));
			}
		}

		public void Execute(T parameter)
		{
			if (_action != null
				&& IsAlive)
			{
				_action(parameter);
			}
		}

		public void ExecuteWithObject(object parameter)
		{
			var parameterCasted = (T)parameter;
			Execute(parameterCasted);
		}
	}
}
