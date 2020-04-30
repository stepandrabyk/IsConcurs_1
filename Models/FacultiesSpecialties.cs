using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IscConcursLr1
{
    public partial class FacultiesSpecialties
    {
        public FacultiesSpecialties()
        {
            Statements = new HashSet<Statements>();
        }

        public int FacultiesSpecialtiesId { get; set; }
        [Display(Name = "Факультет")]
        public int FsFaculties { get; set; }
        [Display(Name = "Спеціальність")]
        public int FsSpecialties { get; set; }
        [Display(Name = "Максимальні Бюджетні місця")]
        public int? Budget { get; set; }
        [Display(Name = "Максимальні Місця на контракт")]
        public int? Contract { get; set; }
        [Display(Name = "Факультет")]
        public virtual Faculties FsFacultiesNavigation { get; set; }
        [Display(Name = "Спеціальність")]
        public virtual Specialties FsSpecialtiesNavigation { get; set; }
        public virtual ICollection<Statements> Statements { get; set; }
    }
}
