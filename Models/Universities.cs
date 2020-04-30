using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace IscConcursLr1
{
    public partial class Universities
    {
        public Universities()
        {
            Faculties = new HashSet<Faculties>();
        }

        public int UniversityId { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name="Університет")]
        [Remote(action: "CorrectName",controller:"Universities")]
        [RegularExpression(@"^(([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+[\s]{1}[АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+)|([АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЮЯ абвгґдеєжзиіїйклмнопрстуфхцчшщьюя]+))$", ErrorMessage = "Неправильне введення назви")]
        public string Name { get; set; }
        [Display(Name="Інформація")]
        public string Info { get; set; }

        public virtual ICollection<Faculties> Faculties { get; set; }
    }
}
