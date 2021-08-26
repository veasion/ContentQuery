using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ContentQuery
{
    public partial class QueryForm : Form
    {

        // 是否正在搜索
        static bool searching = false;
        //后缀
        static string txtreg = "";
        //查询的文件数量
        static int count = 0;
        //文件Panel集合
        static List<Panel> list = new List<Panel>();
        //搜索类型: 0 文件名和内容 1内容 2文件名
        static int searchType = 0;
        // 最大搜索文件(300M)
        static int maxByte = 1024 * 1024 * 300;
        // 内容高度, 分页大小
        static int panelHeight = 60, pageSize = 5;

        public QueryForm()
        {
            ThreadPool.SetMinThreads(3, 3);
            ThreadPool.SetMaxThreads(30, 100);
            InitializeComponent();
            this.cbo_type.SelectedIndex = 0;
            string path = CacheHelper.getOtherText();
            if (path != null && !"".Equals(path) && Directory.Exists(path))
            {
                this.txtpath.Text = path;
            }
            else
            {
                this.txtpath.Text = Directory.GetCurrentDirectory();
            }
        }

        #region 检索按钮Click
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            searching = true;
            searchType = this.cbo_type.SelectedIndex;
            if ("暂停".Equals(this.but_search.Text))
            {
                searching = false;
                this.but_search.Text = "搜索";
                this.but_search.Enabled = false;
                this.timer_query.Enabled = false;
                this.labmess.Text = "正在终止线程...";
                Thread.Sleep(1000);
                searchComplete();
                this.but_search.Enabled = true;
                return;
            }
            if (this.txtnr.Text.Trim().Equals("") || this.txtpath.Text.Trim().Equals(""))
            {
                return;
            }
            list.Clear();
            this.pNr.Controls.Clear();
            this.labjg.Text = "查找结果(0)： 1/1";
            count = 0;
            string reg = "";
            reg += this.cbotxt.Checked ? "|txt" : "";
            reg += this.cbo_docx.Checked ? "|docx" : "";
            reg += this.cbo_xlsx.Checked ? "|xlsx" : "";
            reg += this.cbo_md.Checked ? "|md" : "";
            string hz = this.txthz.Text.Trim();
            if (!hz.Equals(""))
            {
                reg += "|" + hz;
            }
            this.labmess.Text = "查询中，请稍后...";
            this.labmess.Location = new Point(80, (this.Height + 16) / 2);

            if (!reg.Trim().Equals(""))
            {
                reg = reg.Substring(1);
            }

            txtreg = reg;

            ThreadPool.QueueUserWorkItem(new WaitCallback(Run), this.txtpath.Text);

            this.but_search.Text = "暂停";
            this.timer_query.Enabled = true;
            CacheHelper.cacheOtherText(this.txtpath.Text);
        }
        #endregion

        #region 上下翻页
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel link = (LinkLabel)sender;
            string txt = link.Text;
            string str = this.labjg.Text;
            int f = str.LastIndexOf("：");
            int x = str.LastIndexOf("/");
            int page = int.Parse(str.Substring(f + 1, x - f - 1).Trim());
            int countp = int.Parse(str.Substring(x + 1).Trim());
            if (txt.Equals("上一页"))
            {
                if (page > 1)
                {
                    this.pNr.Controls.Clear();
                    this.loadPanel(page - 1);
                }
            }
            else
            {
                if (page < countp)
                {
                    this.pNr.Controls.Clear();
                    this.loadPanel(page + 1);
                }
            }
        }
        #endregion

        #region 浏览
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "请指定文件夹...";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\Windows";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.txtpath.Text = fbd.SelectedPath;
            }
        }
        #endregion

        #region 查询搜索逻辑代码--线程
        public void Run(object name)
        {
            Console.WriteLine(searching + " > " + name);
            if (!searching)
            {
                return;
            }
            string path = name as string;
            //文件夹
            DirectoryInfo di = new DirectoryInfo(path);
            //获取所有文件
            try
            {
                FileInfo[] frr = di.GetFiles();
                if (frr != null)
                {
                    foreach (FileInfo item in frr)
                    {
                        if (!searching)
                        {
                            return;
                        }
                        //文件名
                        string fname = item.Name;
                        //按内容查询
                        if (searchType == 0 || searchType == 1)
                        {
                            if (Regex.IsMatch(item.Extension, ".(" + txtreg + ")") && item.Length <= maxByte)
                            {
                                bool has = SearchFactory.GetSearch(item).hasText(item, this.txtnr.Text.Trim());
                                if (has)
                                {
                                    addPanel(fname, item.FullName);
                                    continue;
                                }
                            }
                        }
                        //按文件名查询
                        if ((searchType == 0 || searchType == 2) && fname.Contains(this.txtnr.Text.Trim()))
                        {
                            addPanel(fname, item.FullName);
                            continue;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.Error.WriteLine("查询异常: " + e.Message);
            }
            if (!searching)
            {
                return;
            }
            //获取所有文件夹
            try
            {
                DirectoryInfo[] drr = di.GetDirectories();
                if (drr != null)
                {
                    foreach (var item in drr)
                    {
                        if (!searching)
                        {
                            return;
                        }
                        if (".git".Equals(item.Name))
                        {
                            continue;
                        }
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Run), item.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("查询异常: " + e.Message);
            }

        }
        #endregion

        #region 创建文件到Panel
        private void addPanel(string fname, string fullname)
        {
            int left = 20, top = 6;
            Panel p = new Panel();
            p.Width = this.panel1.Width - 2;
            p.Height = panelHeight;
            // p.BorderStyle = BorderStyle.FixedSingle;

            Label l = new Label();
            l.Text = fname;
            l.Location = new Point(left, top);
            l.Font = new Font("微软雅黑", 10);
            l.AutoSize = true;

            left += 28; top += 22;

            TextBox t = new TextBox();
            t.Width = 220;
            t.Height = 23;
            t.Location = new Point(left, top);
            t.Font = new Font("微软雅黑", 9);
            t.ReadOnly = true;
            t.Text = fullname;

            left += t.Width + 10;
            top += 4;

            LinkLabel link = new LinkLabel();
            link.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel_Open);
            link.Font = new Font("微软雅黑", 9);
            link.Location = new Point(left, top);
            link.Text = "打开";

            p.Controls.Add(l);
            p.Controls.Add(t);
            p.Controls.Add(link);
            lock (list)
            {
                l.Text = (++count) + "： " + fname;
                list.Add(p);
            }
        }
        #endregion

        #region 按页显示Panel
        private void loadPanel(int page)
        {
            int countp;
            if (count % pageSize == 0)
            {
                countp = count / pageSize;
            }
            else
            {
                countp = count / pageSize + 1;
            }
            this.labjg.Text = "查找结果(" + count + ")： " + page + "/" + countp;
            int i = (page - 1) * pageSize;
            for (int j = 0; i < list.Count; i++, j++)
            {
                Panel p = list[i];
                p.Location = new Point(0, panelHeight * j);
                this.pNr.Controls.Add(p);
                if (j == pageSize - 1)
                {
                    break;
                }
            }
        }
        #endregion 

        #region 浏览/打开
        //open
        private void linkLabel_Open(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel link = (LinkLabel)sender;
            foreach (Control c in link.Parent.Controls)
            {
                if (c is TextBox)
                {
                    Process.Start("explorer.exe", "/select," + c.Text);
                    break;
                }
            }

        }
        #endregion

        #region 计时器检测是否查询完毕
        int mcount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.labjg.Text = "查找结果(" + list.Count + ")： 1/1";
            if (mcount++ == 0)
            {
                this.labmess.Text = "查询中，请稍后";
            }
            else
            {
                this.labmess.Text += ".";
            }
            if (mcount == 4)
            {
                mcount = 0;
            }
            if (checkThreadPoolComplete())
            {
                searchComplete();
                return;
            }
        }

        private void searchComplete()
        {
            searching = false;
            this.but_search.Text = "搜索";
            this.timer_query.Enabled = false;
            this.labmess.Text = "";
            this.labmess.Location = new Point(8, 105);
            loadPanel(1);
            if (list.Count == 0)
            {
                MessageBox.Show("没有找到相关的数据！");
            }
        }

        private static bool checkThreadPoolComplete()
        {
            int maxWorkerThreads, workerThreads, portThreads;
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
            ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
            int count = maxWorkerThreads - workerThreads;
            Console.WriteLine("线程数: " + count);
            return count == 0;
        }
        #endregion

        #region 帮助
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string message = @"
                自写格式：

                    根据指定后缀搜索文件内容

                    多个后缀用 | 隔开，不能有空格

                    如: doc | xls | pdf | pptx | xml | sql | java | vue | js
                    
                                                            -- luozhuowei";
            MessageBox.Show(message, "帮助");
        }
        #endregion


    }
}
