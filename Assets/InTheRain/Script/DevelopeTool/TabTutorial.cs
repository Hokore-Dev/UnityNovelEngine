using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabTutorial : MonoBehaviour
{
    [SerializeField]
    private InputField _script;

    [SerializeField]
    private DevelopeTool _developeTool;

    private string _tutorialText = string.Empty;

    public void OnLoadTutorial()
    {
        if (_tutorialText == string.Empty)
        {
            TextAsset file = Resources.Load("Data/ScriptTutorial") as TextAsset;
            if (file != null)
            {
                _tutorialText = file.text;
                _script.text = _tutorialText;
                _developeTool.OnToggle(0);
                DevelopeLog.LogSystem("튜토리얼이 로드 되었습니다.");
            }
            else
            {
                DevelopeLog.LogError("튜토리얼 파일을 찾을 수 없습니다!");
            }
        }
        else
        {
            _script.text = _tutorialText;
            _developeTool.OnToggle(0);
            DevelopeLog.LogSystem("튜토리얼이 로드 되었습니다.");
        }
    }

    public void OnOpenReference()
    {
        System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/1rGYalR8pxu8FL_feAqViwqJrxNcMXJeFPfsoKg5CW4U/edit?usp=sharing");
    }
}
