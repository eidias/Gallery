using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gallery.Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        Assembly assembly;

        public MainViewModel()
        {
            assembly = GetType().Assembly;
        }

        public IEnumerable<Type> AvailableViews
        {
            get
            {
                return assembly.GetTypes().Where(x => x.Namespace == "Gallery.Wpf.Views");
            }
        }

        public Type SelectedView
        {
            get
            {
                return null;
            }
            set
            {
                EmbeddedView = Activator.CreateInstance(value) as UserControl;
            }
        }

        private UserControl embeddedView;
        public UserControl EmbeddedView
        {
            get
            {
                return embeddedView;
            }
            set
            {
                embeddedView = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return String.Format("{0} ({1})", assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title, assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright);
            }
        }

        public string Version
        {
            get
            {
                return assembly.GetName().Version.ToString();
            }
        }

        public string User
        {
            get
            {
                return Environment.UserName;
            }
        }

        public ICollectionViewLiveShaping OrdersLive
        {
            get
            {
                return CollectionViewSource.GetDefaultView(null) as ICollectionViewLiveShaping;
            }
        }

        public ICollectionView Orders
        {
            get
            {
                return CollectionViewSource.GetDefaultView(null);
            }
        }


    }
}
