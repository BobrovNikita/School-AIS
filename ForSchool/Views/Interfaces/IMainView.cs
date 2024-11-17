using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.Intefraces
{
    public interface IMainView
    {
        event EventHandler LoadAttendance;
        event EventHandler LoadClass;
        event EventHandler LoadFunctionality;
        event EventHandler LoadProject;
        event EventHandler LoadStudent;
        event EventHandler LoadTeacher;
    }
}
