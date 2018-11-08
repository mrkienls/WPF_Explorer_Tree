using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WPF_Explorer_Tree.DB;
using System.Text.RegularExpressions;
using System.Data;
using WPF_Explorer_Tree.EXCEL;


//https://www.codeproject.com/Articles/21248/A-Simple-WPF-Explorer-Tree

namespace WPF_Explorer_Tree
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private object dummyNode = null;

        public Window1()
        {
            InitializeComponent();
            // DBmain.executeSQL("delete  from test","DULIEU_LSN");

            // DBmain.selectSQL("select * from bcss_lsn.hoadon_20181001 where rownum<9", "DULIEU_LSN", GridView);
        }

        public string SelectedImagePath { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string rootPath = Properties.Settings.Default.RootPath;
            txtRootPath.Text = rootPath;
            LoadRootPath(rootPath);
        }

        void LoadRootPath(string rootPath)
        {
            //foreach (string s in Directory.GetLogicalDrives())


            foldersItem.Items.Clear();
            foreach (string s in Directory.GetDirectories(rootPath))

            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);
                foldersItem.Items.Add(item);
            }
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }

        private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = (TreeView)sender;
            TreeViewItem temp = ((TreeViewItem)tree.SelectedItem);

            if (temp == null)
                return;
            SelectedImagePath = "";
            string temp1 = "";
            string temp2 = "";
            while (true)
            {
                temp1 = temp.Header.ToString();
                //if (temp1.Contains(@"\"))
                //{
                //    temp2 = "";
                //}
                SelectedImagePath = temp1 + temp2 + SelectedImagePath;
                if (temp.Parent.GetType().Equals(typeof(TreeView)))
                {
                    break;
                }
                temp = ((TreeViewItem)temp.Parent);
                temp2 = @"\";
            }
            DisplayFiles(SelectedImagePath, listFiles);
        }

        void DisplayFiles(string path, ListView lstView)
        {
            listFiles.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.sql"); //Getting Text files
                                                    //   string str = "";
            foreach (FileInfo file in Files)
            {
                //str = str + ", " + file.Name;
                Button b = new Button();
                b.Content = file.Name;
                b.ToolTip = file.FullName;
                b.Click += Button_click;
                listFiles.Items.Add(b);
            }
        }

        // Khi click vao cac file
        void Button_click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            string s = bt.ToolTip.ToString();
            string content = File.ReadAllText(s);
            txtViewSQL.Text = content;
            txtViewSQL.ToolTip = s;
        }
        #region koxem
        private void listFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listFiles_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void listFiles_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void listFiles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void listFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void cmdRun_SQL(object sender, RoutedEventArgs e)
        {
            string s = txtViewSQL.SelectedText;
            s = Regex.Replace(s, @"\/*(.+)\*\/|--(.+)\n", "");
            DBmain.RunSQLs(s, txtSchema.Text, GridView);
        }

        private void cmdSaveSQL(object sender, RoutedEventArgs e)
        {
            string s;
            s = txtViewSQL.ToolTip as string;
            File.WriteAllText(s, txtViewSQL.Text);
            txtLog.Text = "Save to " + s;
        }

        private void cmdExport(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();

            dt= ExcelMain.grid_To_DataTable(GridView);
            ExcelMain.grid_To_DataTable(GridView);
            ExcelMain.ToExcel("test.xls", dt, "sheet1");
            // ExcelMain.ToExcel("test.xls", dt,"sheet1");
        }

        private void cmdSavePath(object sender, RoutedEventArgs e)
        {
            LoadRootPath(txtRootPath.Text);
            Properties.Settings.Default.RootPath = txtRootPath.Text;
            Properties.Settings.Default.Save();

        }
    }
}