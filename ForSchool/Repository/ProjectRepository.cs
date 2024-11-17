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
    public class ProjectRepository : BaseRepository, IRepository<ProjectViewModel>
    {
        public ProjectRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(ProjectViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Project model = new Project();
                model.ProjectId = viewModel.Id;
                model.Project_Name = viewModel.Project_Name;
                model.Count = viewModel.Count;
                    
                new Common.ModelDataValidation().Validate(model);

                context.Projects.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(ProjectViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Project model = new Project();
                model.ProjectId = viewModel.Id;
                model.Project_Name = viewModel.Project_Name;
                model.Count = viewModel.Count;
                context.Projects.Remove(model);
                context.SaveChanges();  
            }
            
        }

        public IEnumerable<ProjectViewModel> GetAll()
        {
            return db.Projects.Select(o => new ProjectViewModel
            {
                Id = o.ProjectId,
                Project_Name = o.Project_Name,
                Count = o.Count,
            }).ToList();
        }

        public IEnumerable<ProjectViewModel> GetAllByValue(string value)
        {
            var result = db.Projects.Where(s => s.Count.ToString().Contains(value) || 
                                                   s.Project_Name.Contains(value)
                                             );

            return result.Select(o => new ProjectViewModel
            {
                Id = o.ProjectId,
                Project_Name = o.Project_Name,
                Count = o.Count,
            }).ToList();
        }

        public ProjectViewModel GetModel(Guid id)
        {
            var result = db.Projects.First(s => s.ProjectId== id);

            ProjectViewModel model = new ProjectViewModel();
            model.Id = result.ProjectId;
            model.Count = result.Count;
            model.Project_Name = result.Project_Name;

            return model;
        }

        public void Update(ProjectViewModel viewModel)
        {
            using (var context = new ApplicationContext())
            {
                Project model = new Project();
                model.ProjectId = viewModel.Id;
                model.Count = viewModel.Count;
                model.Project_Name = viewModel.Project_Name;
                
                new Common.ModelDataValidation().Validate(model);

                context.Projects.Update(model);
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
