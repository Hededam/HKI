using UnityEngine;
using TMPro;

public class DebugLogViewer : MonoBehaviour
{
    public TMP_Text debugTextField;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logText, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            debugTextField.text += logText + "\n";
        }
    }
}
