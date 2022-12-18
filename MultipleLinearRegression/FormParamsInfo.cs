using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultipleLinearRegression
{
    public partial class FormParamsInfo : Form
    {
        public FormParamsInfo(Dictionary<string, int> paramsNames)
        {
            InitializeComponent();
            InitializeDVG();
            FillDGV(paramsNames);            
        }

        public void InitializeDVG()
        {
            int numberOfColumns = 2;
            var columns = new DataGridViewColumn[numberOfColumns];

            for (int i = 0; i < numberOfColumns; i++)
            {
                columns[i] = new DataGridViewTextBoxColumn();
            }

            columns[0].HeaderText = "Параметр";
            columns[1].HeaderText = "Расшифровка";
            dgvParamsInfo.Columns.AddRange(columns);
        }

        public void FillDGV(Dictionary<string, int> paramsNames)
        {
            foreach (var p in paramsNames)
            {
                if (!p.Equals(paramsNames.Last()))
                    dgvParamsInfo.Rows.Add(string.Format("X{0}", p.Value + 1), p.Key);                
            }
        }
    }
}
