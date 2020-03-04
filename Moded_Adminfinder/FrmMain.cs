using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moded_Adminfinder
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Btn_run_Click(object sender, EventArgs e)
        {
            List<String> AdminPathes = new List<string>();
            AdminPathes.AddRange(File.ReadAllLines("admins.txt"));

            btn_run.Enabled = false;
            Thread t = new Thread(() =>
            {
                foreach (var ap in AdminPathes)
                    Task.Factory.StartNew(() =>
                    {
                        Boolean isAdmin = FindAdmins(txt_url.Text, ap);
                        if (isAdmin)
                            txt_logs.Invoke((MethodInvoker)delegate { txt_logs.AppendText("Founded " + "http://" + txt_url.Text + "/" + "ap" + txt_url.Text); });
                    });
                btn_run.Invoke((MethodInvoker)delegate { this.btn_run.Enabled = true; });
            });
            t.Start();
        }

        private Boolean FindAdmins(String url, String adminPath)
        {
            try
            {
                var clinet = new RestClient("http://www." + url + "/" + adminPath);
                var req = new RestRequest(Method.GET);

                var resp = clinet.Get(req);
                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;

                return false;
            }
            catch { return false; }
        }
    }
}
