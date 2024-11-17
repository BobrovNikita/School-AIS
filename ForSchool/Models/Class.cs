using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Models
{
    public class Class
    {
        [Required]
        public Guid ClassId { get; set; }

        [DisplayName("Номер")]
        [Required(ErrorMessage = "Номер это обязательное поле")]
        [Range(1,11, ErrorMessage = "Номер должен быть от 1 до 11")]
        public int Number { get; set; }

        [DisplayName("Учебний год")]
        [Required(ErrorMessage = "Учебный год это обязательное поле")]
        [Range(2020, 2030, ErrorMessage = "Учебний год должен быть от 2020 до 2030")]
        public int Year { get; set; }

        [DisplayName("Кол-во учащихся")]
        [Required(ErrorMessage = "Кол-во учащихся это обязательное поле")]
        [Range(10, 50, ErrorMessage = "Кол-во учащихся должно быть от 10 до 50")]
        public int Count { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}
