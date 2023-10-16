using System;
using System.Windows.Input;

namespace CV19.Infrastructure.Commands.Base
{
    internal abstract class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            // Передаем управление событием классу CommandManager
            // При этом WPF автоматически генерирует выполнение всех команд, когда возникает событие

            // Добавляем обработчик событий, который пытаются добавить в CanExecuteChanged
            add => CommandManager.RequerySuggested += value;

            // Удаляем обработчик событий.
            remove => CommandManager.RequerySuggested -= value;
        }

        // Когда CanExecute 
        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
        
    }
}
