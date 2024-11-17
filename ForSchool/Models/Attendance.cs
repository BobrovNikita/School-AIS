using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace ForSchool.Models
{
    public class Attendance
    {
        [Required]
        public Guid AttendanceId { get; set; }

        [DisplayName("Кол-во пропусков")]
        [Required(ErrorMessage = "Кол-во пропусков это обязательное поле")]
        [Range(1, 365, ErrorMessage = "Кол-во пропусков должно быть от 1 до 365")]
        public int SkipCount { get; set; }

        [DisplayName("Статус")]
        [Required(ErrorMessage = "Статус это обязательное поле")]
        public string Status { get; set; }

        [DisplayName("Учащийся")]
        [Required(ErrorMessage = "Учащийся это обязательное поле")]
        public Guid StudentId { get; set; }

        public Student Student { get; set; }
    }
}
