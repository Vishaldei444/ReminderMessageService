# Reminder Message Service

A C# Windows Service that writes one reminder to a log every **5 minutes**. After writing the last message, it starts again from the first.

**Built with:** C# · .NET Framework 4.8 · Visual Studio 2022

## Reminder Cycle

| Time after service starts | Message |
| --- | --- |
| 5 minutes | Drink water |
| 10 minutes | Take a short break |
| 15 minutes | Check your tasks |
| 20 minutes | Drink water |

## Output Screenshot

![Reminder Message Service log showing the repeating message cycle](./reminder-output.png)

## Build and Run

1. Open `ReminderMessageService.sln` in Visual Studio 2022.
2. Select **Release** and choose **Build → Build Solution**.
3. Open **Command Prompt** as administrator. Replace the example path with the full path to `ReminderMessageService.exe` from your project's `bin\Release` folder.

```cmd
sc.exe create ReminderMessageService binPath= "\"C:\Projects\ReminderMessageService\bin\Release\ReminderMessageService.exe\"" start= demand
sc.exe start ReminderMessageService
sc.exe query ReminderMessageService
```

The service status should show `RUNNING`.

## View the Log

The log file is created when the first reminder is written. Press **Win + R** and enter:

```text
notepad "%ProgramData%\ReminderMessageService\reminders.log"
```

Each entry includes the date, time, and reminder message. Restarting the service begins the cycle again and appends new entries to the existing log.

## Test the Cycle in 40 Seconds

Stop the service and temporarily change `TimeSpan.FromMinutes(5)` to `TimeSpan.FromSeconds(10)` in `Service1.cs`. Rebuild in **Release** mode and restart the service. After about 40 seconds, check that `Drink water` appears again after `Check your tasks`. Restore the five-minute interval for the final build.

## Stop the Service

```cmd
sc.exe stop ReminderMessageService
```
