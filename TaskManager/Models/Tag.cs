using System.Collections.Generic;

namespace TaskManager.Models
{
    public partial class Tag
    {
        public Tag()
        {
            TagAssignments = new HashSet<TagAssignment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TagAssignment> TagAssignments { get; set; }
    }
}
