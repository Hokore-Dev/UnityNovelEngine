using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System.Collections;

public class DevelopeTool : MonoBehaviour
{
    [SerializeField]
    public GameEngine gameEngine;

    [SerializeField]
    private Text _txtLog;

    [SerializeField]
    private Dropdown _dropDownPlayMode;

    [SerializeField]
    private Image _fadebox;

    [SerializeField]
    public TabSourceEditor _tabSourceEditor;

    [SerializeField]
    public TabSettings _tabSettings;

    [SerializeField]
    public InputField scriptInputField;

    [SerializeField]
    private GameObject[] _tabs;

    [SerializeField]
    private Toggle[] _toggles;

    public VNEngine.Parser parser = new VNEngine.EpisodeParser();
    private GameDataManager _dataManager = GameDataManager.getInstance;

    public void Awake()
    {
        Screen.SetResolution(1805, 720, false);
        DevelopeLog.SetTextLogUI(_txtLog);

        _fadebox.gameObject.SetActive(true);
        LeanTween.alphaCanvas(_fadebox.GetComponent<CanvasGroup>(), 0, 1.0f)
            .setOnComplete(() => 
        {
            _fadebox.gameObject.SetActive(false);
        });
    }

    public void OnToggle(int selectIdx)
    {
        for (int i = 0; i  < _toggles.Length;i++)
        {
            _tabs[i].SetActive(i == selectIdx);
        }
    }

    public void OnDropDownCheck()
    {
        GameObject list = _dropDownPlayMode.transform.Find("Dropdown List").gameObject;
        if (list != null)
        {
            list.GetComponent<Canvas>().sortingLayerName = "DevelopeTool";
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnToggle(0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            OnToggle(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            OnToggle(2);
        }

        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.B))
        {
            _tabSourceEditor.OnBuild();
        }
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.H))
        {
            _tabSettings.OnShowLog();
        }
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.P))
        {
            if (_tabSourceEditor.isPlay)
            {
                _tabSourceEditor.OnStop();
            }
            else
            {
                _tabSourceEditor.OnPlay();
            }
        }
    }
}
