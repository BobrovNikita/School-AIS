using ForSchool.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.Interfaces
{
    public interface ITeacherView
    {
        Guid Id { get; set; }
        ClassViewModel ClassId { get; set; }
        ProjectViewModel ProjectId { get; set; }

        string surname { get; set; }
        string firstname { get; set; }
        string lastname { get; set; }
        string phoneNumber { get; set; }

        string searchValue { get; set; }
        bool IsEdit { get; set; }
        bool IsSuccessful { get; set; }
        string Message { get; set; }

        //Events
        event EventHandler SearchEvent;
        event EventHandler AddNewEvent;
        event EventHandler EditEvent;
        event EventHandler DeleteEvent;
        event EventHandler SaveEvent;
        event EventHandler CancelEvent;
        event EventHandler PrintWord;
        event EventHandler PrintExcel;

        void SetClassBindingSource(BindingSource source);
        void SetProjectBindingSource(BindingSource source);
        void SetTeacherBindingSource(BindingSource source);
        void Show();
    }
}
