using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CV19.ViewModels.Base
{
    /// <summary>
    /// Базовый класс для ViewModel
    /// 
    /// </summary>
    internal abstract class ViewModel : INotifyPropertyChanged // Интерфейс который уведомляет визуальную часть
                                                               // об изменениях в нашей модели
    {
        // Событие на изменение модели
        public event PropertyChangedEventHandler? PropertyChanged;

        // Метод генерации события
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        // CallerMemberName - атрибут для компилятора для самостоятельного
        // определения propertyName
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T> (ref T field, T value, [CallerMemberName] string PropertyName = null)
                                        // field - Ссылка на поле данные из которого необходимо обновить
                                        // value - Новое значение этого поля 
        {
            // Если новое значение == старому прерываем обновление
            if(Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName); 
            return true;
        }
    }
}
