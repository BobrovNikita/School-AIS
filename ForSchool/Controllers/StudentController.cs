using ForSchool.Models;
using ForSchool.Repositories;
using ForSchool.Views.Intefraces;
using ForSchool.Views.Interfaces;
using ForSchool.Views.ViewModels;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace ForSchool.Controllers
{
    public class StudentController
    {
        private readonly IStudentView _view;
        private readonly IMainView _mainView;
        private readonly IRepository<StudentViewModel> _repository;
        private readonly IRepository<ClassViewModel> _classRepository;

        private BindingSource studentBindingSource;
        private BindingSource classBindingSource;

        private IEnumerable<StudentViewModel>? _students;
        private IEnumerable<ClassViewModel>? _classes;

        public StudentController(IStudentView view, IRepository<StudentViewModel> repository, IRepository<ClassViewModel> classRepository, IMainView mainView)
        {
            _view = view;
            _repository = repository;
            _classRepository = classRepository;
            _mainView = mainView;

            studentBindingSource = new BindingSource();
            classBindingSource = new BindingSource();

            view.SearchEvent += Search;
            view.AddNewEvent += Add;
            view.EditEvent += LoadSelectedToEdit;
            view.DeleteEvent += DeleteSelected;
            view.SaveEvent += Save;
            view.CancelEvent += CancelAction;
            view.PrintWord += WordAction;
            view.PrintExcel += ExcelAction;

            LoadStudentList();
            LoadCombobox();

            view.SetStudenBindingSource(studentBindingSource);
            view.SetClassBindingSource(classBindingSource);

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
            var properties = typeof(StudentViewModel).GetProperties();
            int startIndex = 2; // Пропускаем первый столбец

            // Добавляем заголовки столбцов с использованием DisplayName
            for (int i = startIndex; i < properties.Length; i++)
            {
                var prop = properties[i];
                var displayName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? (i - startIndex + 1).ToString();
                worksheet.Cells[1, i - startIndex + 1] = displayName;
            }

            // Заполняем строки данными из коллекции, игнорируя первый столбец
            int rowIndex = 2;
            foreach (var item in _students)
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
            paragraph.Range.Text = "Ученики";
            paragraph.Range.InsertParagraphAfter();
            // Получаем список свойств (столбцов)
            var properties = typeof(StudentViewModel).GetProperties();
            int startIndex = 2;

            // Создаем таблицу в Word с количеством строк и столбцов
            int rowCount = 1 + (_students == null ? 0 : ((ICollection<StudentViewModel>)_students).Count);
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
            foreach (var item in _students)
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

        private void LoadStudentList()
        {
            _students = _repository.GetAll();
            studentBindingSource.DataSource = _students;
        }

        private void LoadCombobox()
        {
            _classes = _classRepository.GetAll();
            classBindingSource.DataSource = _classes;
        }

        private void CleanViewFields()
        {
            _view.Id = Guid.Empty;
            _view.ClassId = new ClassViewModel();
            _view.surname = string.Empty;
            _view.firstname = string.Empty;
            _view.lastname = string.Empty;
            _view.phoneNumber= string.Empty;
            _view.adress = string.Empty;
        }

        private void CancelAction(object? sender, EventArgs e)
        {
            CleanViewFields();
        }

        private void Save(object? sender, EventArgs e)
        {
            var model = new StudentViewModel();
            model.ClassId = _view.ClassId.Id;
            model.Id = _view.Id;
            model.Surname = _view.surname;
            model.FirstName = _view.firstname;
            model.LastName = _view.lastname;
            model.PhoneNumber = _view.phoneNumber;
            model.Adress = _view.adress;
            model.ClassNumber = _view.ClassId.Number;

            try
            {
                if (_view.IsEdit)
                {
                    _repository.Update(model);
                    _view.Message = "Ученик был обновлен";
                }
                else
                {
                    _repository.Create(model);
                    _view.Message = "Ученик был добавлен";
                }
                _view.IsSuccessful = true;
                LoadStudentList();
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
                var model = (StudentViewModel)studentBindingSource.Current;

                _repository.Delete(model);
                _view.IsSuccessful = true;
                _view.Message = "Ученик был удален";
                LoadStudentList();
            }
            catch (Exception)
            {
                _view.IsSuccessful = false;
                _view.Message = "Невозможно удалить ученика";
            }
        }

        private void LoadSelectedToEdit(object? sender, EventArgs e)
        {
            var model = (StudentViewModel)studentBindingSource.Current;
            _view.Id = model.Id;
            _view.ClassId.Id = model.ClassId;
            _view.surname = model.Surname;
            _view.firstname = model.FirstName;
            _view.lastname = model.LastName;
            _view.phoneNumber = model.PhoneNumber;
            _view.adress= model.Adress;
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
                _students = _repository.GetAllByValue(_view.searchValue);
            else
                _students = _repository.GetAll();

            studentBindingSource.DataSource = _students;
        }
    }
}
