using ForSchool.Models;
using ForSchool.Repositories;
using ForSchool.Views;
using ForSchool.Views.Intefraces;
using ForSchool.Views.Interfaces;
using ForSchool.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Controllers
{
    public class MainController
    {
        private readonly IMainView _mainView;

        public MainController(IMainView mainView)
        {
            _mainView = mainView;

            _mainView.LoadAttendance += LoadAttendance;
            _mainView.LoadClass += LoadClass;
            _mainView.LoadFunctionality += LoadFunctionality;
            _mainView.LoadProject += LoadProject;
            _mainView.LoadStudent += LoadStudent;
            _mainView.LoadTeacher += LoadTeacher;
        }

        private void LoadAttendance(object? sender, EventArgs e)
        {
            IAttendanceView view = AttendanceView.GetInstance((MainView)_mainView);
            IRepository<AttendanceViewModel> repository = new AttendanceRepository(new ApplicationContext());
            IRepository<StudentViewModel> studentRepository = new StudentRepository(new ApplicationContext());
            new AttendanceController(view, repository, studentRepository,_mainView);
        }

        private void LoadClass(object? sender, EventArgs e)
        {
            IClassView view = ClassView.GetInstance((MainView)_mainView);
            IRepository<ClassViewModel> repository = new ClassRepository(new ApplicationContext());
            new ClassController(view, repository, _mainView);
        }

        private void LoadFunctionality(object? sender, EventArgs e)
        {
            IFunctionalityView view = FunctionalityView.GetInstance((MainView)_mainView);
            IRepository<FunctionalityViewModel> repository = new FunctionalityRepository(new ApplicationContext());
            IRepository<StudentViewModel> studentRepository = new StudentRepository(new ApplicationContext());
            IRepository<ProjectViewModel> projectRepository = new ProjectRepository(new ApplicationContext());
            new FunctionalityController(view, repository, studentRepository, projectRepository, _mainView);
        }

        private void LoadProject(object? sender, EventArgs e)
        {
            IProjectView view = ProjectView.GetInstance((MainView)_mainView);
            IRepository<ProjectViewModel> repository = new ProjectRepository(new ApplicationContext());
            new ProjectController(view, repository, _mainView);
        }

        private void LoadStudent(object? sender, EventArgs e)
        {
            IStudentView view = StudentView.GetInstance((MainView)_mainView);
            IRepository<StudentViewModel> repository = new StudentRepository(new ApplicationContext());
            IRepository<ClassViewModel> classRepository = new ClassRepository(new ApplicationContext());
            new StudentController(view, repository, classRepository, _mainView);
        }

        private void LoadTeacher(object? sender, EventArgs e)
        {
            ITeacherView view = TeacherView.GetInstance((MainView)_mainView);
            IRepository<TeacherViewModel> repository = new TeacherRepository(new ApplicationContext());
            IRepository<ClassViewModel> classRepository = new ClassRepository(new ApplicationContext());
            IRepository<ProjectViewModel> projectRepository = new ProjectRepository(new ApplicationContext());
            new TeacherController(view, repository, classRepository, projectRepository, _mainView);
        }
    }

}
