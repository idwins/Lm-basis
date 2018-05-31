 
using System.Reflection; 
using log4net.Layout;
using log4net.Layout.Pattern;
using log4net; 

public class LogHelper
{
    #region 变量声明

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogsLevel
    {
        /// <summary>
        ///一般信息
        /// </summary>
        Info,
        /// <summary>
        ///警告信息
        /// </summary>
        Warn,
        /// <summary>
        ///错误信息
        /// </summary>
        Error,
        /// <summary>
        ///严重错误
        /// </summary>
        Fatal
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogsType
    {
        /// <summary>
        ///错误
        /// </summary>
        Erro = 0,
        /// <summary>
        ///新增
        /// </summary>
        Add = 1,
        /// <summary>
        ///修改
        /// </summary>
        Update = 2,
        /// <summary>
        ///删除
        /// </summary>
        Delete = 3,
        /// <summary>
        ///登陆
        /// </summary>
        Login = 4,
        /// <summary>
        ///下载
        /// </summary>
        Dwon = 5
    }

    private static ILog _log;
    public static ILog log
    {
        get
        {
            if (_log == null)
            {
                //从配置文件中读取Logger对象  WebLogger 里面的配置信息是用来将日志录入到数据库的 
                _log = log4net.LogManager.GetLogger("loginfo"); //log4net.LogManager.GetLogger("WebLogger");
            }
            return _log;
        }
    }

    #endregion

    public LogHelper()
    {

    }

    /// <summary>
    ///记录用户在处理业务时每一步操作行为，以及相关事务处理和数据信息访问等操作记录数据、系统日志等。
    /// </summary> 
    /// <param name="ll">日志级别</param>  
    /// <param name="type">日志类型 0错误1增2改3删4登录5下载</param>
    /// <param name="content">日志内容</param> 
    /// <param name="Remark">备注</param>
    public static void doLogs(LogsLevel ll, LogsType type, string content, string Remark)
    {
        //实例化日志内容
        LogContent lc = new LogContent { HandleType = (int)type, Content = content, Remark = Remark };
        //提交日志信息

        //判定日志级别
        switch (ll)
        {
            case LogsLevel.Info:
                log.Info(lc);
                break;
            case LogsLevel.Warn:
                log.Warn(lc);
                break;
            case LogsLevel.Error:
                log.Error(lc);
                break;
            case LogsLevel.Fatal:
                log.Fatal(lc);
                break;
        }
    }

}
/// <summary>
/// 日志实体 
/// </summary>
public class LogContent : System.Web.UI.Page
{
    /// <summary>
    /// SysUserInfo 用户SysId
    /// </summary>
    public string SysId { get; set; }
    /// <summary>
    /// LogType 日志类别  
    /// </summary>
    public int LogType { get; set; }
    /// <summary>
    /// FromSource 日志来源
    /// </summary>
    public int FromSource { get; set; }
    /// <summary>
    /// 0错误1增2改3删4登录5下载
    /// </summary>
    public int HandleType { get; set; }
    /// <summary>
    /// 日志内容
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// 操纵者Ip
    /// </summary>
    public string Ip { get; set; }
    /// <summary>
    /// 日志来源页面
    /// </summary>
    public string LogsSrc { get; set; }
    /// <summary>
    /// 备注信息
    /// </summary>
    public string Remark { get; set; }

    public LogContent()
    {
        SysId = Session["SysId"] == null ? "-1" : Session["SysId"].ToString();
        Ip = System.Web.HttpContext.Current.Request.UserHostAddress;
        LogsSrc = System.Web.HttpContext.Current.Request.Url.ToString();
    }
}

public class MyLayout : PatternLayout
{
    public MyLayout()
    {
        this.AddConverter("property", typeof(MyMessagePatternConverter));
    }
}

public class MyMessagePatternConverter : PatternLayoutConverter
{
    protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
    {
        if (Option != null)
        {
            // Write the value for the specified key  
            WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
        }
        else
        {
            // Write all the key value pairs  
            WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
        }
    }

    /// <summary>  
    /// 通过反射获取传入的日志对象的某个属性的值  
    /// </summary>  
    /// <param name="property"></param>  
    /// <returns></returns>  
    private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
    {
        object propertyValue = string.Empty;
        PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
        if (propertyInfo != null)
            propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
        return propertyValue;
    }
}

