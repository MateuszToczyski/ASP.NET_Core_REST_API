using System.Text.Json.Serialization;

namespace TaskManager.Models
{
    public partial class TagAssignment
    {
        public int TagId { get; set; }
        public int TodoId { get; set; }

        [JsonIgnore]
        public virtual Tag Tag { get; set; }
        [JsonIgnore]
        public virtual Todo Todo { get; set; }
    }
}
