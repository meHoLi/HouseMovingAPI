using HouseMovingAPI.Common;
using HouseMovingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseMovingAPI.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(Comment model)
        {
            using (HouseMovingDBEntities db = new HouseMovingDBEntities())
            {
                ResponseMessage msg = new ResponseMessage();
                try
                {
                    model.CreateTime = DateTime.Now.ToString(FormatDateTime.LongDateTimeStr);
                    var entity = db.Comment.Add(model);
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