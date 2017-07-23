using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gallery.Desktop.ViewModels
{
    delegate void MessageDispatched(object message);

    public abstract class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        internal static MessageDispatched MessageDispatched;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        internal virtual CancelEventHandler Closing { get; set; }

        protected void Show(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.Show();
        }

        protected void ShowDialog(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }
    }
}
