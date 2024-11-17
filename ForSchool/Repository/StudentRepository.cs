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
    public class StudentRepository : BaseRepository, IRepository<StudentViewModel>
    {
        public StudentRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(StudentViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Student model = new Student();
                model.StudentId = viewModel.Id;
                model.ClassId = viewModel.ClassId;
                model.Surname = viewModel.Surname;
                model.Firstname = viewModel.FirstName;
                model.Lastname = viewModel.LastName;
                model.PhoneNumber= viewModel.PhoneNumber;
                model.Adress= viewModel.Adress;

                if (!Regex.IsMatch(model.PhoneNumber, @"^(\+375)\((29|25|44|33)\) (\d{3})-(\d{2})-(\d{2})$"))
                    model.PhoneNumber = "";

                new Common.ModelDataValidation().Validate(model);

                context.Students.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(StudentViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Student model = new Student();
                model.StudentId = viewModel.Id;
                model.ClassId = viewModel.ClassId;
                model.Surname = viewModel.Surname;
                model.Firstname = viewModel.FirstName;
                model.Lastname = viewModel.LastName;
                model.PhoneNumber = viewModel.PhoneNumber;
                model.Adress = viewModel.Adress;
                context.Students.Remove(model);
                context.SaveChanges();  
            }
            
        }

        public IEnumerable<StudentViewModel> GetAll()
        {
            return db.Students.Include(s => s.Class).Select(o => new StudentViewModel
            {
                Id = o.StudentId,
                ClassId = o.ClassId,
                Surname = o.Surname,
                FirstName = o.Firstname,
                LastName = o.Lastname,
                PhoneNumber = o.PhoneNumber,
                Adress = o.Adress,
                ClassNumber = o.Class.Number
            }).ToList();
        }

        public IEnumerable<StudentViewModel> GetAllByValue(string value)
        {
            var result = db.Students.Include(st => st.Class).Where(s => s.Class.Number.ToString().Contains(value) || 
                                                   s.Surname.Contains(value) ||
                                                   s.Firstname.Contains(value) ||
                                                   s.Lastname.Contains(value)
                                             );

            return result.Select(o => new StudentViewModel
            {
                Id = o.StudentId,
                ClassId = o.ClassId,
                Surname = o.Surname,
                FirstName = o.Firstname,
                LastName = o.Lastname,
                PhoneNumber = o.PhoneNumber,
                Adress = o.Adress,
                ClassNumber = o.Class.Number
            }).ToList();
        }

        public StudentViewModel GetModel(Guid id)
        {
            var result = db.Students.Include(s => s.Class).First(s => s.StudentId == id);

            StudentViewModel model = new StudentViewModel();
            model.Id = result.StudentId;
            model.ClassId = result.ClassId;
            model.Surname = result.Surname;
            model.FirstName = result.Firstname;
            model.LastName = result.Lastname;
            model.PhoneNumber = result.PhoneNumber;
            model.Adress = result.Adress;
            model.ClassNumber= result.Class.Number;

            return model;
        }

        public void Update(StudentViewModel viewModel)
        {
            using (var context = new ApplicationContext())
            {
                Student model = new Student();
                model.StudentId = viewModel.Id;
                model.ClassId = viewModel.ClassId;
                model.Surname = viewModel.Surname;
                model.Firstname = viewModel.FirstName;
                model.Lastname = viewModel.LastName;
                model.PhoneNumber = viewModel.PhoneNumber;
                model.Adress = viewModel.Adress;

                if (!Regex.IsMatch(model.PhoneNumber, @"^(\+375)\((29|25|44|33)\) (\d{3})-(\d{2})-(\d{2})$"))
                    model.PhoneNumber = "";

                new Common.ModelDataValidation().Validate(model);

                context.Students.Update(model);
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
