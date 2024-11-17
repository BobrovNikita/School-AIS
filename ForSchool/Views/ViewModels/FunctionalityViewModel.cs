using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.ViewModels
{
    public class FunctionalityViewModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid StudentId { get; set; }

        [DisplayName("Оценка")]
        public int Mark { get; set; }

        [DisplayName("Четверть")]
        public int Quarter { get; set; }

        [DisplayName("Фамилия")]
        public string Surname { get; set; }

        [DisplayName("Предмет")]
        public string Project { get; set; }
    }
}
