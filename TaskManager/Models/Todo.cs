using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskManager.Models
{
    public partial class Todo
    {
        public Todo()
        {
            TagAssignments = new HashSet<TagAssignment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public bool IsDone { get; set; }
        public DateTime? Date { get; set; }

        [JsonIgnore]
        public virtual Project Project { get; set; }

        public virtual ICollection<TagAssignment> TagAssignments { get; set; }
    }
}
