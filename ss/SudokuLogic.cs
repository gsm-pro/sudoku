using System;

namespace SudokuPlus
{
    class SudokuLogic
    {
        private bool[,] used_row = new bool[9, 10];
        private bool[,] used_col = new bool[9, 10];
        private bool[,,] used_sq = new bool[3, 3, 10];
        private int[,] field = new int[9, 9];
        private int[,] ans = new int[9, 9];
        private bool found = false;
        private int[] perm;

        public SudokuLogic(int[,] field)
        {
            this.field = field;
        }

        private void solveSudoku(int x, int y)
        {
            if (y == 9)
            {
                x++;
                y = 0;
            }
            if (x == 9 && y == 0)
            {
                found = true;
                return;
            }
            if (field[x, y] != 0)
            {
                ans[x, y] = field[x, y];
                solveSudoku(x, y + 1);
                return;
            }
            for (int i = 0; i < 9; i++)
            {
                int d = perm[i];
                if (used_row[x, d] || used_col[y, d] || used_sq[x / 3, y / 3, d])
                {
                    continue;
                }
                ans[x, y] = d;
                used_row[x, d] = true;
                used_col[y, d] = true;
                used_sq[x / 3, y / 3, d] = true;
                solveSudoku(x, y + 1);
                if (found)
                {
                    return;
                }
                used_row[x, d] = false;
                used_col[y, d] = false;
                used_sq[x / 3, y / 3, d] = false;
                ans[x, y] = 0;
            }
        }

        public int[,] SolveSudoku(out bool err)
        {
            err = false;
            Random rand = new Random(unchecked((int)(DateTime.Now.Ticks)));
            perm = Tools.getPermutationByNumber(1 + rand.Next() % 362880);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (field[i, j] != 0)
                    {
                        if (!used_row[i, field[i, j]])
                        {
                            used_row[i, field[i, j]] = true;
                        }
                        else
                        {
                            err = true;
                        }
                        if (!used_col[j, field[i, j]])
                        {
                            used_col[j, field[i, j]] = true;
                        }
                        else
                        {
                            err = true;
                        }
                        if (!used_sq[i / 3, j / 3, field[i, j]])
                        {
                            used_sq[i / 3, j / 3, field[i, j]] = true;
                        }
                        else
                        {
                            err = true;
                        }
                    }
                }
            }
            if (err == true)
            {
                return new int[9, 9];
            }
            Action action = () =>
            {
                solveSudoku(0, 0);
            };
            if (!action.BeginInvoke(null, null).AsyncWaitHandle.WaitOne(Constants.TIMEOUT))
            {
                err = true;
                return new int[9, 9];
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (ans[i, j] == 0)
                    {
                        err = true;
                        return new int[9, 9];
                    }
                }
            }
            return ans;
        }
    }
}