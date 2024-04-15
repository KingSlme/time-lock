using UnityEngine;
using System.IO;

public class LogManager : Singleton<LogManager>
{
    private string _log_file_path = Application.dataPath + "/log.txt";

    private string _dateTime;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        if (!File.Exists(_log_file_path))
        {
            UpdateDate();
            File.WriteAllText(_log_file_path, $"Log file created at ${_dateTime}\n\n");
        }
    }

    private void UpdateDate()
    {
        _dateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void Log(string logMessage)
    {
        UpdateDate();
        File.AppendAllText(_log_file_path, $"{_dateTime} - {logMessage}\n");
    }
}