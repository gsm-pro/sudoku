using System.Windows.Forms;

namespace SudokuPlus
{
    public partial class RecordsTable : Form
    {
        public RecordsTable()
        {
            InitializeComponent();
            int t1 = Game.Times[0];
            int t2 = Game.Times[1];
            int t3 = Game.Times[2];
            if (t1 != Constants.INF)
            {
                textBox1.Text = Game.Names[0];
                textBox4.Text = t1.ToString();
            }
            if (t2 != Constants.INF)
            {
                textBox2.Text = Game.Names[1];
                textBox5.Text = t2.ToString();
            }
            if (t3 != Constants.INF)
            {
                textBox3.Text = Game.Names[2];
                textBox6.Text = t3.ToString();
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}