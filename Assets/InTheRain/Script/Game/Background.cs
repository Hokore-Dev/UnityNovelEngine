using UnityEngine;
using System.Collections.Generic;

public class Background : VNEngine.Resource
{
    private int _sortOrder = 1;

    /// <summary>
    /// 페이드인 교체 액션
    /// </summary>
    /// <param name="inResourceName"></param>
    /// <param name="inTime"></param>
    public void FadeIn(string inResourceName, float inTime)
    {
        foreach (var item in _resourceList)
        {
            if (item.Key == inResourceName)
            {
                item.Value.SetActive(true);
                LeanTween.alphaCanvas(item.Value.GetComponent<CanvasGroup>(), 1, inTime);
            }
            else
            {
                LeanTween.alphaCanvas(item.Value.GetComponent<CanvasGroup>(), 0, inTime);
            }
        }
    }

    /// <summary>
    /// 교체 배경을 밀어낸다
    /// </summary>
    /// <param name="inResourceName"></param>
    /// <param name="inDirection"></param>
    /// <param name="inTime"></param>
    public void Push(string inResourceName, BehaviorData.EDirection inDirection, float inTime)
    {
        GameObject gameObject = FindResource(inResourceName);
        gameObject.SetActive(true);
        gameObject.GetComponent<Canvas>().sortingOrder = _sortOrder;
        gameObject.GetComponent<CanvasGroup>().alpha = 1;
        gameObject.transform.position = new Vector2(640 - 1280 + 1280 * (int)inDirection, 360);

        LeanTween.moveX(gameObject, 640, inTime).setEase(LeanTweenType.easeInOutSine);
        _sortOrder++;
    }
}
