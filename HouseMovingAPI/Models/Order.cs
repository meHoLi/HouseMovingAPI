//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HouseMovingAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int ID { get; set; }
        public string OpenID { get; set; }
        public string CarType { get; set; }
        public string CreateTime { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public string EndPlace { get; set; }
        public string Name { get; set; }
        public string OrderNo { get; set; }
        public string OrgPrice { get; set; }
        public Nullable<decimal> PayPrice { get; set; }
        public Nullable<int> PeopleNum { get; set; }
        public string Phone { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public string ServiceTime { get; set; }
        public string StartPlace { get; set; }
        public string Remark { get; set; }
        public string PayTime { get; set; }
        public string PayState { get; set; }
        public string PayType { get; set; }
    }
}
