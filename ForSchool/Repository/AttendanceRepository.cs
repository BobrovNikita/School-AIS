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
    public class AttendanceRepository : BaseRepository, IRepository<AttendanceViewModel>
    {
        public AttendanceRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(AttendanceViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Attendance model = new Attendance();
                model.AttendanceId = viewModel.Id;
                model.SkipCount = viewModel.SkipCount;
                model.Status = viewModel.Status;
                model.StudentId = viewModel.StudentId;
                    
                new Common.ModelDataValidation().Validate(model);

                context.Attendances.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(AttendanceViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Attendance model = new Attendance();
                model.AttendanceId = viewModel.Id;
                model.SkipCount = viewModel.SkipCount;
                model.Status = viewModel.Status;
                model.StudentId = viewModel.StudentId;
                context.Attendances.Remove(model);
                context.SaveChanges();  
            }
            
        }

        public IEnumerable<AttendanceViewModel> GetAll()
        {
            return db.Attendances.Include(s => s.Student).Select(o => new AttendanceViewModel
            {
                Id = o.AttendanceId,
                SkipCount = o.SkipCount,
                Status = o.Status,
                StudentId = o.StudentId,
                Surname = o.Student.Surname,
                FirstName = o.Student.Firstname,
                LastName = o.Student.Lastname
            }).ToList();
        }

        public IEnumerable<AttendanceViewModel> GetAllByValue(string value)
        {
            var result = db.Attendances.Include(st => st.Student).Where(s => s.SkipCount.ToString().Contains(value) || 
                                                   s.Status.Contains(value) ||
                                                   s.Student.Surname.Contains(value) ||
                                                   s.Student.Firstname.Contains(value) ||
                                                   s.Student.Lastname.Contains(value)
                                             );

            return result.Select(o => new AttendanceViewModel
            {
                Id = o.AttendanceId,
                SkipCount = o.SkipCount,
                Status = o.Status,
                StudentId = o.StudentId,
                Surname = o.Student.Surname,
                FirstName = o.Student.Firstname,
                LastName = o.Student.Lastname
            }).ToList();
        }

        public AttendanceViewModel GetModel(Guid id)
        {
            var result = db.Attendances.Include(s => s.Student).First(s => s.AttendanceId== id);

            AttendanceViewModel model = new AttendanceViewModel();
            model.Id = result.AttendanceId;
            model.StudentId = result.StudentId;
            model.SkipCount = result.SkipCount;
            model.Status = result.Status;
            model.Surname = result.Student.Surname;
            model.FirstName = result.Student.Firstname;
            model.LastName = result.Student.Lastname;

            return model;
        }

        public void Update(AttendanceViewModel viewModel)
        {
            using (var context = new ApplicationContext())
            {
                Attendance model = new Attendance();
                model.AttendanceId = viewModel.Id;
                model.StudentId = viewModel.StudentId;
                model.SkipCount = viewModel.SkipCount;
                model.Status = viewModel.Status;
                
                new Common.ModelDataValidation().Validate(model);

                context.Attendances.Update(model);
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
