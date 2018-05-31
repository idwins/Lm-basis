 
using System.Text;
using System.Web.UI;

/// <summary>
///MsgHelper 的摘要说明
/// </summary>
public class MsgHelper
{
    static StringBuilder sb = null;
    //图标类别
    public enum LIcon
    {
        勾 = 1,
        叉 = 2,
        问 = 3,
        锁 = 4,
        哭 = 5,
        笑 = 6,
        叹 = 7
    }

    public MsgHelper()
    {
    }

    #region 消息提示

    /// <summary>
    /// 消息提示
    /// </summary>
    /// <param name="tempPage">提示页面【this】</param>
    /// <param name="tempTitle">标题【?null】</param>
    /// <param name="tempContent">内容</param>
    /// <param name="tempTime">自动消失时间【?0、默认5S自动关闭】</param>
    /// <param name="tempIcon">显示图标【1勾、2叉、3问、4锁、5哭、6笑、7叹】</param>
    /// <param name="tempUrl">提示完成后跳转的页面【?null、相对路径】</param>
    public static void ShowMsg(Page tempPage, string tempTitle, string tempContent, int tempTime, LIcon tempIcon, string tempUrl)
    {
        sb = new StringBuilder();
        sb.Append("<script type='text/javascript'>layui.use('layer',function(){ var time = " + (tempTime != 0 ? tempTime : 5) + "; var index=layer.msg('" + tempContent + "',{ icon:'" + (int)tempIcon + "',area: '350px' ");
        if (!string.IsNullOrEmpty(tempTitle)) sb.Append(" ,title:'" + tempTitle + "【'+time+'】'");
        sb.Append(",shade: [0.2, '#393D49'],time:time* 1000,btn: ['确 定'],yes: function(index){layer.close(index);window.location.href='" + tempUrl + "';}");
        sb.Append(",end: function(index){");
        if (!string.IsNullOrEmpty(tempUrl)) sb.Append(" window.location.href='" + tempUrl + "';");
        sb.Append("}});");
        sb.Append(" var si = setInterval(function () {　time = time - 1; layer.title('" + (string.IsNullOrEmpty(tempTitle) ? "提示" : tempTitle) + "【' + time+'】', index); if (time == 0) clearInterval(si); }, 1000);})");
        sb.Append("</script>");
        tempPage.ClientScript.RegisterStartupScript(tempPage.GetType(), "msg", sb.ToString());
        return;
    }

    /// <summary>
    /// 消息提示
    /// </summary>
    /// <param name="tempPage">提示页面【this】</param>
    /// <param name="tempTitle">标题【?null】</param>
    /// <param name="tempContent">内容</param>
    /// <param name="tempTime">自动消失时间【?0、默认5S自动关闭】</param>
    /// <param name="tempIcon">显示图标【1勾、2叉、3问、4锁、5哭、6笑、7叹】</param>
    /// <param name="tempUrl">提示完成后跳转的页面弹窗【?null、相对路径】</param>
    public static void ShowMsgTC(Page tempPage, string tempTitle, string tempContent, int tempTime, LIcon tempIcon, string endUrl)
    {
        sb = new StringBuilder();
        sb.Append("<script type='text/javascript'>layui.use('layer',function(){ var time = " + (tempTime != 0 ? tempTime : 5) + "; var index=layer.msg('" + tempContent + "',{ icon:'" + (int)tempIcon + "',area: '350px' ");
        if (!string.IsNullOrEmpty(tempTitle)) sb.Append(" ,title:'" + tempTitle + "【'+time+'】'");
        sb.Append(",shade: [0.2, '#393D49'],time:time* 1000,btn: ['确 定'],yes: function(index){layer.close(index);window.location.href='" + endUrl + "';}");
        //sb.Append(",shade: [0.2, '#393D49'],time:time* 1000,btn: ['确 定'],yes: function(index){layer.close(index);layer.open({ type: 2,  title: '办理结果',  shade: 0.3, area: [document.body.clientWidth - 100 + 'px', document.body.clientHeight - 50 + 'px'],content: '" + tempUrl + "', end: function () { window.location.href='" + endUrl + "'; } });}");
        sb.Append(",end: function(index){");
        //if (!string.IsNullOrEmpty(tempUrl)) sb.Append(" layer.open({ type: 2,  title: '办理结果',  shade: 0.3, area: [document.body.clientWidth - 100 + 'px', document.body.clientHeight - 50 + 'px'],content: '" + tempUrl + "', end:function () { window.location.href='" + endUrl + "'; } });");
        if (!string.IsNullOrEmpty(endUrl)) sb.Append(" window.location.href='" + endUrl + "';");
        sb.Append("}});");
        sb.Append(" var si = setInterval(function () {　time = time - 1; layer.title('" + (string.IsNullOrEmpty(tempTitle) ? "提示" : tempTitle) + "【' + time+'】', index); if (time == 0) clearInterval(si); }, 1000);})");
        sb.Append("</script>");
        tempPage.ClientScript.RegisterStartupScript(tempPage.GetType(), "msg", sb.ToString());
        return;
    }

    /// <summary>
    /// 消息提示【增刷新父窗体、关闭本级】
    /// </summary>
    /// <param name="tempPage">提示页面【this】</param>
    /// <param name="tempTitle">标题【?null】</param>
    /// <param name="tempContent">内容</param>
    /// <param name="tempTime">自动消失时间【?0、默认5S自动关闭】</param>
    /// <param name="tempIcon">显示图标【1勾、2叉、3问、4锁、5哭、6笑、7叹】</param>
    /// <param name="needClose">是否关闭当前页</param>
    /// <param name="reloadParentFrom">父页面是否刷新，需设置父级end监听事件，form1</param>
    public static void ShowMsg(Page tempPage, string tempTitle, string tempContent, int tempTime, LIcon tempIcon, bool needClose, bool tempReload)
    {
        sb = new StringBuilder();
        sb.Append("<script type='text/javascript'>layui.use('layer',function(){ var time = " + (tempTime != 0 ? tempTime : 5) + ";  var index=layer.msg('" + tempContent + "',{ icon:'" + (int)tempIcon + "',area:'350px' ");
        if (!string.IsNullOrEmpty(tempTitle)) sb.Append(" ,title:'" + tempTitle + "【'+time+'】'");
        sb.Append(",shade: [0.2, '#393D49'],time:time* 1000,btn: ['确 定'],yes: function(index){layer.close(index);}");
        sb.Append(",end: function(index){");
        if (tempReload) sb.Append(" parent.location.reload(); ");
        if (needClose) sb.Append("  parent.layer.close(parent.layer.getFrameIndex(window.name)); ");
        sb.Append("}});");
        sb.Append(" var si = setInterval(function () {　time = time - 1; layer.title('" + (string.IsNullOrEmpty(tempTitle) ? "提示" : tempTitle) + "【' + time+'】', index); if (time == 0) clearInterval(si); }, 1000);})");
        sb.Append("</script>");
        tempPage.ClientScript.RegisterStartupScript(tempPage.GetType(), "msg", sb.ToString());
        return;
    }

    /// <summary>
    /// 消息提示【增刷新父父级窗体、关闭本级、父级】
    /// </summary>
    /// <param name="tempPage">提示页面【this】</param>
    /// <param name="tempTitle">标题【?null】</param>
    /// <param name="tempContent">内容</param>
    /// <param name="tempTime">自动消失时间【?0、默认5S自动关闭】</param>
    /// <param name="tempIcon">显示图标【1勾、2叉、3问、4锁、5哭、6笑、7叹】</param>
    /// <param name="needClose">是否关闭当前页</param>
    /// <param name="reloadParentFrom">父页面是否刷新，需设置父级end监听事件，form1</param>
    public static void ShowMsg(Page tempPage, string tempTitle, string tempContent, double tempTime, LIcon tempIcon, bool needClose, bool tempReload, bool ffj)
    {
        sb = new StringBuilder();
        sb.Append("<script type='text/javascript'>layui.use('layer',function(){ var time = " + (tempTime != 0 ? tempTime : 5) + ";  var index=layer.msg('" + tempContent + "',{ icon:'" + (int)tempIcon + "',area:'350px' ");
        if (!string.IsNullOrEmpty(tempTitle)) sb.Append(" ,title:'" + tempTitle + "【'+time+'】'");
        sb.Append(",shade: [0.2, '#393D49'],time:time* 1000,btn: ['确 定'],yes: function(index){layer.close(index);parent.location.href='DbsxList.aspx';parent.layer.close(parent.layer.getFrameIndex(window.name));parent.parent.layer.close(parent.layer.getFrameIndex(window.name)); }");
        sb.Append(",end: function(index){");
        if (tempReload) sb.Append(" parent.location.href='DbsxList.aspx'; ");
        if (needClose) sb.Append("  parent.layer.close(parent.layer.getFrameIndex(window.name)); ");
        if (ffj) sb.Append("  parent.parent.layer.close(parent.layer.getFrameIndex(window.name)); ");
        sb.Append("}});");
        sb.Append(" var si = setInterval(function () {　time = time - 1; layer.title('" + (string.IsNullOrEmpty(tempTitle) ? "提示" : tempTitle) + "【' + time+'】', index); if (time == 0) clearInterval(si); }, 1000);})");
        sb.Append("</script>");
        tempPage.ClientScript.RegisterStartupScript(tempPage.GetType(), "msg", sb.ToString());
        return;
    }

    #endregion

    
}