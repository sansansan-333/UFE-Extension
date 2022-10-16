using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DataLogger : ILogger {
    private const string format = "{0}";
    private const string tagFormat = "{0}: {1}";

    private static readonly ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;

    public ILogHandler logHandler { get; set; }
    public bool logEnabled { get; set; }
    public LogType filterLogType { get; set; }

    public DataLogger() {
        logHandler = defaultLogHandler;
        logEnabled = true;
        filterLogType = LogType.Log;
    }

    private static string GetString(object message) {
        if (message == null) {
            return "Null";
        }
        var formattable = message as IFormattable;
        if (formattable != null) {
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        }
        else {
            return message.ToString();
        }
    }

    private static string GetStringArray(Array array) {
        if (array.Length == 0) {
            return "[]";
        }

        StringBuilder sb = new StringBuilder();
        int[] indices = (int[])Array.CreateInstance(typeof(int), array.Rank);

        sb.Append(new string('[', array.Rank));

        for (int n = 0; n < array.Length; n++) {
            // carry up indices
            int carry = 0;
            for (int d = array.Rank - 1; d >= 0; d--) {
                if (indices[d] >= array.GetLength(d)) {
                    indices[d] = 0;
                    indices[d - 1]++;
                    carry++;
                }
            }

            if (carry > 0) {
                sb.Length--; // remove trailing comma
                sb.Append(new string(']', carry)).Append(",").Append(new string('[', carry));
            }

            if (array.GetValue(indices) is Array subarray) {
                sb.Append(GetStringArray(subarray));
            }
            else {
                sb.Append(array.GetValue(indices).ToString());
            }
            sb.Append(",");

            indices[array.Rank - 1]++;
        }

        sb.Length--; // remove trailing comma
        sb.Append(new string(']', array.Rank));

        return sb.ToString();
    }

    public bool IsLogTypeAllowed(LogType logType) {
        if (!logEnabled) return false;

        return (logType <= filterLogType);
    }

    public void Log(LogType logType, object message) {
        if (IsLogTypeAllowed(logType)) logHandler.LogFormat(logType, null, format, new object[] { GetString(message) });
    }

    public void Log(LogType logType, object message, UnityEngine.Object context) {
        if (IsLogTypeAllowed(logType)) logHandler.LogFormat(logType, context, format, new object[] { GetString(message) });
    }

    public void Log(LogType logType, string tag, object message) {
        if (IsLogTypeAllowed(logType)) logHandler.LogFormat(logType, null, tagFormat, new object[] { tag, GetString(message) });
    }

    public void Log(LogType logType, string tag, object message, UnityEngine.Object context) {
        if (IsLogTypeAllowed(logType)) logHandler.LogFormat(logType, context, tagFormat, new object[] { tag, GetString(message) });
    }

    public void Log(object message) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, null, format, new object[] { GetString(message) });
    }

    public void Log(string message) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, null, format, new object[] { message });
    }

    public void Log<T>(IEnumerable<T> message) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, null, format, new object[] { string.Join(", ", message) });
    }

    public void LogArray(Array array) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, null, format, new object[] { GetStringArray(array) });
    }

    public void Log(string tag, object message) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, null, tagFormat, new object[] { tag, GetString(message) });
    }

    public void Log(string tag, object message, UnityEngine.Object context) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, context, tagFormat, new object[] { tag, GetString(message) });
    }

    public void LogError(object message) {
        if (IsLogTypeAllowed(LogType.Error)) logHandler.LogFormat(LogType.Error, null, format, new object[] { GetString(message) });
    }

    public void LogError(string tag, object message) {
        if (IsLogTypeAllowed(LogType.Error)) logHandler.LogFormat(LogType.Error, null, tagFormat, new object[] { tag, GetString(message) });
    }

    public void LogError(string tag, object message, UnityEngine.Object context) {
        if (IsLogTypeAllowed(LogType.Error)) logHandler.LogFormat(LogType.Error, context, tagFormat, new object[] { tag, GetString(message) });
    }

    public void LogException(Exception exception) {
        if (IsLogTypeAllowed(LogType.Exception)) logHandler.LogException(exception, null);
    }

    public void LogException(Exception exception, UnityEngine.Object context) {
        if (IsLogTypeAllowed(LogType.Exception)) logHandler.LogException(exception, context);
    }

    public void LogFormat(LogType logType, string format, params object[] args) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, null, format, args);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) {
        if (IsLogTypeAllowed(LogType.Log)) logHandler.LogFormat(LogType.Log, context, format, args);
    }

    public void LogWarning(object message) {
        if (IsLogTypeAllowed(LogType.Warning)) logHandler.LogFormat(LogType.Warning, null, format, new object[] { GetString(message) });
    }

    public void LogWarning(string tag, object message) {
        if (IsLogTypeAllowed(LogType.Warning)) logHandler.LogFormat(LogType.Warning, null, tagFormat, new object[] { tag, GetString(message) });
    }

    public void LogWarning(string tag, object message, UnityEngine.Object context) {
        if (IsLogTypeAllowed(LogType.Warning)) logHandler.LogFormat(LogType.Warning, context, tagFormat, new object[] { tag, GetString(message) });
    }
}