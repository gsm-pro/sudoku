namespace SudokuPlus
{
    static class Constants
    {
        public const string PROGRAM_NAME = "SudokuPlus.exe";
        public const string SETTINGS_FILE = "SudokuPlus.exe.settings";

        public const string MAIN_WINDOW_TITLE = "Судоку Плюс";
        public const string INIT_ERROR = "Не удалось запустить приложение.";
        public const string INVITATION = "Чтобы начать новую игру, выберите Игра → Новая";
        public const string USE_DIGITS = "Для набора используйте клавиши с цифрами";
        public const string DIGIT_NOT_ALLOWED = "Нельзя поставить цифру ";
        public const string BUT_ALLOWED = ", но можно ";
        public const string COMPUTER_WILL_SOLVE = "Решить";
        public const string PLAYER_GIVES_UP = "Сдаться";
        public const string DISK_WRITE_ERROR = "В процессе записи произошла ошибка. Данные не сохранены.";
        public const string CURRENT_TIME = "Время: {0} сек.";
        public const string SIMPLE_CONGRATULATION = "Поздравляем! Затраченное время: {0} сек.";
        public const string BEST_TIME_CONGRATULATION = "Поздравляем!!! Вы показали лучшее время для данного уровня сложности - {0} сек.";
        public const string NO_SOLUTION = "Судоку не имеет решений";

        public const string CANCEL_GAME = "-";
        public const string PLAYER_SOLVES = "P";
        public const string COMPUTER_SOLVES = "C";
        public const string LEVEL_EASY = "1";
        public const string LEVEL_MEDIUM = "2";
        public const string LEVEL_HARD = "3";
        public const int INF = 999999;
        public const int TIMEOUT = 250;
    }
}