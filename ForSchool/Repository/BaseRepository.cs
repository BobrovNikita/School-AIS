using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Repositories
{
    public abstract class BaseRepository
    {
        protected ApplicationContext db;

        public BaseRepository(ApplicationContext context)
        {
            db = context;
        }
    }
}
