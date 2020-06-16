using System;
using System.Windows.Forms;

namespace SudokuPlus
{
    public partial class StartGame : Form
    {
        private string dialogResult;

        public string Result
        {
            get { return dialogResult; }
        }

        public StartGame(string paramString)
        {
            InitializeComponent();
            if (paramString[0].ToString().Equals(Constants.PLAYER_SOLVES))
            {
                radioButton4.Checked = true;
            }
            if (paramString[0].ToString().Equals(Constants.COMPUTER_SOLVES))
            {
                radioButton5.Checked = true;
            }
            if (paramString[1].ToString().Equals(Constants.LEVEL_EASY))
            {
                radioButton1.Checked = true;
            }
            if (paramString[1].ToString().Equals(Constants.LEVEL_MEDIUM))
            {
                radioButton2.Checked = true;
            }
            if (paramString[1].ToString().Equals(Constants.LEVEL_HARD))
            {
                radioButton3.Checked = true;
            }
            textBox1.Text = paramString.Substring(2, paramString.Length - 2);
            button1.Enabled = radioButton5.Checked || (radioButton4.Checked && Tools.isValidPlayerName(textBox1.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dialogResult = Constants.CANCEL_GAME;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                dialogResult = Constants.PLAYER_SOLVES;
            }
            if (radioButton5.Checked)
            {
                dialogResult = Constants.COMPUTER_SOLVES;
            }
            if (radioButton1.Checked)
            {
                dialogResult += Constants.LEVEL_EASY;
            }
            if (radioButton2.Checked)
            {
                dialogResult += Constants.LEVEL_MEDIUM;
            }
            if (radioButton3.Checked)
            {
                dialogResult += Constants.LEVEL_HARD;
            }
            dialogResult += textBox1.Text;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = Tools.isValidPlayerName(textBox1.Text);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                label1.Enabled = true;
                textBox1.Enabled = true;
                groupBox1.Enabled = true;
                button1.Enabled = Tools.isValidPlayerName(textBox1.Text);
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                label1.Enabled = false;
                textBox1.Enabled = false;
                groupBox1.Enabled = false;
                button1.Enabled = true;
            }
        }
    }
}