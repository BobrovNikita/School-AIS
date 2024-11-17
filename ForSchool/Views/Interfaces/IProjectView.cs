using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.Interfaces
{
    public interface IProjectView
    {
        //Fields
        Guid Id { get; set; }
        string ProjectName { get; set; }
        int Count { get; set; }

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
        void Show();
    }
}
