﻿using HouseMovingAPI.Common;
using HouseMovingAPI.Models;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WxPayAPI;

namespace HouseMovingAPI.Controllers
{
    public class WXPayController : Controller
    {
        // GET: WXPay

        /// <summary>
        /// 微信小程序统一下单
        /// </summary>
        /// <returns></returns>
        public ActionResult WxUnifiedOrder(string openID, int total_fee,string out_trade_no ,string body)
        {
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.openid = openID;
            jsApiPay.total_fee = total_fee;
            jsApiPay.out_trade_no = out_trade_no;
            jsApiPay.body = body;
            WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
            var parameters = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数  
            ResponseMessage msg = new ResponseMessage();
            msg.Status = true;
            msg.Data = parameters;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        /// <summary>
        /// 回调函数
        /// </summary>
        public void PayNotifyUrl()
        {

            PayLogHelper.Debug("进入微信支付回调HttpPost WeChatPay PayNotifyUrl");
            WxPayData notifyData = GetNotifyData();
            string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            string totalFee = notifyData.GetValue("total_fee").ToString();
            if (notifyData.GetValue("return_code").ToString() == "SUCCESS" && notifyData.GetValue("result_code").ToString() == "SUCCESS")
            {
                PayLogHelper.Debug("WX:获取到notifyData值");
                PayLogHelper.Debug("WXout_trade_no:" + out_trade_no);
                Response.Write("成功");
                //支付成功，以下执行业务处理

                using (HouseMovingDBEntities db = new HouseMovingDBEntities())
                {
                    ResponseMessage msg = new ResponseMessage();
                    msg.Status = true;
                    var entity = db.Order.FirstOrDefault(p => p.OrderNo == out_trade_no);
                    if (entity != null)
                    {
                        entity.PayTime = DateTime.Now.ToString(FormatDateTime.LongDateTimeStr);
                        entity.PayState = "1";
                        entity.PayType = "WX";
                        db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    //注释下处代码，允许微信回调，重复通知申请支付的网站，让客户端做重复处理
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    PayLogHelper.Debug("WX:order query success : " + res.ToXml());
                    System.Web.HttpContext.Current.Response.Write(res.ToXml());
                    System.Web.HttpContext.Current.Response.End();
                }
            }
            else
            {
                PayLogHelper.Debug("WX支付失败");
                Response.Write("失败");
                //失败
                //msg.Result = AESEncryption.AESEncrypt("false");
            }
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            PayLogHelper.Debug("WX:" + 1111);

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                PayLogHelper.Debug("WX:若签名错误" + "Sign check error : " + res.ToXml());
            }
            PayLogHelper.Debug("WX:Check sign success");
            return data;
        }

    }
}