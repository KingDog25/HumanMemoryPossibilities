using System;
using System.Drawing;
using System.Windows.Forms;
//using System.Threading;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Linq;

namespace HumanMemoryPossibilities
{
    public partial class MainForm : Form
    {
        private Settings _settings; // Хранит настройки из формы настроек
        private Random _random = new Random(); // Генерирует случайные числа
        private Label[] _numberLabels; // Метки для отображения чисел или букв
        private Label[] _allLabels; // Для хранения всех 9 меток
        private int[] _numbersToRemember; // Хранит сгенерированные числа для запоминания
        private TextBox[] _answerBoxes; // Текстовые поля для ввода ответов
        private const string ConfigFile = "settings.config";
        private System.Windows.Forms.Timer timer; // Timer для паузы

        public MainForm()
        {
            InitializeComponent();
            LoadSettingsFromConfig();
            /*
            int objectCount = _settings.ObjectCount;
            _numberLabels = new Label[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                _numberLabels[i] = new Label
                {
                    AutoSize = true,
                    Font = new Font("Arial", _random.Next(_settings.MinFontSize, _settings.MaxFontSize + 1)),
                    ForeColor = GetColor(_settings.Color),
                    Location = new Point(_random.Next(50, Width - 50), _random.Next(50, Height - 200))
                };
                this.Controls.Add(_numberLabels[i]);
            }

            // Вызов метода для генерации и отображения чисел после инициализации формы.
            GenerateAndShowNumbers("Арабские цифры");
            */
        }

        private void LoadSettingsFromConfig()
        {
            if (File.Exists(ConfigFile))
            {
                string[] lines = File.ReadAllLines(ConfigFile);
                if (lines.Length == 6)
                {
                    _settings = new Settings(lines[0], int.Parse(lines[1]), int.Parse(lines[2]), int.Parse(lines[3]), decimal.Parse(lines[4]), lines[5]);
                }
            }
            else
            {
                // Default settings if file does not exist
                _settings = new Settings("Арабские цифры", 9, 8, 50, 0.1m, "черный");
            }
        }

        private void StartNewGame(string forma)
        {
            // Удалить предыдущие метки
            if (_numberLabels != null)
            {
                foreach (var label in _numberLabels)
                    this.Controls.Remove(label);
            }

            int objectCount = _settings.ObjectCount;

            _numberLabels = new Label[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                _numberLabels[i] = new Label
                {
                    AutoSize = true,
                    Font = new Font("Arial", _random.Next(_settings.MinFontSize, _settings.MaxFontSize + 1)),
                    ForeColor = GetColor(_settings.Color),
                    Location = GetRandomLocation()
                };
                this.Controls.Add(_numberLabels[i]);
            }

            GenerateAndShowNumbers(forma);
        }

        private Point GetRandomLocation()
        {
            int x = _random.Next(70, Math.Abs(this.ClientRectangle.Width - 150));
            int y = _random.Next(70, Math.Abs(this.ClientRectangle.Height - 150));
            return new Point(x, y);
        }


        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearScreen();
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
            if (settingsForm.DialogResult == DialogResult.OK)
            {
                this.Show(); // Отображение главной формы
                LoadSettingsFromConfig(); // Обновить настройки
                StartNewGame(_settings.ObjectType);
            }
        }

        private void начатьЗановоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame(_settings.ObjectType);
            /*
            if (File.Exists(ConfigFile))
            {
                string[] lines = File.ReadAllLines(ConfigFile);
                _settings = new Settings(lines[0], int.Parse(lines[1]), int.Parse(lines[2]), int.Parse(lines[3]), decimal.Parse(lines[4]), lines[5]);
                
                if (_settings.ObjectType == "Арабские цифры")
                {
                    StartNewGame("Арабские цифры");
                    //GenerateAndShowNumbers("Арабские цифры");
                }
                else if (_settings.ObjectType == "Словесная форма")
                {
                    StartNewGame("Словесная форма");
                    //GenerateAndShowNumbers("Словесная форма");
                }
            }
            */

        }

        private Color GetColor(string colorName)
        {
            switch (colorName.ToLower())
            {
                case "красный":
                    return Color.Red;
                case "оранжевый":
                    return Color.Orange;
                case "желтый":
                    return Color.Yellow;
                case "зеленый":
                    return Color.Green;
                case "голубой":
                    return Color.LightBlue;
                case "синий":
                    return Color.Blue;
                case "фиолетовый":
                    return Color.Purple;
                case "все":
                    return GetRandomColor();;
                default:
                    return Color.Black; // Default color if not recognized
            }
        }

        private Color GetRandomColor()
        {
            Random random = new Random();
            int r = random.Next(256);
            int g = random.Next(256);
            int b = random.Next(256);
            return Color.FromArgb(r, g, b);
        }

        private void GenerateNumbersToRemember()
        {
            int objectCount = _settings.ObjectCount;
            _numbersToRemember = new int[objectCount];

            for (int i = 0; i < objectCount; i++)
            {
                int number;
                do
                {
                    number = _random.Next(10);
                } while (Array.IndexOf(_numbersToRemember, number) != -1);

                _numbersToRemember[i] = number;
            }
        }

        private void ClearScreen()
        {
            // Удалить все метки
            if (_numberLabels != null)
            {
                foreach (var label in _numberLabels)
                    this.Controls.Remove(label);
                _numberLabels = null;
            }

            // Удалить все текстовые поля
            if (_answerBoxes != null)
            {
                foreach (var box in _answerBoxes)
                    this.Controls.Remove(box);
                _answerBoxes = null;
            }

            // Удалить все метки для всех объектов
            if (_allLabels != null)
            {
                foreach (var label in _allLabels)
                    this.Controls.Remove(label);
                _allLabels = null;
            }
        }

        private void ShowNumbers(string objectType)
        {
            // Очистить текст меток
            foreach (var label in _numberLabels)
                label.Text = "";

            // Отобразить числа или слова
            for (int i = 0; i < _numberLabels.Length; i++)
            {
                string text;
                switch (objectType)
                {
                    case "Арабские цифры":
                        text = _numbersToRemember[i].ToString();
                        break;
                    case "Словесная форма":
                        text = GetWordForm(_numbersToRemember[i]);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                _numberLabels[i].Text = text;
            }

            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            // Пауза перед скрытием текста без блокировки главного потока.
            timer = new System.Windows.Forms.Timer();
            timer.Interval = (int)(_settings.PauseTime * 1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_numberLabels != null) // Проверка на null
            {
                foreach (var label in _numberLabels)
                    label.Text = "";
            }

            ShowAllObjects();                    // Вывод всех объектов после паузы
            CreateAnswerBoxes();                // Создание полей для ввода ответов пользователя
            timer.Stop();
            timer.Dispose(); // Удалить таймер после использования
        }

        private void CreateAnswerBoxes()
        {
            if (_answerBoxes != null)
            {
                foreach (var box in _answerBoxes)
                    this.Controls.Remove(box);
            }

            _answerBoxes = new TextBox[_settings.ObjectCount];

            for (int i = 0; i < _settings.ObjectCount; i++)
            {
                TextBox box = new TextBox
                {
                    Location = new Point(50, 50 + i * 30),
                    Width = 50
                };

                this.Controls.Add(box);
                _answerBoxes[i] = box;
            }


            //answerButton.Click += buttonAnswers_Click;
        }





        private void ShowAllObjects()
        {
            // Очистить форму от предыдущих меток и добавить новые для всех 9 объектов
            if (_numberLabels != null)
            {
                foreach (var label in _numberLabels)
                    this.Controls.Remove(label);
            }

            //Label[] allLabels = new Label[9];
            _allLabels = new Label[9];

            for (int i = 0; i < 9; i++)
            {
                _allLabels[i] = new Label
                {
                    AutoSize = true,
                    Font = new Font("Arial", _random.Next(_settings.MinFontSize, _settings.MaxFontSize + 1)),
                    ForeColor = GetColor(_settings.Color),
                    Location = GetRandomLocation()
                };

                string text;

                if (_settings.ObjectType == "Арабские цифры")
                    text = (i + 1).ToString();
                else if (_settings.ObjectType == "Словесная форма")
                {
                    text = GetWordForm(i + 1);
                }
                else // "Все"
                {
                    bool isNumber = _random.NextDouble() < 0.5;
                    if (isNumber)
                        text = (i + 1).ToString();
                    else
                    {
                        text = GetWordForm(i);
                    }
                }

                _allLabels[i].Text = text;
                this.Controls.Add(_allLabels[i]);
            }

            // Убрать эти метки после вывода всех объектов на короткое время.
            System.Windows.Forms.Timer showTimer = new System.Windows.Forms.Timer();
            showTimer.Interval = 15000; // Показать на 15 секунд.
            showTimer.Tick += (sender, args) =>
            {
                if (_allLabels != null) // Проверка на null
                {
                    foreach (var label in _allLabels)
                        this.Controls.Remove(label);
                    _allLabels = null; // Установить _allLabels в null после удаления
                }

                showTimer.Stop();
                showTimer.Dispose();
            };

            showTimer.Start();
        }

        private string GetWordForm(int number)
        {
            switch (number)
            {
                case 0:
                    return "ноль";
                case 1:
                    return "один";
                case 2:
                    return "два";
                case 3:
                    return "три";
                case 4:
                    return "четыре";
                case 5:
                    return "пять";
                case 6:
                    return "шесть";
                case 7:
                    return "семь";
                case 8:
                    return "восемь";
                case 9:
                    return "девять";
                default:
                    throw new ArgumentException("Недопустимое значение числа.");
            }
        }

        private void GenerateAndShowNumbers(string objectType)
        {
            GenerateNumbersToRemember();

            if (objectType == "Все")
            {
                ShowMixedNumbers();
            }
            else
            {
                ShowNumbers(objectType);
            }

            // Пауза перед скрытием текста без блокировки главного потока.
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            timer = new System.Windows.Forms.Timer();
            timer.Interval = (int)(_settings.PauseTime * 1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void ShowMixedNumbers()
        {
            // Очистить текст меток
            foreach (var label in _numberLabels)
                label.Text = "";

            // Отобразить числа или слова случайным образом
            for (int i = 0; i < _numberLabels.Length; i++)
            {
                bool isNumber = _random.NextDouble() < 0.5;
                string text;

                if (isNumber)
                    text = _numbersToRemember[i].ToString();
                else
                    text = GetWordForm(_numbersToRemember[i]);

                _numberLabels[i].Text = text;
            }
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            ClearScreen();
            начатьЗановоToolStripMenuItem.PerformClick();
        }

        private void buttonAnswers_Click(object sender, EventArgs e)
        {
            if (_answerBoxes == null)
            {
                MessageBox.Show("Поля для ввода ответов еще не были отображены. Пожалуйста, дождитесь окончания теста.", "Ошибка");
                return;
            }

            int correctAnswers = 0;
            int incorrectAnswers = 0;
            string message = "Результаты:\n";

            // Список не угаданных правильных цифр
            List<int> missedCorrectNumbers = new List<int>();

            for (int i = 0; i < _answerBoxes.Length; i++)
            {
                try
                {
                    int userAnswer = int.Parse(_answerBoxes[i].Text);

                    if (Array.IndexOf(_numbersToRemember, userAnswer) != -1)
                    {
                        _answerBoxes[i].BackColor = Color.Green;
                        correctAnswers++;
                        message += $"Ответ {userAnswer} правильный.\n";
                    }
                    else
                    {
                        _answerBoxes[i].BackColor = Color.Red;
                        incorrectAnswers++;
                        message += $"Ответ {userAnswer} неправильный.\n";

                        // Проверить, есть ли это число в списке правильных цифр
                        if (Array.IndexOf(_numbersToRemember, userAnswer) == -1)
                        {
                            foreach (int number in _numbersToRemember)
                            {
                                if (!Array.Exists(_answerBoxes.Select(box => int.Parse(box.Text)).ToArray(), x => x == number))
                                {
                                    missedCorrectNumbers.Add(number);
                                }
                            }
                        }
                    }
                }
                catch (FormatException)
                {
                    _answerBoxes[i].BackColor = Color.Red;
                    incorrectAnswers++;
                    message += $"Ответ не распознан.\n";

                    // Проверить, есть ли это число в списке правильных цифр
                    foreach (int number in _numbersToRemember)
                    {
                        if (!Array.Exists(_answerBoxes.Select(box => box.Text).ToArray(), x => x == number.ToString()))
                        {
                            missedCorrectNumbers.Add(number);
                        }
                    }
                }
            }

            // Отобразить не угаданные правильные цифры
            if (missedCorrectNumbers.Count > 0)
            {
                message += "\nНе угаданные правильные цифры:\n";
                foreach (var number in missedCorrectNumbers)
                {
                    message += $"Цифра {number} не была угадана.\n";
                }
            }

            message += $"\nВсего правильных ответов: {correctAnswers}\n";
            message += $"\nВсего неправильных ответов: {incorrectAnswers}\n";

            MessageBox.Show(message, "Результаты");
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("v 1.0.0\n" +
                "Практическое изучение возможностей и ограничений памяти человека.");
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("v 1.0.0\n" +
                "Как известно, возможности памяти человека ограничены. В частности считается, что человек не может оперировать более чем семью (плюс-\r\nминус два) объектами одновременно. При этом реальное количество таких объектов может варьироваться в зависимости от типа объектов.\r\nБудем считать, что возможны следующие графические формы представления чисел:\r\nв виде арабских цифр (например: «4»);\r\nв виде римских цифр (например: «IV»);\r\nсловесная форма (например: «четыре»);\r\nпиктографическая (например, «»).\r\nКроме того каждому числу можно придать визуальную индивидуальность с помощью цвета, гарнитуры, стиля и размера шрифта.\r\nНеобходимо проанализировать, в какой степени влияет используемая форма представления на способность человека к запоминанию чисел.\r\nЗадание\r\nРазработайте программу, реализующую следующие функции:\r\nгенерацию набора цифр от нуля до девяти (количество объектов в наборе должно изменяться от одного до девяти, одинаковые цифры в\r\nнаборе не допускаются);\r\nвывод набора цифр пользователю в одной из определённых в индивидуальном задании графических форм в течение фиксированного\r\nпромежутка времени;\r\nвывод пользователю всех цифр (в заданной графической форме) для того, чтобы он мог указать, какие из них входили в набор;\r\nиндикация ошибок пользователя (при их наличии).\r\n\r\nГрафическая форма:\r\nАрабские цифры, словесная форма, цвет и размер\r\nшрифта\r\n");
        }
    }
}