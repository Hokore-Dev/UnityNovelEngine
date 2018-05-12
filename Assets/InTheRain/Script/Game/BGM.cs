using UnityEngine;
using System.Collections;

public class BGM : VNEngine.Resource
{
    private AudioSource _targetAudio;

    /// <summary>
    /// 볼륨 업데이트
    /// </summary>
    /// <param name="value"></param>
    private void VolumeUpdate(float value)
    {
        _targetAudio.volume = value;
    }

    /// <summary>
    /// 사운드 페이드 아웃
    /// </summary>
    /// <param name="inResourceName"></param>
    /// <param name="inTime"></param>
    public void FadeOut(string inResourceName,float inTime)
    {
        _targetAudio = base.FindResource(inResourceName).GetComponent<AudioSource>();
        LeanTween.value(_targetAudio.gameObject, _targetAudio.volume, 0, inTime).setOnUpdate(VolumeUpdate);
    }

    /// <summary>
    /// 사운드 페이드 인
    /// </summary>
    /// <param name="inResourceName"></param>
    /// <param name="inTime"></param>
    public void FadeIn(string inResourceName, float inTime)
    {
        _targetAudio = base.FindResource(inResourceName).GetComponent<AudioSource>();
        _targetAudio.gameObject.SetActive(true);
        _targetAudio.volume = 0;
        _targetAudio.Play();
        LeanTween.value(_targetAudio.gameObject, _targetAudio.volume, 0.5f, inTime).setOnUpdate(VolumeUpdate);
    }

    public void ShowResource(string inResourceName, BehaviorData inData)
    {
        GameObject gameObject = base.ShowResource(inResourceName);
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayDelayed((ulong)inData.delay);
        audioSource.loop = inData.loop;
    }

    private void Awake()
    {
        _resourceType = EResourceType.Sound;
    }
}
