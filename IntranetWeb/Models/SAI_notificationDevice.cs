//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntranetWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SAI_notificationDevice
    {
        public int id { get; set; }
        public string deviceID { get; set; }
        public Nullable<int> notificationType { get; set; }
        public string notificationDesc { get; set; }
        public Nullable<System.DateTime> notificationDate { get; set; }
        public Nullable<System.DateTime> notificationTimeStamp { get; set; }
        public Nullable<bool> notificationStatus { get; set; }
        public Nullable<int> notificationsSource { get; set; }
    }
}