using UnityEngine;
using UnityEngine.UI; // Tilf�j denne linje

public class DebugLogViewer : MonoBehaviour
{
    public Text debugTextField; // �ndret fra TextMeshProUGUI til Text

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
