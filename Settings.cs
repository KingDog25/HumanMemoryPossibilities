using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanMemoryPossibilities
{
    public class Settings
    {
        // Константы по умолчанию
        public const string DefaultObjectType = "Арабские цифры";
        public const int DefaultObjectCount = 9;
        public const int DefaultMinFontSize = 8;
        public const int DefaultMaxFontSize = 50;
        public const decimal DefaultPauseTime = 0.1m;
        public const string DefaultColor = "зеленый";

        // Поля класса
        public string ObjectType { get; set; }
        public int ObjectCount { get; set; }
        public int MinFontSize { get; set; }
        public int MaxFontSize { get; set; }
        public decimal PauseTime { get; set; }
        public string Color { get; set; }

        // Конструктор для инициализации полей
        public Settings(string objectType, int objectCount, int minFontSize, int maxFontSize, decimal pauseTime, string color)
        {
            ObjectType = objectType;
            ObjectCount = objectCount;
            MinFontSize = minFontSize;
            MaxFontSize = maxFontSize;
            PauseTime = pauseTime;
            Color = color;
        }

        // Метод для получения дефолтных настроек
        public static Settings GetDefaultSettings()
        {
            return new Settings(
                DefaultObjectType,
                DefaultObjectCount,
                DefaultMinFontSize,
                DefaultMaxFontSize,
                DefaultPauseTime,
                DefaultColor);
        }
    }
}