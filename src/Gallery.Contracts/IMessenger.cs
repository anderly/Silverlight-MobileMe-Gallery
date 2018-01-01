using System;

namespace Gallery.Contracts
{
	public interface IMessenger
	{
		void Register<TMessage>(object recipient, object token, Action<TMessage> action, bool receiveDerivedMessagesToo = false);
		void Register<TMessage>(object recipient, Action<TMessage> action, bool receiveDerivedMessagesToo = false);
		void Send<TMessage>(TMessage message, object token = null);
		void Send<TMessage, TTarget>(TMessage message);
		void Unregister(object recipient);
		void Unregister<TMessage>(object recipient, Action<TMessage> action = null);
	}
}
