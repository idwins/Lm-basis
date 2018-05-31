using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OAManage
{
    public partial class NoteHelper : Form
    {
        public NoteHelper()
        {
            InitializeComponent();
        }
        public static DataTable dtNotes;

        private void NoteHelper_Load(object sender, EventArgs e)
        {
            btnRemark.Enabled = true;
            webBrow.Navigate(txtUrl.Text.Trim());
            dtNotes = new DataTable();
            dtNotes.Columns.AddRange(new DataColumn[] { new DataColumn("Content", Type.GetType("System.String")), new DataColumn("Date", Type.GetType("System.DateTime")), new DataColumn("WeekDay", Type.GetType("System.String")) });
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            dtNotes.Clear();
            webBrow.Navigate(txtUrl.Text.Trim());
        }

        private void webBrow_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrow.ReadyState != WebBrowserReadyState.Complete)
                return;
            HtmlDocument docs = webBrow.Document;
            string url = webBrow.Url.ToString();
            if (url.IndexOf("index") > 0)//登陆
            {

                labState.Text = "登录中...";
                docs.GetElementById("userName").SetAttribute("value", txtUName.Text.Trim());
                docs.GetElementById("userPwd").SetAttribute("value", txtPwd.Text.Trim());
                docs.GetElementById("CheckSubmit").InvokeMember("click");
            }
            else if (url.IndexOf("HomeMain") > 0)//主页
            {

                labState.Text = "已登录";
                webBrow.Navigate("http://oa.wangan.cn/WrokJour/gzlist.aspx");
            }
            else if (url.IndexOf("Working_Summarize") > 0)
            {

                labState.Text = "已加载总结录入页面";
            }
            else if (url.IndexOf("Working_Sum_List") > 0)
            {
                labState.Text = "已加载总结查询页面";
                docs.GetElementById("sumtype").SetAttribute("value", "周工作总结");
                docs.GetElementById("month").SetAttribute("value", DateTime.Parse(dateE.Text).Month.ToString());

                HtmlElement nextA = docs.GetElementById("Button4");
                nextA.InvokeMember("click");
                webBrow.Stop();
            }
            else if (url.IndexOf("gzlist") > 0)
            {
                labState.Text = "日志页";
                //日志获取 
                string txt = docs.GetElementById("dw").InnerText;
                StringToDataTable(txt);
                dtNotes.DefaultView.Sort = "Date DESC";
                dtNotes = dtNotes.DefaultView.ToTable();
                //上个月  
                DateTime dtPre = DateTime.Now.AddMonths(-1);
                if (DateTime.Parse(dtNotes.Rows[dtNotes.Rows.Count - 1]["Date"].ToString()) > dtPre)
                {
                    HtmlElement nextA = GetElement_Text(webBrow, "下一页");
                    nextA.InvokeMember("click");
                }
                else
                {

                    //填充日志
                    dGv.DataSource = dtNotes;
                }
            }
        }

        /// string 到 DataTable
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public void StringToDataTable(string strdata)
        {
            if (string.IsNullOrEmpty(strdata))
            {
                return;
            }

            string[] strRow = { "\r\n" };
            string[] arrRows = strdata.Split(strRow, StringSplitOptions.None);
            for (int rowIndex = 1; rowIndex < arrRows.Length - 1; rowIndex++)       //行的字符串数组遍历
            {
                string vsRow = arrRows[rowIndex];
                //时间中的减号
                int indexData = vsRow.IndexOf("-");
                if (indexData > 0)
                {
                    string content = vsRow.Substring(0, indexData - 4).Trim();
                    string date = DateTime.Parse(vsRow.Substring(indexData - 4).Replace("修改", "").Trim()).ToString("yyyy-MM-dd");
                    string weekday = DateTime.Parse(date).DayOfWeek.ToString();
                    string[] vsColumns = new string[] { content, date, weekday };
                    dtNotes.Rows.Add(vsColumns);
                }
            }
        }

        public HtmlElement GetElement_Text(WebBrowser wb, string text)
        {
            HtmlElementCollection elements = webBrow.Document.GetElementsByTagName("a");
            foreach (HtmlElement element in elements)
            {
                if (element.InnerText == text)
                    return element;
            }
            return null;
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsWebB_Click(object sender, EventArgs e)
        {
            if (webBrow.Visible)
            {
                IsWebB.Text = "ShowPage";
                webBrow.Visible = false;
            }
            else
            {

                IsWebB.Text = "HidePage";
                webBrow.Visible = true;
            }
        }

        /// <summary>
        /// 时间查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWork_Click(object sender, EventArgs e)
        {
            DataRow[] drs = dtNotes.Select(" Date>='" + dateB.Text.Trim() + "' and Date<='" + dateE.Text.Trim() + "'", "Date DESC");
            DataTable dt = new DataTable();
            if (drs != null && drs.Length > 0) dt = drs.CopyToDataTable();
            dGv.DataSource = dt;

        }
        /// <summary>
        /// 生成描述
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemark_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dGv.DataSource;
            string txt = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt += (i + 1) + "、" + dt.Rows[i][0].ToString() + "\r\n";

            }
            txtWorkMsg.Text = txt;
            txtWorkMsg.Show();
            btnInWord.Enabled = true;
            labState.Text = "已生成综合描述";
        }
        /// <summary>
        /// 填充模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInWord_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)dGv.DataSource;
            string WorkDate = DateTime.Parse(dt.Compute("Max(Date)", "").ToString()).ToString("yyyy-MM-dd");
            byte[] bys = Properties.Resources.工作总结模板;
            MemoryStream ms = new MemoryStream(bys);
            Aspose.Words.Document doc = new Aspose.Words.Document(ms);
            Aspose.Words.DocumentBuilder builder = new Aspose.Words.DocumentBuilder(doc);
            builder.MoveToBookmark("WorkDate");
            builder.Write(WorkDate);//编写日期
            builder.MoveToBookmark("WorkMsg");//综合描述
            builder.Write(txtWorkMsg.Text.Trim());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                builder.MoveToBookmark("Content" + i);
                builder.Write(dt.Rows[i][0].ToString());
                builder.MoveToBookmark("Day" + i);
                builder.Write(DateTime.Parse(dt.Rows[i][1].ToString()).ToString("yyyy-MM-dd"));
            }
            string saveSrc = @"D:\Documents\Desktop\\" + WorkDate + " 工作总结.Doc";
            if (File.Exists(saveSrc)) File.Delete(saveSrc);
            doc.Save(saveSrc);
            btnInWord.Enabled = false;
            txtWorkMsg.Text = "";
            txtWorkMsg.Visible = false;
            labState.Text = "已生成综合描述";

        }

        private void btnZj_Click(object sender, EventArgs e)
        {

            labState.Text = "跳转总结提交页面";
            webBrow.Navigate("http://oa.wangan.cn/WorkingPlan/Working_Summarize.aspx?chenkeyu=22&chenkeyutime=" + DateTime.Parse(dateE.Text.Trim()).ToString("yyyy-MM-dd"));
        }

        private void btnSearchWeek_Click(object sender, EventArgs e)
        {
            labState.Text = "跳转总结页面";
            webBrow.Navigate("http://oa.wangan.cn/WorkingPlan/Working_Sum_List.aspx");
        }
    }
}


 