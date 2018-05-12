using UnityEngine;
using System.Collections;

public class ParticleBehavior : VNEngine.Behavior
{
    [SerializeField]
    private DigitalRuby.RainMaker.RainScript2D _rainScript;

    public override void Clear()
    {
        base.Clear();
        _rainScript.RainEnd(0);
    }

    protected override void Excute(BehaviorData inData)
    {
        if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
        {
            inData.time = 0;
        }

        if (inData.ContainForm("RAIN_START"))
        {
            _rainScript.gameObject.SetActive(true);
            _rainScript.RainStart(inData.time);
        }
        else if (inData.ContainForm("RAIN_END"))
        {
            _rainScript.RainEnd(inData.time);
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 파티클 명령어가 등록되지 않았습니다!", inData.form));
        }
    }
}
