using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Decision { get; set; }

        public List<Dictionary<string, object>>? QueryData { get; set; }
    }
}
