using ForSchool.Models;
using ForSchool.Repositories;
using ForSchool.Views.Intefraces;
using ForSchool.Views.Interfaces;
using ForSchool.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.ComponentModel;

namespace ForSchool.Controllers
{
    public class TeacherController
    {
        private readonly ITeacherView _view;
        private readonly IMainView _mainView;
        private readonly IRepository<TeacherViewModel> _repository;
        private readonly IRepository<ClassViewModel> _classRepository;
        private readonly IRepository<ProjectViewModel> _projectRepository;

        private BindingSource teacherBindingSource;
        private BindingSource classBindingSource;
        private BindingSource projectBindingSource;

        private IEnumerable<TeacherViewModel>? _teachers;
        private IEnumerable<ClassViewModel>? _classes;
        private IEnumerable<ProjectViewModel>? _projects;

        public TeacherController(ITeacherView view, IRepository<TeacherViewModel> repository, IRepository<ClassViewModel> classRepository, IRepository<ProjectViewModel> projectRepository, IMainView mainView)
        {
            _view = view;
            _repository = repository;
            _classRepository = classRepository;
            _projectRepository = projectRepository;
            _mainView = mainView;

            teacherBindingSource = new BindingSource();
            classBindingSource = new BindingSource();
            projectBindingSource = new BindingSource();

            view.SearchEvent += Search;
            view.AddNewEvent += Add;
            view.EditEvent += LoadSelectedToEdit;
            view.DeleteEvent += DeleteSelected;
            view.SaveEvent += Save;
            view.CancelEvent += CancelAction;
            view.PrintWord += WordAction;
            view.PrintExcel += ExcelAction;

            LoadFunctionalityList();
            LoadCombobox();

            view.SetTeacherBindingSource(teacherBindingSource);
            view.SetClassBindingSource(classBindingSource);
            view.SetProjectBindingSource(projectBindingSource);

            _view.Show();
        }

        private void ExcelAction(object? sender, EventArgs e)
        {
            // Создаем новое приложение Excel
            var excelApp = new Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Excel.Worksheet)workbook.Sheets[1];
            excelApp.Visible = true;

            // Получаем список свойств, игнорируя первый столбец (GUID)
            var properties = typeof(TeacherViewModel).GetProperties();
            int startIndex = 3; // Пропускаем первый столбец

            // Добавляем заголовки столбцов с использованием DisplayName
            for (int i = startIndex; i < properties.Length; i++)
            {
                var prop = properties[i];
                var displayName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? (i - startIndex + 1).ToString();
                worksheet.Cells[1, i - startIndex + 1] = displayName;
            }

            // Заполняем строки данными из коллекции, игнорируя первый столбец
            int rowIndex = 2;
            foreach (var item in _teachers)
            {
                for (int colIndex = startIndex; colIndex < properties.Length; colIndex++)
                {
                    var value = properties[colIndex].GetValue(item, null)?.ToString() ?? "";
                    worksheet.Cells[rowIndex, colIndex - startIndex + 1] = value;
                }
                rowIndex++;
            }

            // Настройка форматирования и автонастройка ширины колонок
            worksheet.Columns.AutoFit();

            // Освобождение ресурсов
            ReleaseObject(worksheet);
            ReleaseObject(workbook);
            ReleaseObject(excelApp);
        }

        private void WordAction(object? sender, EventArgs e)
        {
            var wordApp = new Word.Application();
            wordApp.Visible = true;
            var document = wordApp.Documents.Add();
            var paragraph = document.Content.Paragraphs.Add();
            paragraph.Range.Text = "Предметы";
            paragraph.Range.InsertParagraphAfter();
            // Получаем список свойств (столбцов)
            var properties = typeof(TeacherViewModel).GetProperties();
            int startIndex = 3;

            // Создаем таблицу в Word с количеством строк и столбцов
            int rowCount = 1 + (_teachers == null ? 0 : ((ICollection<TeacherViewModel>)_teachers).Count);
            var table = document.Tables.Add(paragraph.Range, rowCount, properties.Length - startIndex);
            table.Borders.Enable = 1;

            // Добавляем заголовки столбцов с использованием DisplayName
            for (int i = startIndex; i < properties.Length; i++)
            {
                var prop = properties[i];
                var displayName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? (i - startIndex + 1).ToString();

                table.Cell(1, i - startIndex + 1).Range.Text = displayName;
                table.Cell(1, i - startIndex + 1).Range.Bold = 1;
                table.Cell(1, i - startIndex + 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }

            // Заполняем строки данными из коллекции, игнорируя первый столбец
            int rowIndex = 2;
            foreach (var item in _teachers)
            {
                for (int colIndex = startIndex; colIndex < properties.Length; colIndex++)
                {
                    var value = properties[colIndex].GetValue(item, null)?.ToString() ?? "";
                    table.Cell(rowIndex, colIndex - startIndex + 1).Range.Text = value;
                }
                rowIndex++;
            }

            // Освобождение ресурсов
            ReleaseObject(table);
            ReleaseObject(paragraph);
            ReleaseObject(document);
            ReleaseObject(wordApp);

        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Ошибка при освобождении объекта: " + ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void LoadFunctionalityList()
        {
            _teachers = _repository.GetAll();
            teacherBindingSource.DataSource = _teachers;
        }

        private void LoadCombobox()
        {
            _classes = _classRepository.GetAll();
            classBindingSource.DataSource = _classes;

            _projects = _projectRepository.GetAll();
            projectBindingSource.DataSource = _projects;
        }

        private void CleanViewFields()
        {
            _view.Id = Guid.Empty;
            _view.ProjectId = new ProjectViewModel();
            _view.ClassId = new ClassViewModel();
            _view.surname = string.Empty;
            _view.firstname = string.Empty;
            _view.lastname = string.Empty;
            _view.phoneNumber = string.Empty;
        }

        private void CancelAction(object? sender, EventArgs e)
        {
            CleanViewFields();
        }

        private void Save(object? sender, EventArgs e)
        {
            var model = new TeacherViewModel();
            model.ProjectId = _view.ProjectId.Id;
            model.ClassId = _view.ClassId.Id;
            model.Id = _view.Id;
            model.Surname = _view.surname;
            model.FirstName = _view.firstname;
            model.LastName = _view.lastname;
            model.PhoneNumber = _view.phoneNumber;
            model.Number = _view.ClassId.Number;
            model.Project = _view.ProjectId.Project_Name;

            try
            {
                if (_view.IsEdit)
                {
                    _repository.Update(model);
                    _view.Message = "Запись была обновлена";
                }
                else
                {
                    _repository.Create(model);
                    _view.Message = "Запись была добавлена";
                }
                _view.IsSuccessful = true;
                LoadFunctionalityList();
                CleanViewFields();
            }
            catch (Exception ex)
            {
                _view.IsSuccessful = false;
                _view.Message = ex.Message;
            }
        }

        private void DeleteSelected(object? sender, EventArgs e)
        {
            try
            {
                var model = (TeacherViewModel)teacherBindingSource.Current;

                _repository.Delete(model);
                _view.IsSuccessful = true;
                _view.Message = "Запись была удалена";
                LoadFunctionalityList();
            }
            catch (Exception)
            {
                _view.IsSuccessful = false;
                _view.Message = "Невозможно удалить запись";
            }
        }

        private void LoadSelectedToEdit(object? sender, EventArgs e)
        {
            var model = (TeacherViewModel)teacherBindingSource.Current;
            _view.Id = model.Id;
            _view.ProjectId.Id = model.ProjectId;
            _view.ClassId.Id = model.ClassId;
            _view.surname = model.Surname;
            _view.firstname = model.FirstName;
            _view.lastname = model.LastName;
            _view.phoneNumber = model.PhoneNumber;
            _view.IsEdit = true;
        }

        private void Add(object? sender, EventArgs e)
        {
            _view.IsEdit = false;
        }

        private void Search(object? sender, EventArgs e)
        {
            bool emptyValue = String.IsNullOrWhiteSpace(_view.searchValue);

            if (emptyValue == false)
                _teachers = _repository.GetAllByValue(_view.searchValue);
            else
                _teachers = _repository.GetAll();

            teacherBindingSource.DataSource = _teachers;
        }
    }
}
