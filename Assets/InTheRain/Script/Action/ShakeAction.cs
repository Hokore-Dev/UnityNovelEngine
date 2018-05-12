using UnityEngine;
using System.Collections;

public class ShakeAction : MonoBehaviour {

    [SerializeField]
    Camera _camera;

    // 기준 X 좌표
    private float _standardPositionX = 640;

    // 줄어드는 거리
    private float _primeDecreseDistance = 0;

    private void Start()
    {
        _standardPositionX = _camera.transform.position.x;
    }

    /// <summary>
    /// 스크린 쉐이크
    /// </summary>
    /// <param name="shakeCount">흔들리는 횟수</param>
    /// <param name="time">시간</param>
    /// <param name="distance">거리</param>
    public void ScreenShake(int shakeCount ,float time, float distance)
    {
        _primeDecreseDistance = distance / shakeCount;
        Shake(shakeCount, time / shakeCount, distance);
    }

    /// <summary>
    /// 재귀 쉐이크
    /// </summary>
    /// <param name="shakeCount"></param>
    /// <param name="time"></param>
    /// <param name="distance"></param>
    private void Shake(int shakeCount, float time, float distance)
    {
        float shakeValue = 0;
        if (shakeCount != 0)
            shakeValue = (shakeCount % 2 == 0) ? distance : -distance;

        LeanTween.moveX(_camera.gameObject, _standardPositionX + shakeValue, time)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() => {
                if (shakeCount != 0)
                {
                    Shake(shakeCount - 1, time, _primeDecreseDistance * shakeCount);
                }
            });
    }
}
