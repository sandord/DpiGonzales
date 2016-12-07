using System;
using System.Windows;
using System.Windows.Input;

namespace DpiGonzales
{
    public class TrayViewModel
    {
        public ICommand ShowMainWindowCommand { get; } = new DelegateCommand
        {
            CanExecuteFunc = () =>
            {
#if DEBUG
                var ns = Application.Current.MainWindow?.GetType().Namespace;

                if (ns != null && ns.Contains("XamlDiagnostics"))
                {
                    return true;
                }
#endif
                return Application.Current.MainWindow == null;
            },

            CommandAction = () =>
            {
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Application.Current.MainWindow.Show();
            }
        };

        public ICommand ExitApplicationCommand { get; } = new DelegateCommand
        {
            CommandAction = () => Application.Current.Shutdown()
        };
    }

    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
