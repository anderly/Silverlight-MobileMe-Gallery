using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

using MefMvvm.ViewModelLocator;
using System.Diagnostics;
using System.Windows;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;

namespace MefMvvm.ViewModelLocator
{
    /// <summary>
    /// Repoitory for the ViewModels
    /// </summary>
    public class ViewModelRepoitory : IPartImportsSatisfiedNotification
    {
        #region Singleton stuff
        //stores the instance of the ViewModel
        static ViewModelRepoitory instance;

        /// <summary>
        /// Singleton for the ViewModelLocator
        /// </summary>
        internal static ViewModelRepoitory Instance
        {
            get
            {
                EnsureContainer();

                return ViewModelRepoitory.instance;
            }
        }

        private static void EnsureContainer()
        {
            if (instance == null)
            {
                instance = new ViewModelRepoitory();
            }
        }
        #endregion

        private BasicViewModelInializer basicVMInitializer;
        private MefMvvmResolver resolver;
        private DataContextAwareViewModelInitializer dataContextAwareVMInitializer;
        private List<KeyValuePair<string, WeakReference>> unsatisfiedContracts = new List<KeyValuePair<string, WeakReference>>();
        private static CompositionContainer _container; 
        //tries to satisfy the imports
        private void TrySatisyImports()
        {
            try
            {
                var tempContainer = LocatorBootstrapper.EnsureLocatorBootstrapper();
                if (tempContainer != null)
                {
                    _container = tempContainer;
                    Debug.WriteLine("MefMvvm Composition Container is changing.");
                }
                resolver = new MefMvvmResolver(_container);
                basicVMInitializer = new BasicViewModelInializer(resolver);
                dataContextAwareVMInitializer = new DataContextAwareViewModelInitializer(resolver);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MefMvvm: ViewModelRepoistory. Unable to satisfy imports. " + ex);
            }
        }

        private ViewModelRepoitory()
        {
            TrySatisyImports();
        }

        public static void AttachViewModelToView( string vmContract, FrameworkElement view)
        {
            var vmExport = Instance.resolver.GetViewModelByContract(vmContract, view);
            if (vmExport != null)
            {
                Debug.WriteLine("Attaching ViewModel " + vmExport.Metadata[ExportViewModel.NameProperty]);
                if ((bool)vmExport.Metadata[ExportViewModel.IsDataContextAwareProperty])
                    ViewModelRepoitory.Instance.dataContextAwareVMInitializer.CreateViewModel(vmExport, view);
                else
                    ViewModelRepoitory.Instance.basicVMInitializer.CreateViewModel(vmExport, view);
            }
            else
            {
                RegisterMissingViewModel(vmContract, view);
            }
        }

        public static void RegisterMissingViewModel(string vmContractName, FrameworkElement view)
        {
            Debug.WriteLine("MefMvvm: ViewModelRepoistory. ViewModel not found. Will try to recompose the ViewModel when a Recomposition is done");
            lock(Instance.unsatisfiedContracts)
                Instance.unsatisfiedContracts.Add( new KeyValuePair<string, WeakReference>( vmContractName, new WeakReference(view) ));
        }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            lock (unsatisfiedContracts)
            {
                for (int i = 0; i < unsatisfiedContracts.Count; i++)
                {
                    var vmContract = unsatisfiedContracts[i];
                    Debug.WriteLine("MefMvvm: ViewModelRepoistory. Recomposition was made. Will try to recompose missing ViewModel " + vmContract);
                    var vm = Instance.resolver.GetViewModelByContract(vmContract.Key, vmContract.Value.IsAlive ? vmContract.Value.Target : null);
                    if (vm != null)
                    {
                        if (vmContract.Value.IsAlive) // if the UI element is still alive
                        {
                            AttachViewModelToView(vmContract.Key, (FrameworkElement)vmContract.Value.Target);
                        }
                        //remove the item from the list of unsatisfied contracts
                        unsatisfiedContracts.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        #endregion
    }
}
