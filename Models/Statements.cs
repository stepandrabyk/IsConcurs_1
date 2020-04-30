using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IscConcursLr1
{
    public partial class Statements
    {
        public int StatementId { get; set; }
        [Remote(action: "UniqueStatement", controller: "Statements", AdditionalFields = nameof(StFacultiesSpecialties))]
        [Display(Name ="Студент")]
        public int StStudent { get; set; }
        [Remote(action: "UniqueStatement", controller: "Statements", AdditionalFields = nameof(StStudent))]
        [Display(Name = "Спеціальність факультету")]
        public int StFacultiesSpecialties { get; set; }
        [Display(Name = "Інформація")]

        public virtual FacultiesSpecialties StFacultiesSpecialtiesNavigation { get; set; }
        public virtual Students StStudentNavigation { get; set; }
    }
}
