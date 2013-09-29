namespace BugGazer
{
    partial class SearchBar
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
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(0, 0);
            this.SearchTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(167, 20);
            this.SearchTextBox.TabIndex = 1;
            // 
            // downButton
            // 
            this.downButton.Image = global::BugGazer.Properties.Resources.down;
            this.downButton.Location = new System.Drawing.Point(200, 1);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(22, 18);
            this.downButton.TabIndex = 1;
            this.downButton.TabStop = false;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Image = global::BugGazer.Properties.Resources.up;
            this.upButton.Location = new System.Drawing.Point(174, 1);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(22, 18);
            this.upButton.TabIndex = 0;
            this.upButton.TabStop = false;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // SearchBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SearchBar";
            this.Size = new System.Drawing.Size(225, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.TextBox SearchTextBox;
    }
}
