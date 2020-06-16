using System;
using System.Windows.Forms;

namespace SudokuPlus
{
    public partial class AboutGame : Form
    {
        public AboutGame()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("mailto:igmilyy@gmail.com?subject=Sudoku 1.4");
            }
            catch (Exception)
            {
            }
        }
    }
}