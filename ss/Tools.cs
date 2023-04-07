namespace SudokuPlus
{
    class Tools
    {
        public static int[] getPermutationByNumber(int k)
        {
            int[] F = new int[10];
            F[0] = 1;
            for (int i = 1; i <= 9; i++)
            {
                F[i] = F[i - 1] * i;
            }
            int n = 9;
            --k;
            int[] p = new int[9];
            for (int i = 0; i < n; i++)
            {
                p[i] = (int)(k / F[n - i - 1]) + 1;
                k %= F[n - i - 1];
            }
            for (int i = n - 2; i >= 0; i--)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (p[j] >= p[i])
                    {
                        p[j]++;
                    }
                }
            }
            return p;
        }

        public static bool isValidPlayerName(string name)
        {
            return name.Length < 21 && name.Trim() != "" && name.IndexOf("/") == -1 &&
                name.IndexOf("\\") == -1 && name.IndexOf("|") == -1;
        }
    }
}