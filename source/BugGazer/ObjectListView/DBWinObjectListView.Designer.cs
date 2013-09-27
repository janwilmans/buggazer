namespace BugGazer
{
    partial class DBWinObjectListView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listview = new BrightIdeasSoftware.FastObjectListView();
            this.columnIndex = new BrightIdeasSoftware.OLVColumn();
            this.columnTimestamp = new BrightIdeasSoftware.OLVColumn();
            this.columnProcess = new BrightIdeasSoftware.OLVColumn();
            this.columnMessage = new BrightIdeasSoftware.OLVColumn();
            ((System.ComponentModel.ISupportInitialize)(this.listview)).BeginInit();
            this.SuspendLayout();
            // 
            // virtualObjectListView
            // 
            this.listview.AllColumns.Add(this.columnIndex);
            this.listview.AllColumns.Add(this.columnTimestamp);
            this.listview.AllColumns.Add(this.columnProcess);
            this.listview.AllColumns.Add(this.columnMessage);
            this.listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnIndex,
            this.columnTimestamp,
            this.columnProcess,
            this.columnMessage});
            this.listview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listview.FullRowSelect = true;
            this.listview.HideSelection = false;
            this.listview.Location = new System.Drawing.Point(0, 0);
            this.listview.Name = "virtualObjectListView";
            this.listview.SelectedColumnTint = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.listview.ShowCommandMenuOnRightClick = true;
            this.listview.ShowGroups = false;
            this.listview.ShowItemCountOnGroups = true;
            this.listview.Size = new System.Drawing.Size(776, 348);
            this.listview.TabIndex = 0;
            this.listview.UseCompatibleStateImageBehavior = false;
            this.listview.View = System.Windows.Forms.View.Details;
            this.listview.VirtualMode = true;
            // 
            // columnIndex
            // 
            this.columnIndex.AspectName = "Index";
            this.columnIndex.AspectToStringFormat = "";
            this.columnIndex.CellPadding = null;
            this.columnIndex.Text = "#";
            // 
            // columnTimestamp
            // 
            this.columnTimestamp.AspectName = "Timestamp";
            this.columnTimestamp.CellPadding = null;
            this.columnTimestamp.Text = "Timestamp";
            this.columnTimestamp.Width = 80;
            // 
            // columnProcess
            // 
            this.columnProcess.AspectName = "Process";
            this.columnProcess.CellPadding = null;
            this.columnProcess.Text = "Process";
            this.columnProcess.Width = 150;
            // 
            // columnMessage
            // 
            this.columnMessage.AspectName = "Message";
            this.columnMessage.CellPadding = null;
            this.columnMessage.FillsFreeSpace = true;
            this.columnMessage.Text = "Message";
            this.columnMessage.Width = 4000;
            // 
            // DBWinObjectListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(776, 348);
            this.Controls.Add(this.listview);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DBWinObjectListView";
            ((System.ComponentModel.ISupportInitialize)(this.listview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView listview;
        private BrightIdeasSoftware.OLVColumn columnIndex;
        private BrightIdeasSoftware.OLVColumn columnTimestamp;
        private BrightIdeasSoftware.OLVColumn columnProcess;
        private BrightIdeasSoftware.OLVColumn columnMessage;
    }
}
