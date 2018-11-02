using HouseMovingAPI.Common;
using HouseMovingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseMovingAPI.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(string openID, string startTime, string endTime)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                msg.Status = true;
                var list = db.Order.Where(p => p.OpenID==openID
                           &&string.Compare(p.CreateTime, startTime, StringComparison.Ordinal) >= 0
                           && string.Compare(p.CreateTime, endTime, StringComparison.Ordinal) <= 0).ToList();
                msg.Data = list;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Add(Order model)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                try
                {
                    //先校验优惠码
                    if (!string.IsNullOrWhiteSpace(model.CouponCode))
                    {
                        var couponModel = db.Coupon.FirstOrDefault(p =>  p.Code == model.CouponCode.Trim());//p.IsUsed == false &&
                        if (couponModel == null)
                        {
                            msg.Status = false;
                            msg.Result = "900";
                            return Json(msg, JsonRequestBehavior.AllowGet);
                        }
                        model.CouponID = couponModel.ID;
                        //优惠金额
                        model.SalePrice = couponModel.Amount;
                        //支付金额
                        model.PayPrice = model.OrgPrice - couponModel.Amount;

                        //db.Children.Attach(model);
                        couponModel.IsUsed = true;
                        db.Entry(couponModel).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    model.PayState = "0";
                    model.OrderNo = DateTime.Now.ToString(FormatDateTime.DateTimeFormatNoStr);
                    model.CreateTime = DateTime.Now.ToString(FormatDateTime.LongDateTimeStr);
                    var entity = db.Order.Add(model);
                    db.SaveChanges();
                    msg.Status = true;
                    msg.Data = model;
                }
                catch (Exception e)
                {
                    msg.Status = false;
                    msg.Result = "500";
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Cancel(int id)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                try
                {
                    db.Database.ExecuteSqlCommand("update [Order] set PayState=-1  where id= " + id);
                    msg.Status = true;
                }
                catch (Exception e)
                {
                    msg.Status = false;
                    msg.Result = "500";
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
    }
}