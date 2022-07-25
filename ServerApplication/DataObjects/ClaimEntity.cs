using ServerApplication.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServerApplication.DataObjects
{
    [DataContract]
    public class ClaimEntity
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Theme { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public CategoryEntity Category { get; set; }
        [DataMember]
        public StatusEntity Status { get; set; }
        [DataMember]
        public TypeEntity ClaimType { get; set; }

        [DataMember]
        public Nullable<int> Initiator { get; set; }
        [DataMember]
        public Nullable<int> Dispatcher { get; set; }
        [DataMember]
        public Nullable<int> Department { get; set; }
        [DataMember]
        public Nullable<int> Executor { get; set; }
        [DataMember]
        public Nullable<int> Contractor { get; set; }

        [DataMember]
        public Nullable<DateTime> CreatedDate { get; set; }
        [DataMember]
        public Nullable<DateTime> ApprovalDate { get; set; }
        [DataMember]
        public Nullable<DateTime> FinishedDate { get; set; }
        [DataMember]
        public Nullable<DateTime> ExecutionDate { get; set; }
        [DataMember]
        public Nullable<DateTime> ConfirmedDate { get; set; }
        [DataMember]
        public Nullable<DateTime> ClosedDate { get; set; }

        public ClaimEntity(claim cl)
        {
            ID = cl.id;
            Theme = cl.theme;
            Description = cl.description;
            Category = new CategoryEntity(cl.category_nav);
            Status = new StatusEntity(cl.status_nav);
            ClaimType = new TypeEntity(cl.type_nav);
            Initiator = cl.initiator;
            Dispatcher = cl.dispatcher;
            Department = cl.department;
            Executor = cl.executor;
            Contractor = cl.contractor;

            CreatedDate = cl.created_date;
            ApprovalDate = cl.approved_date;
            FinishedDate = cl.finished_date;
            ExecutionDate = cl.executed_date;
            ClosedDate = cl.closed_date;
            ConfirmedDate = cl.confirmed_date;  
        }
    }
}