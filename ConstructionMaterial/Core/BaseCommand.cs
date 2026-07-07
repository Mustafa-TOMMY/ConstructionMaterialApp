using System;
using System.Windows.Input;

namespace ConstructionMaterial.Core
{
    public class BaseCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;
        public BaseCommand(Action execute) : this(p => execute())
        {
        }
        public BaseCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            //ArgumentNullException.ThrowIfNull(execute);
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand part
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            //return _canExecute == null ? true : _canExecute(parameter);
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
}