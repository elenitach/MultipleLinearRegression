namespace MultipleLinearRegression
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.labCorrel = new System.Windows.Forms.Label();
            this.numCorrelX = new System.Windows.Forms.NumericUpDown();
            this.numCorrelXY = new System.Windows.Forms.NumericUpDown();
            this.btNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAlpha = new System.Windows.Forms.ComboBox();
            this.tc = new System.Windows.Forms.TabControl();
            this.tpCorrel = new System.Windows.Forms.TabPage();
            this.cbIsRegressionNonLinear = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbIterationsCount = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numDeltaR = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.pbReadingData = new System.Windows.Forms.ProgressBar();
            this.tpParameters = new System.Windows.Forms.TabPage();
            this.btRemoveY = new System.Windows.Forms.Button();
            this.btAddY = new System.Windows.Forms.Button();
            this.btRemoveX = new System.Windows.Forms.Button();
            this.btAddX = new System.Windows.Forms.Button();
            this.lbY = new System.Windows.Forms.ListBox();
            this.lbX = new System.Windows.Forms.ListBox();
            this.lbAllParameters = new System.Windows.Forms.ListBox();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rbAllModels = new System.Windows.Forms.RadioButton();
            this.rbRobustModels = new System.Windows.Forms.RadioButton();
            this.rbBestModels = new System.Windows.Forms.RadioButton();
            this.dgvModels = new System.Windows.Forms.DataGridView();
            this.tpControl = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numPercents = new System.Windows.Forms.NumericUpDown();
            this.numK = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.tbNewXValue = new System.Windows.Forms.TextBox();
            this.lbDependentParams = new System.Windows.Forms.ListBox();
            this.lblRightLimit = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblNewYValue = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblLeftLimit = new System.Windows.Forms.Label();
            this.lbParams = new System.Windows.Forms.ListBox();
            this.btBack = new System.Windows.Forms.Button();
            this.btParamsInfo = new System.Windows.Forms.Button();
            this.epValueOutOfRange = new System.Windows.Forms.ErrorProvider(this.components);
            this.epTooWideRange = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numCorrelX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCorrelXY)).BeginInit();
            this.tc.SuspendLayout();
            this.tpCorrel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeltaR)).BeginInit();
            this.tpParameters.SuspendLayout();
            this.tpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModels)).BeginInit();
            this.tpControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPercents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epValueOutOfRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epTooWideRange)).BeginInit();
            this.SuspendLayout();
            // 
            // btOpenFile
            // 
            this.btOpenFile.Location = new System.Drawing.Point(22, 21);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(95, 23);
            this.btOpenFile.TabIndex = 0;
            this.btOpenFile.Text = "Выбор файла";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // labCorrel
            // 
            this.labCorrel.Location = new System.Drawing.Point(19, 74);
            this.labCorrel.Name = "labCorrel";
            this.labCorrel.Size = new System.Drawing.Size(177, 30);
            this.labCorrel.TabIndex = 1;
            this.labCorrel.Text = "Считать управляющие факторы коррелированными при r =";
            // 
            // numCorrelX
            // 
            this.numCorrelX.DecimalPlaces = 2;
            this.numCorrelX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numCorrelX.Location = new System.Drawing.Point(230, 74);
            this.numCorrelX.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCorrelX.Name = "numCorrelX";
            this.numCorrelX.Size = new System.Drawing.Size(120, 20);
            this.numCorrelX.TabIndex = 3;
            // 
            // numCorrelXY
            // 
            this.numCorrelXY.DecimalPlaces = 2;
            this.numCorrelXY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numCorrelXY.Location = new System.Drawing.Point(230, 113);
            this.numCorrelXY.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCorrelXY.Name = "numCorrelXY";
            this.numCorrelXY.Size = new System.Drawing.Size(120, 20);
            this.numCorrelXY.TabIndex = 4;
            // 
            // btNext
            // 
            this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btNext.Location = new System.Drawing.Point(622, 348);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(75, 23);
            this.btNext.TabIndex = 6;
            this.btNext.Text = "Далее";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 39);
            this.label1.TabIndex = 7;
            this.label1.Text = "Считать управляемый и управляющие факторы коррелированными при r =";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 32);
            this.label2.TabIndex = 8;
            this.label2.Text = "Уровень значимости для проверки гипотез";
            // 
            // cbAlpha
            // 
            this.cbAlpha.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlpha.FormattingEnabled = true;
            this.cbAlpha.Items.AddRange(new object[] {
            "0,01",
            "0,05",
            "0,1"});
            this.cbAlpha.Location = new System.Drawing.Point(233, 152);
            this.cbAlpha.Name = "cbAlpha";
            this.cbAlpha.Size = new System.Drawing.Size(74, 21);
            this.cbAlpha.TabIndex = 9;
            // 
            // tc
            // 
            this.tc.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tc.Controls.Add(this.tpCorrel);
            this.tc.Controls.Add(this.tpParameters);
            this.tc.Controls.Add(this.tpResults);
            this.tc.Controls.Add(this.tpControl);
            this.tc.Dock = System.Windows.Forms.DockStyle.Top;
            this.tc.ItemSize = new System.Drawing.Size(0, 1);
            this.tc.Location = new System.Drawing.Point(0, 0);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(713, 334);
            this.tc.TabIndex = 10;
            this.tc.TabStop = false;
            // 
            // tpCorrel
            // 
            this.tpCorrel.Controls.Add(this.cbIsRegressionNonLinear);
            this.tpCorrel.Controls.Add(this.label5);
            this.tpCorrel.Controls.Add(this.cbIterationsCount);
            this.tpCorrel.Controls.Add(this.label4);
            this.tpCorrel.Controls.Add(this.numDeltaR);
            this.tpCorrel.Controls.Add(this.label3);
            this.tpCorrel.Controls.Add(this.pbReadingData);
            this.tpCorrel.Controls.Add(this.btOpenFile);
            this.tpCorrel.Controls.Add(this.cbAlpha);
            this.tpCorrel.Controls.Add(this.numCorrelX);
            this.tpCorrel.Controls.Add(this.numCorrelXY);
            this.tpCorrel.Controls.Add(this.labCorrel);
            this.tpCorrel.Controls.Add(this.label2);
            this.tpCorrel.Controls.Add(this.label1);
            this.tpCorrel.Location = new System.Drawing.Point(4, 5);
            this.tpCorrel.Name = "tpCorrel";
            this.tpCorrel.Padding = new System.Windows.Forms.Padding(3);
            this.tpCorrel.Size = new System.Drawing.Size(705, 325);
            this.tpCorrel.TabIndex = 1;
            this.tpCorrel.Text = "Главная";
            this.tpCorrel.UseVisualStyleBackColor = true;
            // 
            // cbIsRegressionNonLinear
            // 
            this.cbIsRegressionNonLinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIsRegressionNonLinear.FormattingEnabled = true;
            this.cbIsRegressionNonLinear.Items.AddRange(new object[] {
            "Да"});
            this.cbIsRegressionNonLinear.Location = new System.Drawing.Point(150, 204);
            this.cbIsRegressionNonLinear.Name = "cbIsRegressionNonLinear";
            this.cbIsRegressionNonLinear.Size = new System.Drawing.Size(74, 21);
            this.cbIsRegressionNonLinear.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Нелинейная регрессия";
            // 
            // cbIterationsCount
            // 
            this.cbIterationsCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIterationsCount.FormattingEnabled = true;
            this.cbIterationsCount.Items.AddRange(new object[] {
            "1",
            "2",
            "max"});
            this.cbIterationsCount.Location = new System.Drawing.Point(150, 236);
            this.cbIterationsCount.Name = "cbIterationsCount";
            this.cbIterationsCount.Size = new System.Drawing.Size(74, 21);
            this.cbIterationsCount.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Число итераций";
            // 
            // numDeltaR
            // 
            this.numDeltaR.DecimalPlaces = 3;
            this.numDeltaR.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numDeltaR.Location = new System.Drawing.Point(104, 264);
            this.numDeltaR.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDeltaR.Name = "numDeltaR";
            this.numDeltaR.Size = new System.Drawing.Size(120, 20);
            this.numDeltaR.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "delta r";
            // 
            // pbReadingData
            // 
            this.pbReadingData.Location = new System.Drawing.Point(146, 21);
            this.pbReadingData.Name = "pbReadingData";
            this.pbReadingData.Size = new System.Drawing.Size(204, 23);
            this.pbReadingData.TabIndex = 10;
            // 
            // tpParameters
            // 
            this.tpParameters.Controls.Add(this.btRemoveY);
            this.tpParameters.Controls.Add(this.btAddY);
            this.tpParameters.Controls.Add(this.btRemoveX);
            this.tpParameters.Controls.Add(this.btAddX);
            this.tpParameters.Controls.Add(this.lbY);
            this.tpParameters.Controls.Add(this.lbX);
            this.tpParameters.Controls.Add(this.lbAllParameters);
            this.tpParameters.Location = new System.Drawing.Point(4, 5);
            this.tpParameters.Name = "tpParameters";
            this.tpParameters.Size = new System.Drawing.Size(705, 325);
            this.tpParameters.TabIndex = 2;
            this.tpParameters.Text = "Выбор факторов";
            this.tpParameters.UseVisualStyleBackColor = true;
            // 
            // btRemoveY
            // 
            this.btRemoveY.Location = new System.Drawing.Point(221, 215);
            this.btRemoveY.Name = "btRemoveY";
            this.btRemoveY.Size = new System.Drawing.Size(75, 23);
            this.btRemoveY.TabIndex = 17;
            this.btRemoveY.Text = "<<";
            this.btRemoveY.UseVisualStyleBackColor = true;
            this.btRemoveY.Click += new System.EventHandler(this.btRemoveY_Click);
            // 
            // btAddY
            // 
            this.btAddY.Location = new System.Drawing.Point(222, 177);
            this.btAddY.Name = "btAddY";
            this.btAddY.Size = new System.Drawing.Size(75, 23);
            this.btAddY.TabIndex = 16;
            this.btAddY.Text = ">>";
            this.btAddY.UseVisualStyleBackColor = true;
            this.btAddY.Click += new System.EventHandler(this.btAddY_Click);
            // 
            // btRemoveX
            // 
            this.btRemoveX.Location = new System.Drawing.Point(221, 74);
            this.btRemoveX.Name = "btRemoveX";
            this.btRemoveX.Size = new System.Drawing.Size(75, 23);
            this.btRemoveX.TabIndex = 15;
            this.btRemoveX.Text = "<<";
            this.btRemoveX.UseVisualStyleBackColor = true;
            this.btRemoveX.Click += new System.EventHandler(this.btRemoveX_Click);
            // 
            // btAddX
            // 
            this.btAddX.Location = new System.Drawing.Point(221, 36);
            this.btAddX.Name = "btAddX";
            this.btAddX.Size = new System.Drawing.Size(75, 23);
            this.btAddX.TabIndex = 14;
            this.btAddX.Text = ">>";
            this.btAddX.UseVisualStyleBackColor = true;
            this.btAddX.Click += new System.EventHandler(this.btAddX_Click);
            // 
            // lbY
            // 
            this.lbY.FormattingEnabled = true;
            this.lbY.Location = new System.Drawing.Point(322, 167);
            this.lbY.Name = "lbY";
            this.lbY.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbY.Size = new System.Drawing.Size(189, 82);
            this.lbY.TabIndex = 11;
            // 
            // lbX
            // 
            this.lbX.FormattingEnabled = true;
            this.lbX.Location = new System.Drawing.Point(322, 20);
            this.lbX.Name = "lbX";
            this.lbX.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbX.Size = new System.Drawing.Size(189, 95);
            this.lbX.TabIndex = 10;
            // 
            // lbAllParameters
            // 
            this.lbAllParameters.FormattingEnabled = true;
            this.lbAllParameters.Location = new System.Drawing.Point(22, 20);
            this.lbAllParameters.Name = "lbAllParameters";
            this.lbAllParameters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAllParameters.Size = new System.Drawing.Size(176, 264);
            this.lbAllParameters.TabIndex = 9;
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.splitContainer1);
            this.tpResults.Location = new System.Drawing.Point(4, 5);
            this.tpResults.Name = "tpResults";
            this.tpResults.Size = new System.Drawing.Size(705, 325);
            this.tpResults.TabIndex = 3;
            this.tpResults.Text = "Итоги";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rbAllModels);
            this.splitContainer1.Panel1.Controls.Add(this.rbRobustModels);
            this.splitContainer1.Panel1.Controls.Add(this.rbBestModels);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvModels);
            this.splitContainer1.Size = new System.Drawing.Size(705, 272);
            this.splitContainer1.SplitterDistance = 60;
            this.splitContainer1.TabIndex = 5;
            // 
            // rbAllModels
            // 
            this.rbAllModels.AutoSize = true;
            this.rbAllModels.Checked = true;
            this.rbAllModels.Location = new System.Drawing.Point(35, 21);
            this.rbAllModels.Name = "rbAllModels";
            this.rbAllModels.Size = new System.Drawing.Size(85, 17);
            this.rbAllModels.TabIndex = 0;
            this.rbAllModels.TabStop = true;
            this.rbAllModels.Text = "Все модели";
            this.rbAllModels.UseVisualStyleBackColor = true;
            this.rbAllModels.Click += new System.EventHandler(this.rbAllModels_Click);
            // 
            // rbRobustModels
            // 
            this.rbRobustModels.AutoSize = true;
            this.rbRobustModels.Location = new System.Drawing.Point(193, 21);
            this.rbRobustModels.Name = "rbRobustModels";
            this.rbRobustModels.Size = new System.Drawing.Size(128, 17);
            this.rbRobustModels.TabIndex = 2;
            this.rbRobustModels.Text = "Устойчивые модели";
            this.rbRobustModels.UseVisualStyleBackColor = true;
            this.rbRobustModels.Click += new System.EventHandler(this.rbRobustModels_Click);
            // 
            // rbBestModels
            // 
            this.rbBestModels.AutoSize = true;
            this.rbBestModels.Location = new System.Drawing.Point(400, 21);
            this.rbBestModels.Name = "rbBestModels";
            this.rbBestModels.Size = new System.Drawing.Size(104, 17);
            this.rbBestModels.TabIndex = 1;
            this.rbBestModels.Text = "Лучшие модели";
            this.rbBestModels.UseVisualStyleBackColor = false;
            this.rbBestModels.Click += new System.EventHandler(this.rbBestModels_Click);
            // 
            // dgvModels
            // 
            this.dgvModels.AllowUserToAddRows = false;
            this.dgvModels.AllowUserToDeleteRows = false;
            this.dgvModels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvModels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvModels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvModels.Location = new System.Drawing.Point(0, 0);
            this.dgvModels.Name = "dgvModels";
            this.dgvModels.ReadOnly = true;
            this.dgvModels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvModels.Size = new System.Drawing.Size(705, 208);
            this.dgvModels.TabIndex = 3;
            // 
            // tpControl
            // 
            this.tpControl.Controls.Add(this.label11);
            this.tpControl.Controls.Add(this.label6);
            this.tpControl.Controls.Add(this.label12);
            this.tpControl.Controls.Add(this.numPercents);
            this.tpControl.Controls.Add(this.numK);
            this.tpControl.Controls.Add(this.label13);
            this.tpControl.Controls.Add(this.tbNewXValue);
            this.tpControl.Controls.Add(this.lbDependentParams);
            this.tpControl.Controls.Add(this.lblRightLimit);
            this.tpControl.Controls.Add(this.label9);
            this.tpControl.Controls.Add(this.lblNewYValue);
            this.tpControl.Controls.Add(this.label8);
            this.tpControl.Controls.Add(this.label7);
            this.tpControl.Controls.Add(this.label10);
            this.tpControl.Controls.Add(this.lblLeftLimit);
            this.tpControl.Controls.Add(this.lbParams);
            this.tpControl.Location = new System.Drawing.Point(4, 5);
            this.tpControl.Name = "tpControl";
            this.tpControl.Padding = new System.Windows.Forms.Padding(3);
            this.tpControl.Size = new System.Drawing.Size(705, 325);
            this.tpControl.TabIndex = 4;
            this.tpControl.Text = "Управление";
            this.tpControl.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(503, 164);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "max";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(419, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "min";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "%";
            // 
            // numPercents
            // 
            this.numPercents.Location = new System.Drawing.Point(51, 42);
            this.numPercents.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numPercents.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPercents.Name = "numPercents";
            this.numPercents.Size = new System.Drawing.Size(120, 20);
            this.numPercents.TabIndex = 23;
            this.numPercents.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numPercents.ValueChanged += new System.EventHandler(this.numPercents_ValueChanged);
            // 
            // numK
            // 
            this.numK.Location = new System.Drawing.Point(51, 16);
            this.numK.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numK.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numK.Name = "numK";
            this.numK.Size = new System.Drawing.Size(120, 20);
            this.numK.TabIndex = 22;
            this.numK.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numK.ValueChanged += new System.EventHandler(this.numK_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "k";
            // 
            // tbNewXValue
            // 
            this.tbNewXValue.Location = new System.Drawing.Point(496, 192);
            this.tbNewXValue.Name = "tbNewXValue";
            this.tbNewXValue.Size = new System.Drawing.Size(52, 20);
            this.tbNewXValue.TabIndex = 17;
            this.tbNewXValue.TextChanged += new System.EventHandler(this.tbNewXValue_TextChanged);
            // 
            // lbDependentParams
            // 
            this.lbDependentParams.FormattingEnabled = true;
            this.lbDependentParams.Location = new System.Drawing.Point(422, 41);
            this.lbDependentParams.Name = "lbDependentParams";
            this.lbDependentParams.Size = new System.Drawing.Size(271, 95);
            this.lbDependentParams.TabIndex = 16;
            // 
            // lblRightLimit
            // 
            this.lblRightLimit.AutoSize = true;
            this.lblRightLimit.Location = new System.Drawing.Point(535, 164);
            this.lblRightLimit.Name = "lblRightLimit";
            this.lblRightLimit.Size = new System.Drawing.Size(26, 13);
            this.lblRightLimit.TabIndex = 2;
            this.lblRightLimit.Text = "max";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(419, 232);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "New Y value:";
            // 
            // lblNewYValue
            // 
            this.lblNewYValue.AutoSize = true;
            this.lblNewYValue.Location = new System.Drawing.Point(510, 232);
            this.lblNewYValue.Name = "lblNewYValue";
            this.lblNewYValue.Size = new System.Drawing.Size(14, 13);
            this.lblNewYValue.TabIndex = 2;
            this.lblNewYValue.Text = "Y";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(419, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "New X value:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(419, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "New X value:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(419, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(129, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Зависимые параметры:";
            // 
            // lblLeftLimit
            // 
            this.lblLeftLimit.AutoSize = true;
            this.lblLeftLimit.Location = new System.Drawing.Point(448, 164);
            this.lblLeftLimit.Name = "lblLeftLimit";
            this.lblLeftLimit.Size = new System.Drawing.Size(23, 13);
            this.lblLeftLimit.TabIndex = 2;
            this.lblLeftLimit.Text = "min";
            // 
            // lbParams
            // 
            this.lbParams.FormattingEnabled = true;
            this.lbParams.Location = new System.Drawing.Point(22, 77);
            this.lbParams.Name = "lbParams";
            this.lbParams.Size = new System.Drawing.Size(333, 212);
            this.lbParams.TabIndex = 0;
            this.lbParams.SelectedIndexChanged += new System.EventHandler(this.lbParams_SelectedIndexChanged);
            // 
            // btBack
            // 
            this.btBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btBack.Location = new System.Drawing.Point(531, 348);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 13;
            this.btBack.Text = "Назад";
            this.btBack.UseVisualStyleBackColor = true;
            this.btBack.Visible = false;
            this.btBack.Click += new System.EventHandler(this.btBack_Click);
            // 
            // btParamsInfo
            // 
            this.btParamsInfo.Location = new System.Drawing.Point(26, 340);
            this.btParamsInfo.Name = "btParamsInfo";
            this.btParamsInfo.Size = new System.Drawing.Size(88, 38);
            this.btParamsInfo.TabIndex = 14;
            this.btParamsInfo.Text = "Расшифровка параметров";
            this.btParamsInfo.UseVisualStyleBackColor = true;
            this.btParamsInfo.Click += new System.EventHandler(this.btParamInfo_Click);
            // 
            // epValueOutOfRange
            // 
            this.epValueOutOfRange.ContainerControl = this;
            // 
            // epTooWideRange
            // 
            this.epTooWideRange.ContainerControl = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 398);
            this.Controls.Add(this.btParamsInfo);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.btBack);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.numCorrelX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCorrelXY)).EndInit();
            this.tc.ResumeLayout(false);
            this.tpCorrel.ResumeLayout(false);
            this.tpCorrel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeltaR)).EndInit();
            this.tpParameters.ResumeLayout(false);
            this.tpResults.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvModels)).EndInit();
            this.tpControl.ResumeLayout(false);
            this.tpControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPercents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epValueOutOfRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epTooWideRange)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.Label labCorrel;
        private System.Windows.Forms.ComboBox cbAlpha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btNext;
        public System.Windows.Forms.NumericUpDown numCorrelXY;
        public System.Windows.Forms.NumericUpDown numCorrelX;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpCorrel;
        private System.Windows.Forms.TabPage tpParameters;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.Button btRemoveY;
        private System.Windows.Forms.Button btAddY;
        private System.Windows.Forms.Button btRemoveX;
        private System.Windows.Forms.Button btAddX;
        private System.Windows.Forms.Button btBack;
        private System.Windows.Forms.ListBox lbY;
        private System.Windows.Forms.ListBox lbX;
        private System.Windows.Forms.ListBox lbAllParameters;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton rbAllModels;
        private System.Windows.Forms.RadioButton rbRobustModels;
        private System.Windows.Forms.RadioButton rbBestModels;
        private System.Windows.Forms.DataGridView dgvModels;
        private System.Windows.Forms.ProgressBar pbReadingData;
        private System.Windows.Forms.Button btParamsInfo;
        private System.Windows.Forms.ComboBox cbIterationsCount;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown numDeltaR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbIsRegressionNonLinear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpControl;
        private System.Windows.Forms.Label lblRightLimit;
        private System.Windows.Forms.Label lblNewYValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblLeftLimit;
        private System.Windows.Forms.ListBox lbParams;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbNewXValue;
        private System.Windows.Forms.ListBox lbDependentParams;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ErrorProvider epValueOutOfRange;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numPercents;
        private System.Windows.Forms.NumericUpDown numK;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ErrorProvider epTooWideRange;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
    }
}

