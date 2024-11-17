using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Models
{
    public class Teacher
    {
        [Required]
        public Guid TeacherId { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Фамилия это обязательное поле")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Фамилия должна быть от 3 до 50 символов")]
        public string Surname { get; set; }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Имя это обязательное поле")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Имя должно быть от 3 до 50 символов")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        [Required(ErrorMessage = "Отчество это обязательное поле")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Отчество должно быть от 3 до 50 символов")]
        public string Lastname { get; set; }

        [DisplayName("Номер телефона")]
        [Required(ErrorMessage = "Номер телефона это обязательное поле")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Предмет это обязательное поле")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "Класс это обязательное поле")]
        public Guid ClassId { get; set; }

        public Project Project { get; set; }
        public Class Class { get; set; }
    }
}
