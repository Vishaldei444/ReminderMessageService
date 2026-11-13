using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace ReminderMessageService
{
    public partial class Service1 : ServiceBase
    {
        private readonly string[] messages =
        {
            "Drink water",
            "Take a short break",
            "Check your tasks"
        };

        private readonly object timerLock = new object();
        private Timer reminderTimer;
        private string logFilePath;
        private int nextMessageIndex;

        public Service1()
        {
            InitializeComponent();
            ServiceName = "ReminderMessageService";
        }

        protected override void OnStart(string[] args)
        {
            lock (timerLock)
            {
                string logFolder = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.CommonApplicationData),
                    "ReminderMessageService");

                Directory.CreateDirectory(logFolder);
                logFilePath = Path.Combine(logFolder, "reminders.log");
                nextMessageIndex = 0;

                reminderTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
                reminderTimer.Elapsed += WriteNextMessage;
                reminderTimer.Start();
            }
        }

        private void WriteNextMessage(object sender, ElapsedEventArgs e)
        {
            lock (timerLock)
            {
                if (!ReferenceEquals(sender, reminderTimer))
                {
                    return;
                }

                string message = messages[nextMessageIndex];
                string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

                File.AppendAllText(logFilePath, logLine + Environment.NewLine);
                nextMessageIndex = (nextMessageIndex + 1) % messages.Length;
            }
        }

        protected override void OnStop()
        {
            lock (timerLock)
            {
                reminderTimer?.Dispose();
                reminderTimer = null;
            }
        }
    }
}