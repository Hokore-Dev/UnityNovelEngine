using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Distractor : MonoBehaviour {

    [SerializeField]
    GameObject _canvasDistactor;

    private List<Button> _distactorList = new List<Button>();

    /// <summary>
    /// 선택지를 생성한다
    /// </summary>
    /// <param name="text"></param>
    public void CreateDistractor(string text, float time)
    {
        CancelInvoke();

        var obj = Resources.Load("Prefab/Distractor");
        var gameObj = MonoBehaviour.Instantiate(obj) as GameObject;
        gameObj.SetActive(true);
        gameObj.transform.SetParent(_canvasDistactor.transform);

        int count = _distactorList.Count;
        Button button = gameObj.GetComponent<Button>();
        button.transform.Find("Text").GetComponent<Text>().text = text;
        button.onClick.AddListener(() => { OnClick(count); });
        _distactorList.Add(button);

        if (int.Parse(time.ToString()) != 0)
        {
            Invoke("NoneSelectDistactor", time);
        }
    }

    /// <summary>
    /// 선택지를 고르지 않았을 때
    /// </summary>
    public void NoneSelectDistactor()
    {
        VanishDistractorButton();
        GameDataManager.getInstance.distractorHistory.Add(GameDataManager.getInstance.NONE_SELECT_DISTRACTOR);
        GameDataManager.getInstance.followDistactor = GameDataManager.getInstance.NONE_SELECT_DISTRACTOR;
        GameDataManager.getInstance.readCount = GameDataManager.getInstance.FindDisractorStartArea(GameDataManager.getInstance.NONE_SELECT_DISTRACTOR);
        GameDataManager.getInstance.stopBehavior = false;
    }

    /// <summary>
    /// 리스트 위치를 정렬한다
    /// </summary>
    public void RefreshDistractorPosition()
    {
        for (int i = 0;i< _distactorList.Count;i++)
        {
            _distactorList[i].transform.position = new Vector2(540, -((80 * _distactorList.Count) / 2) + (80 / 2) + 80 * i + 410);
            LeanTween.move(_distactorList[i].gameObject, new Vector2(640, -((80 * _distactorList.Count) / 2) + (80 / 2) + 80 * i + 410), 0.5f + i * 0.1f).setEase(LeanTweenType.easeInOutSine);
            LeanTween.alphaCanvas(_distactorList[i].GetComponent<CanvasGroup>(), 1, 0.5f + i * 0.1f).setEase(LeanTweenType.easeInOutSine);
        }
    }

    /// <summary>
    /// 화면 상에서 지움
    /// </summary>
    public void VanishDistractorButton(float time = 0.5f)
    {
        CancelInvoke();
        for (int i = 0; i < _distactorList.Count; i++)
        {
            LeanTween.move(_distactorList[i].gameObject, new Vector2(540, -((80 * _distactorList.Count) / 2) + (80 / 2) + 80 * i + 410), time + i * 0.1f).setEase(LeanTweenType.easeInOutSine);
            LeanTween.alphaCanvas(_distactorList[i].GetComponent<CanvasGroup>(), 0, time + i * 0.1f).setEase(LeanTweenType.easeInOutSine);
        }
    }

    public void ClearDistractorList()
    {
        _distactorList.Clear();
    }
    
    public void OnClick(int count)
    {
        GameDataManager.getInstance.followDistactor = count + 1;
        VanishDistractorButton();
        GameDataManager.getInstance.stopBehavior = false;
        GameDataManager.getInstance.distractorHistory.Add(count + 1);
    }
}
