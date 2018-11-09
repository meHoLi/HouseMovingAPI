using HouseMovingAPI.Common;
using HouseMovingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseMovingAPI.Controllers
{
    public class CouponController : Controller
    {
        // GET: Coupon
        public ActionResult Index()
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                msg.Status = true;
                msg.Data = db.Coupon.ToList();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Add(Coupon model)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                try
                {
                    //先校验优惠码是否重复
                    var isExist = db.Coupon.Any(p => p.Code == model.Code.Trim());
                    if (isExist)
                    {
                        msg.Status = false;
                        msg.Result = "900";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    model.IsUsed = false;
                    var entity = db.Coupon.Add(model);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    msg.Status = false;
                    msg.Result = "500";
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                try
                {
                    db.Database.ExecuteSqlCommand("delete Coupon where id= " + id);
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