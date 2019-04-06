using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xaml;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace SIIRC
{
    /// <summary>
    /// Конфигурация игры
    /// Содержит всю информацию об игровых настройках
    /// </summary>
    [Serializable]
    public sealed class GameConfiguration
    {
        #region Fields

        private string defPackagePath = string.Empty;
        private string defServerName = string.Empty;
        private string defUserName = string.Empty;
        private string defChannelName = string.Empty;
        private decimal defServerPort = 6667;

        public enum GameTypes { Sport, TeleSI, Erudit4 };
        private GameTypes gameType = GameTypes.Sport;

        private PlayerList players = new PlayerList();

        private bool automaticAnswererChoice = true;
        private bool signalsOnlyOnTimer = true;
        private bool playSpecial = true;

        private bool wholeQuest = false;

        private decimal stringsInterval = 1700;
        private decimal wordsInString = 3;
        private decimal signalTime = 10;
        private decimal printAnswerTime = 15;
        private decimal afterWrongTime = 4;
        private decimal finalTime = 60;

        private decimal colorNum = 12;

        private decimal roundNum = 0;
        private decimal themeNum = 0;
        private decimal questNum = 0;

        private bool closedGame = false;

        private decimal price = 0;
        private decimal person = 0;
        private bool needDef = false;
        private string cost = string.Empty;
        private bool fast = false;
        private decimal time = 0;
        private string[] phrases = null;
        private int phraseIndex = 0;
        private bool print = false;

        private Player chooser = null;
        private int[][] questTable = new int[0][];

        private List<int> timerDurations = new List<int>();

        /// <summary>
        /// Состояние игры
        /// </summary>
        public enum SIStage 
        {
            /// <summary>
            /// Нет игры
            /// </summary>
            None,
            /// <summary>
            /// Начало игры
            /// </summary>
            Begin,
            /// <summary>
            /// Имя пакета
            /// </summary>
            PName,
            /// <summary>
            /// Имя раунда
            /// </summary>
            RName,
            /// <summary>
            /// Имя темы
            /// </summary>
            TName,
            /// <summary>
            /// Стоимость вопроса
            /// </summary>
            QPrice,
            /// <summary>
            /// Тип вопроса
            /// </summary>
            QType,
            /// <summary>
            /// Кот в мешке
            /// </summary>
            QCat,
            /// <summary>
            /// Обобщённый кот в мешке
            /// </summary>
            QBagCat,
            /// <summary>
            /// Вопрос от спонсора
            /// </summary>
            QSponsored,
            /// <summary>
            /// Промежуточное состояние
            /// </summary>
            Proceed,
            /// <summary>
            /// Обобщённый кот в мешке (ч. 2)
            /// </summary>
            QBagCat2,
            /// <summary>
            /// Обобщённый кот в мешке (ч. 3)
            /// </summary>
            QBagCat3,
            /// <summary>
            /// Вывод текста вопроса
            /// </summary>
            QText,
            /// <summary>
            /// Вопрос показан
            /// </summary>
            QShown,
            /// <summary>
            /// Ожидание сигнала от игроков
            /// </summary>
            WaitSignal,
            /// <summary>
            /// Ожидание ответа игрока
            /// </summary>
            WaitAns,
            /// <summary>
            /// Ожидание верного ответа
            /// </summary>
            QRight,
            /// <summary>
            /// Переход к следующему вопросу
            /// </summary>
            QNext,
            /// <summary>
            /// Завершение игры
            /// </summary>
            End,
            /// <summary>
            /// Ожидание выбора
            /// </summary>
            WaitChoose,
            /// <summary>
            /// Ожидание выбора 2
            /// </summary>
            WaitChoose2,
            /// <summary>
            /// Ожидание выбора 3
            /// </summary>
            WaitChoose3,
            LastT,
            LastQ,
            WaitFinal,
            /// <summary>
            /// Вывод единицы сценария
            /// </summary>
            ShowAtom
        }
        /// <summary>
        /// Текущее состояние игры
        /// </summary>
        private SIStage currentStage = SIStage.None;

        #endregion

        #region Properties

        private bool link = true;

        [LocalizedDescription("Склеивать части вопроса")]
        public bool Link
        {
            get { return link; }
            set { link = value; }
        }
        
        /// <summary>
        /// Путь к пакету по умолчанию
        /// </summary>
        [LocalizedDescription("DefPackagePath")]
        public string DefPackagePath
        {
            get { return defPackagePath; }
            set { defPackagePath = value; }
        }

        /// <summary>
        /// Имя сервера по умолчанию
        /// </summary>
        [LocalizedDescription("Имя сервера по умолчанию")]
        public string DefServerName
        {
            get { return defServerName; }
            set { defServerName = value; }
        }

        /// <summary>
        /// Ник по умолчанию
        /// </summary>
        [LocalizedDescription("Ник по умолчанию")]
        public string DefUserName
        {
            get { return defUserName; }
            set { defUserName = value; }
        }

        /// <summary>
        /// Игровой канал по умолчанию
        /// </summary>
        [LocalizedDescription("Игровой канал по умолчанию")]
        public string DefChannelName
        {
            get { return defChannelName; }
            set { defChannelName = value; }
        }

        /// <summary>
        /// Номер порта по умолчанию
        /// </summary>
        [LocalizedDescription("Номер порта по умолчанию")]
        public decimal DefServerPort
        {
            get { return defServerPort; }
            set { defServerPort = value; }
        }

        /// <summary>
        /// Тип игры
        /// </summary>
        [LocalizedDescription("Тип игры")]
        public GameTypes GameType
        {
            get { return gameType; }
            set { gameType = value; }
        }

        /// <summary>
        /// Список игроков
        /// </summary>
        [LocalizedDescription("Список игроков")]
        public PlayerList Players
        {
            get { return players; }
        }

        /// <summary>
        /// Автоматически назначать отвечающего
        /// </summary>
        [LocalizedDescription("Автоматически назначать отвечающего")]
        [DefaultValue(true)]
        public bool AutomaticAnswererChoice
        {
            get { return automaticAnswererChoice; }
            set { automaticAnswererChoice = value; }
        }

        /// <summary>
        /// Принимать сигналы только во время отсчёта таймера
        /// </summary>
        [LocalizedDescription("Принимать сигналы только во время отсчёта таймера")]
        [DefaultValue(true)]
        public bool SignalsOnlyOnTimer
        {
            get { return signalsOnlyOnTimer; }
            set { signalsOnlyOnTimer = value; }
        }

        /// <summary>
        /// Принимать сигналы только во время отсчёта таймера
        /// </summary>
        [LocalizedDescription("Играть музыку")]
        [DefaultValue(false)]
        public bool PlayMusic { get; set; }

        /// <summary>
        /// Играть спецвопросы
        /// </summary>
        [LocalizedDescription("Играть спецвопросы")]
        [DefaultValue(true)]
        public bool PlaySpecial
        {
            get { return playSpecial; }
            set { playSpecial = value; }
        }

        /// <summary>
        /// Выводить вопрос целиком
        /// </summary>
        [LocalizedDescription("Выводить вопрос целиком")]
        [DefaultValue(false)]
        public bool WholeQuest
        {
            get { return wholeQuest; }
            set { wholeQuest = value; }
        }

        /// <summary>
        /// Интервал между строками текста
        /// </summary>
        [LocalizedDescription("Интервал между строками текста")]
        [DefaultValue(1700)]
        public decimal StringsInterval
        {
            get { return stringsInterval; }
            set { stringsInterval = value; }
        }

        /// <summary>
        /// Число слов в строке
        /// </summary>
        [LocalizedDescription("Число слов в строке")]
        [DefaultValue(3)]
        public decimal WordsInString
        {
            get { return wordsInString; }
            set { wordsInString = value; }
        }

        /// <summary>
        /// Время на подачу сигнала
        /// </summary>
        [LocalizedDescription("Время на подачу сигнала")]
        [DefaultValue(10)]
        public decimal SignalTime
        {
            get { return signalTime; }
            set { signalTime = value; }
        }

        /// <summary>
        /// Время на печать ответа
        /// </summary>
        [LocalizedDescription("Время на печать ответа")]
        [DefaultValue(15)]
        public decimal PrintAnswerTime
        {
            get { return printAnswerTime; }
            set { printAnswerTime = value; }
        }

        /// <summary>
        /// Время на размышление после неверного ответа
        /// </summary>
        [LocalizedDescription("Время на размышление после неверного ответа")]
        [DefaultValue(4)]
        public decimal AfterWrongTime
        {
            get { return afterWrongTime; }
            set { afterWrongTime = value; }
        }

        /// <summary>
        /// Время на размышление в финале
        /// </summary>
        [LocalizedDescription("Время на размышление в финале")]
        [DefaultValue(60)]
        public decimal FinalTime
        {
            get { return finalTime; }
            set { finalTime = value; }
        }

        /// <summary>
        /// Номер цвета для ведущего
        /// </summary>
        [LocalizedDescription("Номер цвета для ведущего")]
        [DefaultValue(12)]
        public decimal ColorNum
        {
            get { return colorNum; }
            set { colorNum = value; }
        }

        /// <summary>
        /// Номер раунда
        /// </summary>
        [LocalizedDescription("Номер раунда")]
        public decimal RoundNum
        {
            get { return roundNum; }
            set { roundNum = value; }
        }

        /// <summary>
        /// Номер темы
        /// </summary>
        [LocalizedDescription("Номер темы")]
        public decimal ThemeNum
        {
            get { return themeNum; }
            set { themeNum = value; }
        }

        /// <summary>
        /// Номер вопроса
        /// </summary>
        [LocalizedDescription("Номер вопроса")]
        public decimal QuestNum
        {
            get { return questNum; }
            set { questNum = value; }
        }

        /// <summary>
        /// Закрытая ли игра
        /// </summary>
        [LocalizedDescription("Закрытая ли игра")]
        public bool ClosedGame
        {
            get { return closedGame; }
            set { closedGame = value; }
        }

        /// <summary>
        /// Текущая стадия игры
        /// </summary>
        [LocalizedDescription("Текущая стадия игры")]
        public SIStage CurrentStage
        {
            get { return currentStage; }
            set { currentStage = value; }
        }

        /// <summary>
        /// Цена вопроса
        /// </summary>
        [LocalizedDescription("Цена вопроса")]
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// Отвечающий
        /// </summary>
        [LocalizedDescription("Отвечающий")]
        public decimal Person
        {
            get { return person; }
            set { person = value; }
        }

        /// <summary>
        /// Нужно ли уточнять цену Кота в мешке
        /// </summary>
        [LocalizedDescription("Нужно ли уточнять цену Кота в мешке")]
        public bool NeedDef
        {
            get { return needDef; }
            set { needDef = value; }
        }

        /// <summary>
        /// Описание цены Кота в мешке
        /// </summary>
        [LocalizedDescription("Описание цены Кота в мешке")]
        public string Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        /// <summary>
        /// Уставновлен ли режим быстрого чтения
        /// </summary>
        [LocalizedDescription("Уставновлен ли режим быстрого чтения")]
        public bool Fast
        {
            get { return fast; }
            set { fast = value; }
        }

        /// <summary>
        /// Отсчёт времени на сигнал
        /// </summary>
        [LocalizedDescription("Отсчёт времени на сигнал")]
        public decimal Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// Вопрос, разбитый на фразы
        /// </summary>
        [LocalizedDescription("Вопрос, разбитый на фразы")]
        public string[] Phrases
        {
            get { return phrases; }
            set { phrases = value; }
        }

        /// <summary>
        /// Номер текущей фразы
        /// </summary>
        [LocalizedDescription("Номер текущей фразы")]
        public int PhraseIndex
        {
            get { return phraseIndex; }
            set { phraseIndex = value; }
        }

        /// <summary>
        /// Нужно ли сейчас выводить текст
        /// </summary>
        [LocalizedDescription("Нужно ли сейчас выводить текст")]
        public bool Print
        {
            get { return print; }
            set { print = value; }
        }

        [LocalizedDescription("Номер игрока, выбирающего вопрос/тему в финале")]
        public int Chooser
        {
            get
            {
                if (chooser != null && players != null)
                {
                    int i = players.IndexOf(chooser);
                    if (i == -1)
                    {
                        chooser = null;
                        return -1;
                    }
                    return i;
                }
                return -1;
            }
            set 
            {
                if (value > -1 && value < players.Count)
                    chooser = players[value];
                else
                    chooser = null;
            }
        }

        [LocalizedDescription("Темы-секреты")]
        public bool?[] HiddenThemes { get; set; }

        [LocalizedDescription("Таблица вопросов")]
        public int[][] QuestTable
        {
            get {return questTable;}
            set {questTable = value;}
        }

        [LocalizedDescription("Интервалы для таймера")]
        public List<int> TimerDurations
        {
            get { return timerDurations; }
        }

        [LocalizedDescription("Индекс теккущей единицы сценария")]
        public int AtomIndex { get; set; }

        public enum ShowAtomExitModes
        {
            Simple,
            Partial,
            Special,
            Final
        }

        [LocalizedDescription("Вариант действия по завершению показа сценария")]
        public ShowAtomExitModes ShowAtomExitMode { get; set; }

        #endregion

        private const string ConfigFile = @"Config.xaml";

        public GameConfiguration()
        {
            this.PlayMusic = false;
        }

        /// <summary>
        /// Сохранить игровую конфигурацию
        /// </summary>
        /// <param name="programPath">Путь расположения исполняемого файла клиента</param>
        internal void Save()
        {
            try
            {
                using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    using (var stream = new IsolatedStorageFileStream(ConfigFile, FileMode.Create, FileAccess.Write, file))
                    {
                        XamlServices.Save(stream, this);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Загрузить игровую конфигурацию
        /// </summary>
        /// <param name="programPath">Путь, по которому расположена программа</param>
        /// <returns>Загруженная программа</returns>
        internal static GameConfiguration Load()
        {
            try
            {
                using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    if (file.FileExists(ConfigFile))
                    {
                        using (var stream = new IsolatedStorageFileStream(ConfigFile, FileMode.Open, FileAccess.Read, file))
                        {
                            return (GameConfiguration)XamlServices.Load(stream);
                        }
                    }
                }
            }
            catch
            {

            }

            var gameConfiguration = new GameConfiguration();
            gameConfiguration.timerDurations.Add(10);
            gameConfiguration.timerDurations.Add(30);
            return gameConfiguration;
        }
    }
}
