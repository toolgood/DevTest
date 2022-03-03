using DevTest.HttpServer;
using System.Text.RegularExpressions;

namespace DevTest
{
    public partial class MainForm : Form
    {
        MyHttpServer myHttpServer = null;
        List<UrlMap> maps = new List<UrlMap>();
        const string mapFile = "config.ini";

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            maps = UrlMap.Load(mapFile);
            foreach (var item in maps) {
                this.listBox1.Items.Add(item);
            }
            myHttpServer = new MyHttpServer();
            myHttpServer.SetUrlMap(maps);
            myHttpServer.Start();
            //��Host
            button4_Click(null, null);
        }
        // ���ڹر�
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //�����
            button5_Click(null, null);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            UrlMap map = new UrlMap();
            map.Used = true;
            map.NewIp = "127.0.0.1:8080";
            UrlMapForm form = new UrlMapForm();
            form.UrlMap = map;
            form.Render();
            if (form.ShowDialog() == DialogResult.OK) {
                maps.Add(map);
                UrlMap.Save(maps, mapFile);
                this.listBox1.Items.Clear();
                foreach (var item in maps) {
                    this.listBox1.Items.Add(item);
                }
                myHttpServer.SetUrlMap(maps);
                //��Host
                button4_Click(null, null);
            }
        }
        /// <summary>
        /// ˫���༭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex >= 0) {
                var map = this.listBox1.SelectedItem as UrlMap;
                UrlMapForm form = new UrlMapForm();
                form.UrlMap = map;
                form.Render();
                if (form.ShowDialog() == DialogResult.OK) {
                    UrlMap.Save(maps, mapFile);
                    this.listBox1.Items.Clear();
                    foreach (var item in maps) {
                        this.listBox1.Items.Add(item);
                    }
                    myHttpServer.SetUrlMap(maps);
                    //��Host
                    button4_Click(null, null);
                }
            }

        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex >= 0) {
                var map = this.listBox1.SelectedItem as UrlMap;
                if (MessageBox.Show("�Ƿ�ɾ��" + map.Host, "", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    maps.Remove(map);
                    UrlMap.Save(maps, mapFile);
                    this.listBox1.Items.Clear();
                    foreach (var item in maps) {
                        this.listBox1.Items.Add(item);
                    }
                    myHttpServer.SetUrlMap(maps);
                    //��Host
                    button4_Click(null, null);
                }
            }
        }
        /// <summary>
        /// ��Host
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            var hostFile = @"C:\windows\system32\drivers\etc\hosts";
            var list = File.ReadAllLines(hostFile).ToList();

            foreach (var item in maps) {
                Regex regex = new Regex(@$"^{item.NewIp.Split(':')[0].Replace(".", "\\.")}[ \t]+{item.Host.Replace(".", "\\.")}[ \t]*$", RegexOptions.IgnoreCase);
                for (int i = list.Count - 1; i >= 0; i--) {
                    var str = list[i];
                    if (regex.IsMatch(str)) {
                        list.RemoveAt(i);
                    }
                }
            }
            foreach (var item in maps) {
                if (item.Used) {
                    list.Insert(0, $"{item.NewIp.Split(':')[0]} {item.Host}");
                }
            }
            File.WriteAllLines(hostFile, list.ToArray());
        }

        /// <summary>
        /// ���Host
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            var hostFile = @"C:\windows\system32\drivers\etc\hosts";
            var list = File.ReadAllLines(hostFile).ToList();

            foreach (var item in maps) {
                Regex regex = new Regex(@$"^{item.NewIp.Split(':')[0].Replace(".", "\\.")}[ \t]+{item.Host.Replace(".", "\\.")}[ \t]*$", RegexOptions.IgnoreCase);
                for (int i = list.Count - 1; i >= 0; i--) {
                    var str = list[i];
                    if (regex.IsMatch(str)) {
                        list.RemoveAt(i);
                    }
                }
            }
            File.WriteAllLines(hostFile, list.ToArray());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/toolgood/DevTest");
        }
    }
}