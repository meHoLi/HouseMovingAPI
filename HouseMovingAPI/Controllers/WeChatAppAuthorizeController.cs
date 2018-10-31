using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using System.Net;
using System.IO;
using System.Web.Security;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using HouseMovingAPI.Common;

namespace HouseMovingAPI.Controllers
{
    public class WeChatAppAuthorizeController : Controller
    {
        // GET: WeChatAppAuthorize
        //cqs
        //private const string APPID = "wx3acda84288bf9573";
        //private const string AppSecret = "06a0ccc89e812a7234a851aa9ea06fc4";
        //my
        private const string APPID = "wx8bf5faf6bc3ec3ef";
        private const string AppSecret = "e2f46976ee3294a02eff9124e132cbdc";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetToken()
        {

            ResponseMessage msg = new ResponseMessage();
            var result =  WeChatAppDecrypt.GetToken();
            msg.Status = true;
            msg.Data = result;
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetOpenIdAndSessionKeyString(string code)
        {

            ResponseMessage msg = new ResponseMessage();
            msg.Status = true;
            try
            {
                string temp =  WeChatAppDecrypt.GetOpenIdAndSessionKeyString(code);
                msg.Data = temp;
            }
            catch (Exception e)
            {
                msg.Status = false;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        //https://blog.csdn.net/ivanyoung66/article/details/72523231
        //https://bbs.csdn.net/topics/392320383



        //小程序提醒
        //http://www.cnblogs.com/vanteking/p/7606222.html
        //https://www.cnblogs.com/qiujz/articles/5913796.html

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <param name="data">发送的模板数据</param>
        /// <returns></returns>

        public string SendTemplateMsg(string accessToken, string data)
        {
            //https://blog.csdn.net/qq_31583959/article/details/52087987
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}", accessToken);
            HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(data); //通过UTF-8编码
            hwr.ContentLength = payload.Length;
            Stream writer = hwr.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            var result = hwr.GetResponse() as HttpWebResponse; //此句是获得上面URl返回的数据
            string strMsg = WebResponseGet(result);
            return strMsg;
        }
        public string WebResponseGet(HttpWebResponse webResponse)
        {

            StreamReader responseReader = null;
            string responseData = "";
            try
            {
                responseReader = new StreamReader(webResponse.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception e)
            {
            }
            finally
            {
                webResponse.GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }

    }
}