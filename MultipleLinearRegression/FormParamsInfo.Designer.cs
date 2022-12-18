namespace MultipleLinearRegression
{
    partial class FormParamsInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvParamsInfo = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamsInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvParamsInfo
            // 
            this.dgvParamsInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvParamsInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParamsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvParamsInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvParamsInfo.Name = "dgvParamsInfo";
            this.dgvParamsInfo.Size = new System.Drawing.Size(441, 291);
            this.dgvParamsInfo.TabIndex = 0;
            // 
            // FormParamsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 291);
            this.Controls.Add(this.dgvParamsInfo);
            this.Name = "FormParamsInfo";
            this.Text = "FormParamsInfo";
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamsInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvParamsInfo;
    }
}