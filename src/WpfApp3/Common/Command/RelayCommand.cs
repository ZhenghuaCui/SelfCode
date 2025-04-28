using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wuhua.Main.Common.Command
{
    /// <summary>
    /// 带参数的泛型命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;

        public RelayCommand(Action<T> execute)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
            this.execute = execute;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null ? true : canExecute.Invoke((T)parameter);
        }

        public void Execute(object? parameter)
        {
            execute((T)parameter);
        }
    }

    /// <summary>
    /// 不带参数的泛型命令
    /// </summary>
    //public class RelayCommand : ICommand
    //{
    //    public event EventHandler? CanExecuteChanged;

    //    private readonly Action execute;
    //    public RelayCommand(Action execute)
    //    {
    //        ArgumentNullException.ThrowIfNull(execute, nameof(execute));
    //        this.execute = execute;
    //    }

    //    private readonly Func<bool> canExecute;
    //    public RelayCommand(Action execute, Func<bool> canExecute)
    //    {
    //        ArgumentNullException.ThrowIfNull(execute, nameof(execute));
    //        ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
    //        this.execute = execute;
    //        this.canExecute = canExecute;
    //    }

    //    public bool CanExecute(object? parameter)
    //    {
    //        return canExecute?.Invoke() ?? true;
    //    }

    //    public void Execute(object? parameter)
    //    {
    //        execute();
    //    }
    //}

}
