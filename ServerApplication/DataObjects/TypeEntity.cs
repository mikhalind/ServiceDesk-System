using System.Runtime.Serialization;
using ServerApplication.EDMX;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class TypeEntity
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }

        public TypeEntity(type type)
        {
            ID = type.id;
            Name = type.name;
        }
    }
}