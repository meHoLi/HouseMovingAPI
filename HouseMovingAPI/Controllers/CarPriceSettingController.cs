using HouseMovingAPI.Common;
using HouseMovingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseMovingAPI.Controllers
{
    public class CarPriceSettingController : Controller
    {
        // GET: CarPriceSetting
        public ActionResult Index()
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                msg.Status = true;
                msg.Data = db.CarPriceSetting.ToList();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetModelByCarCode(string carCode)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                msg.Status = true;
                var model = db.CarPriceSetting.FirstOrDefault(p => p.CarCode == carCode);
                msg.Data = model;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(CarPriceSetting model)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                try
                {
                    db.CarPriceSetting.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
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