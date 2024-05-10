using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    public TMP_Text consoleOutput;
    public int maxLines = 20;

    private Queue<string> logQueue = new Queue<string>();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string newLogEntry = logString;

        if (type == LogType.Error || type == LogType.Exception)
        {
            // Here we attempt to parse the stack trace to get script name and line number
            string sourceInfo = ParseStackTrace(stackTrace);
            newLogEntry += "\n" + sourceInfo;
        }

        logQueue.Enqueue(newLogEntry);
        if (logQueue.Count > maxLines)
        {
            logQueue.Dequeue();
        }

        consoleOutput.text = string.Join("\n", logQueue.ToArray());
    }

    // This method tries to extract the script name and line number from the stack trace
    string ParseStackTrace(string stackTrace)
    {
        // Split the stack trace into lines
        string[] lines = stackTrace.Split('\n');
        if (lines.Length > 0)
        {
            // The line with the script name and line number typically follows this pattern:
            // at ClassName.MethodName (parameters) [0x00000] in <filename>:line <lineNumber>
            // We look for the first line containing ":line " to find the relevant trace
            foreach (string line in lines)
            {
                if (line.Contains(":line "))
                {
                    int pathIndex = line.LastIndexOf('\\') + 1;  // Handles file paths in Windows format
                    if (pathIndex == 0) // If not found, it's likely a UNIX-style path
                        pathIndex = line.LastIndexOf('/') + 1;
                    int lineIndex = line.IndexOf(":line ") + 6; // "+6" to skip the ":line " text itself

                    if (pathIndex != -1 && lineIndex != -1)
                    {
                        string filePath = line.Substring(pathIndex);
                        string lineNumber = line.Substring(lineIndex);
                        return filePath + " " + lineNumber;
                    }
                }
            }
        }
        return "Source unknown"; // Fallback in case parsing fails
    }
}
