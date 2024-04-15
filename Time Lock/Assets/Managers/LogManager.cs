using UnityEngine;
using System;
using System.IO;

Public class LogManager : Singleton<LogManager>
{
    private const string LOG_FILE_PATH = "log.txt";

    public event Eventhandler<string> OnGameStarted;

    public void Log(string logMessage)
    {
        string _dateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Initialize();

        file.AppendAllText(LOG_FILE_PATH, _dateTime + " - " + logMessage + "\n");
    }

    private void Initialize()
    {
        if (!File.Exists(LOG_FILE_PATH))
        {
            File.WriteAllText(LOG_FILE_PATH, "Log file created at " + _dateTime + "\n\n");
        }
    }


}