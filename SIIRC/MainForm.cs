using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIPackages;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using CIRCe.Base;
using IRC.Client.Base;
using System.Linq;
using SIPackages.Core;

namespace SIIRC
{
    public partial class MainForm : UserControl
    {
        private GameConfiguration gameConfig = null;
        private ICIRCeApplication main = null;
        private ICIRCeServer server = null;
        private ICIRCeChannel window = null;
        private SIDocument doc = null;
        private CommandPanel commandPanel = null;
        private bool noAnswer;

        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string QText
        {
            get 
            {
                if (CurrentQuest == null)
                    return string.Empty;
                else
                    return CurrentQuest.Scenario.ToString();
            }
        }

        /// <summary>
        /// Правильные ответы
        /// </summary>
        public string[] QAnswers
        {
            get
            {
                if (CurrentQuest == null)
                    return new string[0];
                else
                    return CurrentQuest.Right.ToArray();
            }
        }

        /// <summary>
        /// Коммментарий к вопросу
        /// </summary>
        public string QComment
        {
            get
            {
                if (CurrentQuest == null)
                    return "";
                else
                    return CurrentQuest.Info.Comments.Text;
            }
        }

        /// <summary>
        /// Номер выбирающего
        /// </summary>
        private int Chooser
        {
            get { return gameConfig.Chooser; }
            set
            {
                if (gameConfig.Chooser != -1)
                {
                    dataGridView1.Rows[gameConfig.Chooser].Cells[0].Style.ForeColor = Color.Black;
                    dataGridView1.Rows[gameConfig.Chooser].Cells[0].Style.SelectionForeColor = dataGridView1.Rows[gameConfig.Chooser].Cells[1].Style.SelectionForeColor;
                }
                gameConfig.Chooser = value;
                ShowChooser();
            }
        }

        private Round CurrentRound
        {
            get 
            {
                var roundNum = (int)gameConfig.RoundNum;
                if (doc.Package == null || roundNum >= doc.Package.Rounds.Count)
                    return null;
                return doc.Package.Rounds[roundNum];
            }
        }

        private Theme CurrentTheme
        {
            get 
            {
                if (CurrentRound == null)
                    return null;

                var themeNum = (int)gameConfig.ThemeNum;
                if (themeNum >= CurrentRound.Themes.Count)
                    return null;
                return CurrentRound.Themes[themeNum];
            }
        }

        private Question CurrentQuest
        {
            get
            {
                if (CurrentTheme == null)
                    return null;

                var questNum = (int)gameConfig.QuestNum;
                if (questNum >= CurrentTheme.Questions.Count)
                    return null;
                return CurrentTheme.Questions[questNum];
            }
        }

        private int QCount
        {
            get
            {
                int total = 0;
                for (int i = 0; i < gameConfig.QuestTable.Length; i++)
                {
                    for (int j = 0; j < gameConfig.QuestTable[i].Length; j++)
                    {
                        if (gameConfig.QuestTable[i][j] > -1)
                        {
                            total++;
                        }
                    }
                }
                return total;
            }
        }

        private const string RightAnswer = "Правильный ответ: ";

        public MainForm(GameConfiguration gameConfig, SIDocument doc, ICIRCeApplication main, ICIRCeServer server, ICIRCeChannel window, CommandPanel commandPanel, SIAddon addon)
        {
            InitializeComponent();  

            this.gameConfig = gameConfig;
            this.main = main;
            this.server = server;
            this.window = window;
            this.commandPanel = commandPanel;

            this.doc = doc;
            
            sourcePlayers.DataSource = gameConfig.Players;

            ((ISupportInitialize)this.dataGridView1).BeginInit();

            this.dataGridView1.AutoGenerateColumns = true;
            this.dataGridView1.DataSource = sourcePlayers;

            this.dataGridView1.Columns["Name"].HeaderText = "Игрок";
            this.dataGridView1.Columns["Name"].Width = 170;
            this.dataGridView1.Columns["Name"].DisplayIndex = 0;

            this.dataGridView1.Columns["Sum"].HeaderText = "Сумма";
            this.dataGridView1.Columns["Sum"].Width = 90;
            this.dataGridView1.Columns["Sum"].DisplayIndex = 1;

            this.dataGridView1.Columns["Right"].HeaderText = "Верно";
            this.dataGridView1.Columns["Right"].Width = 75;
            this.dataGridView1.Columns["Right"].DisplayIndex = 2;

            this.dataGridView1.Columns["Wrong"].HeaderText = "Неверно";
            this.dataGridView1.Columns["Wrong"].Width = 75;
            this.dataGridView1.Columns["Wrong"].DisplayIndex = 3;

            this.dataGridView1.Columns["CanPress"].Visible = false;

            ((ISupportInitialize)this.dataGridView1).EndInit();

            this.tBQuest.DataBindings.Add(new Binding("Text", this, "QText", false, DataSourceUpdateMode.OnPropertyChanged));
            this.lBAnswers.DataSource = this.QAnswers;
            this.tBComment.DataBindings.Add(new Binding("Text", this, "QComment", false, DataSourceUpdateMode.OnPropertyChanged));

            this.cBAutoSetAnswerer.DataBindings.Add(new Binding("Checked", gameConfig, "AutomaticAnswererChoice", false, DataSourceUpdateMode.OnPropertyChanged));
            this.cBSignalsOnTimer.DataBindings.Add(new Binding("Checked", gameConfig, "SignalsOnlyOnTimer", false, DataSourceUpdateMode.OnPropertyChanged));
            this.cBSpecial.DataBindings.Add(new Binding("Checked", gameConfig, "PlaySpecial", false, DataSourceUpdateMode.OnPropertyChanged));

            this.cBWholeQuest.DataBindings.Add(new Binding("Checked", gameConfig, "WholeQuest", false, DataSourceUpdateMode.OnPropertyChanged));
            this.cbPlayMusic.DataBindings.Add(new Binding("Checked", gameConfig, "PlayMusic", false, DataSourceUpdateMode.OnPropertyChanged));

            this.cbLink.DataBindings.Add(new Binding("Checked", gameConfig, "Link", false, DataSourceUpdateMode.OnPropertyChanged));

            this.nUDStringInterval.DataBindings.Add(new Binding("Value", gameConfig, "StringsInterval", false, DataSourceUpdateMode.OnPropertyChanged));
            this.nUDWordsInString.DataBindings.Add(new Binding("Value", gameConfig, "WordsInString", false, DataSourceUpdateMode.OnPropertyChanged));
            this.nUDSignalTime.DataBindings.Add(new Binding("Value", gameConfig, "SignalTime", false, DataSourceUpdateMode.OnPropertyChanged));
            this.nUDAnswerTime.DataBindings.Add(new Binding("Value", gameConfig, "PrintAnswerTime", false, DataSourceUpdateMode.OnPropertyChanged));
            this.nUDAfterWrongTime.DataBindings.Add(new Binding("Value", gameConfig, "AfterWrongTime", false, DataSourceUpdateMode.OnPropertyChanged));
            this.nUDFinalTime.DataBindings.Add(new Binding("Value", gameConfig, "FinalTime", false, DataSourceUpdateMode.OnPropertyChanged));
            this.nUDShowmanColor.DataBindings.Add(new Binding("Value", gameConfig, "ColorNum", false, DataSourceUpdateMode.OnPropertyChanged));

            this.commandPanel.Next += tSBNext_Click;
            this.commandPanel.Ready += tSBReady_Click;

            this.commandPanel.ShowScore += tSMUScore_Click;
            this.commandPanel.ShowStats += tSMUStat_Click;

            this.commandPanel.Yes += tSBYes_Click;
            this.commandPanel.No += tSBNo_Click;
            this.commandPanel.Cancel += commandPanel_Cancel;

            this.commandPanel.Closed += tSMIClosed_Click;
            this.commandPanel.Select += tSMIScroll_Click;

            this.commandPanel.InputSumVisible = false;

            this.window.MessageReceived += addon.Wrap<SessionMessageEventArgs>(window_MessageReceived);

            if (gameConfig.CurrentStage != GameConfiguration.SIStage.Begin)
                this.commandPanel.NextText = "ДАЛЬШЕ";

            if (gameConfig.GameType == GameConfiguration.GameTypes.TeleSI)
            {
                tSMISetChooser.Visible = true;
                tSMIQRemove.Visible = true;
                toolStripSeparator3.Visible = true;
            }

            propertyGrid1.SelectedObject = gameConfig;
            this.commandPanel.ClosedChecked = gameConfig.ClosedGame;

            this.commandPanel.ConfigureTimer += tSMIConfigureTimer_Click;

            main.Status("Готово");
        }

        internal void commandPanel_Cancel(object sender, EventArgs e)
        {
            timer1.Stop();
            Say("Выбор ведущего отменён");
            gameConfig.Players[(int)gameConfig.Person].CanPress = true;
            if (!gameConfig.PlaySpecial || CurrentQuest.Type.Name == QuestionTypes.Simple)
            {
                if (!gameConfig.WholeQuest && timer2.Enabled)
                {
                    gameConfig.Print = true;
                }
                if (gameConfig.WholeQuest || !timer2.Enabled)
                    if (gameConfig.AfterWrongTime > 0)
                        StartTimer((int)gameConfig.AfterWrongTime);
                gameConfig.CurrentStage = GameConfiguration.SIStage.WaitSignal;
            }
            else
            {
                gameConfig.CurrentStage = GameConfiguration.SIStage.QRight;
            }
            this.commandPanel.NextVisible = true;
            this.commandPanel.YesVisible = false;
            this.commandPanel.NoVisible = false;
            this.commandPanel.CancelVisible = false;
        }

        void UpdateTimerList()
        {
            if (this.commandPanel.InvokeRequired)
            {
                Debug.WriteLine(String.Format("Invoke: {0}", Thread.CurrentThread.Name));
                Action del = UpdateTimerList;
                this.BeginInvoke(del, null);
            }
            else
            {
                while (this.commandPanel.TimerDropDownItems.Count > 2)
                    this.commandPanel.TimerDropDownItems.RemoveAt(0);
                
                foreach (int item in gameConfig.TimerDurations)
                {
                    var menuItem = new ToolStripMenuItem(item.ToString());
                    menuItem.Click += RunTimer;
                    this.commandPanel.TimerDropDownItems.Insert(this.commandPanel.TimerDropDownItems.Count - 2, menuItem);
                }
            }
        }

        internal void tSMIConfigureTimer_Click(object sender, EventArgs e)
        {
            using (var dialog = new TimerDurationDialog(gameConfig))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    UpdateTimerList();
            }
        }

        private void RunTimer(object sender, EventArgs e)
        {
            for (int i = 0; i < this.commandPanel.TimerDropDownItems.Count - 2; i++)
            {
                if (this.commandPanel.TimerDropDownItems[i] == sender)
                {
                    int time = gameConfig.TimerDurations[i];
                    StartTimer(time);
                    Say(string.Format("ВРЕМЯ ПОШЛО ({0})!", time));
                    return;
                }
            }
        }

        /// <summary>
        /// На канале получено сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void window_MessageReceived(object sender, SessionMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.EndInvoke(this.BeginInvoke(new EventHandler<SessionMessageEventArgs>(window_MessageReceived), sender, e));
                return;
            }

            if (gameConfig.CurrentStage == GameConfiguration.SIStage.WaitSignal || gameConfig.CurrentStage == GameConfiguration.SIStage.ShowAtom && gameConfig.ShowAtomExitMode == GameConfiguration.ShowAtomExitModes.Partial)
            {
                var text = e.Text;
                if (text.Text != "q" && text.Text != "й" && text.Text != "йй" && text.Text != "ййй" && text.Text != "!пип" && text.Text != "!ПИП")
                    return;

                if (!gameConfig.AutomaticAnswererChoice || CurrentQuest.Type.Name != QuestionTypes.Simple)
                    return;

                var signaller = gameConfig.Players.Find(player => player.Name == e.Sender || gameConfig.GameType == GameConfiguration.GameTypes.Erudit4 && player.Name.Contains(e.Sender));
                if (signaller == null)
                {
                    if (gameConfig.ClosedGame)
                        return;
                    signaller = new Player(e.Sender);
                    gameConfig.Players.Add(signaller);
                    sourcePlayers.ResetBindings(false);
                }
                if (!signaller.CanPress)
                    return;
                timer1.Stop();
                gameConfig.Print = false;
                gameConfig.Person = gameConfig.Players.IndexOf(signaller);
                Say(signaller.Name);
                signaller.CanPress = false;
                Echo(String.Format("[Правильный ответ: {0}]", string.Join(", ", CurrentQuest.Right.ToArray())));
                gameConfig.CurrentStage = GameConfiguration.SIStage.WaitAns;
                AskAnswer();
            }
            else if (gameConfig.CurrentStage == GameConfiguration.SIStage.WaitChoose2)
            {
                if (Chooser > -1 && Chooser < gameConfig.Players.Count && gameConfig.Players[Chooser].Name == e.Sender)
                {
                    Regex r = null;
                    if (CurrentRound.Type == RoundTypes.Standart)
                        r = new Regex(@"\s*(?<theme>\d+)\s+(?<quest>\d+)(\s+(?<num>\d+))?\s*");
                    else
                        r = new Regex(@"\s*(?<theme>\d+)\s*");

                    var text = e.Text;
                    var m = r.Match(text.Text);

                    if (m.Success)
                    {
                        int theme = int.Parse(m.Groups["theme"].Value) - 1;
                        int quest = 0;
                        int num = 1;
                        if (CurrentRound.Type == RoundTypes.Standart)
                        {
                            quest = int.Parse(m.Groups["quest"].Value);

                            if (m.Groups["num"].Length > 0)
                                num = int.Parse(m.Groups["num"].Value);
                        }

                        if (theme < gameConfig.QuestTable.Length)
                        {
                            if (CurrentRound.Type == RoundTypes.Standart)
                            {
                                bool found = false;
                                for (int j = 0; j < gameConfig.QuestTable[theme].Length; j++)
                                {
                                    if (gameConfig.QuestTable[theme][j] == quest)
                                    {
                                        if (num > 1)
                                            num--;
                                        else
                                        {
                                            quest = j;
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                                if (found)
                                {
                                    gameConfig.ThemeNum = theme;
                                    gameConfig.QuestNum = quest;
                                    gameConfig.QuestTable[theme][quest] = -1;
                                    gameConfig.CurrentStage = GameConfiguration.SIStage.WaitChoose3;

                                    UpdateQuestInfo();
                                    Futher();
                                }
                            }
                            else
                            {
                                if (gameConfig.QuestTable[theme][0] > 0)
                                {
                                    gameConfig.QuestTable[theme][0] = 0;
                                    Say("Удалена тема " + CurrentRound.Themes[theme].Name + PlayMusic("si_shrink"));
                                    int total = 0;
                                    foreach (int[] item in gameConfig.QuestTable)
                                    {
                                        if (item[0] > 0)
                                            total++;
                                    }
                                    if (total > 1)
                                    {
                                        int ch = gameConfig.Chooser;
                                        do
                                        {
                                            ch++;
                                            if (ch >= gameConfig.Players.Count)
                                                ch = 0;
                                        } while (gameConfig.Players[ch].Sum <= 0 && ch != gameConfig.Chooser);
                                        Chooser = ch;
                                        gameConfig.CurrentStage = GameConfiguration.SIStage.WaitChoose;
                                    }
                                    else
                                    {
                                        gameConfig.CurrentStage = GameConfiguration.SIStage.LastT;
                                        int n = 0;
                                        foreach (int[] item in gameConfig.QuestTable)
                                        {
                                            if (item[0] > 0)
                                            {
                                                gameConfig.ThemeNum = n;
                                                gameConfig.QuestNum = new Random().Next(CurrentTheme.Questions.Count);
                                                break;
                                            }
                                            n++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AskAnswer()
        {
            if (this.commandPanel.InvokeRequired)
            {
                this.BeginInvoke(new Action(AskAnswer));
                return;
            }

            this.commandPanel.NextVisible = false;
            this.commandPanel.YesVisible = true;
            this.commandPanel.NoVisible = true;
            this.commandPanel.CancelVisible = true;
            if (gameConfig.PrintAnswerTime > 0)
                StartTimer((int)gameConfig.PrintAnswerTime);
        }

        private delegate void Del();

        private void UpdateQuestInfo()
        {
            if (InvokeRequired)
            {
                Del del = UpdateQuestInfo;
                this.BeginInvoke(del, null);
            }
            else
            {
                this.tBQuest.DataBindings[0].ReadValue();
                this.lBAnswers.DataSource = QAnswers;
                this.tBComment.DataBindings[0].ReadValue();
            }
        }

        /// <summary>
        /// Прокрутка дерева
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void tSMIScroll_Click(object sender, EventArgs e)
        {
            PackageTreeViewDialog diag = new PackageTreeViewDialog(doc, gameConfig.GameType);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                this.gameConfig.RoundNum = diag.Round;
                this.gameConfig.ThemeNum = diag.Theme;
                this.gameConfig.QuestNum = diag.Question;

                this.commandPanel.NextText = "ДАЛЬШЕ";

                if (diag.Question == 0)
                {
                    if (diag.Theme == 0)
                    {
                        if (diag.Round == 0)
                        {
                            gameConfig.CurrentStage = GameConfiguration.SIStage.Begin;
                            this.commandPanel.NextText = "НАЧАТЬ ИГРУ";
                        }
                        else
                            gameConfig.CurrentStage = GameConfiguration.SIStage.RName;
                    }
                    else
                        gameConfig.CurrentStage = GameConfiguration.SIStage.TName;
                }
                else
                    gameConfig.CurrentStage = GameConfiguration.SIStage.QPrice;

                this.tBQuest.DataBindings[0].ReadValue();
                this.lBAnswers.DataSource = QAnswers;
                this.tBComment.DataBindings[0].ReadValue();

                this.commandPanel.NextVisible = true;
                this.commandPanel.YesVisible = false;
                this.commandPanel.NoVisible = false;
                this.commandPanel.CancelVisible = false;

                timer1.Stop();
                timer2.Stop();

                HideSumBox();
            }
        }

        internal void tSMIClosed_Click(object sender, EventArgs e)
        {
            commandPanel.ClosedChecked = !commandPanel.ClosedChecked;
            gameConfig.ClosedGame = commandPanel.ClosedChecked;
            if (commandPanel.ClosedChecked)
            {
                Announce("Действие: игра переведена в закрытый режим");
            }
            else
            {
                Announce("Действие: игра переведена в открытый режим");
            }
        }

        void tSMITimer_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                Echo("Таймер уже идёт!");
                return;
            }
            if (this.tSTBActionParam.Text.Length == 0)
            {
                Echo("Укажите число секунд для таймера!");
                return;
            }
            int time = 0;
            if (!int.TryParse(this.tSTBActionParam.Text, out time))
            {
                Echo("Неверный формат времени!");
                return;
            }
            StartTimer(time);
            Say(string.Format("ВРЕМЯ ПОШЛО ({0})!", time));
        }

        internal void tSBYes_Click(object sender, EventArgs e)
        {
            if (gameConfig.Person == -1)
            {
                Echo("Не выбран игрок для изменения суммы!");
                return;
            }
            timer1.Stop();
            var player = gameConfig.Players[(int)gameConfig.Person];
            player.Sum += (int)gameConfig.Price;
            if (gameConfig.GameType == GameConfiguration.GameTypes.TeleSI)
                this.Chooser = (int)gameConfig.Person;
            SortSums();
            player.Right++;
            Say("+" + gameConfig.Price + PlayMusic("si_applause"));
            this.noAnswer = false;
            if ((!gameConfig.PlaySpecial || CurrentQuest.Type.Name == QuestionTypes.Simple) && !gameConfig.WholeQuest)
            {
                //gameConfig.Fast = true;                
                timer2.Stop();
                gameConfig.Print = true;
                var text = new StringBuilder();
                while (gameConfig.PhraseIndex < gameConfig.Phrases.Length)
                {
                    if (text.Length > 0)
                        text.Append(' ');
                    text.Append(gameConfig.Phrases[gameConfig.PhraseIndex++]);
                }

                while (this.gameConfig.AtomIndex < CurrentQuest.Scenario.Count)
                {
                    text.AppendLine();
                    var atom = CurrentQuest.Scenario[this.gameConfig.AtomIndex];
                    if (atom.Type == AtomTypes.Image || atom.Type == AtomTypes.Video || atom.Type == AtomTypes.Audio)
                    {
                        if (atom.Type == AtomTypes.Image)
                            text.Append("Изображение: ");
                        else if (atom.Type == AtomTypes.Video)
                            text.Append("Видео: ");
                        else
                            text.Append("Аудио: ");

                        text.Append(Symbols.Color);
                        text.Append("0url ");
                        text.Append(atom.Text);

                        this.gameConfig.AtomIndex++;
                    }
                    else
                    {
                        text.Append(atom.Text);
                        while (++this.gameConfig.AtomIndex < CurrentQuest.Scenario.Count)
                        {
                            atom = CurrentQuest.Scenario[this.gameConfig.AtomIndex];
                            if (atom.Type == AtomTypes.Text || atom.Type == AtomTypes.Audio)
                            {
                                text.AppendLine();
                                text.Append(atom.Text);
                            }
                            else
                                break;
                        }
                    }
                }               

                Say(text.ToString());
            }

            gameConfig.CurrentStage = GameConfiguration.SIStage.QRight;
            this.commandPanel.NextVisible = true;
            this.commandPanel.YesVisible = false;
            this.commandPanel.NoVisible = false;
            this.commandPanel.CancelVisible = false;
        }

        internal void tSBNo_Click(object sender, EventArgs e)
        {
            if (gameConfig.Person == -1)
            {
                Echo("Не выбран игрок для изменения суммы!");
                return;
            }
            timer1.Stop();
            if (gameConfig.PlaySpecial && CurrentQuest.Type.Name == QuestionTypes.Sponsored)
                gameConfig.Price = 0;
            Player player = gameConfig.Players[(int)gameConfig.Person];
            player.Sum -= (int)gameConfig.Price;
            SortSums();
            player.Wrong++;
            Say("-" + gameConfig.Price);

            this.commandPanel.NextVisible = true;
            this.commandPanel.YesVisible = false;
            this.commandPanel.NoVisible = false;
            this.commandPanel.CancelVisible = false;

            if (!gameConfig.PlaySpecial || CurrentQuest.Type.Name == QuestionTypes.Simple)
            {
                if (!gameConfig.WholeQuest)
                {
                    if (timer2.Enabled)
                        gameConfig.Print = true;
                    else if (this.gameConfig.AtomIndex < CurrentQuest.Scenario.Count)
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.ShowAtom;
                        Futher();
                        return;
                    }
                }
                if (gameConfig.WholeQuest || !timer2.Enabled)
                    if (gameConfig.AfterWrongTime > 0)
                        StartTimer((int)gameConfig.AfterWrongTime);
                gameConfig.CurrentStage = GameConfiguration.SIStage.WaitSignal;
            }
            else
            {
                gameConfig.CurrentStage = GameConfiguration.SIStage.QRight;
            }
        }

        internal void tSBNext_Click(object sender, EventArgs e)
        {
            Futher();
        }

        /// <summary>
        /// Продолжить игру
        /// </summary>
        private void Futher()
        {
            Futher(new StringBuilder());
        }
        
        /// <summary>
        /// Продолжить игру
        /// </summary>
        private void Futher(StringBuilder text)
        {
            if (this.InvokeRequired)
            {
                this.EndInvoke(this.BeginInvoke(new Action<StringBuilder>(Futher), text));
                return;
            }
            
            switch (gameConfig.CurrentStage)
            {
                case GameConfiguration.SIStage.None:
                    break;

                case GameConfiguration.SIStage.Begin:
                    #region Begin

                    Say("ПОЕХАЛИ!");
                    switch (gameConfig.GameType)
                    {
                        case GameConfiguration.GameTypes.Sport:
                            Say("Упрощённая SIGame" + PlayMusic("si_begingame"));
                            break;
                        case GameConfiguration.GameTypes.TeleSI:
                            Say("Классическая SIGame" + PlayMusic("si_begingame"));
                            break;
                        case GameConfiguration.GameTypes.Erudit4:
                            Say("«Эрудит-квартет»" + PlayMusic("si_begingame"));
                            break;
                        default:
                            break;
                    }

                    gameConfig.CurrentStage = GameConfiguration.SIStage.PName;
                    gameConfig.RoundNum = gameConfig.ThemeNum = gameConfig.QuestNum = 0;
                    this.commandPanel.NextText = "ДАЛЬШЕ";
                    this.Chooser = -1;
                    main.Status("Далее: пакет");
                    this.tBQuest.DataBindings[0].ReadValue();
                    this.lBAnswers.DataSource = QAnswers;
                    this.tBComment.DataBindings[0].ReadValue();
                    break;

                    #endregion

                case GameConfiguration.SIStage.PName:
                    #region PName

                    text.AppendLine(String.Format("ПАКЕТ: {0}", doc.Package.Name));
                    if (doc.Package.Info.Authors.Count > 0)
                        text.AppendLine(String.Format("Авторы: {0}", string.Join(", ", RealAuthors(doc, doc.Package.Info.Authors))));
                    if (doc.Package.Info.Sources.Count > 0)
                        text.AppendLine(String.Format("Источники: {0}", string.Join(", ", RealSources(doc, doc.Package.Info.Sources))));
                    if (doc.Package.Info.Comments.Text.Length > 0)
                        PrintComments(text, doc.Package.Info.Comments.Text);
                    Say(text.ToString());
                    gameConfig.CurrentStage = GameConfiguration.SIStage.RName;
                    main.Status("Далее: раунд");
                    break;

                    #endregion

                case GameConfiguration.SIStage.RName:
                    #region RName

                    text.AppendLine(String.Format("РАУНД: {0}", CurrentRound.Name) + PlayMusic("si_beginround"));
                    if (CurrentRound.Info.Authors.Count > 0)
                        text.AppendLine(String.Format("Авторы: {0}", string.Join(", ", RealAuthors(doc, CurrentRound.Info.Authors))));
                    if (CurrentRound.Info.Sources.Count > 0)
                        text.AppendLine(String.Format("Источники: {0}", string.Join(", ", RealSources(doc, CurrentRound.Info.Sources))));
                    if (CurrentRound.Info.Comments.Text.Length > 0)
                        PrintComments(text, CurrentRound.Info.Comments.Text);
                    Say(text.ToString());
                    if (gameConfig.GameType == GameConfiguration.GameTypes.TeleSI)
                    {
                        SetQuestTable();
                        // Say(PlayMusic("si_cathegories"));
                        gameConfig.CurrentStage = GameConfiguration.SIStage.WaitChoose;
                        if (CurrentRound.Type != RoundTypes.Final)
                            main.Status("Далее: таблица вопросов");
                        else
                        {
                            if (gameConfig.Players.Count > 0)
                            {
                                int themeNum = CurrentRound.Themes.Count;
                                int ch = gameConfig.Players.Count - 1;
                                for (int i = themeNum; i > 2; i--)
                                {
                                    int oldCh = ch;
                                    do 
                                    {
                                        ch--;
                                        if (ch < 0)
                                            ch = gameConfig.Players.Count - 1;
                                    } while (gameConfig.Players[ch].Sum <= 0 && ch != oldCh);
                                }
                                Chooser = ch;
                            }
                            main.Status("Далее: список тем");
                        }
                    }
                    else
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.TName;
                        main.Status("Далее: тема");
                    }
                    break;

                    #endregion

                case GameConfiguration.SIStage.TName:
                    #region TName

                    text.AppendLine(String.Format("ТЕМА: {0}", CurrentTheme.Name));
                    if (CurrentTheme.Info.Authors.Count > 0)
                        text.AppendLine(String.Format("Авторы: {0}", string.Join(", ", RealAuthors(doc, CurrentTheme.Info.Authors))));
                    if (CurrentTheme.Info.Sources.Count > 0)
                        text.AppendLine(String.Format("Источники: {0}", string.Join(", ", RealSources(doc, CurrentTheme.Info.Sources))));
                    if (CurrentTheme.Info.Comments.Text.Length > 0)
                        PrintComments(text, CurrentTheme.Info.Comments.Text);

                    Say(text.ToString());
                    gameConfig.CurrentStage = GameConfiguration.SIStage.QPrice;
                    main.Status("Далее: вопрос");
                    break;

                    #endregion

                case GameConfiguration.SIStage.WaitChoose:
                    #region WaitChoose
                    
                    // Состояние происходит только в ТелеСИ, поэтому дополнительная проверка не требуется
                    PrintTable();
                    gameConfig.CurrentStage = GameConfiguration.SIStage.WaitChoose2;
                    if (CurrentRound.Type == RoundTypes.Standart)
                        main.Status("Далее: ожидайте, когда выбирающий сделает свой выбор в формате \"<Номер темы> <Стоимость вопроса> (<Номер вопроса>)\"");
                    else
                    {
                        main.Status("Далее: выбирайте убирающего и ожидайте, когда он сделает свой выбор в формате \"<Номер темы>\"");
                    }
                    break;

                    #endregion

                case GameConfiguration.SIStage.QPrice:
                    #region QPrice

                    text.AppendLine(CurrentQuest.Price.ToString());
                    gameConfig.Price = CurrentQuest.Price;

                    // Ищем комментарии ведущему
                    string[] comments = CurrentQuest.Info.Comments.Text.Split(new string[] { Symbols.StringSeparator }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string comment in comments)
                    {
                        if (comment.Length > 2 && comment[0] == '*')
                            Echo(String.Format("Комментарии ведущему: {0}", comment.Substring(1)));
                    }

                    Say(text.ToString());
                    if (gameConfig.PlaySpecial)
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.QType;
                        main.Status("Далее: тип вопроса/текст простого вопроса");
                    }
                    else
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                        main.Status("Далее: текст вопроса");
                    }
                    break;

                    #endregion

                case GameConfiguration.SIStage.WaitChoose3:
                    #region WaitChoose3
                    var themeName = CurrentTheme.Name;
                    var themeNumber = (int)gameConfig.ThemeNum;
                    if (this.gameConfig.HiddenThemes != null && this.gameConfig.HiddenThemes[themeNumber].HasValue)
                    {
                        themeName = this.gameConfig.HiddenThemes[themeNumber].Value ? "ПОКА СЕКРЕТ" : themeName.TrimStart().Substring(1);
                    }
                    text.AppendLine(String.Format("ТЕМА: {0}", themeName));
                    if (CurrentTheme.Info.Authors.Count > 0)
                        text.AppendLine(String.Format("Авторы: {0}", string.Join(", ", RealAuthors(doc, CurrentTheme.Info.Authors))));
                    if (CurrentTheme.Info.Sources.Count > 0)
                        text.AppendLine(String.Format("Источники: {0}", string.Join(", ", RealSources(doc, CurrentTheme.Info.Sources))));
                    if (CurrentTheme.Info.Comments.Text.Length > 0)
                    {
                        PrintComments(text, CurrentTheme.Info.Comments.Text);
                    }
                    Say(text.ToString());
                    gameConfig.Price = CurrentQuest.Price;

                    // Ищем комментарии ведущему
                    var comments2 = CurrentQuest.Info.Comments.Text.Split(new string[] { Symbols.StringSeparator }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string comment in comments2)
                    {
                        if (comment.Length > 2 && comment[0] == '*')
                            window.Echo("Комментарии ведущему: " + comment.Substring(1));
                    }

                    if (gameConfig.PlaySpecial)
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.QType;
                        main.Status("Далее: тип вопроса/текст простого вопроса");
                    }
                    else
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                        main.Status("Далее: текст вопроса");
                    }
                    break;
                    #endregion

                case GameConfiguration.SIStage.LastT:
                    Say(String.Format("Итак, мы играем тему {0}. Уважаемые игроки, делайте, пожалуйста, ваши ставки!", CurrentTheme.Name));
                    gameConfig.CurrentStage = GameConfiguration.SIStage.LastQ;
                    main.Status("Примите ставки от всех игроков");
                    break;

                case GameConfiguration.SIStage.LastQ:
                    Say("Внимание, вопрос!");
                    main.Status("Далее: текст вопроса");
                    gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                    break;

                case GameConfiguration.SIStage.QType:
                    #region QType

                    if (CurrentQuest.Type.Name == QuestionTypes.Auction)
                    {                        
                            Say("Вам достаётся ВОПРОС-АУКЦИОН!" + PlayMusic("si_auction"));
                            gameConfig.Person = -1;
                            gameConfig.Price = -1;
                            gameConfig.CurrentStage = GameConfiguration.SIStage.Proceed;
                            this.commandPanel.InputSum = string.Empty;
                            this.commandPanel.InputSumVisible = true;
                            main.Status("Введите в текстовое поле размер ставки, выберите игрока в списке и с помощью контекстного меню назначьте его отвечающим");
                    }
                    else if (CurrentQuest.Type.Name == QuestionTypes.Cat)
                    {
                            text.AppendLine("Вам достаётся КОТ В МЕШКЕ!" + PlayMusic("si_catinbag"));
                            text.AppendLine("Вопрос нужно отдать");
                            Say(text.ToString());
                            gameConfig.Person = -1;
                            gameConfig.CurrentStage = GameConfiguration.SIStage.Proceed;
                            main.Status("Выберите игрока в списке и с помощью контекстного меню назначьте его отвечающим");
                    }
                    else if (CurrentQuest.Type.Name == QuestionTypes.BagCat)
                    {
                            Say("Вам достаётся КОТ В МЕШКЕ!" + PlayMusic("si_catinbag"));
                            gameConfig.CurrentStage = GameConfiguration.SIStage.QBagCat;
                            main.Status("Далее: информация о коте");
                    }
                    else if (CurrentQuest.Type.Name == QuestionTypes.Sponsored)
                    {
                            Say("Вам достаётся ВОПРОС ОТ СПОНСОРА!");
                            gameConfig.Price = (int)gameConfig.Price * 2;
                            gameConfig.Person = gameConfig.Chooser;
                            gameConfig.CurrentStage = GameConfiguration.SIStage.QSponsored;
                            main.Status("Далее: информация о вопросе");
                    }
                    else if (CurrentQuest.Type.Name == QuestionTypes.Simple)
                    {
                            gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                            Futher();
                    }
                    else
                    {
                        Say(String.Format("Обнаружен неподдерживаемый тип вопроса {0}. Ведущий может его разыграть самостоятельно, а игра продолжится своим чередом.", CurrentQuest.Type.Name));

                        gameConfig.CurrentStage = GameConfiguration.SIStage.QNext;
                            Futher();
                            main.Status("Далее: переход к следующему вопросу");
                    }
                    break;

                    #endregion

                case GameConfiguration.SIStage.QCat:
                    Say(string.Format("ТЕМА: {0}, СТОИМОСТЬ: {1}", CurrentQuest.Type[QuestionTypeParams.Cat_Theme], CurrentQuest.Type[QuestionTypeParams.Cat_Cost]));
                    int cPrice = 0;
                    if (!int.TryParse(CurrentQuest.Type[QuestionTypeParams.Cat_Cost], out cPrice))
                    {
                        Echo("Не удалось распознать стоимость Кота в мешке. Ожидалось число. Было задано значение: " + CurrentQuest.Type[QuestionTypeParams.Cat_Cost] + ". Вопрос разыгрывается за номинал.");
                        cPrice = CurrentQuest.Price;
                    }
                    gameConfig.Price = cPrice;
                    gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                    main.Status("Далее: текст вопроса");
                    break;

                case GameConfiguration.SIStage.QBagCat:
                    #region QBagCat
                    string self = CurrentQuest.Type[QuestionTypeParams.BagCat_Self];
                    string knows = CurrentQuest.Type[QuestionTypeParams.BagCat_Knows];
                    if (self == QuestionTypeParams.BagCat_Self_Value_True)
                        text.AppendLine("Этого Кота можно оставить себе");
                    else
                        text.AppendLine("Вопрос нужно отдать");

                    #region Set cost

                    gameConfig.NeedDef = true;
                    string cost = CurrentQuest.Type[QuestionTypeParams.Cat_Cost];
                    if (cost == "0")
                        cost = "Минимум или максимум в раунде (на выбор)";
                    else
                    {
                        int result = -1;
                        if (int.TryParse(cost, out result))
                        {
                            gameConfig.Price = result;
                            gameConfig.NeedDef = false;
                        }
                        else
                        {
                            var rx = new Regex(@"\[(?'min'\d+);(?'max'\d+)\](/(?'step'\d+))?");
                            var m = rx.Match(cost);
                            if (m.Success)
                            {
                                int minCost = int.Parse(m.Groups["min"].ToString());
                                int maxCost = int.Parse(m.Groups["max"].ToString());
                                string th = m.Groups["step"].ToString();
                                if (th.Length > 0)
                                {
                                    int step = int.Parse(th);
                                    cost = "От " + minCost + " до " + maxCost + " с шагом в " + step + " (на выбор)";
                                }
                                else
                                {
                                    cost = minCost + " или " + maxCost + " (на выбор)";
                                }
                            }
                            else
                            {
                                Echo("Не удалось распознать стоимость Обобщённого Кота в мешке. Ожидались число, 0 или интервал. Было задано значение: " + cost + ". Вопрос разыгрывается за номинал.");
                                gameConfig.Price = CurrentQuest.Price;
                                gameConfig.NeedDef = false;
                            }
                        }
                    }

                    #endregion

                    gameConfig.CurrentStage = GameConfiguration.SIStage.Proceed;
                    gameConfig.Person = -1;
                    gameConfig.Cost = cost;

                    if (knows == QuestionTypeParams.BagCat_Knows_Value_Before)
                    {
                        text.AppendLine(string.Format("ТЕМА: {0}, СТОИМОСТЬ: {1}", CurrentQuest.Type[QuestionTypeParams.Cat_Theme], cost));
                    }
                    Say(text.ToString());
                    main.Status("Выберите игрока в списке и с помощью контекстного меню назначьте его отвечающим");
                    break;
                    #endregion

                case GameConfiguration.SIStage.QSponsored:
                    #region QSponsored
		            Say(String.Format("Вопрос звучит только для вас. В случае верного ответа вы заработаете {0}, в случае неверного ничего не теряете", gameConfig.Price));
                    gameConfig.CurrentStage = GameConfiguration.SIStage.Proceed;
                    main.Status("Выберите игрока в списке и с помощью контекстного меню назначьте его отвечающим. По умолчанию вопрос будет отдан ВЫБРАВШЕМУ его");
                    break;
                    #endregion

                case GameConfiguration.SIStage.Proceed:
                    #region Proceed
                    if (CurrentQuest.Type.Name == QuestionTypes.Auction)
                    {
                            int price = 0;
                            bool b = int.TryParse(this.commandPanel.InputSum, out price);
                            if (!b)
                                b = int.TryParse(this.tSTBActionParam.Text, out price);
                            if (!b)
                                break;
                            gameConfig.Price = price;
                            if ((int)gameConfig.Person == -1 || (int)gameConfig.Price <= 0)
                                return;
                            HideSumBox();
                            Say(string.Format("Играет {0} за {1}", gameConfig.Players[(int)gameConfig.Person].Name, gameConfig.Price));
                            gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                            return;
                    }

                    if (CurrentQuest.Type.Name == QuestionTypes.Cat)
                    {
                            if ((int)gameConfig.Person == -1)
                                return;
                            else
                            {
                                gameConfig.CurrentStage = GameConfiguration.SIStage.QCat;
                                Futher();
                                return;
                            }
                    }

                    if (CurrentQuest.Type.Name == QuestionTypes.BagCat)
                    {
                            if ((int)gameConfig.Person == -1)
                                return;
                            else
                            {
                                gameConfig.CurrentStage = GameConfiguration.SIStage.QBagCat2;
                                Futher();
                                return;
                            }
                    }

                    if (CurrentQuest.Type.Name == QuestionTypes.Sponsored)
                    {
                            if ((int)gameConfig.Person == -1)
                                return;
                    }
                    
                    gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                    Futher();
                    break;
                    #endregion

                case GameConfiguration.SIStage.QBagCat2:
                    #region QBagCat2
                    gameConfig.CurrentStage = GameConfiguration.SIStage.QBagCat3;
                    tSTBActionParam.Clear();
                    if (gameConfig.NeedDef)
                    {
                        this.commandPanel.InputSum = string.Empty;
                        this.commandPanel.InputSumVisible = true;
                    }
                    if (CurrentQuest.Type[QuestionTypeParams.BagCat_Knows] == QuestionTypeParams.BagCat_Knows_Value_Before)
                    {
                        Futher();
                    }
                    else
                    {
                        Say(string.Format("ТЕМА: {0}, СТОИМОСТЬ: {1}", CurrentQuest.Type[QuestionTypeParams.Cat_Theme], gameConfig.Cost));
                        main.Status("Далее: вопрос/ввод цены/простое начисление");
                    }
                    break;
                    #endregion

                case GameConfiguration.SIStage.QBagCat3:
                    #region QBagCat3
                    if (CurrentQuest.Type[QuestionTypeParams.BagCat_Knows] == QuestionTypeParams.BagCat_Knows_Value_Never)
                    {
                        text.AppendLine("А в этом Коте вопроса нет! Деньги просто перечисляются на ваш счёт!");
                        int person = (int)gameConfig.Person;
                        int price = (int)gameConfig.Price;
                        gameConfig.Players[person].Sum += price;
                        gameConfig.Person = -1;
                        text.AppendLine("+" + price);
                        gameConfig.CurrentStage = GameConfiguration.SIStage.QNext;
                        Say(text.ToString());
                    }
                    else
                    {
                        if (gameConfig.NeedDef)
                        {
                            if (this.commandPanel.InputSum.Length == 0 && tSTBActionParam.Text.Length == 0)
                            {
                                main.Status("Введите в текстовое поле цену кота");
                                return;
                            }
                            else
                            {
                                int price = 0;
                                bool b = int.TryParse(this.commandPanel.InputSum, out price);
                                if (!b)
                                    b = int.TryParse(tSTBActionParam.Text, out price);
                                if (!b)
                                    return;
                                gameConfig.Price = price;
                                gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                                HideSumBox();
                            }
                        }
                        else
                            gameConfig.CurrentStage = GameConfiguration.SIStage.QText;
                    }
                    Futher();
                    break;
#endregion

                case GameConfiguration.SIStage.QText:
                    #region QText
                    if (CurrentRound.Type == RoundTypes.Final)
                    {
                        this.gameConfig.CurrentStage = GameConfiguration.SIStage.ShowAtom;
                        this.gameConfig.AtomIndex = 0;
                        this.gameConfig.ShowAtomExitMode = GameConfiguration.ShowAtomExitModes.Final;
                        Futher();
                        return;
                    }
                    if (!gameConfig.PlaySpecial && (CurrentQuest.Type.Name == QuestionTypes.Cat || CurrentQuest.Type.Name == QuestionTypes.BagCat))
                    {
                        text.AppendLine("(кот)");
                    }
                    if (!gameConfig.PlaySpecial || CurrentQuest.Type.Name == QuestionTypes.Simple)
                    {
                        gameConfig.Players.ForEach(player => player.CanPress = true);
                        gameConfig.Person = -1;
                        if (gameConfig.WholeQuest)
                        {
                            this.gameConfig.CurrentStage = GameConfiguration.SIStage.ShowAtom;
                            this.gameConfig.AtomIndex = 0;
                            this.gameConfig.ShowAtomExitMode = GameConfiguration.ShowAtomExitModes.Simple;
                            Futher(text);
                            return;                     
                        }
                        else
                        {
                            this.gameConfig.CurrentStage = GameConfiguration.SIStage.ShowAtom;
                            this.gameConfig.AtomIndex = 0;
                            this.gameConfig.ShowAtomExitMode = GameConfiguration.ShowAtomExitModes.Partial;
                            gameConfig.Print = false;
                            //gameConfig.CurrentStage = GameConfiguration.SIStage.WaitSignal;
                            gameConfig.Fast = false;
                            //ShowQuest();
                            Futher(text);
                            return;
                        }
                    }
                    else
                    {
                        this.gameConfig.CurrentStage = GameConfiguration.SIStage.ShowAtom;
                        this.gameConfig.AtomIndex = 0;
                        this.gameConfig.ShowAtomExitMode = GameConfiguration.ShowAtomExitModes.Special;
                        Futher();
                        return;       
                    }
                    #endregion

                case GameConfiguration.SIStage.ShowAtom:
                    #region ShowAtom
                    if (!gameConfig.WholeQuest && gameConfig.Print)
                    {
                        if (gameConfig.StringsInterval > 0)
                            Echo("Дождитесь, пока выведется текст");
                        else
                            timer2_Tick(timer2, EventArgs.Empty);                        
                        return;
                    }

                    if (this.gameConfig.AtomIndex < CurrentQuest.Scenario.Count)
                    {
                        var atom = CurrentQuest.Scenario[this.gameConfig.AtomIndex];
                        if (atom.Type == AtomTypes.Image || atom.Type == AtomTypes.Video || atom.Type == AtomTypes.Audio)
                        {
                            if (atom.Type == AtomTypes.Image)
                                text.Append("Изображение: ");
                            else if (atom.Type == AtomTypes.Video)
                                text.Append("Видео: ");
                            else
                                text.Append("Аудио: ");

                            text.Append(Symbols.Color);
                            text.Append("0url ");
                            text.Append(Symbols.Color);
                            text.Append((int)gameConfig.ColorNum);
                            text.Append(atom.Text);

                            this.gameConfig.AtomIndex++;
                        }
                        else
                        {
                            text.Append(atom.Text);
                            if (this.gameConfig.Link) // Текстовые атомы склеиваются
                            {
                                while (++this.gameConfig.AtomIndex < CurrentQuest.Scenario.Count)
                                {
                                    atom = CurrentQuest.Scenario[this.gameConfig.AtomIndex];
                                    if (atom.Type == AtomTypes.Text || atom.Type == AtomTypes.Audio)
                                    {
                                        text.AppendLine();
                                        text.Append(atom.Text);
                                    }
                                    else
                                        break;
                                }
                            }
                            else
                                this.gameConfig.AtomIndex++;

                            if (this.gameConfig.ShowAtomExitMode == GameConfiguration.ShowAtomExitModes.Partial)
                            {
                                ShowQuest(text.ToString());
                                return;
                            }
                        }
                    }

                    if (this.gameConfig.AtomIndex < CurrentQuest.Scenario.Count)
                    {
                        text.Append(Symbols.Color);
                        text.Append("1 [->]");
                    }
                    else if (this.gameConfig.ShowAtomExitMode == GameConfiguration.ShowAtomExitModes.Partial)
                    {
                        text.AppendLine();
                        text.Append("[*]");
                    }

                    foreach (var item in text.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Say(item);
                    }

                    text = new StringBuilder();

                    if (this.gameConfig.AtomIndex == CurrentQuest.Scenario.Count)
                    {
                        // Сценарий отображён полностью
                        switch (this.gameConfig.ShowAtomExitMode)
                        {
                            case GameConfiguration.ShowAtomExitModes.Simple:
                                {
                                    gameConfig.CurrentStage = GameConfiguration.SIStage.WaitSignal;
                                    this.noAnswer = true;
                                    if (gameConfig.SignalTime > 0)
                                        StartTimer((int)gameConfig.SignalTime + text.Length / 40);
                                    main.Status("Ожидание сигналов игроков");
                                }
                                break;
                            case GameConfiguration.ShowAtomExitModes.Partial:
                                {                                    
                                    gameConfig.CurrentStage = GameConfiguration.SIStage.QShown;
                                    gameConfig.Print = false;
                                    Futher();
                                }
                                break;
                            case GameConfiguration.ShowAtomExitModes.Special:
                                {
                                    gameConfig.CurrentStage = GameConfiguration.SIStage.WaitAns;
                                    if (gameConfig.PrintAnswerTime > 0)
                                        StartTimer((int)gameConfig.PrintAnswerTime + text.Length / 40);
                                    main.Status("Ожидание ответа");
                                    this.commandPanel.NextVisible = false;
                                    this.commandPanel.YesVisible = true;
                                    this.commandPanel.NoVisible = true;
                                    if (gameConfig.GameType == GameConfiguration.GameTypes.TeleSI)
                                        this.Chooser = (int)gameConfig.Person;
                                    Echo("[Правильный ответ: " + string.Join(", ", CurrentQuest.Right.ToArray()) + "]");
                                }
                                break;
                            case GameConfiguration.ShowAtomExitModes.Final:
                                {
                                    text.AppendLine(String.Format("{0} секунд", gameConfig.FinalTime));
                                    Say(text.ToString() + PlayMusic("si_finalthink"));
                                    if (gameConfig.FinalTime > 0)
                                    {
                                        StartTimer((int)gameConfig.FinalTime);
                                        gameConfig.CurrentStage = GameConfiguration.SIStage.WaitFinal;
                                    }
                                    else
                                        gameConfig.CurrentStage = GameConfiguration.SIStage.QRight;
                                    main.Status("Ожидание ответов");
                                }
                                break;
                            default:
                                break;
                        }                        
                    }

                    break;
                    #endregion

                case GameConfiguration.SIStage.QShown:
                    if (!gameConfig.PlaySpecial || CurrentQuest.Type.Name == QuestionTypes.Simple)
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.WaitSignal;
                        this.noAnswer = true;
                        if (gameConfig.SignalTime > 0)
                            StartTimer((int)gameConfig.SignalTime);
                        main.Status("Ожидание сигналов игроков");
                    }
                    break;

                case GameConfiguration.SIStage.WaitSignal:
                    if (gameConfig.Time > 0 && timer1.Enabled || (!gameConfig.WholeQuest && gameConfig.Print))
                        Echo("Дождитесь, пока истечёт время для нажатия на кнопку");
                    else
                    {
                        gameConfig.CurrentStage = GameConfiguration.SIStage.QRight;
                        this.noAnswer = true;
                        Futher();
                    }
                    break;

                case GameConfiguration.SIStage.WaitAns:
                    Echo("Для начала определитесь с тем, правильно или нет ответил игрок");
                    break;

                case GameConfiguration.SIStage.QRight:
                    this.commandPanel.TimerValue = 0;
                    var answers = string.Join(Environment.NewLine, CurrentQuest.Right.ToArray()).Trim();
                    var noSign = string.IsNullOrWhiteSpace(answers) || answers.EndsWith(".") || answers.EndsWith("!") || answers.EndsWith("?") || answers.EndsWith(".\"") || answers.EndsWith("!\"") || answers.EndsWith("?\"");
                    text.Append(RightAnswer + answers + (noSign ? "" : "."));

                    if (this.noAnswer)
                        text.AppendLine(PlayMusic("si_noanswer"));
                    else
                        text.AppendLine();

                    if (CurrentQuest.Info.Authors.Count > 0)
                        text.AppendLine("Авторы: " + string.Join(", ", RealAuthors(doc, CurrentQuest.Info.Authors)));

                    if (CurrentQuest.Info.Sources.Count > 0)
                        text.AppendLine("Источники: " + string.Join(", ", RealSources(doc, CurrentQuest.Info.Sources)));
                                        
                    if (CurrentQuest.Info.Comments.Text.Length > 0)
                        text.Append("Комментарии: ");

                    Array.ForEach(CurrentQuest.Info.Comments.Text.Split(new string[] { Symbols.StringSeparator }, StringSplitOptions.RemoveEmptyEntries), comment =>
                    {
                        if (comment.Length > 2 && comment[0] != '*')
                            text.AppendLine(comment);
                    });                                       

                    Say(text.ToString());

                    if (gameConfig.HiddenThemes != null && (int)gameConfig.QuestNum == 0)
                    {
                        var themeNum = (int)gameConfig.ThemeNum;
                        if (gameConfig.HiddenThemes[themeNum].HasValue)
                            gameConfig.HiddenThemes[themeNum] = false;
                    }

                    gameConfig.CurrentStage = GameConfiguration.SIStage.QNext;
                    Futher();
                    break;

                case GameConfiguration.SIStage.QNext:
                    if (gameConfig.GameType == GameConfiguration.GameTypes.TeleSI)
                    {
                        if (CurrentRound.Type == RoundTypes.Standart && QCount > 0)
                            gameConfig.CurrentStage = GameConfiguration.SIStage.WaitChoose;
                        else
                        {
                            gameConfig.RoundNum++;
                            if (gameConfig.RoundNum >= doc.Package.Rounds.Count)
                            {
                                gameConfig.CurrentStage = GameConfiguration.SIStage.End;
                                gameConfig.RoundNum = 0;
                            }
                            else
                                gameConfig.CurrentStage = GameConfiguration.SIStage.RName;
                        }
                    }
                    else
                    {
                        gameConfig.QuestNum++;
                        if (gameConfig.QuestNum >= CurrentTheme.Questions.Count)
                        {
                            gameConfig.ThemeNum++;
                            gameConfig.QuestNum = 0;
                            if (gameConfig.ThemeNum >= CurrentRound.Themes.Count)
                            {
                                gameConfig.RoundNum++;
                                gameConfig.ThemeNum = 0;
                                if (gameConfig.RoundNum >= doc.Package.Rounds.Count)
                                {
                                    gameConfig.CurrentStage = GameConfiguration.SIStage.End;
                                    gameConfig.RoundNum = 0;
                                }
                                else
                                    gameConfig.CurrentStage = GameConfiguration.SIStage.RName;
                            }
                            else
                                gameConfig.CurrentStage = GameConfiguration.SIStage.TName;
                        }
                        else
                            gameConfig.CurrentStage = GameConfiguration.SIStage.QPrice;

                        this.tBQuest.DataBindings[0].ReadValue();
                        this.lBAnswers.DataSource = QAnswers;
                        this.tBComment.DataBindings[0].ReadValue();
                    }
                    break;

                case GameConfiguration.SIStage.End:
                    ShowScore();
                    End();
                    gameConfig.CurrentStage = GameConfiguration.SIStage.None;
                    break;

                default:
                    break;
            }
        }

        private void PrintComments(StringBuilder text, string comments)
        {
            foreach (var comment in comments.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                text.AppendLine(String.Format("Комментарии: {0}", comment));
            }
        }

        private void HideSumBox()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(HideSumBox));
                return;
            }

            this.commandPanel.InputSumVisible = false;
        }

        /// <summary>
        /// Выделить настоящих авторов
        /// </summary>
        /// <param name="authors">Список авторов</param>
        /// <returns>Набор авторов, где вычислены все ссылки</returns>
        private string[] RealAuthors(SIDocument doc, Authors authors)
        {
            var result = new List<string>();
            for (int i = 0; i < authors.Count; i++)
            {
                var value = doc.GetLink(authors, i);
                result.Add(value != null ? value.ToString() : authors[i]);                
            }
            return result.ToArray();
        }

        /// <summary>
        /// Выделить настоящие источники
        /// </summary>
        /// <param name="authors">Список источникоа</param>
        /// <returns>Набор источников, где вычислены все ссылки</returns>
        private string[] RealSources(SIDocument doc, Sources sources)
        {
            var result = new List<string>();
            for (int i = 0; i < sources.Count; i++)
            {
                var value = doc.GetLink(sources, i);
                result.Add(value != null ? value.ToString() : sources[i]);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Сыграть на канале музыкальный файл
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string PlayMusic(string file)
        {
            return gameConfig.PlayMusic ? string.Format(" {0}0play {1}.mp3", Symbols.Color, file) : string.Empty;
        }

        /// <summary>
        /// Заполнение таблицы вопросов раунда (ТелеСИ)
        /// </summary>
        private void SetQuestTable()
        {
            gameConfig.QuestTable = new int[CurrentRound.Themes.Count][];
            gameConfig.HiddenThemes = new bool?[CurrentRound.Themes.Count];

            int i = 0;
            foreach (var theme in CurrentRound.Themes)
            {
                var name = theme.Name.TrimStart();
                if (name.Length > 0 && name[0] == '*')
                    gameConfig.HiddenThemes[i] = true;
                int j = 0;
                if (CurrentRound.Type == RoundTypes.Standart)
                {
                    gameConfig.QuestTable[i] = new int[theme.Questions.Count];
                    theme.Questions.ForEach(quest =>
                    {
                        gameConfig.QuestTable[i][j] = quest.Price;
                        j++;
                    });
                }
                else
                {
                    gameConfig.QuestTable[i] = new int[1];
                    gameConfig.QuestTable[i][0] = 1;
                }
                i++;
            }
        }

        /// <summary>
        /// Вывод таблицы вопросов/списка тем на экран
        /// </summary>
        private void PrintTable()
        {
            int i = 0;
            var text = new StringBuilder();
            foreach (var theme in CurrentRound.Themes)
            {
                bool print = false;
                if (CurrentRound.Type == RoundTypes.Standart)
                {
                    for (int j = 0; j < gameConfig.QuestTable[i].Length; j++)
                        if (gameConfig.QuestTable[i][j] > -1)
                        {
                            print = true;
                            break;
                        }
                }
                else 
                    print = gameConfig.QuestTable[i][0] > 0;
                if (print)
                {
                    text.Append(String.Format("{0}{1}0,2(", Symbols.Bold, Symbols.Color));
                    text.Append(i + 1);
                    text.Append(") ");
                    var themeName = theme.Name;
                    if (this.gameConfig.HiddenThemes[i].HasValue)
                    {
                        themeName = this.gameConfig.HiddenThemes[i].Value ? "ПОКА СЕКРЕТ" : theme.Name.TrimStart().Substring(1);
                    }

                    text.Append(themeName);
                    text.Append(String.Format("{0}2", Symbols.Color));
                    for (int k = themeName.Length; k < 40; k++)
                    {                        
                        text.Append(".");
                    }
                    if (CurrentRound.Type == RoundTypes.Standart)
                    {
                        text.Append(String.Format("{0}0", Symbols.Color));
                        for (int j = 0; j < gameConfig.QuestTable[i].Length; j++)
                        {
                            text.Append(' ');
                            string toPrint = gameConfig.QuestTable[i][j] > -1 ? gameConfig.QuestTable[i][j].ToString() : String.Format("{0}2....", Symbols.Color);
                            if (toPrint.Length < 4)
                            {
                                text.Append(String.Format("{0}2", Symbols.Color));
                                for (int k = 0; k < 4 - toPrint.Length; k++)
                                {
                                    text.Append(".");
                                }                                
                            }
                            text.Append(String.Format("{0}00", Symbols.Color));
                            text.Append(toPrint);
                        }
                    }
                    text.Append(Environment.NewLine);
                }
                i++;
            }
            Say(text.ToString());
        }

        private void End()
        {
            timer1.Stop();
            timer2.Stop();
            Say("Игра окончена, спасибо за игру!");
        }

        private void ShowQuest(string text)
        {
            if ((int)gameConfig.WordsInString == 0)
            {
                // Вывод построчно
                gameConfig.Phrases = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                var words = text.Split(new char[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var phrases = new List<string>();
                for (int i = 0; i < words.Length; i += (int)gameConfig.WordsInString)
                {
                    var phrase = new StringBuilder();
                    for (int j = 0; i + j < words.Length && j < (int)gameConfig.WordsInString; j++)
                    {
                        if (phrase.Length > 0)
                            phrase.Append(' ');
                        phrase.Append(words[i + j]);
                    }
                    phrases.Add(phrase.ToString());
                }
                gameConfig.Phrases = phrases.ToArray();
            }

            gameConfig.PhraseIndex = 0;
            gameConfig.Print = true;
            gameConfig.Fast = false;

            timer2_Tick(timer2, EventArgs.Empty);

            if (gameConfig.StringsInterval > 0)
            {
                timer2.Interval = (int)gameConfig.StringsInterval;
                timer2.Start();
            }
        }

        private void StartTimer(int time)
        {
            gameConfig.Time = time;
            timer1.Start();
            commandPanel.TimerValue = 0;
            commandPanel.TimerMaximum = time;
        }

        /// <summary>
        /// Сказать (выполнить) на канале
        /// </summary>
        /// <param name="text">Текст</param>
        void Say(string text)
        {
            var lines = text.Split(new string[] { Symbols.StringSeparator }, StringSplitOptions.None);
            var result = new StringBuilder();
            Array.ForEach(lines, line => result.AppendLine(ColorFormat(line, (int)gameConfig.ColorNum)));

            window.SendMessage(result.ToString());
        }

        /// <summary>
        /// Сказать на канале
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colorIndex"></param>
        string ColorFormat(string text, int colorIndex)
        {
            if (text.Length == 0) return string.Empty;
            return Symbols.Color + ((Char.IsDigit(text[0]) && colorIndex <= 10) ? "0" : "") + colorIndex + text;
        }

        /// <summary>
        /// Сказать на канале
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colorIndex"></param>
        string ColorFormat(string text, int colorIndex, int backColorIndex)
        {
            return Symbols.Color + colorIndex + "," + ((Char.IsDigit(text[0]) && backColorIndex <= 1) ? "0" : "") + backColorIndex + text;
        }

        internal void tSBReady_Click(object sender, EventArgs e)
        {
            Say("ГОТОВЫ?");
        }

        internal void tSMUScore_Click(object sender, EventArgs e)
        {
            ShowScore();
        }

        private void ShowScore()
        {
            var result = new StringBuilder();
            result.AppendLine(ColorFormat("Текущие результаты:", 5));
            foreach (Player player in gameConfig.Players)
            {
                result.AppendLine(ColorFormat(string.Format("{0}: {1}", player.Name, player.Sum), 5));
            }
            Say(result.ToString());
        }

        internal void tSMUStat_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(ColorFormat("Текущая статистика:", 5));
            foreach (Player player in gameConfig.Players)
            {
                result.AppendLine(ColorFormat(string.Format("{0}: {1}/{2}", player.Name, player.Right, player.Wrong), 5));
            }
            Say(result.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int time = (int)this.gameConfig.Time - 1;
                this.gameConfig.Time = time;
                if (time >= 0 && time <= this.commandPanel.TimerMaximum)
                    this.commandPanel.TimerValue = this.commandPanel.TimerMaximum - time;

                if (time < 4 || CurrentRound.Type == RoundTypes.Final && (time == 20 || time == 10))
                {
                    StringBuilder text = new StringBuilder();
                    text.AppendLine(time.ToString() + (time == 0 ? PlayMusic("si_noanswer") : string.Empty));
                    if (time == 0)
                    {
                        timer1.Stop();
                        switch (gameConfig.CurrentStage)
                        {
                            case GameConfiguration.SIStage.WaitSignal:
                                if (gameConfig.SignalsOnlyOnTimer)
                                    gameConfig.CurrentStage = GameConfiguration.SIStage.QRight;
                                break;

                            case GameConfiguration.SIStage.WaitFinal:
                                gameConfig.CurrentStage = GameConfiguration.SIStage.QRight;
                                Echo(String.Format("[Правильный ответ: {0}]", string.Join(", ", CurrentQuest.Right.ToArray())));
                                goto default;

                            default:
                                text.AppendLine("ВРЕМЯ ВЫШЛО!");
                                break;
                        }
                    }
                    Say(text.ToString());
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void Announce(string text)
        {
            Say(ColorFormat(text, 4));
        }

        private void Echo(string text)
        {
            window.Echo(ColorFormat(text, 73));
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            int sum = 0;
            switch (this.dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "Name":
                    if (gameConfig.Players[e.RowIndex].Name != e.Value.ToString())
                        Announce(string.Format("Действие: переименован игрок {0}. Новое имя: {1}", gameConfig.Players[e.RowIndex].Name, e.Value));
                    break;

                case "Sum":
                    if (int.TryParse(e.Value.ToString(), out sum) && gameConfig.Players[e.RowIndex].Sum != sum)
                    {
                        Announce(string.Format("Действие: изменена сумма на счёте игрока {0}. Теперь она равна {1}", gameConfig.Players[e.RowIndex].Name, e.Value));
                    }
                    break;

                case "Right":
                    if (int.TryParse(e.Value.ToString(), out sum) && gameConfig.Players[e.RowIndex].Right != sum)
                        Announce(string.Format("Действие: изменено число верных ответов у игрока {0}. Теперь оно равно {1}", gameConfig.Players[e.RowIndex].Name, e.Value));
                    break;

                case "Wrong":
                    if (int.TryParse(e.Value.ToString(), out sum) && gameConfig.Players[e.RowIndex].Wrong != sum)
                        Announce(string.Format("Действие: изменено число неверных ответов у игрока {0}. Теперь оно равно {1}", gameConfig.Players[e.RowIndex].Name, e.Value));
                    break;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.ReadOnly = true;
            ShowChooser();
        }

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.BeginEdit(false);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!gameConfig.Print)
                    return;
                int index = (int)gameConfig.PhraseIndex;
                /*if (gameConfig.Fast)
                {
                    var phrase = new StringBuilder();
                    for (int i = index; i < gameConfig.Phrases.Length; i++)
                    {
                        if (phrase.Length > 0)
                            phrase.Append(' ');
                        phrase.Append(gameConfig.Phrases[i]);
                    }
                    Say(phrase.ToString());
                    timer2.Stop();
                    return;
                }*/
                var text = new StringBuilder();
                if (index < gameConfig.Phrases.Length)
                    text.AppendLine(gameConfig.Phrases[index]);

                index++;
                gameConfig.PhraseIndex = index;
                if (index >= gameConfig.Phrases.Length)
                {
                    //text.AppendLine("[*]");
                    timer2.Stop();

                    gameConfig.CurrentStage = GameConfiguration.SIStage.ShowAtom;
                    gameConfig.Print = false;
                    Futher(text);

                    //gameConfig.CurrentStage = GameConfiguration.SIStage.QShown;
                    //gameConfig.Print = false;
                    //Futher();
                }
                else
                {
                    Say(text.ToString());
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void tsmiAdd_Click(object sender, EventArgs e)
        {
            string name = this.tSTBActionParam.Text;
            if (name.Length == 0)
            {
                MessageBox.Show("Введите имя игрока!");
                return;
            }
            AddPlayer(new Player(name));
        }

        private void tSMIRemove_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                this.dataGridView1.BeginInvoke(new EventHandler(tSMIRemove_Click), sender, e);
                return;
            }

            List<Player> resList = new List<Player>();
            for (int i = 0; i < gameConfig.Players.Count; i++)
            {
                bool found = false;
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.Index == i)
                    {
                        found = true;
                        Announce(String.Format("Действие: удалён игрок {0}", gameConfig.Players[i].Name));
                        break;
                    }
                }
                if (!found)
                    resList.Add(gameConfig.Players[i]);
            }
            gameConfig.Players.Clear();
            gameConfig.Players.AddRange(resList);
            sourcePlayers.ResetBindings(false);
            ShowChooser();
        }

        private void ShowChooser()
        {
            int chooser = gameConfig.Chooser;
            if (chooser != -1 && chooser < dataGridView1.Rows.Count && dataGridView1.Rows[chooser].Cells.Count > 0)
            {
                dataGridView1.Rows[chooser].Cells[0].Style.ForeColor = Color.Red;
                dataGridView1.Rows[chooser].Cells[0].Style.SelectionForeColor = Color.Red;
            }
        }

        private void очиститьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                this.dataGridView1.BeginInvoke(new EventHandler(очиститьToolStripMenuItem1_Click), sender, e);
                return;
            }

            gameConfig.Players.Clear();
            sourcePlayers.ResetBindings(false);
            Announce("Действие: очищен список всех игроков");
        }

        private void прибавитьСуммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tSTBActionParam.Text.Length == 0)
            {
                Echo("Введите сумму в текстовом поле!");
                return;
            }
            int sum = 0;
            if (!int.TryParse(this.tSTBActionParam.Text, out sum))
            {
                Echo("Формат суммы неверен!");
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                gameConfig.Players[row.Index].Sum += sum;
                Announce(string.Format("Действие: на счёт игрока {0} зачислено {1}, и теперь у него {2}", gameConfig.Players[row.Index].Name, sum, gameConfig.Players[row.Index].Sum));
            }
            SortSums();
        }

        /// <summary>
        /// Сортировка игроков по суммам
        /// </summary>
        private void SortSums()
        {
            if (this.dataGridView1.InvokeRequired)
            {
                this.dataGridView1.BeginInvoke(new Action(SortSums));
                return;
            }

            gameConfig.Players.Sort((p1, p2) => p1.Sum.CompareTo(p2.Sum));
            sourcePlayers.ResetBindings(false);
            ShowChooser();
        }

        private void вычестьСуммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tSTBActionParam.Text.Length == 0)
            {
                Echo("Введите сумму в текстовом поле!");
                return;
            }
            int sum = 0;
            if (!int.TryParse(this.tSTBActionParam.Text, out sum))
            {
                Echo("Формат суммы неверен!");
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                gameConfig.Players[row.Index].Sum -= sum;
                Announce(string.Format("Действие: со счёт игрока {0} снято {1}, и теперь у него {2}", gameConfig.Players[row.Index].Name, sum, gameConfig.Players[row.Index].Sum));
            }
            SortSums();
        }

        private void обратитьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder message = new StringBuilder();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (message.Length > 0)
                    message.Append(", ");
                message.Append(gameConfig.Players[row.Index].Name);
            }
            Say(message.ToString());
        }

        private void назначитьОтвечающимToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            gameConfig.Person = dataGridView1.SelectedRows[0].Index;
            Futher();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Say(gameConfig.Players[e.RowIndex].Name);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Неверный формат данных для текущей ячейки, исправьте!");
        }

        private void nUDShowmanColor_ValueChanged(object sender, EventArgs e)
        {
            lColor.BackColor = this.main.ColorsTable[(int)nUDShowmanColor.Value];
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
                SortSums();
        }

        internal void AddPlayer(Player player)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                this.dataGridView1.BeginInvoke(new Action<Player>(AddPlayer), player);
                return;
            }

            Announce(String.Format("Действие: добавлен игрок {0}", player.Name));
            gameConfig.Players.Add(player);
            sourcePlayers.ResetBindings(false);
            SortSums();
        }

        private void tSMISetChooser_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
                Chooser = dataGridView1.SelectedRows[0].Index;
        }

        private void tSMIQRemove_Click(object sender, EventArgs e)
        {
            Regex r = null;
            if (CurrentRound.Type == RoundTypes.Standart)
                r = new Regex(@"(?<theme>\d+) (?<quest>\d+)");
            else
                r = new Regex(@"(?<theme>\d+)");
            Match m = r.Match(tSTBActionParam.Text);
            if (m.Success)
            {
                int theme = int.Parse(m.Groups["theme"].Value) - 1;
                int quest = 0;
                if (CurrentRound.Type == RoundTypes.Standart)
                    quest = int.Parse(m.Groups["quest"].Value);

                if (theme < gameConfig.QuestTable.Length)
                {
                    if (CurrentRound.Type == RoundTypes.Standart)
                    {
                        for (int j = 0; j < gameConfig.QuestTable[theme].Length; j++)
                        {
                            if (gameConfig.QuestTable[theme][j] == quest)
                            {
                                gameConfig.QuestTable[theme][j] = -1;
                                Echo("Удалено");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (gameConfig.QuestTable[theme][0] > 0)
                        {
                            gameConfig.QuestTable[theme][0] = -1;
                            Echo("Удалено");
                        }
                    }
                }
            }
            else
                MessageBox.Show("Введите в текстовое поле номер темы (и стоимость) удаляемого вопроса");
        }

        internal void SetAnswerer(string p)
        {
            int i = 0;
            foreach (Player player in gameConfig.Players)
            {
                if (player.Name == p)
                {
                    gameConfig.Person = i;
                    Futher();
                    return;
                }
                i++;
            }
            Player newPlayer = new Player(p);
            AddPlayer(newPlayer);
            gameConfig.Person = gameConfig.Players.IndexOf(newPlayer);
        }

        internal void SetChooser(string p)
        {
            if (gameConfig.GameType != GameConfiguration.GameTypes.TeleSI)
                return;
            int i = 0;
            foreach (Player player in gameConfig.Players)
            {
                if (player.Name == p)
                {
                    Chooser = i;
                    return;
                }
                i++;
            }
            Player newPlayer = new Player(p);
            AddPlayer(newPlayer);
            Chooser = gameConfig.Players.IndexOf(newPlayer);
        }

        internal void Call(string p)
        {
            Say(p);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowChooser();
            UpdateTimerList();
        }

        #region IVisual Members

        public void Deactivated()
        {
            
        }

        #endregion
    }
}
