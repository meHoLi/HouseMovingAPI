﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace HouseMovingAPI.Common
{
    /// <summary>
    /// 处理微信小程序用户数据的签名验证和解密
    /// </summary>
    public static class WeChatAppDecrypt
    {
        private static string appId= "wx8bf5faf6bc3ec3ef";
        private static string appSecret= "e2f46976ee3294a02eff9124e132cbdc";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appId">应用程序的AppId</param>
        /// <param name="appSecret">应用程序的AppSecret</param>
        //public WeChatAppDecrypt(string appId= "wx694ef3488ec1381b", string appSecret= "67ed5d3fa3aa51f7d4ceb063d9caf457")
        //{
        //    this.appId = appId;
        //    this.appSecret = appSecret;
        //    return;
        //}

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns>Json数据包</returns>
        public static string GetToken()
        {
            string temp ="https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&" +
                "appid=" + appId
                + "&secret=" + appSecret;

            return GetResponse(temp);

        }

        /// <summary>
        /// 获取OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="code">客户端发来的code</param>
        /// <returns>Json数据包</returns>
        public static string GetOpenIdAndSessionKeyString(string code)
        {

            string temp = "https://api.weixin.qq.com/sns/jscode2session?" +
                "appid=" + appId
                + "&secret=" + appSecret
                + "&js_code=" + code
                + "&grant_type=authorization_code";

            return GetResponse(temp);

        }

        /// <summary>
        /// 反序列化包含OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="code">Json数据包</param>
        /// <returns>包含OpenId和SessionKey的类</returns>
        public static OpenIdAndSessionKey DecodeOpenIdAndSessionKey(WechatLoginInfo loginInfo)
        {
            OpenIdAndSessionKey oiask = JsonConvert.DeserializeObject<OpenIdAndSessionKey>(GetOpenIdAndSessionKeyString(loginInfo.code));
            if (!String.IsNullOrEmpty(oiask.errcode))
                return null;
            return oiask;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="rawData">公开的用户资料</param>
        /// <param name="signature">公开资料携带的签名信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public static bool VaildateUserInfo(string rawData, string signature, string sessionKey)
        {
            //创建SHA1签名类
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            //编码用于SHA1验证的源数据
            byte[] source = Encoding.UTF8.GetBytes(rawData + sessionKey);
            //生成签名
            byte[] target = sha1.ComputeHash(source);
            //转化为string类型，注意此处转化后是中间带短横杠的大写字母，需要剔除横杠转小写字母
            string result = BitConverter.ToString(target).Replace("-", "").ToLower();
            //比对，输出验证结果
            return signature == result;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public static bool VaildateUserInfo(WechatLoginInfo loginInfo, string sessionKey)
        {
            return VaildateUserInfo(loginInfo.rawData, loginInfo.signature, sessionKey);
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="idAndKey">包含OpenId和SessionKey的类</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public static bool VaildateUserInfo(WechatLoginInfo loginInfo, OpenIdAndSessionKey idAndKey)
        {
            return VaildateUserInfo(loginInfo, idAndKey.session_key);
        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        public static WechatUserInfo Decrypt(string encryptedData, string iv, string sessionKey)
        {
            WechatUserInfo userInfo;
            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //格式化待处理字符串
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
            byte[] byte_iv = Convert.FromBase64String(iv);
            byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

            aes.IV = byte_iv;
            aes.Key = byte_sessionKey;
            //根据设置好的数据生成解密器实例
            ICryptoTransform transform = aes.CreateDecryptor();

            //解密
            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);

            //反序列化结果，生成用户信息实例
            userInfo = JsonConvert.DeserializeObject<WechatUserInfo>(result);

            return userInfo;

        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据，推荐直接使用此方法
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <returns>用户信息</returns>
        public static WechatUserInfo Decrypt(WechatLoginInfo loginInfo)
        {
            if (loginInfo == null)
                return null;

            if (String.IsNullOrEmpty(loginInfo.code))
                return null;

            OpenIdAndSessionKey oiask = DecodeOpenIdAndSessionKey(loginInfo);

            if (oiask == null)
                return null;

            if (!VaildateUserInfo(loginInfo, oiask))
                return null;

            WechatUserInfo userInfo = Decrypt(loginInfo.encryptedData, loginInfo.iv, oiask.session_key);

            return userInfo;
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {

            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }


    }
    /// <summary>
    /// 微信小程序登录信息结构
    /// </summary>
    public class WechatLoginInfo
    {
        public string code { get; set; }
        public string encryptedData { get; set; }
        public string iv { get; set; }
        public string rawData { get; set; }
        public string signature { get; set; }
    }
    /// <summary>
    /// 微信小程序用户信息结构
    /// </summary>
    public class WechatUserInfo
    {
        public string openId { get; set; }
        public string nickName { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string avatarUrl { get; set; }
        public string unionId { get; set; }
        public Watermark watermark { get; set; }

        public class Watermark
        {
            public string appid { get; set; }
            public string timestamp { get; set; }
        }
    }
    /// <summary>
    /// 微信小程序从服务端获取的OpenId和SessionKey信息结构
    /// </summary>
    public class OpenIdAndSessionKey
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }
    /// <summary>
    /// 微信小程序从服务端获取的token信息结构
    /// </summary>
    public class WechatToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}
