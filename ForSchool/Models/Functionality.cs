using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Models
{
    public class Functionality
    {
        [Required]
        public Guid FunctionalityId { get; set; }

        [DisplayName("Оценка")]
        [Required(ErrorMessage = "Оценка это обязательное поле")]
        [Range(1, 10, ErrorMessage = "Оценка должна быть от 1 до 10")]
        public int Mark { get; set; }

        [DisplayName("Четверть")]
        [Required(ErrorMessage = "Четверть это обязательное поле")]
        [Range(1, 4, ErrorMessage = "Четверть должна быть от 1 до 4")]
        public int Quarter { get; set; }

        [Required(ErrorMessage = "Предмет это обязательное поле")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "Учащийся это обязательное поле")]
        public Guid StudentId { get; set; }

        public Student Student { get; set; }
        public Project Project { get; set; }
    }
}
