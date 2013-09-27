namespace BugGazer
{
    partial class DBWinListView
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
            this.listView = new System.Windows.Forms.ListView();
            this.columnIndex = new System.Windows.Forms.ColumnHeader();
            this.columnTimestamp = new System.Windows.Forms.ColumnHeader();
            this.columnProcess = new System.Windows.Forms.ColumnHeader();
            this.columnMessage = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnIndex,
            this.columnTimestamp,
            this.columnProcess,
            this.columnMessage});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(742, 324);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnIndex
            // 
            this.columnIndex.Text = "#";
            // 
            // columnTimestamp
            // 
            this.columnTimestamp.Text = "Timestamp";
            this.columnTimestamp.Width = 100;
            // 
            // columnProcess
            // 
            this.columnProcess.Text = "Process";
            this.columnProcess.Width = 100;
            // 
            // columnMessage
            // 
            this.columnMessage.Text = "Message";
            this.columnMessage.Width = 2000;
            // 
            // DBWinListView
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(742, 324);
            this.Controls.Add(this.listView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DBWinListView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnIndex;
        private System.Windows.Forms.ColumnHeader columnTimestamp;
        private System.Windows.Forms.ColumnHeader columnProcess;
        private System.Windows.Forms.ColumnHeader columnMessage;
    }
}
