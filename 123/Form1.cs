using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace _123
{
    public partial class Form1 : Form
    {
        private List<string> ReqestsList;
        private List<string> RKKList;
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
                MessageBox.Show($"Первая строка файла: {ReqestsList.FirstOrDefault()}");
            }                
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }

        private void bt_DownloadRKK_Click(object sender, EventArgs e)
        {

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
    }
}
