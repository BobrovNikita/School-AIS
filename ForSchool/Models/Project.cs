using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Models
{
    public class Project
    {
        [Required]
        public Guid ProjectId { get; set; }

        [DisplayName("Название")]
        [Required(ErrorMessage = "Название предмета это обязательное поле")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название предмета должно быть от 3 до 50 символов")]
        public string Project_Name { get; set; }


        [DisplayName("Кол-во занятий")]
        [Required(ErrorMessage = "Кол-во занятий это обязательное поле")]
        [Range(1, 20, ErrorMessage = "Число занятий должно быть от 1 до 20")]
        public int Count { get; set; }


        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Functionality> Functionalities { get; set; }
    }
}
