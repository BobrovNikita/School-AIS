using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Views.ViewModels
{
    public class ClassViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Номер")]
        public int Number { get; set; }

        [DisplayName("Учебный год")]
        public int Year { get; set; }

        [DisplayName("Кол-во учащихся")]
        public int Count { get; set; }
    }
}
