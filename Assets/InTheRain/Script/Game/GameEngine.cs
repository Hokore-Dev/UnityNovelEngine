using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameEngine : MonoBehaviour
{
    enum EParseMode
    {
        ScriptCompile,
        ScriptCompileAndPlay,
    }

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private EParseMode _parseMode = EParseMode.ScriptCompileAndPlay;

    [SerializeField]
    private GameDataManager.EScriptPlayMode _scriptPlayMode = GameDataManager.EScriptPlayMode.Touch;

    [SerializeField]
    private int _loadLine = 0;

    [SerializeField]
    private string _distractorHistory = string.Empty;

    [SerializeField]
    private PopupSave _popupSave;

    [SerializeField]
    private PopupLoad _popupLoad;

    [SerializeField]
    private GameObject[] _modeMessage;

    [Header("로드 스크립트")]
    [SerializeField]
    string[] _loadData = {
        "Data/Character/character",
        "Data/Episode/episode_1_0",
        "Data/Episode/episode_1_1",
        "Data/Episode/episode_1_2"
    };

    [Header("행동 매니저")]
    public VNEngine.Behavior[] _managerArray;

    private BehaviorData data;
    private GameDataManager _dataManager;

    // 리소스 데이터 집합
    [HideInInspector] public Background background;
    [HideInInspector] public Distractor distractor;
    [HideInInspector] public Character character;
    [HideInInspector] public Dialogue dialogue;
    [HideInInspector] public BGM bgm;
    [HideInInspector] public SE se;

    private void Awake()
    {
        _dataManager = GameDataManager.getInstance;
        if (_scriptPlayMode != GameDataManager.EScriptPlayMode.Touch)
        {
            _dataManager.loadLine = _loadLine;
            _dataManager.scriptPlayMode = _scriptPlayMode;
            _dataManager.ParseDistractorHistory(_distractorHistory);
        }

        // 스크립트 로드
        List<VNEngine.Parser> _parser = new List<VNEngine.Parser>();
        _parser.Add(new VNEngine.CharacterParser());
        _parser.Add(new VNEngine.EpisodeParser());
        _parser.Add(new VNEngine.EpisodeParser());
        _parser.Add(new VNEngine.EpisodeParser());

        for (int i = 0; i < _parser.Count; i++)
        {
            _parser[i].Init();
            _parser[i].LoadData(_loadData[i]);
        }

        // 데이터 집합 초기화
        dialogue = GetComponent<Dialogue>();
        distractor = GetComponent<Distractor>();
        background = this.transform.Find("GameResource").GetComponent<Background>();
        character = this.transform.Find("GameResource").GetComponent<Character>();
        bgm = this.transform.Find("GameResource").GetComponent<BGM>();
        se = this.transform.Find("GameResource").GetComponent<SE>();

        for (int i = 0; i < _managerArray.Length; i++)
        {
            _managerArray[i].Init();
        }
    }

    void Start()
    {
        Utils.SetResolution(_camera);

        //if (_parseMode == EParseMode.ScriptCompileAndPlay)
        //    StartCoroutine(Co_ActionBehavior());
    }

    public void StopActionBehavior()
    {
        ClearAllResource();
        StopAllCoroutines();
        dialogue.Clear();
        for (int i = 0; i < _managerArray.Length; i++)
        {
            _managerArray[i].Clear();
        }
    }

    public void StartActionBehavior()
    {
        StartCoroutine(Co_ActionBehavior());
    }

    public void ClearAllResource()
    {
        background.ClearResoure();
        character.ClearResoure();
        bgm.ClearResoure();
        se.ClearResoure();
    }

    public void OnSave()
    {
        _popupSave.gameObject.SetActive(true);
        _popupSave.Init(background.FindActiveResource().GetComponent<Image>());
    }

    public void OnLoad()
    {
        _popupLoad.gameObject.SetActive(true);
        _popupLoad.Init();
    }

    public void OnAuto()
    {
        if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
            return;
        for (int i = 0; i < _modeMessage.Length; i++)
        {
            _modeMessage[i].SetActive(false);
        }
        GameDataManager.getInstance.scriptPlayMode = (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Auto) ? GameDataManager.EScriptPlayMode.Touch : GameDataManager.EScriptPlayMode.Auto;
        if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Auto)
        {
            _modeMessage[0].SetActive(true);
        }
    }

    public void OnSkip()
    {
        if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
            return;
        for (int i = 0; i < _modeMessage.Length; i++)
        {
            _modeMessage[i].SetActive(false);
        }
        GameDataManager.getInstance.scriptPlayMode = (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Skip) ? GameDataManager.EScriptPlayMode.Touch : GameDataManager.EScriptPlayMode.Skip;
        if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Skip)
        {
            _modeMessage[1].SetActive(true);
        }
    }

    IEnumerator Co_ActionBehavior()
    {
        while (_dataManager.readCount < _dataManager._behaviorList.Count)
        {
            if (Input.GetMouseButton(0))
            {
                _dataManager.next = true;
            }
            if (_dataManager.next == false)
            {
                if (_dataManager.scriptPlayMode == GameDataManager.EScriptPlayMode.Skip ||
                    _dataManager.scriptPlayMode == GameDataManager.EScriptPlayMode.Auto)
                {
                    _dataManager.next = true;
                }
            }

            if (_dataManager.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
            {
                if (GameDataManager.getInstance.readCount >= GameDataManager.getInstance.loadLine)
                {
                    _dataManager.scriptPlayMode = GameDataManager.EScriptPlayMode.Touch;
                }
            }
            if (_dataManager.stopBehavior || !_dataManager.next)
            {
                yield return null;
            }
            else
            {
                GameDataManager.getInstance.behaviorDelayTime = 0;
                data = GameDataManager.getInstance._behaviorList[_dataManager.readCount];
                data._read = true;
                _dataManager.readCount++;

                //if (_dataManager.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
                // {
                DevelopeLog.Log(StringHelper.Format("[{0}] Line {1} Skip", data.form, GameDataManager.getInstance.readCount));
                    Debug.Log(StringHelper.Format("[{0}] Line {1} Skip", data.form, GameDataManager.getInstance.readCount));
               // }

                bool find = false;
                int findBehaviorType = -1;
                for (int i = 0; i < _managerArray.Length; i++)
                {
                    find = _managerArray[i].ExcuteBehavior(data);
                    if (find)
                    {
                        findBehaviorType = i;
                        break;
                    }
                }


                if (findBehaviorType == 7)
                {
                    if (_dataManager.scriptPlayMode == GameDataManager.EScriptPlayMode.Touch && data.ContainForm("TALK"))
                    {
                        _dataManager.next = false;
                        yield return new WaitForSeconds(0.1f + GameDataManager.getInstance.behaviorDelayTime);
                    }
                    else
                    {
                        yield return new WaitForSeconds(GameDataManager.getInstance.behaviorDelayTime);
                    }
                }
                else
                    yield return new WaitForSeconds(GameDataManager.getInstance.behaviorDelayTime);
            }
        }
    }
}
