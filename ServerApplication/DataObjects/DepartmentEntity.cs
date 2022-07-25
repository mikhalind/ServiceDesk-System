using ServerApplication.EDMX;
using System.Runtime.Serialization;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class DepartmentEntity
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Head { get; set; }
        [DataMember]
        public string Address { get; set; }

        public DepartmentEntity(department dept)
        {
            ID = dept.id;
            Name = dept.name;
            Head = dept.head;
            Address = dept.address;
        }
    }
}