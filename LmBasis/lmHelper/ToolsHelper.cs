/*----------------------------------------------------------------
// Copyright (C) 2012
//
// 文件名：FileEncrypt.cs
// 文件功能描述： 加密类
//
// 
// 创建标识：   lm -- 2015/11/28 
//
// 添加标识：   
//
// 添加标识：   
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;

public class ToolsHelper
{
    #region -- 常量 --

    /// <summary>
    /// 用于 AES 算法的初始化向量。
    /// </summary>
    private static byte[] keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    #endregion

    #region 加密

    /// <summary>
    /// 不可逆转md5加密
    /// </summary>
    /// <param name="str">加密内容</param> 
    /// <returns>加密后值</returns>
    /// 
    public static string MD5Encode(string str)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
    }

    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="encryptkey">加密密钥，要求为8位</param>
    /// <returns>加密成功返回加密后的字符串，失败返回源字符串</returns>
    public static string EncryptDES(string encryptString, string encryptkey)
    {
        try
        {
            byte[] rgbkey = Encoding.UTF8.GetBytes(encryptkey.Substring(0, 8));
            byte[] rgbIV = keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbkey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            return encryptString;
        }
    }

    
    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的字符串</param>
    /// <param name="decryptKey">解密密钥，要求为8位，和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串，失败返回源字符串</returns>
    public static string DecryptDES(string decryptString, string decryptKey)
    {
        try
        {
            byte[] rgbkey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbkey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            return decryptString;
        }
    }
     
    #endregion

    #region 常用方法

    /// <summary>
    /// 将对象转为json格式
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetJsonStr(object obj)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(obj);
    }

    /// <summary>
    /// Json反序列化对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="json">json字符串</param>
    /// <returns></returns>
    public static T JsonDeserialize<T>(string json)
    {
        if (string.IsNullOrEmpty(json)) { return default(T); }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        T obj = jss.Deserialize<T>(json);
        return obj;
    }

    /// <summary>   
    /// 根据Json返回DateTable,JSON数据格式如:   
    /// {table:[{column1:1,column2:2,column3:3},{column1:1,column2:2,column3:3}]}   
    /// </summary>   
    /// <param name="strJson">Json字符串</param>   
    /// <returns></returns>   
    public static DataTable JsonToDataTable(string strJson)
    {
        //取出表名   
        var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
        string strName = rg.Match(strJson).Value;
        DataTable tb = null;
        //去除表名   
        strJson = strJson.Substring(strJson.IndexOf("[") + 1);
        strJson = strJson.Substring(0, strJson.LastIndexOf("]"));
        //获取数据   
        rg = new Regex(@"(?<={)[^}]+(?=})");
        MatchCollection mc = rg.Matches(strJson);
        for (int i = 0; i < mc.Count; i++)
        {
            string strRow = mc[i].Value;
            string[] strRows = strRow.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
            //创建表   
            if (tb == null)
            {
                tb = new DataTable();
                tb.TableName = strName;
                foreach (string str in strRows)
                {
                    var dc = new DataColumn();
                    string[] strCell = str.Split(new string[] { "\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    dc.ColumnName = strCell[0].Replace("\"", "").Replace(",", "").Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace(":", "");
                    if (strCell[0].ToLower().IndexOf("date") != -1 || strCell[0].ToLower().IndexOf("time") != -1)
                    {
                        try
                        {
                            Convert.ToDateTime(str.Substring(str.IndexOf(":") + 2).Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "").Replace("/", "-"));
                            dc.DataType = typeof(DateTime);
                        }
                        catch (FormatException)
                        {
                            dc.DataType = typeof(string);
                        }
                    }
                    tb.Columns.Add(dc);
                    tb.AcceptChanges();
                }
            }
            //增加内容   
            DataRow dr = tb.NewRow();
            for (int r = 0; r < strRows.Length; r++)
            {
                try
                {
                    if (tb.Columns[r].DataType == typeof(DateTime))
                    {
                        try
                        {
                            dr[r] = Convert.ToDateTime(strRows[r].Substring(strRows[r].IndexOf(":") + 2).Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "").Replace("/", "-"));

                        }
                        catch (FormatException)
                        {
                            dr[r] = strRows[r].Split(new string[] { "\":\"" }, StringSplitOptions.RemoveEmptyEntries)[1].ToString().Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                        }
                    }
                    else
                    {
                        dr[r] = strRows[r].Split(new string[] { "\":\"" }, StringSplitOptions.RemoveEmptyEntries)[1].ToString().Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            tb.Rows.Add(dr);
            tb.AcceptChanges();
        }
        return tb;
    }
    /// <summary>
    /// 截取字符串 默认增加“...”
    /// </summary>
    /// <param name="str">原字符串</param>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static string SubString(string str, int length)
    {
        if (str.Length > length)
        {
            str = str.Substring(0, length) + "...";
        }
        return str;
    }

    ///   <summary>
    ///   去除HTML标记
    ///   </summary>
    ///   <param   name="NoHTML">包括HTML的源码</param>
    ///   <returns>已经去除后的文字</returns>
    public static string NoHTML(string Htmlstring)
    {
        //删除脚本
        Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
          RegexOptions.IgnoreCase);
        //删除HTML

        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&&&",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
          RegexOptions.IgnoreCase);


        Htmlstring = Htmlstring.Replace("&middot;", "");
        Htmlstring = Htmlstring.Replace("&rdquo;", "");
        Htmlstring = Htmlstring.Replace("&ldquo;", "");
        Htmlstring = Htmlstring.Replace("amp;", "");
        Htmlstring.Replace("<", "");
        Htmlstring.Replace(">", "");
        Htmlstring.Replace("\r\n", "");
        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
        Htmlstring = Htmlstring.Replace("mdash;", "");
        Htmlstring = Htmlstring.Replace("hellip;", "");

        return Htmlstring;
    }

    /// <summary>
    /// 获得一个16位时间随机数
    /// </summary>
    /// <returns>返回随机数</returns>
    public static string GetDataRandom()
    {
        string strData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        strData = strData.Replace(":", "");
        strData = strData.Replace("-", "");
        strData = strData.Replace(" ", "");
        strData = strData.Replace("/", "");
        Random r = new Random();
        strData = strData + r.Next(100000);
        return strData;
    }

     
    #endregion

     

}

