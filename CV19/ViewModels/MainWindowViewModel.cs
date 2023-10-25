using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Models.Decanat;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /*-----------------------------------------------------------------------------*/

        public ObservableCollection<Group> Groups { get; set; }

        #region Выбранная группа

        private Group _selectedGroup;

        /// <summary>Выбранная группа</summary>
        public Group SelectedGroup { get => _selectedGroup; set => Set(ref _selectedGroup, value); }

        #endregion

        #region Номер выбранной вкладки

        private int _selectedPageIndex;

        public int SelectedPageIndex
        {
            get => _selectedPageIndex;
            set => Set(ref _selectedPageIndex, value);
        }

        #endregion

        #region Тестовый набор данных для визуализации графиков

        /// <summary>
        /// Тестовый набор данных для визуализации графиков
        /// </summary>
        private IEnumerable<DataPoint> _TestDataPoints;

        public IEnumerable<DataPoint> TestDataPoints
        {
            get => _TestDataPoints;
            set => Set(ref _TestDataPoints, value);
        }
        #endregion

        #region Заголовок окна

        private string _title = "Анализ статистики";
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        #endregion

        #region Статус программы

        private string _status = "Готов";

        /// <summary>Статус программы</summary>
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        #endregion
        /*-----------------------------------------------------------------------------*/

        #region Команды

        // Реализация команды закрытия приложения через LambdaCommand
        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object p) => true;
        #endregion

        #region

        public ICommand ChangeTabIndexCommand { get; }

        private bool CanChangeTabIndexCommandExecute(object p) => _selectedPageIndex >=0;
        
        private void OnChangeTabIndexCommandExecuted(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }

        #endregion

        #endregion

        /*-----------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            #region Команды
            // Создание команды на закрытие приложения через инстанцирования LambdaCommand двумя методами
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            // Команда на переключение между вкладками
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecuted, CanChangeTabIndexCommandExecute);

            #endregion

            var dataPoints = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double toRad = Math.PI / 180;
                var y = Math.Sin(x * toRad);

                dataPoints.Add(new DataPoint { XValue = x, YValue = y });
            }

            TestDataPoints = dataPoints;

            var studentIndex = 1;

            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"name {studentIndex}",
                Surname = $"Surname {studentIndex}",
                Patronymic = $"Patronymic {studentIndex++}",
                Birthday = DateTime.Now,
                Rating =0
            });

            var groups = Enumerable.Range(1, 20).Select(i => new Group
            {
                Name = $"Группа {i}",
                Students = new ObservableCollection<Student>(students)
            });

            Groups = new ObservableCollection<Group>(groups);


        }
    }
}
