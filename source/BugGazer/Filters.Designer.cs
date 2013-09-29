namespace BugGazer
{
    partial class Filters
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
            this.addColorButton = new System.Windows.Forms.Button();
            this.removeColorButton = new System.Windows.Forms.Button();
            this.addFilterActionButton = new System.Windows.Forms.Button();
            this.removeFilterActionButton = new System.Windows.Forms.Button();
            this.groupColors = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fgColorButton = new System.Windows.Forms.Button();
            this.bgColorButton = new System.Windows.Forms.Button();
            this.colorListView = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupFilters = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.colorDropBox = new System.Windows.Forms.ComboBox();
            this.FilterLabel = new System.Windows.Forms.Label();
            this.actionDropBox = new System.Windows.Forms.ComboBox();
            this.patternDropbox = new System.Windows.Forms.ComboBox();
            this.filterActionListView = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.removePatternButton = new System.Windows.Forms.Button();
            this.patternListView = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.addPatternButton = new System.Windows.Forms.Button();
            this.groupPatterns = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.tableLayoutRows = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutColomns = new System.Windows.Forms.TableLayoutPanel();
            this.panelUpperLeft = new System.Windows.Forms.Panel();
            this.panelUpperRight = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.groupColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorListView)).BeginInit();
            this.groupFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterActionListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternListView)).BeginInit();
            this.groupPatterns.SuspendLayout();
            this.tableLayoutRows.SuspendLayout();
            this.tableLayoutColomns.SuspendLayout();
            this.panelUpperLeft.SuspendLayout();
            this.panelUpperRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // addColorButton
            // 
            this.addColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addColorButton.Location = new System.Drawing.Point(274, 110);
            this.addColorButton.Name = "addColorButton";
            this.addColorButton.Size = new System.Drawing.Size(75, 23);
            this.addColorButton.TabIndex = 7;
            this.addColorButton.Text = "Add";
            this.addColorButton.UseVisualStyleBackColor = true;
            // 
            // removeColorButton
            // 
            this.removeColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeColorButton.Location = new System.Drawing.Point(274, 81);
            this.removeColorButton.Name = "removeColorButton";
            this.removeColorButton.Size = new System.Drawing.Size(75, 23);
            this.removeColorButton.TabIndex = 6;
            this.removeColorButton.Text = "Remove";
            this.removeColorButton.UseVisualStyleBackColor = true;
            // 
            // addFilterActionButton
            // 
            this.addFilterActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addFilterActionButton.Location = new System.Drawing.Point(653, 174);
            this.addFilterActionButton.Name = "addFilterActionButton";
            this.addFilterActionButton.Size = new System.Drawing.Size(75, 23);
            this.addFilterActionButton.TabIndex = 10;
            this.addFilterActionButton.Text = "Add";
            this.addFilterActionButton.UseVisualStyleBackColor = true;
            this.addFilterActionButton.Click += new System.EventHandler(this.addActionButton_Click);
            // 
            // removeFilterActionButton
            // 
            this.removeFilterActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeFilterActionButton.Location = new System.Drawing.Point(653, 130);
            this.removeFilterActionButton.Name = "removeFilterActionButton";
            this.removeFilterActionButton.Size = new System.Drawing.Size(75, 23);
            this.removeFilterActionButton.TabIndex = 9;
            this.removeFilterActionButton.Text = "Remove";
            this.removeFilterActionButton.UseVisualStyleBackColor = true;
            this.removeFilterActionButton.Click += new System.EventHandler(this.removeActionButton_Click);
            // 
            // groupColors
            // 
            this.groupColors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupColors.Controls.Add(this.label1);
            this.groupColors.Controls.Add(this.fgColorButton);
            this.groupColors.Controls.Add(this.bgColorButton);
            this.groupColors.Controls.Add(this.colorListView);
            this.groupColors.Controls.Add(this.addColorButton);
            this.groupColors.Controls.Add(this.removeColorButton);
            this.groupColors.Location = new System.Drawing.Point(4, 3);
            this.groupColors.Name = "groupColors";
            this.groupColors.Size = new System.Drawing.Size(363, 158);
            this.groupColors.TabIndex = 0;
            this.groupColors.TabStop = false;
            this.groupColors.Text = "Colors";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Tip: Doubeclick a value to edit";
            // 
            // fgColorButton
            // 
            this.fgColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fgColorButton.Location = new System.Drawing.Point(274, 52);
            this.fgColorButton.Name = "fgColorButton";
            this.fgColorButton.Size = new System.Drawing.Size(75, 23);
            this.fgColorButton.TabIndex = 10;
            this.fgColorButton.Text = "FG Color";
            this.fgColorButton.UseVisualStyleBackColor = true;
            // 
            // bgColorButton
            // 
            this.bgColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bgColorButton.Location = new System.Drawing.Point(274, 23);
            this.bgColorButton.Name = "bgColorButton";
            this.bgColorButton.Size = new System.Drawing.Size(75, 23);
            this.bgColorButton.TabIndex = 9;
            this.bgColorButton.Text = "BG Color";
            this.bgColorButton.UseVisualStyleBackColor = true;
            this.bgColorButton.Click += new System.EventHandler(this.bgColorButton_Click);
            // 
            // colorListView
            // 
            this.colorListView.AllColumns.Add(this.olvColumn6);
            this.colorListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.colorListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.colorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn6});
            this.colorListView.FullRowSelect = true;
            this.colorListView.HideSelection = false;
            this.colorListView.Location = new System.Drawing.Point(6, 19);
            this.colorListView.Name = "colorListView";
            this.colorListView.Size = new System.Drawing.Size(262, 114);
            this.colorListView.TabIndex = 8;
            this.colorListView.UseCompatibleStateImageBehavior = false;
            this.colorListView.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "Text";
            this.olvColumn6.CellPadding = null;
            this.olvColumn6.FillsFreeSpace = true;
            this.olvColumn6.Groupable = false;
            this.olvColumn6.Hideable = false;
            this.olvColumn6.Text = "Color";
            // 
            // groupFilters
            // 
            this.groupFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupFilters.Controls.Add(this.button2);
            this.groupFilters.Controls.Add(this.button1);
            this.groupFilters.Controls.Add(this.label3);
            this.groupFilters.Controls.Add(this.label2);
            this.groupFilters.Controls.Add(this.colorDropBox);
            this.groupFilters.Controls.Add(this.FilterLabel);
            this.groupFilters.Controls.Add(this.actionDropBox);
            this.groupFilters.Controls.Add(this.patternDropbox);
            this.groupFilters.Controls.Add(this.filterActionListView);
            this.groupFilters.Controls.Add(this.addFilterActionButton);
            this.groupFilters.Controls.Add(this.removeFilterActionButton);
            this.groupFilters.Location = new System.Drawing.Point(3, 3);
            this.groupFilters.Name = "groupFilters";
            this.groupFilters.Size = new System.Drawing.Size(739, 203);
            this.groupFilters.TabIndex = 0;
            this.groupFilters.TabStop = false;
            this.groupFilters.Text = "Filter, track and colorize";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(653, 75);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(653, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Pattern:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(486, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Color:";
            // 
            // colorDropBox
            // 
            this.colorDropBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.colorDropBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorDropBox.FormattingEnabled = true;
            this.colorDropBox.Location = new System.Drawing.Point(489, 176);
            this.colorDropBox.Name = "colorDropBox";
            this.colorDropBox.Size = new System.Drawing.Size(158, 21);
            this.colorDropBox.TabIndex = 15;
            // 
            // FilterLabel
            // 
            this.FilterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterLabel.AutoSize = true;
            this.FilterLabel.Location = new System.Drawing.Point(322, 158);
            this.FilterLabel.Name = "FilterLabel";
            this.FilterLabel.Size = new System.Drawing.Size(40, 13);
            this.FilterLabel.TabIndex = 14;
            this.FilterLabel.Text = "Action:";
            // 
            // actionDropBox
            // 
            this.actionDropBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.actionDropBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.actionDropBox.FormattingEnabled = true;
            this.actionDropBox.Location = new System.Drawing.Point(325, 176);
            this.actionDropBox.Name = "actionDropBox";
            this.actionDropBox.Size = new System.Drawing.Size(158, 21);
            this.actionDropBox.TabIndex = 13;
            // 
            // patternDropbox
            // 
            this.patternDropbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patternDropbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.patternDropbox.FormattingEnabled = true;
            this.patternDropbox.Location = new System.Drawing.Point(12, 176);
            this.patternDropbox.Name = "patternDropbox";
            this.patternDropbox.Size = new System.Drawing.Size(307, 21);
            this.patternDropbox.TabIndex = 12;
            // 
            // filterActionListView
            // 
            this.filterActionListView.AllColumns.Add(this.olvColumn5);
            this.filterActionListView.AllColumns.Add(this.olvColumn3);
            this.filterActionListView.AllColumns.Add(this.olvColumn4);
            this.filterActionListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterActionListView.CheckBoxes = true;
            this.filterActionListView.CheckedAspectName = "Enabled";
            this.filterActionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn5,
            this.olvColumn3,
            this.olvColumn4});
            this.filterActionListView.FullRowSelect = true;
            this.filterActionListView.HideSelection = false;
            this.filterActionListView.Location = new System.Drawing.Point(12, 19);
            this.filterActionListView.Name = "filterActionListView";
            this.filterActionListView.Size = new System.Drawing.Size(635, 132);
            this.filterActionListView.TabIndex = 11;
            this.filterActionListView.UseCompatibleStateImageBehavior = false;
            this.filterActionListView.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "Pattern";
            this.olvColumn5.CellPadding = null;
            this.olvColumn5.Groupable = false;
            this.olvColumn5.Hideable = false;
            this.olvColumn5.Text = "Pattern";
            this.olvColumn5.Width = 364;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Action";
            this.olvColumn3.CellPadding = null;
            this.olvColumn3.Groupable = false;
            this.olvColumn3.Hideable = false;
            this.olvColumn3.Text = "Action";
            this.olvColumn3.Width = 113;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "TextColor.Text";
            this.olvColumn4.CellPadding = null;
            this.olvColumn4.FillsFreeSpace = true;
            this.olvColumn4.Groupable = false;
            this.olvColumn4.Hideable = false;
            this.olvColumn4.Text = "TextColor";
            this.olvColumn4.Width = 123;
            // 
            // removePatternButton
            // 
            this.removePatternButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removePatternButton.Location = new System.Drawing.Point(274, 81);
            this.removePatternButton.Name = "removePatternButton";
            this.removePatternButton.Size = new System.Drawing.Size(75, 23);
            this.removePatternButton.TabIndex = 3;
            this.removePatternButton.Text = "Remove";
            this.removePatternButton.UseVisualStyleBackColor = true;
            this.removePatternButton.Click += new System.EventHandler(this.removePatternButton_Click);
            // 
            // patternListView
            // 
            this.patternListView.AllColumns.Add(this.olvColumn2);
            this.patternListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patternListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.patternListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn2});
            this.patternListView.FullRowSelect = true;
            this.patternListView.HideSelection = false;
            this.patternListView.Location = new System.Drawing.Point(6, 19);
            this.patternListView.Name = "patternListView";
            this.patternListView.Size = new System.Drawing.Size(262, 114);
            this.patternListView.TabIndex = 11;
            this.patternListView.UseCompatibleStateImageBehavior = false;
            this.patternListView.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Text";
            this.olvColumn2.CellPadding = null;
            this.olvColumn2.FillsFreeSpace = true;
            this.olvColumn2.Groupable = false;
            this.olvColumn2.Hideable = false;
            this.olvColumn2.Text = "Pattern";
            this.olvColumn2.Width = 128;
            // 
            // addPatternButton
            // 
            this.addPatternButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addPatternButton.Location = new System.Drawing.Point(274, 110);
            this.addPatternButton.Name = "addPatternButton";
            this.addPatternButton.Size = new System.Drawing.Size(75, 23);
            this.addPatternButton.TabIndex = 4;
            this.addPatternButton.Text = "Add";
            this.addPatternButton.UseVisualStyleBackColor = true;
            this.addPatternButton.Click += new System.EventHandler(this.addPatternButton_Click);
            // 
            // groupPatterns
            // 
            this.groupPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPatterns.Controls.Add(this.label4);
            this.groupPatterns.Controls.Add(this.addPatternButton);
            this.groupPatterns.Controls.Add(this.patternListView);
            this.groupPatterns.Controls.Add(this.removePatternButton);
            this.groupPatterns.Location = new System.Drawing.Point(3, 3);
            this.groupPatterns.Name = "groupPatterns";
            this.groupPatterns.Size = new System.Drawing.Size(363, 158);
            this.groupPatterns.TabIndex = 12;
            this.groupPatterns.TabStop = false;
            this.groupPatterns.Text = "Patterns";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(305, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Tip: Regular expressions support color names as named groups";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(656, 212);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 13;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // tableLayoutRows
            // 
            this.tableLayoutRows.ColumnCount = 1;
            this.tableLayoutRows.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutRows.Controls.Add(this.tableLayoutColomns, 0, 0);
            this.tableLayoutRows.Controls.Add(this.panelBottom, 0, 1);
            this.tableLayoutRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutRows.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutRows.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutRows.Name = "tableLayoutRows";
            this.tableLayoutRows.RowCount = 2;
            this.tableLayoutRows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutRows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutRows.Size = new System.Drawing.Size(752, 411);
            this.tableLayoutRows.TabIndex = 14;
            // 
            // tableLayoutColomns
            // 
            this.tableLayoutColomns.ColumnCount = 2;
            this.tableLayoutColomns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutColomns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutColomns.Controls.Add(this.panelUpperLeft, 0, 0);
            this.tableLayoutColomns.Controls.Add(this.panelUpperRight, 1, 0);
            this.tableLayoutColomns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutColomns.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutColomns.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutColomns.Name = "tableLayoutColomns";
            this.tableLayoutColomns.RowCount = 1;
            this.tableLayoutColomns.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutColomns.Size = new System.Drawing.Size(752, 164);
            this.tableLayoutColomns.TabIndex = 0;
            // 
            // panelUpperLeft
            // 
            this.panelUpperLeft.Controls.Add(this.groupPatterns);
            this.panelUpperLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUpperLeft.Location = new System.Drawing.Point(0, 0);
            this.panelUpperLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelUpperLeft.Name = "panelUpperLeft";
            this.panelUpperLeft.Size = new System.Drawing.Size(376, 164);
            this.panelUpperLeft.TabIndex = 0;
            // 
            // panelUpperRight
            // 
            this.panelUpperRight.Controls.Add(this.groupColors);
            this.panelUpperRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUpperRight.Location = new System.Drawing.Point(376, 0);
            this.panelUpperRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelUpperRight.Name = "panelUpperRight";
            this.panelUpperRight.Size = new System.Drawing.Size(376, 164);
            this.panelUpperRight.TabIndex = 1;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.groupFilters);
            this.panelBottom.Controls.Add(this.closeButton);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 164);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(752, 247);
            this.panelBottom.TabIndex = 1;
            // 
            // Filters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 411);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutRows);
            this.MinimumSize = new System.Drawing.Size(768, 450);
            this.Name = "Filters";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filters";
            this.groupColors.ResumeLayout(false);
            this.groupColors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorListView)).EndInit();
            this.groupFilters.ResumeLayout(false);
            this.groupFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterActionListView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternListView)).EndInit();
            this.groupPatterns.ResumeLayout(false);
            this.groupPatterns.PerformLayout();
            this.tableLayoutRows.ResumeLayout(false);
            this.tableLayoutColomns.ResumeLayout(false);
            this.panelUpperLeft.ResumeLayout(false);
            this.panelUpperRight.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addColorButton;
        private System.Windows.Forms.Button removeColorButton;
        private System.Windows.Forms.Button addFilterActionButton;
        private System.Windows.Forms.Button removeFilterActionButton;
        private System.Windows.Forms.GroupBox groupColors;
        private System.Windows.Forms.GroupBox groupFilters;
        private System.Windows.Forms.Button removePatternButton;
        private BrightIdeasSoftware.ObjectListView patternListView;
        private System.Windows.Forms.Button addPatternButton;
        private System.Windows.Forms.GroupBox groupPatterns;
        private System.Windows.Forms.Button closeButton;
        private BrightIdeasSoftware.ObjectListView colorListView;
        private BrightIdeasSoftware.ObjectListView filterActionListView;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox patternDropbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox colorDropBox;
        private System.Windows.Forms.Label FilterLabel;
        private System.Windows.Forms.ComboBox actionDropBox;
        private System.Windows.Forms.Button fgColorButton;
        private System.Windows.Forms.Button bgColorButton;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutRows;
        private System.Windows.Forms.TableLayoutPanel tableLayoutColomns;
        private System.Windows.Forms.Panel panelUpperLeft;
        private System.Windows.Forms.Panel panelUpperRight;
        private System.Windows.Forms.Panel panelBottom;

    }
}