using ForSchool.Models;
using ForSchool.Views.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ForSchool.Repositories
{
    public class TeacherRepository : BaseRepository, IRepository<TeacherViewModel>
    {
        public TeacherRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(TeacherViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Teacher model = new Teacher();
                model.TeacherId = viewModel.Id;
                model.ProjectId = viewModel.ProjectId;
                model.ClassId = viewModel.ClassId;
                model.Surname = viewModel.Surname;
                model.Firstname = viewModel.FirstName;
                model.PhoneNumber= viewModel.PhoneNumber;
                model.Lastname = viewModel.LastName;

                if (!Regex.IsMatch(model.PhoneNumber, @"^(\+375)\((29|25|44|33)\) (\d{3})-(\d{2})-(\d{2})$"))
                    model.PhoneNumber = "";

                new Common.ModelDataValidation().Validate(model);

                context.Teachers.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(TeacherViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Teacher model = new Teacher();
                model.TeacherId = viewModel.Id;
                model.ProjectId = viewModel.ProjectId;
                model.ClassId = viewModel.ClassId;
                model.Surname = viewModel.Surname;
                model.Firstname = viewModel.FirstName;
                model.Firstname = viewModel.FirstName;
                model.Lastname = viewModel.LastName;
                context.Teachers.Remove(model);
                context.SaveChanges();  
            }
            
        }

        public IEnumerable<TeacherViewModel> GetAll()
        {
            return db.Teachers.Include(s => s.Project).Include(p => p.Class).Select(o => new TeacherViewModel
            {
                Id = o.TeacherId,
                ProjectId = o.ProjectId,
                ClassId = o.ClassId,
                Surname = o.Surname,
                FirstName = o.Firstname,
                LastName = o.Lastname,
                PhoneNumber = o.PhoneNumber,
                Project = o.Project.Project_Name,
                Number = o.Class.Number,

            }).ToList();
        }

        public IEnumerable<TeacherViewModel> GetAllByValue(string value)
        {
            var result = db.Teachers.Include(st => st.Class).Include(p => p.Project).Where(s => s.Class.Number.ToString().Contains(value) || 
                                                   s.Surname.Contains(value) ||
                                                   s.Firstname.Contains(value) ||
                                                   s.Lastname.Contains(value) ||
                                                   s.Project.Project_Name.Contains(value)
                                             );

            return result.Select(o => new TeacherViewModel
            {
                Id = o.TeacherId,
                ProjectId = o.ProjectId,
                ClassId = o.ClassId,
                Surname = o.Surname,
                FirstName = o.Firstname,
                LastName = o.Lastname,
                PhoneNumber = o.PhoneNumber,
                Project = o.Project.Project_Name,
                Number = o.Class.Number
            }).ToList();
        }

        public TeacherViewModel GetModel(Guid id)
        {
            var result = db.Teachers.Include(s => s.Class).Include(p => p.Project).First(s => s.TeacherId == id);

            TeacherViewModel model = new TeacherViewModel();
            model.Id = result.TeacherId;
            model.ProjectId = result.ProjectId;
            model.ClassId = result.ClassId;
            model.Surname = result.Surname;
            model.FirstName = result.Firstname;
            model.LastName = result.Lastname;
            model.PhoneNumber = result.PhoneNumber;
            model.Project = result.Project.Project_Name;
            model.Number = result.Class.Number;
            return model;
        }

        public void Update(TeacherViewModel viewModel)
        {
            using (var context = new ApplicationContext())
            {
                Teacher model = new Teacher();
                model.TeacherId = viewModel.Id;
                model.ProjectId = viewModel.ProjectId;
                model.ClassId = viewModel.ClassId;
                model.Surname = viewModel.Surname;
                model.Firstname = viewModel.FirstName;
                model.PhoneNumber = viewModel.PhoneNumber;
                model.Lastname = viewModel.LastName;

                if (!Regex.IsMatch(model.PhoneNumber, @"^(\+375)\((29|25|44|33)\) (\d{3})-(\d{2})-(\d{2})$"))
                    model.PhoneNumber = "";

                new Common.ModelDataValidation().Validate(model);

                context.Teachers.Update(model);
                context.SaveChanges();
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
