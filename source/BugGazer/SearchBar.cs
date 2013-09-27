using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BugGazer
{
    public partial class SearchBar : UserControl
    {
        private Controller mController;

        public SearchBar()
        {
            InitializeComponent();
            SearchTextBox.KeyDown += new KeyEventHandler(SearchTextBox_KeyDown);
        }

        public void Initialize(Controller controller)
        {
            mController = controller;
        }

        public void FocusControl()
        {
            SearchTextBox.Focus();
        }

        public void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift)
                {
                    SearchPrevious();
                }
                else
                {
                    SearchNext();
                }
            }
        }

        public void SearchNext()
        {
            mController.Search(SearchTextBox.Text, true);
        }

        public void SearchPrevious()
        {
            mController.Search(SearchTextBox.Text, false);
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            mController.Search(SearchTextBox.Text, true);
            SearchTextBox.Focus();
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            mController.Search(SearchTextBox.Text, false);
            SearchTextBox.Focus();
        }
    }
}
