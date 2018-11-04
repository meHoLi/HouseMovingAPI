using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mail;
using System.Web;
using System.Web.Mvc;

namespace HouseMovingAPI.Controllers
{
    public class SendEMailController : Controller
    {
        // GET: SendEMail
        public ActionResult Index()
        {
            return View();
        }
        public void SendMailUseGmail()
        {
            //https://www.cnblogs.com/atree/p/smtp-qq-email.html
            MailMessage mail = new  MailMessage();
            try
            { 
                //mail.To = "735939357@qq.com";
                //mail.From = "735939357@qq.com";                
                mail.To = "346857553@qq.com";
                mail.From = "346857553@qq.com";
                mail.Subject = "小程序订单通知";
                mail.BodyFormat = System.Web.Mail.MailFormat.Html;
                mail.Body = "您的小程序订单有变化，请进入小程序查看";

                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //身份验证
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mail.From); //邮箱登录账号，这里跟前面的发送账号一样就行
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "gkuxvctylledbjai");// "tkutqkiorfksbbdf"); //这个密码要注意：如果是一般账号，要用授权码；企业账号用登录密码
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);//端口
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//SSL加密
                System.Web.Mail.SmtpMail.SmtpServer = "smtp.qq.com";    //企业账号用smtp.exmail.qq.com
                System.Web.Mail.SmtpMail.Send(mail);

                //邮件发送成功
            }
            catch (Exception ex)
            {
                //失败，错误信息：ex.Message;
            }
        }



        //public void SendMailLocalhost()
        //{
        //    MailMessage msg = new MailMessage();
        //    msg.To.Add("1779789881@qq.com");
        //    //msg.To.Add("b@b.com");
        //    /* msg.To.Add("b@b.com"); 
        //    * msg.To.Add("b@b.com"); 
        //    * msg.To.Add("b@b.com");可以发送给多人 
        //    */
        //    //msg.CC.Add(c@c.com);
        //    /* 
        //    * msg.CC.Add("c@c.com"); 
        //    * msg.CC.Add("c@c.com");可以抄送给多人 
        //    */
        //    msg.From = new MailAddress("1779789881@qq.com", "AlphaWu", System.Text.Encoding.UTF8);
        //    /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
        //    msg.Subject = "这是测试邮件";//邮件标题 
        //    msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
        //    msg.Body = "邮件内容";//邮件内容 
        //    msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
        //    msg.IsBodyHtml = false;//是否是HTML邮件 
        //    msg.Priority = MailPriority.High;//邮件优先级 

        //    SmtpClient client = new SmtpClient();
        //    client.Host = "localhost";
        //    object userState = msg;
        //    try
        //    {
        //        //client.SendAsync(msg, userState);
        //        client.Send(msg); 
        //        //MessageBox.Show("发送成功");
        //    }
        //    catch (System.Net.Mail.SmtpException ex)
        //    {
        //        //MessageBox.Show(ex.Message, "发送邮件出错");
        //    }
        //}


        //public void SendMailUseGmail()
        //{
        //     MailMessage msg = new  MailMessage();
        //    msg.To.Add("1779789881@qq.com");

        //    /* 
        //    * msg.To.Add("b@b.com"); 
        //    * msg.To.Add("b@b.com"); 
        //    * msg.To.Add("b@b.com");可以发送给多人 
        //    */
        //    //msg.CC.Add(c@c.com);
        //    /* 
        //    * msg.CC.Add("c@c.com"); 
        //    * msg.CC.Add("c@c.com");可以抄送给多人 
        //    */
        //    msg.From = new MailAddress("346857553@qq.com", "lihe", System.Text.Encoding.UTF8);
        //    /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
        //    msg.Subject = "一些说明";//邮件标题 
        //    msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
        //    msg.Body = "明天来我家吃饭";//邮件内容 
        //    msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
        //    msg.IsBodyHtml = false;//是否是HTML邮件 
        //    msg.Priority = MailPriority.High;//邮件优先级 
        //    SmtpClient client = new SmtpClient();
        //    client.Credentials = new System.Net.NetworkCredential("346857553@qq.com", "gkuxvctylledbjai");
        //    //上述写你的GMail邮箱和密码 
        //    client.Port =  587;// 465;//Gmail使用的端口 
        //    client.Host = "smtp.qq.com";
        //    client.EnableSsl = true;//经过ssl加密 
        //    object userState = msg;
        //    try
        //    {
        //        //client.SendAsync(msg, userState);
        //        client.Send(msg); 
        //        //MessageBox.Show("发送成功");
        //    }
        //    catch (System.Net.Mail.SmtpException ex)
        //    {
        //        //MessageBox.Show(ex.Message, "发送邮件出错");
        //    }
        //}

        //public void SendMailUseZj()
        //{
        //    System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        //    msg.To.Add("1779789881@qq.com");
        //    /* 
        //    * msg.To.Add("b@b.com"); 
        //    * msg.To.Add("b@b.com"); 
        //    * msg.To.Add("b@b.com");可以发送给多人 
        //    */
        //    /* 
        //    * msg.CC.Add("c@c.com"); 
        //    * msg.CC.Add("c@c.com");可以抄送给多人 
        //    */
        //    msg.From = new MailAddress("a@a.com", "AlphaWu", System.Text.Encoding.UTF8);
        //    /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
        //    msg.Subject = "这是测试邮件";//邮件标题 
        //    msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
        //    msg.Body = "邮件内容";//邮件内容 
        //    msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
        //    msg.IsBodyHtml = false;//是否是HTML邮件 
        //    msg.Priority = MailPriority.High;//邮件优先级 

        //    SmtpClient client = new SmtpClient();
        //    client.Credentials = new System.Net.NetworkCredential("18916840930", "gmj12345");
        //    //在zj.com注册的邮箱和密码 
        //    client.Host = "smtp.zj.com";
        //    object userState = msg;
        //    try
        //    {
        //        client.Send(msg); 
        //    }
        //    catch (System.Net.Mail.SmtpException ex)
        //    {
        //    }
        //}
    }
}