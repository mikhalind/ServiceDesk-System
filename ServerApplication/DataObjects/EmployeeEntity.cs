using ServerApplication.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class EmployeeEntity
    {
        [DataMember]
        public int TableNumber { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public int Position { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember]
        public int Department { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Nullable<decimal> Phone { get; set; }
        [DataMember]
        public byte[] PassHash { get; set; }
        [DataMember]
        public string Address { get; set; }

        public EmployeeEntity(employee emp)
        {
            TableNumber = emp.table_number;
            LastName = emp.last_name;
            FirstName = emp.first_name;
            MiddleName = emp.middle_name;
            Position = emp.position;
            Photo = emp.photo_path;
            Department = emp.department;
            BirthDate = emp.birth_date;
            Email = emp.email;
            Phone = emp.phone;
            PassHash = emp.pass_hash;
            Address = emp.address;
        }
    }
}