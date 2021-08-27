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
        static int searchCount = 0;
        //文件Panel集合
        static List<Panel> list = new List<Panel>();
        //搜索类型: 0 文件名和内容 1内容 2文件名
        static int searchType = 0;
        // 最大搜索文件(300M)
        static int maxByte = 1024 * 1024 * 300;
        // 内容高度, 分页大小
        static int panelHeight = 60, pageIndex = 1, pageSize = 5;
        // 支持搜索中实时展示
        static bool searchShow = false;
        // 多少个文件一个线程
        const int maxFileToThread = 10;
        // 总共扫描文件数
        static int searchFileCount = 0;

        public QueryForm()
        {
            ThreadPool.SetMinThreads(5, 5);
            ThreadPool.SetMaxThreads(50, 100);
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
            searchShow = false;
            searchType = this.cbo_type.SelectedIndex;
            if ("暂停".Equals(this.but_search.Text))
            {
                searching = false;
                this.but_search.Text = "搜索";
                this.but_search.Enabled = false;
                this.timer_query.Enabled = false;
                if (this.pNr.Controls.Count > 0)
                {
                    this.Text = "正在终止线程...";
                }
                else
                {
                    this.labmess.Text = "正在终止线程...";
                }
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
            searchFileCount = 0;
            this.labjg.Text = "查找结果(0)： 1/1";
            mcount = 0;
            searchCount = 0;
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
            this.labmess.Location = new Point(60, (this.Height + 16) / 2);
            this.labmess.Visible = true;

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
            if (txt.Equals("上一页"))
            {
                this.loadPanel(pageIndex - 1);
            }
            else
            {
                this.loadPanel(pageIndex + 1);
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
            Console.WriteLine("正在搜索: " + name);
            if (!searching)
            {
                return;
            }
            string path = name as string;
            //文件夹
            DirectoryInfo di = new DirectoryInfo(path);
            try
            {
                //获取所有文件
                FileInfo[] frr = di.GetFiles();
                if (frr != null)
                {
                    splitFilesToThread(frr);
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
            try
            {
                //获取所有文件夹
                DirectoryInfo[] drr = di.GetDirectories();
                if (drr != null)
                {
                    searchDirectory(drr);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("查询异常: " + e.Message);
            }
        }

        private void splitFilesToThread(FileInfo[] frr)
        {
            if (searchType == 2 || frr.Length <= maxFileToThread)
            {
                searchFiles(frr);
            }
            else
            {
                for (int idx = 10; idx < frr.Length; idx += maxFileToThread)
                {
                    FileInfo[] arr = split(frr, idx, maxFileToThread);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(searchFiles), arr);
                }
                searchFiles(split(frr, 0, maxFileToThread));
            }
        }

        private FileInfo[] split(FileInfo[] frr, int startIndex, int length)
        {
            if (startIndex + length > frr.Length)
            {
                length = frr.Length - startIndex;
            }
            FileInfo[] arr = new FileInfo[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = frr[startIndex + i];
            }
            return arr;
        }

        private void searchFiles(object files)
        {
            FileInfo[] frr = files as FileInfo[];
            foreach (FileInfo item in frr)
            {
                if (!searching)
                {
                    return;
                }
                Interlocked.Increment(ref searchFileCount);
                //文件名
                string fname = item.Name;
                //按文件名查询
                if ((searchType == 0 || searchType == 2) && fname.Contains(this.txtnr.Text.Trim()))
                {
                    addPanel(fname, item.FullName);
                    continue;
                }
                //按内容查询
                if (searchType == 0 || searchType == 1)
                {
                    if (Regex.IsMatch(item.Extension, ".(" + txtreg + ")") && item.Length <= maxByte)
                    {
                        Console.WriteLine("正在搜索: " + item.FullName);
                        bool has = SearchFactory.GetSearch(item).hasText(item, this.txtnr.Text.Trim());
                        if (has)
                        {
                            addPanel(fname, item.FullName);
                            continue;
                        }
                    }
                }
            }
        }

        private void searchDirectory(DirectoryInfo[] drr)
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
                l.Text = Interlocked.Increment(ref searchCount) + "： " + fname;
                list.Add(p);
            }
        }
        #endregion

        #region 按页显示Panel
        private void loadPanel(int page)
        {
            int maxPage = (searchCount - 1) / pageSize + 1;
            if (page < 1)
            {
                page = 1;
            }
            else if (page > maxPage)
            {
                page = maxPage;
            }
            pageIndex = page;
            this.pNr.Controls.Clear();
            this.labjg.Text = "查找结果(" + searchCount + ")： " + pageIndex + "/" + maxPage;
            int i = (pageIndex - 1) * pageSize;
            for (int j = 0; i < searchCount; i++, j++)
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
        static int mcount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.labjg.Text = "查找结果(" + searchCount + ")： " + pageIndex + "/" + ((searchCount - 1) / pageSize + 1);
            if (searchShow)
            {
                this.labmess.Text = "";
                if (mcount == 0)
                {
                    if (!searching)
                    {
                        this.Text = "正在终止线程";
                    }
                    else
                    {
                        this.Text = "搜索中(" + searchFileCount + ")，请稍后";
                    }
                }
                else
                {
                    this.Text += ".";
                }
            }
            else
            {
                if (mcount == 0)
                {
                    if (!searching)
                    {
                        this.labmess.Text = "正在终止线程";
                    }
                    else
                    {
                        this.labmess.Text = "搜索中(" + searchFileCount + ")，请稍后";
                    }
                }
                else
                {
                    this.labmess.Text += ".";
                }
            }
            if (++mcount == 5)
            {
                mcount = 0;
            }
            if (checkThreadPoolComplete())
            {
                searchComplete();
                return;
            }
            else if (!searchShow && searchCount > 1)
            {
                searchShow = searchCount >= pageSize;
                showPanel(1);
            }
        }

        private void searchComplete()
        {
            searching = false;
            searchShow = false;
            this.Text = "查找相关内容的文件";
            this.but_search.Text = "搜索";
            this.timer_query.Enabled = false;
            showPanel(pageIndex);
            Console.WriteLine("线程数: " + runThreadCount());
            searchCount = list.Count;
            if (searchCount == 0)
            {
                MessageBox.Show("没有找到相关的数据！");
            }
        }

        private void showPanel(int page)
        {
            this.labmess.Text = "";
            this.labmess.Location = new Point(8, 105);
            this.labmess.Visible = false;
            loadPanel(page);
        }

        private static bool checkThreadPoolComplete()
        {
            return runThreadCount() == 0;
        }

        private static int runThreadCount()
        {
            int maxWorkerThreads, workerThreads, portThreads;
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
            ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
            return maxWorkerThreads - workerThreads;
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
