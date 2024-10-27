using HumanMemoryPossibilities.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HumanMemoryPossibilities
{
    public partial class SettingsForm : Form
    {
        public Settings Settings { get; private set; }
        private const string ConfigFile = "settings.config";

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
            //MessageBox.Show($"ObjectType: {Settings.ObjectType}, ObjectCount: {Settings.ObjectCount}, MinFontSize: {Settings.MinFontSize}, MaxFontSize: {Settings.MaxFontSize}, PauseTime: {Settings.PauseTime}, Color: {Settings.Color}");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Settings = new Settings(
                comboBox1.Text,
                Convert.ToInt32(numericUpDown4.Value),
                Convert.ToInt32(numericUpDown1.Value),
                Convert.ToInt32(numericUpDown2.Value),
                numericUpDown3.Value,
                comboBox2.Text
                );
            SaveSettings();
            
            this.DialogResult = DialogResult.OK;
        }

        private void LoadSettings()
        {
            if (File.Exists(ConfigFile))
            {
                string[] lines = File.ReadAllLines(ConfigFile);
                if (lines.Length == 6)
                {
                    Settings = new Settings(
                        lines[0],
                        int.Parse(lines[1]),
                        int.Parse(lines[2]),
                        int.Parse(lines[3]),
                        decimal.Parse(lines[4]),
                        lines[5]
                    );

                    comboBox1.Text = Settings.ObjectType;
                    numericUpDown4.Value = Settings.ObjectCount;
                    numericUpDown1.Value = Settings.MinFontSize;
                    numericUpDown2.Value = Settings.MaxFontSize;
                    numericUpDown3.Value = Convert.ToDecimal(Settings.PauseTime); // Исправлено на правильный элемент управления
                    comboBox2.Text = Settings.Color;
                }
            }
            else
            {
                // Default settings if file does not exist
                Settings = new Settings("Арабские цифры", 9, 8, 50, 0.1m, "черный");
                comboBox1.Text = Settings.ObjectType;
                numericUpDown4.Value = Settings.ObjectCount;
                numericUpDown1.Value = Settings.MinFontSize;
                numericUpDown2.Value = Settings.MaxFontSize;
                numericUpDown4.Value = (decimal)Settings.PauseTime; // Исправлено на правильный элемент управления
                comboBox2.Text = Settings.Color;
            }
        }

        private void SaveSettings()
        {
            string[] lines = new string[]
            {
                Settings.ObjectType,
                Settings.ObjectCount.ToString(),
                Settings.MinFontSize.ToString(),
                Settings.MaxFontSize.ToString(),
                Settings.PauseTime.ToString(),
                Settings.Color
            };

            File.WriteAllLines(ConfigFile, lines);
        }
    }
}
