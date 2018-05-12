using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabSettings : MonoBehaviour
{
    [SerializeField]
    private DevelopeTool _developeTool;

    [SerializeField]
    private InputField _inputFieldScript;

    [SerializeField]
    private InputField _txtFileName;

    [SerializeField]
    private Text _txtShowLogButton;

    [SerializeField]
    private GameObject _logBox;

    private GameDataManager _dataManager = GameDataManager.getInstance;
    private bool drawGUI = false;
    private FileBrowser fileBroswer = null;
    public bool isShowLog = true;

    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;

    private void Awake()
    {
        InitFileBroswer();
    }

    void InitFileBroswer()
    {
#if UNITY_EDITOR
        fileBroswer = new FileBrowser(Application.dataPath + "/Resources/Data", 1);
#else
        fileBroswer = new FileBrowser(1);
#endif
        //fileBroswer.guiSkin = skins[0];
        fileBroswer.guiSkin = skins[0];

        fileBroswer.fileTexture = file;
        fileBroswer.directoryTexture = folder;
        fileBroswer.backTexture = back;
        fileBroswer.driveTexture = drive;
        //show the search bar
        fileBroswer.showSearch = true;
        //search recursively (setting recursive search may cause a long delay)
        fileBroswer.searchRecursively = false;
    }

    public void OnOpenFile()
    {
        if (drawGUI)
            return;
        drawGUI = true;
    }

    public void OnOpenResourceFolder()
    {
        System.Diagnostics.Process.Start(Application.dataPath + @"\Resources");

    }

    public void  OnClearLog()
    {
        DevelopeLog.ClearLog();
    }

    public void OnShowLog()
    {
        isShowLog = !isShowLog;
        _txtShowLogButton.text = (isShowLog) ? "Hide Log" : "Show Log";
        LeanTween.moveLocalX(_logBox, (isShowLog == true) ? 490 : 880, 0.5f).setEase(LeanTweenType.easeInSine);
    }

    public void OnSaveFile()
    {
        string savePath = Application.dataPath + "/Resources/Data/Episode/" + _txtFileName.text;
        System.IO.File.WriteAllText(savePath, _inputFieldScript.text);
        DevelopeLog.LogSystem(savePath + "에 저장 완료되었습니다.");
    }

    void OnGUI()
    {
        if (!drawGUI)
            return;

        if (fileBroswer.draw())
        {
            if (fileBroswer.outputFile == null)
            {
                drawGUI = false;
            }
            else
            {
                if (fileBroswer.outputFile.FullName.Contains(".txt"))
                {
                    DevelopeLog.LogSystem(fileBroswer.outputFile.FullName + " 로드 완료되었습니다.");

                    drawGUI = false;

                    _inputFieldScript.text = System.IO.File.ReadAllText(fileBroswer.outputFile.FullName);

                    string[] splitArray = fileBroswer.outputFile.FullName.Split('\\');
                    _txtFileName.text = splitArray[splitArray.Length - 1];
                    _developeTool.OnToggle(0);
                }
            }
        }
    }
}