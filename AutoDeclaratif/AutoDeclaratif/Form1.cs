using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace AutoDeclaratif
{
    public partial class Form1 : Form
    {
        private const int DURATION_LINE = 3;
        private const int NUMBER_OF_LINES = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //Lines Durée and separator a readonly
            if (e.RowIndex % 5 == 3 || e.RowIndex % 5 == 4)
            {
                e.Cancel = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BindDataGridView();
        }

        private void BindDataGridView()
        {
            //Set columns
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[10] {
                new DataColumn(" ", typeof(string)),
                new DataColumn("Lundi", typeof(string)),
                new DataColumn("Mardi", typeof(string)),
                new DataColumn("Mercredi", typeof(string)),
                new DataColumn("Jeudi", typeof(string)),
                new DataColumn("Vendredi", typeof(string)),
                new DataColumn("Samedi", typeof(string)),
                new DataColumn("Dimanche", typeof(string)),
                new DataColumn("Total", typeof(string)),
                new DataColumn("Moyenne",typeof(string)) }
            );
            dt.Columns[0].ReadOnly = true;
            dt.Columns["Total"].ReadOnly = true;
            dt.Columns["Moyenne"].ReadOnly = true;

            for (int i = 0; i < 6; i++)
            {
                //Set data for a week
                dt.Rows.Add("Arrivée", "09:00", "09:20", "09:20", "09:20", "09:20", "", "", "", "");
                dt.Rows.Add("Pause", "00:20", "00:20", "00:40", "00:20", "00:20", "", "", "", "");
                dt.Rows.Add("Départ", "18:00", "18:20", "18:20", "18:20", "18:20", "", "", "", "");
                dt.Rows.Add("Durée", "09:00", "09:20", "09:20", "09:20", "09:20", "", "", "36,5", "7,33");

                if (i != 5)
                {
                    dt.Rows.Add();
                }
            }

            dataGridView1.DataSource = dt;

            //Disable sort for all columns
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //Set colors for read only cells and week ends
            dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns["Samedi"].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            dataGridView1.Columns["Dimanche"].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            dataGridView1.Columns["Total"].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns["Moyenne"].DefaultCellStyle.BackColor = Color.LightGray;

            for (int i = 0; i < 6; i++)
            {
                dataGridView1.Rows[i * NUMBER_OF_LINES + DURATION_LINE].DefaultCellStyle.BackColor = Color.LightGray;
            }

            //Activate the double buffer to speed draw and resize
            typeof(DataGridView).InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               dataGridView1,
               new object[] { true });
        }
    }
}