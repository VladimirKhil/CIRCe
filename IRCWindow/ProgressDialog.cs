using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace IRCWindow
{
    /// <summary>
    /// Диалог, отображающий прогресс выполнения какой-либо операции
    /// </summary>
    public sealed class ProgressDialog: IDisposable
    {
        /// <summary>
        /// Объект progress dialog box
        /// </summary>
        private Guid CLSID_ProgressDialog = new Guid("F8383852-FCD3-11d1-A6B9-006097DF5BD4");

        /// <summary>
        /// Флаги используемого диалога
        /// </summary>
        [Flags]
        public enum ProgressDialogFlags: uint
        {
            /// <summary>
            /// Обычное поведение
            /// </summary>
            Normal = 0,
            /// <summary>
            /// Диалог будет модальным по отношению к заданному родительскому окну
            /// </summary>
            Modal = 1,
            /// <summary>
            /// Автоматически вычислять оставшее время и отображать его в строке 3.
            /// При установке этого флага функцию SetLine можно использовать лишь для установки значений в строках 1 и 2
            /// </summary>
            Autotime = 2,
            /// <summary>
            /// Не показывать текст "Осталось времени"
            /// </summary>
            NoTime = 4,
            /// <summary>
            /// Не отображать кнопку минимизации
            /// </summary>
            NoMinimize = 8,
            /// <summary>
            /// Не отображать полосу прогресса
            /// </summary>
            NoProgressBar = 16,
            /// <summary>
            /// Сделать полосу прогресса "бегущей"
            /// </summary>
            MarqueeProgress = 32,
            /// <summary>
            /// Vista и выше. Не отображать кнопку отмены операции
            /// </summary>
            NoCancel = 64
        }

        /// <summary>
        /// Действия с таймером
        /// </summary>
        public enum TimerActions: uint
        {
            /// <summary>
            /// Сбросить на 0
            /// </summary>
            Reset = 1,
            /// <summary>
            /// Vista и выше. Приостановить таймер
            /// </summary>
            Pause = 2,
            /// <summary>
            /// Vista и выше. Возобновить таймер
            /// </summary>
            Resume = 3
        }

        [ComImport, SuppressUnmanagedCodeSecurity,
        Guid("EBBC7C04-315E-11d2-B62F-006097DF5BD4"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IProgressDialog
        {
            [PreserveSig]
            int StartProgressDialog([In] IntPtr hwndParent, [In, MarshalAs(UnmanagedType.IUnknown)] object punkEnableModless, [In] ProgressDialogFlags dwFlags, [In] IntPtr pvReserved);

            [PreserveSig]
            int StopProgressDialog();

            [PreserveSig]
            int SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pwzTitle);

            [PreserveSig]
            int SetAnimation(IntPtr hInstAnimation, uint idAnimation);

            [PreserveSig]
            [return: MarshalAs(UnmanagedType.Bool)]
            bool HasUserCancelled();

            [PreserveSig]
            int SetProgress(uint dwCompleted, uint dwTotal);

            [PreserveSig]
            int SetProgress64(ulong ullCompleted, ulong ullTotal);

            [PreserveSig]
            int SetLine(uint dwLineNum, [MarshalAs(UnmanagedType.LPWStr)] string pwzString, [MarshalAs(UnmanagedType.Bool)] bool fCompactPath, IntPtr pvReserved);

            [PreserveSig]
            int SetCancelMsg([MarshalAs(UnmanagedType.LPWStr)] string pwzCancelMsg, IntPtr pvReserved);

            [PreserveSig]
            int Timer(TimerActions dwTimerAction, IntPtr pvReserved);
        }

        private IProgressDialog progressDialog = null;

        /// <summary>
        /// Устанавливает заголовок диалога
        /// </summary>
        public string Title
        {
            set
            {
                this.progressDialog.SetTitle(value);
            }
        }

        /// <summary>
        /// Устанавливает сообщение, отображаемое при отмене операции
        /// </summary>
        public string CancelMessage
        {
            set { this.progressDialog.SetCancelMsg(value, IntPtr.Zero); }
        }

        /// <summary>
        /// Нажал ли пользователь кнопку "Отмена"
        /// </summary>
        public bool HasUserCancelled
        {
            get { return this.progressDialog.HasUserCancelled(); }
        }

        /// <summary>
        /// Устанавливает ход выполнения таймера
        /// </summary>
        public TimerActions Timer
        {
            set { this.progressDialog.Timer(value, IntPtr.Zero); }
        }
        
        public ProgressDialog()
        {
            this.progressDialog = Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_ProgressDialog)) as IProgressDialog;
        }

        /// <summary>
        /// Показать диалог прогресса
        /// </summary>
        public void Start()
        {
            this.progressDialog.StartProgressDialog(IntPtr.Zero, null, ProgressDialogFlags.Normal, IntPtr.Zero);
        }

        /// <summary>
        /// Показать диалог прогресса
        /// </summary>
        /// <param name="flags">Флаги диалога</param>
        public void Start(ProgressDialogFlags flags)
        {
            this.progressDialog.StartProgressDialog(IntPtr.Zero, null, flags, IntPtr.Zero);
        }

        /// <summary>
        /// Показать диалог прогресса
        /// </summary>
        /// <param name="parent">Окно-владелец</param>
        public void Start(IWin32Window parent)
        {
            this.progressDialog.StartProgressDialog(parent.Handle, null, ProgressDialogFlags.Normal, IntPtr.Zero);
        }

        /// <summary>
        /// Показать диалог прогресса
        /// </summary>
        /// <param name="parent">Окно-владелец</param>
        /// <param name="flags">Флаги диалога</param>
        public void Start(IWin32Window parent, ProgressDialogFlags flags)
        {
            this.progressDialog.StartProgressDialog(parent.Handle, null, flags, IntPtr.Zero);
        }

        /// <summary>
        /// Скрыть диалог прогресса
        /// </summary>
        public void Stop()
        {
            this.progressDialog.StopProgressDialog();
        }

        /// <summary>
        /// Установить прогресс выполнения в диалоге
        /// </summary>
        /// <param name="completed">Сколько выполнено</param>
        /// <param name="total">Сколько всего надо выполнить</param>
        public void SetProgress(uint completed, uint total)
        {
            this.progressDialog.SetProgress(completed, total);
        }

        /// <summary>
        /// Установить прогресс выполнения в диалоге
        /// </summary>
        /// <param name="completed">Сколько выполнено</param>
        /// <param name="total">Сколько всего надо выполнить</param>
        public void SetProgress(ulong completed, ulong total)
        {
            this.progressDialog.SetProgress64(completed, total);
        }

        /// <summary>
        /// Установить текст в одной из строк диалога
        /// </summary>
        /// <param name="lineNumber">Номер строки от 1 до 3</param>
        /// <param name="text">Текст строки</param>
        /// <param name="compact">Использовать ли компактную версию пути в строке, если текст окажется слишком длинным</param>
        public void SetLine(uint lineNumber, string text, bool compact)
        {
            this.progressDialog.SetLine(lineNumber, text, compact, IntPtr.Zero);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.progressDialog != null)
            {
                Marshal.ReleaseComObject(this.progressDialog);
                this.progressDialog = null;
            }
        }

        #endregion
    }
}
