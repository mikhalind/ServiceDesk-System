using ServerApplication.EDMX;
using System.Runtime.Serialization;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class PositionEntity
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

        public PositionEntity(position pos)
        {
            ID = pos.id;
            Name = pos.name;
            Description = pos.description;
        }
    }
}