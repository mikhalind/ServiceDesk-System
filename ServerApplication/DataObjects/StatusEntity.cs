using ServerApplication.EDMX;
using System.Runtime.Serialization;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class StatusEntity
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Comment { get; set; }

        public StatusEntity(status stat)
        {
            Code = stat.code;
            Name = stat.name;
            Comment = stat.comment;
        }
    }
}