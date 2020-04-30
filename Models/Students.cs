using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IscConcursLr1
{
    public partial class Students
    {
        public Students()
        {
            Statements = new HashSet<Statements>();
        }
        
        public int StudentId { get; set; }


        
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name="Студент")]
        [RegularExpression(@"^(([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+[\s]{1}[АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+)|([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+))$", ErrorMessage ="Неправильне введення імені або прізвища")]
        public string Name { get; set; }

        [Display(Name = "Інформаія")]
        public string Info { get; set; }

        [Display(Name = "Бал")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(100,200, ErrorMessage ="Неможлива оцінка")]
        public int Mark { get; set; }

        public virtual ICollection<Statements> Statements { get; set; }
    }
}
