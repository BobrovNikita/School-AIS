using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }

        [DisplayName("Фамилия")]
        public string Surname { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string LastName { get; set; }

        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }

        [DisplayName("Адрес")]
        public string Adress { get; set; }

        [DisplayName("Номер класса")]
        public int ClassNumber { get; set; }
    }
}
