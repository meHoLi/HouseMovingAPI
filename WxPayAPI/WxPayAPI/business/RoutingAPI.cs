using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.UI;
using System.Web;

namespace WxPayAPI
{
    /// <summary>
    ///  分账
    /// </summary>
public  class RoutingAPI
{
    #region 微信分帐接口
      private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;  
            return false; 
        }
      public static string ToXml(SortedDictionary<string, object> m_values)
      {
          //数据为空时不能转化为xml格式
          if (0 == m_values.Count)
          {

              //throw new WxPayException("WxPayData数据为空!");
          }

          string xml = "<xml>";
          foreach (KeyValuePair<string, object> pair in m_values)
          {
              //字段值不能为null，会影响后续流程
              if (pair.Value == null)
              {
                  //throw new WxPayException("WxPayData内部含有值为null的字段!");
              }

              //if (pair.Value.GetType() == typeof(int))
              //{
              //    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
              //}
              //else if (pair.Value.GetType() == typeof(string))
              //{
              //    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
              //}
              else
              {

                   xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
              }
                  
              //else //除了string和int类型不能含有其他数据类型
              //{
              //    PayLogHelper.Error("WxPayData字段数据类型错误!");
              //    throw new WxPayException("WxPayData字段数据类型错误!");
              //}
          }
          xml += "</xml>";
          return xml;
      }
      public static string ToUrl(SortedDictionary<string, object> m_values)
      {
          string buff = "";
          foreach (KeyValuePair<string, object> pair in m_values)
          {
              if (pair.Value == null)
              {
                  //throw new WxPayException("WxPayData内部含有值为null的字段!");
              }

              if (pair.Key != "sign" && pair.Value.ToString() != "")
              {
                  buff += pair.Key + "=" + pair.Value + "&";
              }
          }
          buff = buff.Trim('&');
          return buff;
      }
      public static string MakeSign(string KEY, SortedDictionary<string, object> m_values)
      {
          //转url格式
          string str = ToUrl(m_values);
          //在string后加入API KEY
          str += "&key=" + KEY;
          //MD5加密
          var md5 = MD5.Create();
          var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
          var sb = new StringBuilder();
          foreach (byte b in bs)
          {
              sb.Append(b.ToString("x2"));
          }
          //所有字符转为大写
          return sb.ToString().ToUpper();
      }
      public  static string WxRoutAPI( SortedDictionary<string, object> m_values )
      {

          string data = ToXml(m_values); 
          try
          {
              string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
              string strSyspath = HttpRuntime.AppDomainAppPath.ToString();
              //string cert = strSyspath + "Cert\\apiclient_cert.p12";
              //string cert =@"D:\DaogeneRoot\live.akafroup.cn\wwwroot\Cert\apiclient_cert.p12";
              //string cert = @"D:\publish\Cert\apiclient_cert.p12";
              string path = HttpContext.Current.Request.PhysicalApplicationPath;
              string cert = path + m_values["SSLCERT_PATH"].ToString();// @"D:\Work\PayCenter\PayCenter.Web\PayCenter.Web\cert\apiclient_cert.p12";
              string password = m_values["mchid"].ToString();// "1284112901";
              ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
              //cert = @"D:\Cert\apiclient_cert.p12";
              //此处，有两种方式，一种是使用X509Certificate，但是在服务器上会报证书错误，即使证书是对，因此使用X509Certificate2
              X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

              HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
              byte[] bytes = Encoding.UTF8.GetBytes(data);
              webrequest.ContentType = "application/json; charset=utf-8";
              webrequest.ClientCertificates.Add(cer);
              webrequest.Method = "post";
              webrequest.ContentLength = bytes.Length;   
              webrequest.GetRequestStream().Write(bytes, 0, bytes.Length);
              HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();

              Stream stream = webreponse.GetResponseStream();

              
              string resp = string.Empty;
              using (StreamReader reader = new StreamReader(stream))
              {
                  string strnimhao = reader.ReadToEnd();
                  return strnimhao;
              }
            
          }
          catch (Exception exp)
          {
              return null; 
          }
      }
          
      public string RoutAPI(SortedDictionary<string, object> m_values)
      {

         
          string data = WxRoutAPI(m_values);
          return data;

      }
    #endregion
}
}
