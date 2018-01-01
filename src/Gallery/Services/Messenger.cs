using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gallery.Contracts;
using Gallery.Core;

namespace Gallery.Services
{
	[Export(typeof(IMessenger))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class Messenger : IMessenger
	{
		#region : Private Fields :

		private static Messenger _defaultInstance;

		private Dictionary<Type, List<WeakActionAndToken>> _recipientsOfSubclassesAction;

		private Dictionary<Type, List<WeakActionAndToken>> _recipientsStrictAction;

		#endregion : Private Fields :

		#region : Public Properties :

		/// <summary>
		/// Gets the Messenger's default instance, allowing
		/// to register and send messages in a static manner.
		/// </summary>
		public static Messenger Default
		{
			get
			{
				if (_defaultInstance == null)
				{
					_defaultInstance = new Messenger();
				}

				return _defaultInstance;
			}
		}

		#endregion : Public Properties :

		#region : IMessenger Members :

		public virtual void Register<TMessage>(object recipient, Action<TMessage> action, bool receiveDerivedMessagesToo = false)
		{
			Register<TMessage>(recipient, null, action, receiveDerivedMessagesToo);
		}

		public void Register<TMessage>(object recipient, object token, Action<TMessage> action, bool receiveDerivedMessagesToo = false)
		{
			var messageType = typeof(TMessage);

			Dictionary<Type, List<WeakActionAndToken>> recipients;

			if (receiveDerivedMessagesToo)
			{
				if (_recipientsOfSubclassesAction == null)
				{
					_recipientsOfSubclassesAction = new Dictionary<Type, List<WeakActionAndToken>>();
				}

				recipients = _recipientsOfSubclassesAction;
			}
			else
			{
				if (_recipientsStrictAction == null)
				{
					_recipientsStrictAction = new Dictionary<Type, List<WeakActionAndToken>>();
				}

				recipients = _recipientsStrictAction;
			}

			List<WeakActionAndToken> list;

			if (!recipients.ContainsKey(messageType))
			{
				list = new List<WeakActionAndToken>();
				recipients.Add(messageType, list);
			}
			else
			{
				list = recipients[messageType];
			}

			var weakAction = new WeakAction<TMessage>(recipient, action);
			var item = new WeakActionAndToken
			{
				Action = weakAction,
				Token = token
			};
			list.Add(item);

			Cleanup();
		}

		public virtual void Send<TMessage, TTarget>(TMessage message)
		{
			SendToTargetOrType(message, typeof(TTarget));
		}

		public virtual void Send<TMessage>(TMessage message, object token)
		{
			SendToTargetOrType(message, token: token);
		}

		public virtual void Unregister(object recipient)
		{
			UnregisterFromLists(recipient, _recipientsOfSubclassesAction);
			UnregisterFromLists(recipient, _recipientsStrictAction);
		}

		public virtual void Unregister<TMessage>(object recipient)
		{
			Unregister<TMessage>(recipient, null);
		}

		public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action)
		{
			UnregisterFromLists(recipient, action, _recipientsStrictAction);
			UnregisterFromLists(recipient, action, _recipientsOfSubclassesAction);
			Cleanup();
		}

		#endregion : IMessenger Members :

		#region : Private Methods :

		private static void CleanupList(IDictionary<Type, List<WeakActionAndToken>> lists)
		{
			if (lists == null)
			{
				return;
			}

			var listsToRemove = new List<Type>();
			foreach (var list in lists)
			{
				var recipientsToRemove = list.Value.Where(item => item.Action == null || !item.Action.IsAlive).ToList();

				foreach (var recipient in recipientsToRemove)
				{
					list.Value.Remove(recipient);
				}

				if (list.Value.Count == 0)
				{
					listsToRemove.Add(list.Key);
				}
			}

			foreach (var key in listsToRemove)
			{
				lists.Remove(key);
			}
		}

		private static bool Implements(Type instanceType, Type interfaceType)
		{
			if (interfaceType == null
				|| instanceType == null)
			{
				return false;
			}

			var interfaces = instanceType.GetInterfaces();
			return interfaces.Any(currentInterface => currentInterface == interfaceType);
		}

		private static void SendToList<TMessage>(
			TMessage message,
			IEnumerable<WeakActionAndToken> list,
			Type messageTargetType,
			object token)
		{
			if (list != null)
			{
				// Clone to protect from people registering in a "receive message" method
				// Bug correction Messaging BL0004.007
				var listClone = list.Take(list.Count()).ToList();

				foreach (var item in listClone)
				{
					var executeAction = item.Action as IExecuteWithObject;

					if (executeAction != null
						&& item.Action.IsAlive
						&& item.Action.Target != null
						&& (messageTargetType == null
							|| item.Action.Target.GetType() == messageTargetType
							|| Implements(item.Action.Target.GetType(), messageTargetType))
						&& ((item.Token == null && token == null)
							|| item.Token != null && item.Token.Equals(token)))
					{
						executeAction.ExecuteWithObject(message);
					}
				}
			}
		}

		private static void UnregisterFromLists(object recipient, Dictionary<Type, List<WeakActionAndToken>> lists)
		{
			if (recipient == null
				|| lists == null
				|| lists.Count == 0)
			{
				return;
			}

			lock (lists)
			{
				foreach (var messageType in lists.Keys)
				{
					foreach (var item in lists[messageType])
					{
						var weakAction = item.Action;

						if (weakAction != null
							&& recipient == weakAction.Target)
						{
							weakAction.MarkForDeletion();
						}
					}
				}
			}
		}

		private static void UnregisterFromLists<TMessage>(
			object recipient,
			Action<TMessage> action,
			Dictionary<Type, List<WeakActionAndToken>> lists)
		{
			var messageType = typeof(TMessage);

			if (recipient == null
				|| lists == null
				|| lists.Count == 0
				|| !lists.ContainsKey(messageType))
			{
				return;
			}

			lock (lists)
			{
				foreach (var item in lists[messageType])
				{
					var weakActionCasted = item.Action as WeakAction<TMessage>;

					if (weakActionCasted != null
						&& recipient == weakActionCasted.Target
						&& (action == null
							|| action == weakActionCasted.Action))
					{
						item.Action.MarkForDeletion();
					}
				}
			}
		}

		private void Cleanup()
		{
			CleanupList(_recipientsOfSubclassesAction);
			CleanupList(_recipientsStrictAction);
		}

		private void SendToTargetOrType<TMessage>(TMessage message, Type messageTargetType = null, object token = null)
		{
			var messageType = typeof(TMessage);

			if (_recipientsOfSubclassesAction != null)
			{
				// Clone to protect from people registering in a "receive message" method
				// Bug correction Messaging BL0008.002
				var listClone = _recipientsOfSubclassesAction.Keys.Take(_recipientsOfSubclassesAction.Count()).ToList();

				foreach (var type in listClone)
				{
					List<WeakActionAndToken> list = null;

					if (messageType == type
						|| messageType.IsSubclassOf(type)
						|| Implements(messageType, type))
					{
						list = _recipientsOfSubclassesAction[type];
					}

					SendToList(message, list, messageTargetType, token);
				}
			}

			if (_recipientsStrictAction != null)
			{
				if (_recipientsStrictAction.ContainsKey(messageType))
				{
					var list = _recipientsStrictAction[messageType];
					SendToList(message, list, messageTargetType, token);
				}
			}

			Cleanup();
		}

		private struct WeakActionAndToken
		{
			public WeakAction Action;

			public object Token;
		}

		#endregion : Private Methods :

	} //end public class Messenger : IMessenger

} //end namespace Gallery.Services
