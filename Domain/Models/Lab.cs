using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Lab
    {
        public int Id { get; set; }

        [Display(Name = "Название лабараторной работы")]
        public string Name { get; set; }

        [Display(Name = "Задания")]
        public ICollection<LabTask> Tasks { get; set; }
    }
}
