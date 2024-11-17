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
    public class FunctionalityRepository : BaseRepository, IRepository<FunctionalityViewModel>
    {
        public FunctionalityRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(FunctionalityViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Functionality model = new Functionality();
                model.FunctionalityId = viewModel.Id;
                model.ProjectId = viewModel.ProjectId;
                model.StudentId = viewModel.StudentId;
                model.Mark = viewModel.Mark;
                model.Quarter = viewModel.Quarter;
                    
                new Common.ModelDataValidation().Validate(model);

                context.Functionalities.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(FunctionalityViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Functionality model = new Functionality();
                model.FunctionalityId = viewModel.Id;
                model.ProjectId = viewModel.ProjectId;
                model.StudentId = viewModel.StudentId;
                model.Mark = viewModel.Mark;
                model.Quarter = viewModel.Quarter;
                context.Functionalities.Remove(model);
                context.SaveChanges();  
            }
            
        }

        public IEnumerable<FunctionalityViewModel> GetAll()
        {
            return db.Functionalities.Include(s => s.Student).Include(p => p.Project).Select(o => new FunctionalityViewModel
            {
                Id = o.FunctionalityId,
                ProjectId = o.ProjectId,
                StudentId = o.StudentId,
                Mark = o.Mark,
                Quarter = o.Quarter,
                Surname = o.Student.Surname,
                Project = o.Project.Project_Name
            }).ToList();
        }

        public IEnumerable<FunctionalityViewModel> GetAllByValue(string value)
        {
            var result = db.Functionalities.Include(st => st.Student).Include(p => p.Project).Where(s => s.Mark.ToString().Contains(value) || 
                                                   s.Quarter.ToString().Contains(value) ||
                                                   s.Student.Surname.Contains(value) ||
                                                   s.Project.Project_Name.Contains(value)
                                             );

            return result.Select(o => new FunctionalityViewModel
            {
                Id = o.FunctionalityId,
                ProjectId = o.ProjectId,
                StudentId = o.StudentId,
                Mark = o.Mark,
                Quarter = o.Quarter,
                Surname = o.Student.Surname,
                Project = o.Project.Project_Name
            }).ToList();
        }

        public FunctionalityViewModel GetModel(Guid id)
        {
            var result = db.Functionalities.Include(s => s.Student).Include(p => p.Project).First(s => s.FunctionalityId == id);

            FunctionalityViewModel model = new FunctionalityViewModel();
            model.Id = result.FunctionalityId;
            model.ProjectId = result.ProjectId;
            model.StudentId = result.StudentId;
            model.Mark = result.Mark;
            model.Quarter = result.Quarter;
            model.Surname = result.Student.Surname;
            model.Project = result.Project.Project_Name;

            return model;
        }

        public void Update(FunctionalityViewModel viewModel)
        {
            using (var context = new ApplicationContext())
            {
                Functionality model = new Functionality();
                model.FunctionalityId = viewModel.Id;
                model.ProjectId = viewModel.ProjectId;
                model.StudentId = viewModel.StudentId;
                model.Mark = viewModel.Mark;
                model.Quarter = viewModel.Quarter;
                
                new Common.ModelDataValidation().Validate(model);

                context.Functionalities.Update(model);
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
