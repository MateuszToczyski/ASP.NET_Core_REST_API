using System.Collections.Generic;

namespace TaskManager.Models
{
    public partial class Project
    {
        public Project()
        {
            Todoes = new HashSet<Todo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }

        public virtual ICollection<Todo> Todoes { get; set; }
    }
}
