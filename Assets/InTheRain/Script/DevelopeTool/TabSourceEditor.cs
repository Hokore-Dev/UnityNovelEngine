using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TabSourceEditor : MonoBehaviour
{
    [SerializeField]
    private DevelopeTool _developeTool;

    [SerializeField]
    public Text _inputFieldScript;

    [SerializeField]
    private Text _script;

    [SerializeField]
    private InputField _input;

    [SerializeField]
    private string[] _systemHighLight;

    [SerializeField]
    private string[] _typeHighLight;

    [SerializeField]
    private string[] _optionHighLight;

    [HideInInspector]
    public bool isPlay = false;

    private GameDataManager _dataManager = GameDataManager.getInstance;

    int save = 0;
    int check = 0;

    public void TEST()
    {
        //_developeTool.scriptInputField.ActivateInputField();
        //_developeTool.scriptInputField.Select();

        //Debug.Log("caretPosition " + _developeTool.scriptInputField.caretPosition);
        //Debug.Log("anchorPosition " + _developeTool.scriptInputField.selectionAnchorPosition);
        //Debug.Log("focusPosition " + _developeTool.scriptInputField.selectionFocusPosition);

        if (check == 0)
        {
            if (_developeTool.scriptInputField.caretPosition < _developeTool.scriptInputField.text.Length)
            {
                _developeTool.scriptInputField.caretPosition = _developeTool.scriptInputField.caretPosition - 2;
                _developeTool.scriptInputField.selectionFocusPosition = _developeTool.scriptInputField.selectionFocusPosition - 1;
                _developeTool.scriptInputField.ForceLabelUpdate();

                //string asdd = Input.compositionString;
                //Invoke("INITTT", 1);
                check = 1;
            }
        }
    }

    private void PPAP()
    {
        _developeTool.scriptInputField.text = _developeTool.scriptInputField.text.Replace("*", "");
    }

    private void Start()
    {
        
    }

    public void Update()
    {
        _inputFieldScript.text = _inputFieldScript.text + Input.compositionString;

        string text = _inputFieldScript.text;
        for (int i = 0; i < _typeHighLight.Length; i++)
        {
            text = text.Replace(_typeHighLight[i], StringHelper.Format("<color=#1A5D11FF>{0}</color>", _typeHighLight[i]));
        }
        for (int i = 0;i< _systemHighLight.Length;i++)
        {
            text = text.Replace(_systemHighLight[i], StringHelper.Format("<color=#DBFF00FF>{0}</color>", _systemHighLight[i]));
        }
        for (int i = 0; i < _optionHighLight.Length; i++)
        {
            text = text.Replace(_optionHighLight[i], StringHelper.Format("<color=#9D90FFFF>{0}</color>", _optionHighLight[i]));
        }
        text= text.Replace("*", "");
        text = text.Replace("@", "<color=#FFFFFFFFF>@</color>");
        string[] splitValue = text.Split('\n');

        System.Text.StringBuilder applyText = new System.Text.StringBuilder();
        for (int i = 0; i < splitValue.Length; i++)
        {
            if (splitValue[i].Length > 0)
            {
                if (splitValue[i][0] == '[')
                {
                    splitValue[i] = StringHelper.Format("<color=#2F31F9FF>{0}</color>", splitValue[i]);
                }
                else if (splitValue[i].Contains("@"))
                {
                    splitValue[i] = splitValue[i].Replace("<color=#FFFFFFFFF>", "");
                    splitValue[i] = splitValue[i].Replace("<color=#1A5D11FF>", "");
                    splitValue[i] = splitValue[i].Replace("<color=#9D90FFFF>", "");
                    splitValue[i] = splitValue[i].Replace("</color>", "");
                }
                else if (splitValue[i].Length >= 2 && StringHelper.Format("{0}{1}", splitValue[i][0], splitValue[i][1]) == "--")
                {
                    splitValue[i] = splitValue[i].Replace("<color=#1A5D11FF>", "");
                    splitValue[i] = splitValue[i].Replace("<color=#9D90FFFF>", "");
                    splitValue[i] = splitValue[i].Replace("</color>", "");
                    splitValue[i] = StringHelper.Format("<color=#A61818FF>{0}</color>", splitValue[i]);
                }
                else if (splitValue[i][0] == '-')
                {
                    splitValue[i] = StringHelper.Format("<color=#9D90FFFF>{0}</color>", splitValue[i]);
                }
            }
            splitValue[i] = StringHelper.Format("{0}\n", splitValue[i]);
            applyText.Append(splitValue[i]);
        }
        _script.text = applyText.ToString();
    }

    public void OnPlay()
    {
        isPlay = !isPlay;
        if (_dataManager._behaviorList.Count == 0)
        {
            DevelopeLog.LogError("빌드를 먼저 해주세요!");
        }
        else
        {
            DevelopeLog.ClearLog();
            DevelopeLog.LogSystem("============= Game Start =============");
            _developeTool.gameEngine.StartActionBehavior();
        }
    }

    public void OnStop()
    {
        _developeTool.gameEngine.StopActionBehavior();
        _dataManager.Init();
        DevelopeLog.LogSystem("============= Game Stop =============");
    }

    public void OnBuild()
    {
        _dataManager.Init();
        DevelopeLog.ClearLog();
        DevelopeLog.LogSystem("============= Build Start =============");
        _developeTool.parser.BuildScript(_developeTool.scriptInputField.text);
        DevelopeLog.LogSystem("====================================");
        if (!_developeTool._tabSettings.isShowLog)
        {
            _developeTool._tabSettings.OnShowLog();
        }
    }
}
