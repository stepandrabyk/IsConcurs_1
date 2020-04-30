using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IscConcursLr1
{
    public partial class Specialties
    {
        public Specialties()
        {
            FacultiesSpecialties = new HashSet<FacultiesSpecialties>();
        }

        public int SpecialtiesId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        [RegularExpression(@"^(([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+[\s]{1}[АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+)|([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+))$", ErrorMessage = "Неправильне введення назви")]
        public string Name { get; set; }
        [Display(Name = "Інформація")]
        public string Info { get; set; }
       

        public virtual ICollection<FacultiesSpecialties> FacultiesSpecialties { get; set; }
    }
}
