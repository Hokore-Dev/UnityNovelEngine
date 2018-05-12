using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatAction : MonoBehaviour {

    [SerializeField]
    GameObject _chatBox;

    [SerializeField]
    CanvasGroup _chatTextAlpha;

    /// <summary>
    /// 채팅 글자 페이드 인
    /// </summary>
    /// <param name="time"></param>
    public void ChatFadeIn(float time)
    {
        if (time == 0)
        {
            _chatTextAlpha.alpha = 1;
        }
        else
        {
            _chatTextAlpha.alpha = 0;
            LeanTween.alphaCanvas(_chatTextAlpha, 1, time);
        }
    }

    /// <summary>
    /// 채팅 글자 페이드 아웃
    /// </summary>
    /// <param name="time"></param>
    public void ChatFadeOut(float time)
    {
        if (time == 0)
        {
            _chatTextAlpha.alpha = 0;
        }
        else
        {
            _chatTextAlpha.alpha = 1;
            LeanTween.alphaCanvas(_chatTextAlpha, 0, time);
        }
    }

    /// <summary>
    /// 대화창 표시
    /// </summary>
    /// <param name="time"></param>
	public void ShowChatBox(float time)
    {
        if (time == 0)
        {
            _chatBox.GetComponent<CanvasGroup>().alpha = 1;
            _chatBox.gameObject.transform.position = new Vector2(640 - 717, -34);
            GameDataManager.getInstance.showChatBox = true;
        }
        else
        {
            LeanTween.moveLocal(_chatBox.gameObject, new Vector2(640 - 717, -34), time).setEase(LeanTweenType.easeInOutSine);
            LeanTween.alphaCanvas(_chatBox.GetComponent<CanvasGroup>(), 1, time - 0.1f).setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    GameDataManager.getInstance.showChatBox = true;
                });
        }
    }

    /// <summary>
    /// 대화창 숨김
    /// </summary>
    /// <param name="time"></param>
    public void HideChatBox(float time)
    {
        if (time == 0)
        {
            _chatBox.GetComponent<CanvasGroup>().alpha = 0;
            _chatBox.gameObject.transform.position = new Vector2(-707, -565);
            GameDataManager.getInstance.showChatBox = false;
        }
        else
        {
            LeanTween.moveLocal(_chatBox.gameObject, new Vector2(-707, -565), time).setEase(LeanTweenType.easeInOutSine);
            LeanTween.alphaCanvas(_chatBox.GetComponent<CanvasGroup>(), 0, time - 0.1f).setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() => {
                    GameDataManager.getInstance.showChatBox = false;
                });
        }
    }
}
