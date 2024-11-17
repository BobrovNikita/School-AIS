using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.ViewModels
{
    public class AttendanceViewModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }

        [DisplayName("Кол-во пропусков")]
        public int SkipCount { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Фамилия")]
        public string Surname { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string LastName { get; set; }

    }
}
