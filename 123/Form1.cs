using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using TabelDoc.Models;

namespace _123
{
    public partial class Form1 : Form
    {
        private List<string> ReqestsList;
        private List<string> RKKList;
        private List<Tabel> Tabels;

        public Form1()
        {
            InitializeComponent();
        }

        private void bt_DownloadReqests_Click(object sender, EventArgs e)
        {
            string fileName;

            ReqestsList = GetLinesFromFile(out fileName);

            if (fileName != string.Empty)
            {
                label1.Text = Path.GetFileName(fileName);
                ShowResult();
            }                
            else
                MessageBox.Show("Файл не выбран");
        }

        private void bt_DownloadRKK_Click(object sender, EventArgs e)
        {
            string fileName;

            RKKList = GetLinesFromFile(out fileName);

            if (fileName != string.Empty)
            {
                label2.Text = Path.GetFileName(fileName);
                ShowResult();
            }
            else
                MessageBox.Show("Файл не выбран");
        }

        private List<string> GetLinesFromFile(out string fileName)
        {
            fileName = string.Empty;
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Multiselect = false;
                DialogResult dialogResult = fileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    fileName = fileDialog.FileName;
                }
            }

            if (fileName != string.Empty)
                return File.ReadAllLines(fileName).ToList();
            
            return null;
        }
        
        private void ShowResult()
        {
            if (RKKList == null || ReqestsList == null)
                return;

            Tabels = new List<Tabel>();

            GetValuesFromReqests();
            GetValuesFromRKK();

            //TODO: вывести табель в грид
        }

        private void GetValuesFromReqests()
        {
            foreach (var reqest in ReqestsList)
            {
                var tabel = new Tabel();
                var otv = SelectOtv(reqest);

                if (Tabels.Any(x => x.FullName == otv))
                {
                    Tabels.First(x => x.FullName == otv).Unfulfilledletter += 1;
                }
                else
                {
                    tabel.FullName = otv;
                    tabel.Unfulfilledletter = 1;
                    Tabels.Add(tabel);
                }
            }
        }

        private void GetValuesFromRKK()
        {
            foreach (var reqest in RKKList)
            {
                var tabel = new Tabel();
                var otv = SelectOtv(reqest);

                if (Tabels.Any(x => x.FullName == otv))
                {
                    Tabels.First(x => x.FullName == otv).UnfulfilledDoc += 1;
                }
                else
                {
                    tabel.FullName = otv;
                    tabel.UnfulfilledDoc = 1;
                    Tabels.Add(tabel);
                }
            }
        }

        private string SelectOtv(string line)
        {
            const string otvKey = "(Отв.)";
            List<string> otvList = new List<string>();
            var separator = new[] { '\t', ';' };
            var splitedLine = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            otvList.AddRange(splitedLine);

            var otv = otvList.FirstOrDefault(x => x.Contains(otvKey));
            if (otv != default)
                return otv.Replace(otvKey, "").Trim();

            return splitedLine[0].Trim();
        }
    }
}
