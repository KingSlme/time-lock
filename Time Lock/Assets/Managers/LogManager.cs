using UnityEngine;
using System;
using System.IO;

public class LogManager : Singleton<LogManager>
{
    private const string LOG_FILE_PATH = "log.txt";

    public event EventHandler<string> OnGameStarted;

    private string _dateTime;

    private void GetDate()
    {
        _dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void Log(string logMessage)
    {
        Initialize();

        // file.AppendAllText(LOG_FILE_PATH, _dateTime + " - " + logMessage + "\n");
    }

    private void Initialize()
    {
        if (!File.Exists(LOG_FILE_PATH))
        {
            File.WriteAllText(LOG_FILE_PATH, "Log file created at " + _dateTime + "\n\n");
        }
    }


}