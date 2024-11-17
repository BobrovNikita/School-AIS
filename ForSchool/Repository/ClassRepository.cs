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
    public class ClassRepository : BaseRepository, IRepository<ClassViewModel>
    {
        public ClassRepository(ApplicationContext context) : base(context)
        {
        }

        public void Create(ClassViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Class model = new Class();
                model.ClassId = viewModel.Id;
                model.Number = viewModel.Number;
                model.Year = viewModel.Year;
                model.Count = viewModel.Count;
                    
                new Common.ModelDataValidation().Validate(model);

                context.Classes.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(ClassViewModel viewModel)
        {
            using(var context = new ApplicationContext())
            {
                Class model = new Class();
                model.ClassId = viewModel.Id;
                model.Number = viewModel.Number;
                model.Year = viewModel.Year;
                model.Count = viewModel.Count;
                context.Classes.Remove(model);
                context.SaveChanges();  
            }
            
        }

        public IEnumerable<ClassViewModel> GetAll()
        {
            return db.Classes.Select(o => new ClassViewModel
            {
                Id = o.ClassId,
                Number = o.Number,
                Year = o.Year,
                Count = o.Count
            }).ToList();
        }

        public IEnumerable<ClassViewModel> GetAllByValue(string value)
        {
            var result = db.Classes.Where(s => s.Number.ToString().Contains(value) || 
                                                   s.Year.ToString().Contains(value) ||
                                                   s.Count.ToString().Contains(value)
                                             );

            return result.Select(o => new ClassViewModel
            {
                Id = o.ClassId,
                Number = o.Number,
                Year = o.Year,
                Count = o.Count,
            }).ToList();
        }

        public ClassViewModel GetModel(Guid id)
        {
            var result = db.Classes.First(s => s.ClassId== id);

            ClassViewModel model = new ClassViewModel();
            model.Id = result.ClassId;
            model.Count = result.Count;
            model.Number = result.Number;
            model.Year = result.Year;

            return model;
        }

        public void Update(ClassViewModel viewModel)
        {
            using (var context = new ApplicationContext())
            {
                Class model = new Class();
                model.ClassId = viewModel.Id;
                model.Count = viewModel.Count;
                model.Number = viewModel.Number;
                model.Year = viewModel.Year;
                
                new Common.ModelDataValidation().Validate(model);

                context.Classes.Update(model);
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
