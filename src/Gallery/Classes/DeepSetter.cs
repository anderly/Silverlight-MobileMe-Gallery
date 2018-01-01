using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Gallery
{

	/// <remarks>
	/// The Silverlight object model has a design deficiency in that
	/// bindings only work on FrameworkElements, but we sometimes want
	/// to animate properties of DependencyObjects that are not
	/// FrameworkElements (such as the X property of a TranslateTransform).
	/// 
	/// This workaround allows up to two such properties to be
	/// animated on any such DependencyObject (if more are needed,
	/// they can be added in a fairly straightforward way).
	/// 
	/// The basic idea is that a DeepSetter element can be instantiated
	/// as a resource, and an attached property, DeepSetter.BindingImport,
	/// (or DeepSetter.BindingImport2) can be attached to the target 
	/// DependencyObject and can refer to the DeepSetter element.   The 
	/// DeepSetter element can then bind to the external data value 
	/// (using its BindingExport property), and will forwarded any changes
	/// to this value to all of the DependencyObjects that have "Imported"
	/// that DeepSetter.
	/// 
	/// The only remaining use issue is specifying the Property within
	/// the DependencyObject that is to be updated.  This is done
	/// with the TargetProperty property of the DeepSetter.
	/// 
	/// 2008-04-21 Shack Toms, LiveData, Inc.
	/// </remarks>
	/// 
	/// <summary>
	/// Allows data binding for export to any DependencyObject.
	/// </summary>
	public class DeepSetter : Control
	{
		/// <remarks>
		/// We report values via the C# event mechanism.  This is
		/// a private event, so we don't bother with conformance to
		/// .NET standards.
		/// </remarks>
		private event WeakDelegate.WeakDelegateType ExportValueEvent;

		/// <summary>
		/// The property we are going to set on the target object.
		/// </summary>
		public static readonly DependencyProperty TargetPropertyProperty =
			 DependencyProperty.Register("TargetProperty",
				  typeof(string),
				  typeof(DeepSetter),
				  new PropertyMetadata(OnTargetPropertyChanged));
		public string TargetProperty
		{
			get { return (string)GetValue(TargetPropertyProperty); }
			set { SetValue(TargetPropertyProperty, value); }
		}
		private static void OnTargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DeepSetter setter = d as DeepSetter;
			if (null != setter && null != setter.ExportValueEvent)
				throw new Exception("TargetProperty cannot be changed while bindings exist.");
		}

		/// <summary>
		/// The BindingExport is typically be bound to the outside world.  The value of this
		/// property will be forwarded to any and all BindingImports that reference this
		/// DeepSetter object.
		/// </summary>
		public static readonly DependencyProperty BindingExportProperty =
			 DependencyProperty.Register(
				  "BindingExport",
				  typeof(object),
				  typeof(DeepSetter),
				  new PropertyMetadata(OnBindingExportChanged));
		public object BindingExport
		{
			get { return GetValue(BindingExportProperty); }
			set { SetValue(BindingExportProperty, value); }
		}
		private static void OnBindingExportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DeepSetter setter = d as DeepSetter;
			if (null != setter && null != setter.ExportValueEvent)
				setter.ExportValueEvent(e.NewValue);
		}

		/// <summary>
		/// There are two binding imports, BindingImport and BindingImport2.  This
		/// is just so that we can control up to two properties on the target object.
		/// </summary>
		public static readonly DependencyProperty BindingImportProperty =
			 DependencyProperty.RegisterAttached(
				  "BindingImport",
				  typeof(DeepSetter),
				  typeof(DeepSetter),
				  new PropertyMetadata(OnBindingImportChanged));
		public static void SetBindingImport(DependencyObject d, DeepSetter setter)
		{
			d.SetValue(BindingImportProperty, setter);
		}
		public static DeepSetter GetBindingImport(DependencyObject d)
		{
			return (DeepSetter)d.GetValue(BindingImportProperty);
		}
		private static void OnBindingImportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			OnBindingImportNChanged(BindingImportSetterProperty, d, e);
		}

		/// <summary>
		/// There are two binding imports, BindingImport and BindingImport2.  This
		/// is just so that we can control up to two properties on the target object.
		/// </summary>
		public static readonly DependencyProperty BindingImport2Property =
			 DependencyProperty.RegisterAttached(
				  "BindingImport2",
				  typeof(DeepSetter),
				  typeof(DeepSetter),
				  new PropertyMetadata(OnBindingImport2Changed));
		public static void SetBindingImport2(DependencyObject d, DeepSetter setter)
		{
			d.SetValue(BindingImport2Property, setter);
		}
		public static DeepSetter GetBindingImport2(DependencyObject d)
		{
			return (DeepSetter)d.GetValue(BindingImport2Property);
		}
		private static void OnBindingImport2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			OnBindingImportNChanged(BindingImportSetter2Property, d, e);
		}

		// Common code for BindingImport and BindingImport2 properties.
		private static void OnBindingImportNChanged(DependencyProperty SetterProperty, DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DeepSetter setter;
			SetterDelegate setterDelegate;

			// detach from the old setter, by nulling the SetterDelegate
			if (null != e.OldValue)
			{
				setterDelegate = d.GetValue(SetterProperty) as SetterDelegate;
				Debug.Assert(null != setterDelegate, "SetterProperty was not a SetterDelegate");
				d.SetValue(SetterProperty, null);
			}

			// attach to the new setter, by setting the SetterDelegate
			if (null != e.NewValue)
			{
				setter = e.NewValue as DeepSetter;
				if (null == setter) throw new Exception("The value is not a DeepSetter.");
				string propertyName = (string)setter.GetValue(DeepSetter.TargetPropertyProperty);
				if (null == propertyName) throw new Exception("The DeepSetter has no TargetProperty.");
				setterDelegate = new SetterDelegate(d, propertyName);
				d.SetValue(SetterProperty, setterDelegate);
				setterDelegate.ReportValue(setter.BindingExport);
			}
		}

		/// <summary>
		/// The BindingImportSetterProperty and BindingImportSetter2Property are for internal use, and
		/// allow us to associate SetterDelegates with the target DependencyObject that imports
		/// a binding.
		/// 
		/// Through experimentation, I find that by failing to define the Setxxx and Getxxx methods,
		/// the property won't show up in Intellisense, but the OnxxxChanged method is still called 
		/// when the property is set.
		/// </summary>
		private static readonly DependencyProperty BindingImportSetterProperty =
			 DependencyProperty.RegisterAttached(
				  "BindingImportSetter",
				  typeof(SetterDelegate),
				  typeof(DeepSetter),
				  new PropertyMetadata(OnBindingImportSetterChanged));
		private static readonly DependencyProperty BindingImportSetter2Property =
			 DependencyProperty.RegisterAttached(
				  "BindingImportSetter2",
				  typeof(SetterDelegate),
				  typeof(DeepSetter),
				  new PropertyMetadata(OnBindingImportSetterChanged));

		// Common code for all BindingImportSetters
		private static void OnBindingImportSetterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DeepSetter setter;
			SetterDelegate del;

			// detach from previously-specified DeepSetter
			del = e.OldValue as SetterDelegate;
			if (null != del)
			{
				del.Unsubscribe();
			}

			// attach to the new setter
			if (null != e.NewValue)
			{
				setter = d.GetValue(DeepSetter.BindingImportProperty) as DeepSetter;
				if (null == setter) throw new Exception("BindingImport does not refer to a DeepSetter.");
				del = e.NewValue as SetterDelegate;
				if (null == del) throw new Exception("SetterDelegate must refer to a SetterDelegate.");
				del.Subscribe(setter);
			}
		}

		/// <remarks>
		/// So far, things have been pretty straightforward (I hope).  What follows
		/// is code that, to me, should be possible to do in a more straightforward way,
		/// but I haven't yet been able to find it.
		/// 
		/// SetterDelegate exists for three main reasons.  The first is that
		/// we want to avoid creating a strong reference to the target DependencyObject
		/// that will hold it in memory after it is removed from the VisualTree.  
		/// So we reference the DependencyObject via a WeakReference.
		/// 
		/// The second reason for SetterDelegate is that we don't want to impose
		/// on the datatypes involved in the interaction between the source of the
		/// BindingExport and the target property of the DependencyObject.  The 
		/// Delegate.CreateDelegate method doesn't allow the same flexibility in
		/// matching data types as does MethodInfo.Invoke.
		/// 
		/// The third reason for SetterDelegate is that its finalizer will undo
		/// the subscription to the DeepSetter (this is related to the first
		/// reason, which is also necessary for accomplishing this goal).  Because
		/// the SetterDelegate is an attached property on the target DependencyObject,
		/// when the DependencyObject is finalized, our finalizer will be called
		/// and we can clean up.
		/// </remarks>
		internal class SetterDelegate
		{
			private WeakReference boundobj;
			private MethodInfo mi;
			private WeakDelegate.WeakDelegateType PassThrough;
			private DeepSetter SubscribedSetter;

			/// <remarks>
			/// The reason this method is here is so that we can
			/// let mi.Invoke do all the work of matching the type
			/// in NewValue to the type required by mi.  However,
			/// if we wanted to do additional type conversions, we
			/// could also do them here.
			/// </remarks>
			public void ReportValue(params object[] NewValue)
			{
				DependencyObject d = boundobj.Target as DependencyObject;
				if (null != d)
					mi.Invoke(d, NewValue);
				else
					Unsubscribe();
			}
			private static readonly MethodInfo ReportValueMethodInfo = typeof(SetterDelegate).GetMethod("ReportValue");

			/// <summary>
			/// Attach (indirectly) to a DeepSetter event;
			/// </summary>
			public void Subscribe(DeepSetter setter)
			{
				if (null != PassThrough) Unsubscribe();
				PassThrough = WeakDelegate.CreateWeakDelegate(this, ReportValueMethodInfo);
				setter.ExportValueEvent += PassThrough;
				SubscribedSetter = setter;
			}

			/// <summary>
			/// From the DeepSetter event;
			/// </summary>
			public void Unsubscribe()
			{
				if (null != PassThrough)
				{
					SubscribedSetter.ExportValueEvent -= PassThrough;
					PassThrough = null;
					SubscribedSetter = null;
				}
			}

			/// <remarks>
			/// These objects are purpose-built to handle a particular
			/// method (a set accessor) of a DependencyObject.
			/// </remarks>
			public SetterDelegate(DependencyObject boundobj, string propertyName)
			{
				PropertyInfo pi = boundobj.GetType().GetProperty(propertyName);
				if (null == pi) throw new Exception(String.Format("No such property: {0}", propertyName));
				MethodInfo mi = pi.GetSetMethod();
				if (null == mi) throw new Exception(String.Format("Property {0} has no set accessor", propertyName));
				this.boundobj = new WeakReference(boundobj);
				this.mi = mi;
			}

			~SetterDelegate()
			{
				Unsubscribe();
				boundobj = null;
				mi = null;
			}
		}
	}

	/// <summary>
	/// The problem WeakDelegate solves is this...
	/// 
	/// Suppose we have an object that, as an internal part of 
	/// its operation, should subscribe (add) a delegate to an event.  
	/// When the object is finalized, it should unsubsribe its
	/// delegate from the event.  The problem is that the subscription 
	/// itself maintains a reference to the delegate, and thus
	/// to the object.  This prevents the finalizer from running
	/// on the object so long as the underlying event exists.
	/// 
	/// The WeakDelegate object defined here permits a delegate
	/// to be added to an event indirectly via a weak reference.
	/// The event will be forwarded so long as the delegate exists.
	/// 
	/// This will permit the finalizer to run on the target object,
	/// it is still up to the target object to unsubscribe the
	/// WeakDelegate, but at least it has control.
	/// 
	/// Note that, for now, the event must be of type WeakDelegateEvent.
	/// (Unfortunately, although C# allows generic delegates, it doesn't 
	/// appear to allow GenericTypes based on delegates, nor does it 
	/// permit events to be passed as parameters.)
	/// </summary>
	internal class WeakDelegate
	{
		public delegate void WeakDelegateType(params object[] paramList);
		private WeakReference TargetDelegateRef;
		private MethodInfo Method;
		private WeakDelegateType ThisDelegate;

		// we leave off the params keyword here, so that the object array will
		// not be unpacked into separate parameters when we invoke Method.
		private void PassThrough(object[] paramList)
		{
			if (null != TargetDelegateRef && null != TargetDelegateRef.Target)
				Method.Invoke(TargetDelegateRef.Target, paramList);
		}

		// the constructor is private, new instances should be created via
		// the CreateWeakDelegate method (which returns a delegate that
		// references a new WeakDelegate object instance).
		private WeakDelegate(object baseobj, MethodInfo method)
		{
			TargetDelegateRef = new WeakReference(baseobj);
			Method = method;
			ThisDelegate = new WeakDelegateType(PassThrough);
		}

		/// <summary>
		/// Creates a new delegate which maintains only a weak reference to the given baseobj.
		/// </summary>
		/// <param name="baseobj">The object to use when invoking the method.</param>
		/// <param name="method">The method to invoke on baseobj.</param>
		/// <returns>A delegate that only weakly references baseobj.</returns>
		public static WeakDelegateType CreateWeakDelegate(object baseobj, MethodInfo method)
		{
			return new WeakDelegate(baseobj, method).ThisDelegate;
		}
	}
}