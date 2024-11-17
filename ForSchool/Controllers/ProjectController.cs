using ForSchool.Repositories;
using ForSchool.Views;
using ForSchool.Views.Intefraces;
using ForSchool.Views.Interfaces;
using ForSchool.Views.ViewModels;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace ForSchool.Controllers
{
    public class ProjectController
    {
        private readonly IProjectView _view;
        private readonly IMainView _mainView;
        private readonly IRepository<ProjectViewModel> _repository;

        private BindingSource projectBindingSource;

        private IEnumerable<ProjectViewModel>? _projects;

        public ProjectController(IProjectView view, IRepository<ProjectViewModel> repository, IMainView mainView)
        {
            _view = view;
            _repository = repository;
            _mainView = mainView;

            projectBindingSource = new BindingSource();

            view.SearchEvent += Search;
            view.AddNewEvent += Add;
            view.EditEvent += LoadSelectedToEdit;
            view.DeleteEvent += DeleteSelected;
            view.SaveEvent += Save;
            view.CancelEvent += CancelAction;
            view.PrintWord += WordAction;
            view.PrintExcel += ExcelAction;

            LoadProjectList();

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
            var properties = typeof(ProjectViewModel).GetProperties();
            int startIndex = 1; // Пропускаем первый столбец

            // Добавляем заголовки столбцов с использованием DisplayName
            for (int i = startIndex; i < properties.Length; i++)
            {
                var prop = properties[i];
                var displayName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? (i - startIndex + 1).ToString();
                worksheet.Cells[1, i - startIndex + 1] = displayName;
            }

            // Заполняем строки данными из коллекции, игнорируя первый столбец
            int rowIndex = 2;
            foreach (var item in _projects)
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
            var properties = typeof(ProjectViewModel).GetProperties();
            int startIndex = 1;

            // Создаем таблицу в Word с количеством строк и столбцов
            int rowCount = 1 + (_projects == null ? 0 : ((ICollection<ProjectViewModel>)_projects).Count);
            var table = document.Tables.Add(paragraph.Range, rowCount, properties.Length - 1);
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
            foreach (var item in _projects)
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

        private void LoadProjectList()
        {
            _projects = _repository.GetAll();
            projectBindingSource.DataSource = _projects;
        }

        private void CleanViewFields()
        {
            _view.Id = Guid.Empty;
            _view.Count = -1;
            _view.ProjectName = string.Empty;
        }

        private void CancelAction(object? sender, EventArgs e)
        {
            CleanViewFields();
        }

        private void Save(object? sender, EventArgs e)
        {
            var model = new ProjectViewModel();
            model.Id = _view.Id;
            model.Count = _view.Count;
            model.Project_Name = _view.ProjectName;
            try
            {
                if (_view.IsEdit)
                {
                    _repository.Update(model);
                    _view.Message = "Предмет успешно обновлен";
                }
                else
                {
                    _repository.Create(model);
                    _view.Message = "Предмет успешно добавлен";
                }
                _view.IsSuccessful = true;
                LoadProjectList();
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
                var model = (ProjectViewModel)projectBindingSource.Current;

                _repository.Delete(model);
                _view.IsSuccessful = true;
                _view.Message = "Предмет успешно удалён";
                LoadProjectList();
            }
            catch (Exception)
            {
                _view.IsSuccessful = false;
                _view.Message = "Невозможно удалить предмет";
            }
        }

        private void LoadSelectedToEdit(object? sender, EventArgs e)
        {
            var model = (ProjectViewModel)projectBindingSource.Current;
            _view.Id = model.Id;
            _view.Count = model.Count;
            _view.ProjectName = model.Project_Name;
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
                _projects = _repository.GetAllByValue(_view.searchValue);
            else
                _projects = _repository.GetAll();

            projectBindingSource.DataSource = _projects;
        }
    }
}
