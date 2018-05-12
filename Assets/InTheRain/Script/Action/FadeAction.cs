using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeAction : MonoBehaviour {

    [SerializeField]
    CanvasGroup _fadeBox;

    [SerializeField]
    GameObject _axisXLeft;

    [SerializeField]
    GameObject _axisXRight;

    [SerializeField]
    GameObject _axisYDown;

    [SerializeField]
    GameObject _axisYUp;

    [SerializeField]
    CanvasGroup _coludBox;

    public void Clear()
    {
        _fadeBox.gameObject.SetActive(true);
        _fadeBox.alpha = 1;
    }

    /// <summary>
    /// 페이드인
    /// </summary>
    /// <param name="time"></param>
	public void FadeIn(float time)
    {
        if (time == 0)
        {
            _fadeBox.alpha = 1;
        }
        else
        {
        _fadeBox.alpha = 0;
        LeanTween.alphaCanvas(_fadeBox, 1, time);
        }
    }

    /// <summary>
    /// 페이드 아웃
    /// </summary>
    /// <param name="time"></param>
    public void FadeOut(float time)
    {
        if (time == 0)
        {
            _fadeBox.alpha = 0;
        }
        else
        {
            _fadeBox.alpha = 1;
            LeanTween.alphaCanvas(_fadeBox, 0, time);
        }
    }

    /// <summary>
    /// 흐려지기 
    /// </summary>
    /// <param name="time"></param>
    public void CloudFadeIn(float time)
    {
        _coludBox.alpha = 0;
        _coludBox.gameObject.SetActive(true);
        LeanTween.alphaCanvas(_coludBox, 1, time);
    }

    /// <summary>
    /// 맑아지기
    /// </summary>
    /// <param name="time"></param>
    public void CloudFadeOut(float time)
    {
        _coludBox.alpha = 1;
        LeanTween.alphaCanvas(_coludBox, 0, time).setOnComplete(()=> { _coludBox.gameObject.SetActive(false); });
    }

    /// <summary>
    /// 축 기준 페이드인
    /// </summary>
    /// <param name="time"></param>
    public void AxisXFadeIn(float time)
    {
        _axisXRight.SetActive(true);
        _axisXLeft.SetActive(true);
        _axisXLeft.transform.position = new Vector2(640,360);
        _axisXRight.transform.position = new Vector2(640, 360);

        LeanTween.moveLocalX(_axisXLeft, -1100, time).setEase(LeanTweenType.easeInOutSine).setOnComplete(() => { _axisXLeft.SetActive(false); });
        LeanTween.moveLocalX(_axisXRight, 1100, time).setEase(LeanTweenType.easeInOutSine).setOnComplete(() => { _axisXRight.SetActive(false); });
    }
}
