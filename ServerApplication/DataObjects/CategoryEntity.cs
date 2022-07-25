using ServerApplication.EDMX;
using System.Runtime.Serialization;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class CategoryEntity
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Hours { get; set; }

        public CategoryEntity(category cat)
        {
            Code = cat.code;
            Name = cat.name;
            Hours = cat.hours;
        }
    }
}