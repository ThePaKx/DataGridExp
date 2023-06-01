using DataGridExp.Db;
using DataGridExp.Models;
using DataGridExp.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataGridExp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            localDb = new localDb();
            QueryCommand = new RelayCommand(Query);
            ResetCommand = new RelayCommand(() =>
            {
                Search = string.Empty;

                this.Query();
            });
            EditCommand = new RelayCommand<int>(t => Edit(t));
            DeleteCommand = new RelayCommand<int>(t => Del(t));
            AddCommand = new RelayCommand(Add);
        }

        private localDb localDb;
        private string search = string.Empty;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Student> gridModelList;

        public ObservableCollection<Student> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        #region Command

        public RelayCommand QueryCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }

        public RelayCommand<int> EditCommand { get; set; }
        public RelayCommand<int> DeleteCommand { get; set; }
        public RelayCommand AddCommand { get; set; }

        #endregion Command

        public void Query()
        {
            var models = localDb.GetStudentsByName(Search);
            GridModelList = new ObservableCollection<Student>();
            if (models != null)
            {
                models.ForEach(arg =>
                {
                    GridModelList.Add(arg);
                });
            }
        }

        public void Edit(int id)
        {
            var model = localDb.GetStudentById(id);
            if (model != null)
            {
                UserView view = new UserView(model);
                var r = view.ShowDialog();
                if (r.Value)
                {
                    var newModel = GridModelList.FirstOrDefault(t => t.Id == model.Id);
                    if (newModel != null)
                    {
                        newModel.Name = model.Name;
                    }
                }
            }
        }

        public void Add()
        {
            Student student = new Student();
            UserView view = new UserView(student);
            var r = view.ShowDialog();
            if (r.Value)
            {
                student.Id = GridModelList.Max(t => t.Id) + 1;
                localDb.AddStudent(student);
                this.Query();
            }
        }

        public void Del(int id)
        {
            var model = localDb.GetStudentById(id);
            if (model != null)
            {
                var r = MessageBox.Show($"确认删除当前用户：{model.Name}?", "操作提示", MessageBoxButton.OK, MessageBoxImage.Question);
                if (r == MessageBoxResult.OK)
                    localDb.DelStudent(model.Id);
                this.Query();
            }
        }
    }
}