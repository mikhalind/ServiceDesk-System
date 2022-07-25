using ServerApplication.EDMX;
using System.Runtime.Serialization;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class ContractorEntity
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal Phone { get; set; }
        [DataMember]
        public int Rating { get; set; }
        [DataMember]
        public string Address { get; set; }

        public ContractorEntity(contractor contr)
        {
            ID = contr.id; 
            Name = contr.name;
            Phone = contr.contact_phone;
            Rating = contr.rating;
            Address = contr.address;
        }
    }
}