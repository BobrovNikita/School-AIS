using ForSchool.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.Interfaces
{
    public interface IFunctionalityView
    {
        Guid Id { get; set; }
        ProjectViewModel ProjectId { get; set; }
        StudentViewModel StudentId { get; set; }
        
        int Mark { get; set; }
        int Quarter { get; set; }

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

        void SetProjectBindingSource(BindingSource source);
        void SetStudenBindingSource(BindingSource source);
        void SetFunctionalityBindingSource(BindingSource source);
        void Show();
    }
}
