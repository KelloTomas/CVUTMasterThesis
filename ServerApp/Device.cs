//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Device
    {
        public int IdDevice { get; set; }
        public int IdAutomat { get; set; }
        public string IP { get; set; }
        public Nullable<int> Port { get; set; }
    
        public virtual Automat Automat { get; set; }
    }
}