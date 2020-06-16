using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SudokuPlus
{
    public partial class Game : Form
    {
        private List<Button> buttons = new List<Button>();
        private GameMode gameMode = GameMode.NO_GAME;
        private int seconds = 0;
        private static string[] names = new string[3];
        private static int[] times = new int[3];
        private int level = -1;
        private string currentGameState = Constants.PLAYER_SOLVES + Constants.LEVEL_EASY;

        public static string[] Names
        {
            get { return names; }
        }

        public static int[] Times
        {
            get { return times; }
        }

        public Game()
        {
            InitializeComponent();
            Text = Constants.MAIN_WINDOW_TITLE;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Application.ExecutablePath.EndsWith("\\" + Constants.PROGRAM_NAME))
            {
                MessageBox.Show(Constants.INIT_ERROR, Constants.MAIN_WINDOW_TITLE, MessageBoxButtons.OK);
                Close();
            }
            новыйToolStripMenuItem.Visible = true;
            решитьToolStripMenuItem.Visible = false;
            toolStripStatusLabel1.Text = Constants.INVITATION;
            toolStripStatusLabel2.Text = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button b = new Button();
                    b.Width = 40;
                    b.Height = 40;
                    b.Font = new Font("Times New Roman", 16, FontStyle.Regular);
                    b.ForeColor = Color.Blue;
                    b.Text = "";
                    b.Left = 5 + 45 * j;
                    b.Top = 5 + 45 * i;
                    b.TabIndex = i * 9 + j;
                    b.KeyDown += buttons_KeyDown;
                    b.PreviewKeyDown += buttons_PreviewKeyDown;
                    panel1.Controls.Add(b);
                    buttons.Add(b);
                }
            }
            loadScoreInfo();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.Black, 2);
            e.Graphics.DrawLine(p, new Point(137, 5), new Point(137, 405));
            e.Graphics.DrawLine(p, new Point(272, 5), new Point(272, 405));
            e.Graphics.DrawLine(p, new Point(5, 137), new Point(405, 137));
            e.Graphics.DrawLine(p, new Point(5, 272), new Point(405, 272));
        }

        private void buttons_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameMode == GameMode.NO_GAME)
            {
                return;
            }
            Button b = (Button)sender;
            if (b.ForeColor == Color.Blue && gameMode == GameMode.FROM_EXISTING)
            {
                return;
            }
            Keys code = e.KeyCode;
            Keys[] allowedKeys = { Keys.Space, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
                                   Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
                                   Keys.Space, Keys.D1, Keys.D2, Keys.D3, Keys.D4,
                                   Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
            if (!allowedKeys.Contains(code))
            {
                if (Control.ModifierKeys != Keys.Shift
                    && Control.ModifierKeys != Keys.Alt
                    && Control.ModifierKeys != Keys.Control
                    && Control.ModifierKeys != (Keys.Shift | Keys.Alt)
                    && Control.ModifierKeys != (Keys.Shift | Keys.Control)
                    && Control.ModifierKeys != (Keys.Alt | Keys.Control)
                    && Control.ModifierKeys != (Keys.Shift | Keys.Alt | Keys.Control))
                {
                    toolStripStatusLabel1.Text = Constants.USE_DIGITS;
                    timer2.Enabled = false;
                    timer2.Enabled = true;
                }
                return;
            }
            int index = -1;
            foreach (Button btn in buttons)
            {
                ++index;
                if (btn == b)
                {
                    break;
                }
            }
            int digit = allowedKeys.TakeWhile(ch => (ch != code)).Count() % 10;
            if (code != Keys.Space)
            {
                int row = index / 9, col = index % 9;
                List<int> digits = new List<int>();
                for (int z = 1; z <= 9; z++)
                {
                    bool can = true;
                    string text = z.ToString();
                    for (int i = 9 * row; i < 9 * (row + 1); i++)
                    {
                        if (buttons[i].Text == text)
                        {
                            can = false;
                        }
                    }
                    for (int i = col; i < 81; i += 9)
                    {
                        if (buttons[i].Text == text)
                        {
                            can = false;
                        }
                    }
                    for (int i = (row / 3) * 3; i < (row / 3) * 3 + 3; i++)
                    {
                        for (int j = (col / 3) * 3; j < (col / 3) * 3 + 3; j++)
                        {
                            if (buttons[i * 9 + j].Text == text)
                            {
                                can = false;
                            }
                        }
                    }
                    if (can)
                    {
                        digits.Add(z);
                    }
                }
                if (!digits.Contains(digit) && b.Text != digit.ToString())
                {
                    string mes = Constants.DIGIT_NOT_ALLOWED + digit;
                    int size = digits.Count;
                    if (size != 0)
                    {
                        mes += Constants.BUT_ALLOWED;
                        for (int i = 0; i < size; i++)
                        {
                            mes += digits[i];
                            if (i != size - 1)
                            {
                                mes += ", ";
                            }
                        }
                    }
                    toolStripStatusLabel1.Text = mes;
                    timer2.Enabled = false;
                    timer2.Enabled = true;
                    return;
                }
                b.Text = "" + digit;
            }
            else
            {
                b.Text = "";
            }
            if (gameMode == GameMode.FROM_EMPTY)
            {
                buttons[(index == 80) ? 0 : ++index].Focus();
            }
        }

        private void buttons_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    {
                        buttons[i * 9 + j].TabIndex = j * 9 + i;
                    }
                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    {
                        buttons[i * 9 + j].TabIndex = i * 9 + j;
                    }
                }
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Control c = contextMenuStrip1.SourceControl;
            c.Text = e.ClickedItem.Text;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void решитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            int[,] field = new int[9, 9];
            int index = -1;
            foreach (Button btn in buttons)
            {
                ++index;
                field[index / 9, index % 9] = (btn.Text == "") ? 0 : Int32.Parse(btn.Text);
                if (gameMode == GameMode.FROM_EXISTING && btn.ForeColor == Color.Red)
                {
                    field[index / 9, index % 9] = 0;
                }
            }
            SudokuLogic sudoku = new SudokuLogic(field);
            bool err;
            int[,] ans = sudoku.SolveSudoku(out err);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (field[i, j] == 0)
                    {
                        buttons[i * 9 + j].Text = ans[i, j].ToString();
                        buttons[i * 9 + j].ForeColor = Color.Red;
                    }
                    else
                    {
                        buttons[i * 9 + j].ForeColor = Color.Blue;
                    }
                    buttons[i * 9 + j].ContextMenuStrip = null;
                }
            }
            if (err)
            {
                MessageBox.Show(Constants.NO_SOLUTION, Constants.MAIN_WINDOW_TITLE, MessageBoxButtons.OK);
            }
            новыйToolStripMenuItem.Visible = true;
            решитьToolStripMenuItem.Visible = false;
            gameMode = GameMode.NO_GAME;
            toolStripStatusLabel1.Text = Constants.INVITATION;
            toolStripStatusLabel2.Text = "";
        }

        private void emptyGame()
        {
            timer1.Enabled = false;
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            новыйToolStripMenuItem.Visible = true;
            решитьToolStripMenuItem.Visible = true;
            решитьToolStripMenuItem.Text = Constants.COMPUTER_WILL_SOLVE;
            foreach (Button b in buttons)
            {
                b.ForeColor = Color.Blue;
                b.Text = "";
                b.ContextMenuStrip = contextMenuStrip1;
            }
            buttons[0].Focus();
            gameMode = GameMode.FROM_EMPTY;
        }

        private bool checkIfSolved()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (buttons[i * 9 + j].Text == "")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void existingGame()
        {
            if (currentGameState[1].ToString().Equals(Constants.LEVEL_EASY))
            {
                level = 0;
            }
            if (currentGameState[1].ToString().Equals(Constants.LEVEL_MEDIUM))
            {
                level = 1;
            }
            if (currentGameState[1].ToString().Equals(Constants.LEVEL_HARD))
            {
                level = 2;
            }
            toolStripStatusLabel1.Text = toolStripStatusLabel2.Text = "";
            timer1.Enabled = false;
            новыйToolStripMenuItem.Visible = false;
            решитьToolStripMenuItem.Visible = true;
            решитьToolStripMenuItem.Text = Constants.PLAYER_GIVES_UP;
        onemoretime:
            int[,] field = new int[9, 9];
            Random rand = new Random(unchecked((int)(DateTime.Now.Ticks)));
            field[0, 0] = 1 + rand.Next() % 9;
            field[1, 3] = 1 + rand.Next() % 9;
            field[2, 6] = 1 + rand.Next() % 9;
            field[3, 1] = 1 + rand.Next() % 9;
            field[4, 4] = 1 + rand.Next() % 9;
            field[5, 7] = 1 + rand.Next() % 9;
            field[6, 2] = 1 + rand.Next() % 9;
            field[7, 5] = 1 + rand.Next() % 9;
            field[8, 8] = 1 + rand.Next() % 9;
            SudokuLogic logic = new SudokuLogic(field);
            bool err;
            int[,] res = logic.SolveSudoku(out err);
            if (err)
            {
                goto onemoretime;
            }
            int[] perm = Tools.getPermutationByNumber(1 + rand.Next() % 362880);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    res[i, j] = perm[res[i, j] - 1];
                }
            }
            int curPos = 0, block = 1, space = 1;
            if (level == 0)
            {
                block = 3;
                space = 3;
            }
            if (level == 1)
            {
                block = 6;
                space = 3;
            }
            if (level == 2)
            {
                block = 7;
                space = 2;
            }
            while (curPos < 81)
            {
                int x = 1 + rand.Next() % block;
                int y = 1 + rand.Next() % space;
                for (int i = curPos; i <= curPos + x - 1; i++)
                {
                    if (i < 81)
                    {
                        res[i / 9, i % 9] = 0;
                    }
                }
                curPos += (x + y);
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (res[i, j] > 0)
                    {
                        buttons[i * 9 + j].Text = res[i, j].ToString();
                        buttons[i * 9 + j].ForeColor = Color.Blue;
                        buttons[i * 9 + j].ContextMenuStrip = null;
                    }
                    else
                    {
                        buttons[i * 9 + j].Text = "";
                        buttons[i * 9 + j].ForeColor = Color.Red;
                        buttons[i * 9 + j].ContextMenuStrip = contextMenuStrip1;
                    }
                }
            }
            foreach (Button btn in buttons)
            {
                if (btn.ForeColor == Color.Red)
                {
                    btn.Focus();
                    break;
                }
            }
            gameMode = GameMode.FROM_EXISTING;
            timer1.Enabled = true;
            seconds = 0;
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Button b = (Button)contextMenuStrip1.SourceControl;
            int index = -1;
            foreach (Button btn in buttons)
            {
                ++index;
                if (btn == b)
                {
                    break;
                }
            }
            int row = index / 9, col = index % 9;
            for (int z = 0; z < contextMenuStrip1.Items.Count; z++)
            {
                contextMenuStrip1.Items[z].Enabled = true;
            }
            for (int z = 0; z < contextMenuStrip1.Items.Count - 1; z++)
            {
                string text = contextMenuStrip1.Items[z].Text;
                for (int i = 9 * row; i < 9 * (row + 1); i++)
                {
                    if (buttons[i].Text == text)
                    {
                        contextMenuStrip1.Items[z].Enabled = false;
                    }
                }
                for (int i = col; i < 81; i += 9)
                {
                    if (buttons[i].Text == text)
                    {
                        contextMenuStrip1.Items[z].Enabled = false;
                    }
                }
                for (int i = (row / 3) * 3; i < (row / 3) * 3 + 3; i++)
                {
                    for (int j = (col / 3) * 3; j < (col / 3) * 3 + 3; j++)
                    {
                        if (buttons[i * 9 + j].Text == text)
                        {
                            contextMenuStrip1.Items[z].Enabled = false;
                        }
                    }
                }
            }
        }

        private void loadScoreInfo()
        {
            try
            {
                StreamReader sr = File.OpenText(Constants.SETTINGS_FILE);
                string text = sr.ReadLine();
                sr.Close();
                bool ok = true;
                string[] data = text.Split('|');
                if (data.Length != 7)
                {
                    ok = false;
                }
                string s = data[0] + "|" + data[1] + "|" + data[2] + "|" + data[3] + "|" + data[4] + "|" + data[5];
                string check = (((s.GetHashCode()).ToString()).GetHashCode()).ToString();
                if (!data[6].Equals(check))
                {
                    ok = false;
                }
                if (ok)
                {
                    names[0] = data[0];
                    names[1] = data[1];
                    names[2] = data[2];
                    times[0] = Int32.Parse(data[3]);
                    times[1] = Int32.Parse(data[4]);
                    times[2] = Int32.Parse(data[5]);
                }
                else
                {
                    names[0] = names[1] = names[2] = "";
                    times[0] = times[1] = times[2] = Constants.INF;
                    saveScoreInfo();
                }
            }
            catch
            {
                names[0] = names[1] = names[2] = "";
                times[0] = times[1] = times[2] = Constants.INF;
                saveScoreInfo();
            }
        }

        private void saveScoreInfo()
        {
            try
            {
                StreamWriter sw = File.CreateText(Constants.SETTINGS_FILE);
                string s = names[0] + "|" + names[1] + "|" + names[2] + "|" +
                           times[0].ToString() + "|" + times[1].ToString() + "|" + times[2].ToString();
                string check = (((s.GetHashCode()).ToString()).GetHashCode()).ToString();
                sw.Write(s + "|" + check);
                sw.Close();
            }
            catch
            {
                MessageBox.Show(Constants.DISK_WRITE_ERROR, Constants.MAIN_WINDOW_TITLE, MessageBoxButtons.OK);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gameMode == GameMode.FROM_EXISTING)
            {
                if (checkIfSolved())
                {
                    timer1.Enabled = false;
                    новыйToolStripMenuItem.Visible = true;
                    решитьToolStripMenuItem.Visible = false;
                    gameMode = GameMode.NO_GAME;
                    loadScoreInfo();
                    if (times[level] > seconds && times[level] == Constants.INF)
                    {
                        MessageBox.Show(String.Format(Constants.SIMPLE_CONGRATULATION, seconds), Constants.MAIN_WINDOW_TITLE, MessageBoxButtons.OK);
                        names[level] = currentGameState.Substring(2, currentGameState.Length - 2);
                        times[level] = seconds;
                        saveScoreInfo();
                        loadScoreInfo();
                        RecordsTable table = new RecordsTable();
                        table.ShowDialog();
                    }
                    else
                        if (times[level] > seconds && times[level] != Constants.INF)
                    {
                        MessageBox.Show(String.Format(Constants.BEST_TIME_CONGRATULATION, seconds), Constants.MAIN_WINDOW_TITLE, MessageBoxButtons.OK);
                        names[level] = currentGameState.Substring(2, currentGameState.Length - 2);
                        times[level] = seconds;
                        saveScoreInfo();
                        loadScoreInfo();
                        RecordsTable table = new RecordsTable();
                        table.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(String.Format(Constants.SIMPLE_CONGRATULATION, seconds), Constants.MAIN_WINDOW_TITLE, MessageBoxButtons.OK);
                    }
                    toolStripStatusLabel1.Text = Constants.INVITATION;
                    toolStripStatusLabel2.Text = "";
                    seconds = 0;
                    return;
                }
                ++seconds;
                toolStripStatusLabel2.Text = String.Format(Constants.CURRENT_TIME, seconds);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            timer2.Enabled = false;
        }

        private void рекордыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadScoreInfo();
            RecordsTable table = new RecordsTable();
            table.ShowDialog();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutGame about = new AboutGame();
            about.ShowDialog();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame level = new StartGame(currentGameState);
            level.ShowDialog();
            if (level.Result == Constants.CANCEL_GAME)
            {
                return;
            }
            currentGameState = level.Result;
            if (currentGameState[0].ToString().Equals(Constants.COMPUTER_SOLVES))
            {
                emptyGame();
            }
            if (currentGameState[0].ToString().Equals(Constants.PLAYER_SOLVES))
            {
                existingGame();
            }
        }
    }
}