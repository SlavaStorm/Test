using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using TabelDoc.Models;
using System.Text;

namespace _123
{
    public partial class Form1 : Form
    {
        private List<string> ReqestsList;
        private List<string> RKKList;
        private List<Tabel> Tabels;
        private BindingSource _binding;        

        public Form1()
        {
            InitializeComponent();
            _binding = new BindingSource();
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

            _binding.DataSource = Tabels;
            dataGridView1.DataSource = _binding;
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
                    Tabels.First(x => x.FullName == otv).TotalNumber += 1;
                }
                else
                {
                    tabel.FullName = otv;
                    tabel.Unfulfilledletter = 1;
                    tabel.Number = Tabels.Count + 1;
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
                    Tabels.First(x => x.FullName == otv).TotalNumber += 1;
                }
                else
                {
                    tabel.FullName = otv;
                    tabel.UnfulfilledDoc = 1;
                    tabel.Number = Tabels.Count + 1;
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
            if (splitedLine[0] != "Климов Сергей Александрович")
                return ShortName(splitedLine[0].Trim());

            otvList.AddRange(splitedLine);

            var otv = otvList.FirstOrDefault(x => x.Contains(otvKey));
            if (otv != default)
                return otv.Replace(otvKey, "").Trim();
            
            return ShortName(splitedLine[0].Trim());


        }
        private string ShortName(string fio)
        {
            string[] str = fio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (str.Length != 3) throw new ArgumentException("ФИО задано в неверно формате");
            return $"{str[0]} {str[1][0]}.{str[2][0]}";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int columnCount = dataGridView1.ColumnCount;
            string[] line = new string[columnCount];
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(sfd.FileName, FileMode.OpenOrCreate), Encoding.UTF8))
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            line[j] = (dataGridView1.Rows[i].Cells[j].Value ?? "").ToString();
                        }
                        writer.WriteLine(string.Join(";", line));
                    }
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    Tabels = Tabels.OrderBy(x => x.Number).ToList();
                    _binding.DataSource = Tabels;
                    break;
                case 1:
                    Tabels = Tabels.OrderBy(x => x.FullName).ToList();
                    _binding.DataSource = Tabels;
                    break;
                case 2:
                    Tabels = Tabels.OrderBy(x => x.UnfulfilledDoc).ToList();
                    _binding.DataSource = Tabels;
                    break;
                case 3:
                    Tabels = Tabels.OrderBy(x => x.Unfulfilledletter).ToList();
                    _binding.DataSource = Tabels;
                    break;
                case 4:
                    Tabels = Tabels.OrderBy(x => x.TotalNumber).ToList();
                    _binding.DataSource = Tabels;
                    break;
            }            
        }
    }


}
