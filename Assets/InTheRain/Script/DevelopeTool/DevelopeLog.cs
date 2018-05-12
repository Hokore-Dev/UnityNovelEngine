using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DevelopeLog : MonoBehaviour
{
    static Text _txtLog;

    static public void SetTextLogUI(Text log)
    {
        _txtLog = log;
    }

    static public void ClearLog()
    {
        if (_txtLog == null)
        {
            if (SceneManager.GetActiveScene().name == "DelopeTool")
                Debug.LogError("[DevelopeLog] Text is NULL!");
            return;
        }
        _txtLog.text = string.Empty;
    }

    static public void LogSystem(string log)
    {
        if (_txtLog == null)
        {
            if (SceneManager.GetActiveScene().name == "DelopeTool")
                Debug.LogError("[DevelopeLog] Text is NULL!");
            return;
        }
        _txtLog.text += StringHelper.Format("\n<color=#DBFF00FF>{0}</color>", log);
    }

    static public void LogError(string log)
    {
        if (_txtLog == null)
        {
            if (SceneManager.GetActiveScene().name == "DelopeTool")
                Debug.LogError("[DevelopeLog] Text is NULL!");
            return;
        }
        _txtLog.text += StringHelper.Format("\n<color=#FF0000FF>{0}</color>",log);
    }

    static public void Log(string log)
    {
        if (_txtLog == null)
        {
            if (SceneManager.GetActiveScene().name == "DelopeTool")
                Debug.LogError("[DevelopeLog] Text is NULL!");
            return;
        }
        _txtLog.text += StringHelper.Format("\n<color=#FFFFFFFF>{0}</color>",log);
    }
}
