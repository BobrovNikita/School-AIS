using ForSchool.Views.Intefraces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForSchool.Views
{
    public partial class MainView : Form, IMainView
    {
        public MainView()
        {
            InitializeComponent();

            InitializeBtnEvents();
        }

        private void InitializeBtnEvents()
        {
            AttendanceBtn.Click += delegate { LoadAttendance?.Invoke(this, EventArgs.Empty); };
            ClassBtn.Click += delegate { LoadClass?.Invoke(this, EventArgs.Empty); };
            FunctionalityBtn.Click += delegate { LoadFunctionality?.Invoke(this, EventArgs.Empty); };
            ProjectBtn.Click += delegate { LoadProject?.Invoke(this, EventArgs.Empty); };
            StudentBtn.Click += delegate { LoadStudent?.Invoke(this, EventArgs.Empty); };
            TeacherBtn.Click += delegate { LoadTeacher?.Invoke(this, EventArgs.Empty); };
            
            FormClosed += delegate { Application.Exit(); };
        }

        public event EventHandler LoadAttendance;
        public event EventHandler LoadClass;
        public event EventHandler LoadFunctionality;
        public event EventHandler LoadProject;
        public event EventHandler LoadStudent;
        public event EventHandler LoadTeacher;

    }
}
