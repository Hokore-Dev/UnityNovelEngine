using UnityEngine;
using System.Collections.Generic;

public class GameDataManager
{
    private static GameDataManager _instance = null;

    public static GameDataManager getInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameDataManager();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 선택지 시작 판별 
    /// </summary>
    /// <param name="distractorNumber">선택지 시작</param>
    /// <returns></returns>
    public int FindDisractorStartArea(int distractorNumber)
    {
        int result = -1;
        for (int i = 0; i < GameDataManager.getInstance._behaviorList.Count; i++)
        {
            BehaviorData data = GameDataManager.getInstance._behaviorList[i];
            if (data._read)
            {
                continue;
            }
            if (data.ContainForm("DISTRACTOR_START") &&
                data.distractorNumber.CompareTo(distractorNumber) == 0)
            {
                result = i;
                break;
            }
        }
        return result + 1;
    }

    /// <summary>
    /// 선택지 끝 판별
    /// </summary>
    /// <param name="distractorNumber">선택지 종료</param>
    /// <returns></returns>
    public int FindDisractorEndArea(int distractorNumber)
    {
        int result = -1;
        for (int i = 0; i < GameDataManager.getInstance._behaviorList.Count; i++)
        {
            BehaviorData data = GameDataManager.getInstance._behaviorList[i];
            if (data._read)
            {
                continue;
            }
            if (data.ContainForm( "DISTRACTOR_END" )&&
                data.distractorNumber.CompareTo(distractorNumber) == 0)
            {
                result = i;
                break;
            }
        }
        return result + 1;
    }

    /// <summary>
    /// 선택지 히스토리를 파싱한다
    /// </summary>
    /// <param name="history"></param>
    public void ParseDistractorHistory(string history)
    {
        string []split = history.Split(',');
        for (int i = 0; i < split.Length;i++)
        {
            distractorHistory.Add(int.Parse(split[i]));
        }
    }
    
    /// <summary>
    /// 선택지 리스트를 문자열로 변환한다
    /// </summary>
    /// <returns></returns>
    public string DistractorToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int i = 0; i < distractorHistory.Count;i++)
        {
            builder.Append(distractorHistory[i].ToString());
            if (i != (distractorHistory.Count - 1))
                builder.Append(",");
        }
        return builder.ToString();
    }

    public void Init()
    {
        _behaviorList.Clear();

        scriptPlayMode = EScriptPlayMode.Touch;         // 플레이 모드
        distractorHistory.Clear();                      // 선택지 초기화
        readDistractorHistory = 0;                      // 로드시 읽어온 선택지 갯수

        loadLine = -1;               // 로드하는 행동 라인
        readCount = 0;               // 현재 읽고 있는 행동
        followDistactor = -1;        // 따르고 있는 선택지
        behaviorDelayTime = 0;      // 행동 시간

        nowLoading = false;         // 로딩중 여부
        next = true;                // 다음 행동으로 넘어갈지 선택
        stopBehavior = false;       // 행동을 중단 (선택지)
        showChatBox = true;         // 대화창을 표시 중
    }

    public enum EScriptPlayMode
    {
        Auto,
        Load,
        Touch,
        Skip,
    }

    public List<BehaviorData>   _behaviorList        = new List<BehaviorData>();  // 행동 리스트 
    public List<CharacterData>  _characterDataList   = new List<CharacterData>(); // 캐릭터 리스트

    public EScriptPlayMode scriptPlayMode = EScriptPlayMode.Touch;      // 플레이 모드
    public List<int> distractorHistory = new List<int>();               // 선택지 히스토리
    readonly public int NONE_SELECT_DISTRACTOR = 10000;                 // 선택지를 선택하지 않을 때 분류 번호
    public int readDistractorHistory = 0;                               // 로드시 읽어온 선택지 갯수

    public int loadLine = -1;               // 로드하는 행동 라인
    public int readCount = 0;               // 현재 읽고 있는 행동
    public int followDistactor = -1;        // 따르고 있는 선택지
    public float behaviorDelayTime = 0;     // 행동 시간
    public string savePath = "SaveData_";   // 저장소 키

    public bool nowLoading = false;         // 로딩중 여부
    public bool next = true;                // 다음 행동으로 넘어갈지 선택
    public bool playBGM = false;            // 메인 메뉴에서 BGM에 실행중인지 판별
    public bool stopBehavior = false;       // 행동을 중단 (선택지)
    public bool showChatBox = true;         // 대화창을 표시 중

}