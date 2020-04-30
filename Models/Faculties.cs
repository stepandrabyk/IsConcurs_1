using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IscConcursLr1
{
    public partial class Faculties
    {
        public Faculties()
        {
            FacultiesSpecialties = new HashSet<FacultiesSpecialties>();
        }

        public int FacultiesId { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name="Факультети вибраного Університету")]
        [Remote(action: "CorrectName", controller: "Faculties")]
        [RegularExpression(@"^(([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+[\s]{1}[АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+)|([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+))$", ErrorMessage = "Неправильне введення назви")]
        public string Name { get; set; }
        [Display(Name = "Університет")]
        public int FacultiesUniversity { get; set; }
        [Display(Name="Університет")]
       

        public virtual Universities FacultiesUniversityNavigation { get; set; }
        public virtual ICollection<FacultiesSpecialties> FacultiesSpecialties { get; set; }
    }
}
