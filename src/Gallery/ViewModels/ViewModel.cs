using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using Gallery.Contracts;

namespace Gallery.ViewModels
{
	public interface IViewModelBase
	{
		event EventHandler Loaded;
	}

	public abstract class ViewModel : DependencyObject, INotifyPropertyChanged, IViewModelBase
	{
		[Import]
		public IGalleryContext GalleryContext { get; set; }

		[Import]
		public IMessenger Messenger { get; set; }

		//[ImportingConstructor]
		//protected ViewModel([Import(RequiredCreationPolicy = CreationPolicy.Shared)]IGalleryContext galleryContext,
		//    [Import(RequiredCreationPolicy = CreationPolicy.Shared)]IMessenger messenger)
		public ViewModel()
		{
			CompositionInitializer.SatisfyImports(this);
			//GalleryContext = galleryContext;
			//Messenger = messenger;
		}

		public event EventHandler Loaded = delegate { };
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		protected void OnLoaded()
		{
			var handler = this.Loaded;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		public void NotifyPropertyChanged(string propertyName)
		{
			var handler = this.PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Raises this object's PropertyChanged event for each of the properties.
		/// </summary>
		/// <param name="propertyNames">The properties that have a new value.</param>
		protected void NotifyPropertyChanged(params string[] propertyNames)
		{
			foreach (var name in propertyNames)
			{
				this.NotifyPropertyChanged(name);
			}
		}

		/// <summary>
		/// Raises this object's PropertyChanged event.
		/// </summary>
		/// <param name="propertyExpresssion">A Lambda expression representing the property that has a new value.</param>
		protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
		{
			var propertyName = ExtractPropertyName(propertyExpresssion);
			this.NotifyPropertyChanged(propertyName);
		}

		private string ExtractPropertyName<T>(Expression<Func<T>> propertyExpresssion)
		{
			if (propertyExpresssion == null)
			{
				throw new ArgumentNullException("propertyExpression");
			}

			var memberExpression = propertyExpresssion.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
			}

			var property = memberExpression.Member as PropertyInfo;
			if (property == null)
			{
				throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
			}

			if (!property.DeclaringType.IsAssignableFrom(this.GetType()))
			{
				throw new ArgumentException("The referenced property belongs to a different type.", "propertyExpression");
			}

			var getMethod = property.GetGetMethod(true);
			if (getMethod == null)
			{
				// this shouldn't happen - the expression would reject the property before reaching this far
				throw new ArgumentException("The referenced property does not have a get method.", "propertyExpression");
			}

			if (getMethod.IsStatic)
			{
				throw new ArgumentException("The referenced property is a static property.", "propertyExpression");
			}

			return memberExpression.Member.Name;
		}
	}

}
