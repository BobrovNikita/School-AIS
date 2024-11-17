using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.ViewModels
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Предмет")]
        public string Project_Name { get; set; }

        [DisplayName("Кол-во занятий")]
        public int Count { get; set; }
    }
}
