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
    
    public partial class Coupon
    {
        public int ID { get; set; }
        public Nullable<bool> IsUsed { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
