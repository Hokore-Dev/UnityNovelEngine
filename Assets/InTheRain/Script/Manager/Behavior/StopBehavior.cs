using UnityEngine;
using System.Collections;

public class StopBehavior : VNEngine.Behavior
{

    protected override void Excute(BehaviorData inData)
    {
        if (inData.ContainForm("BGM"))
        {
            _bgm.HideAllResource();
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 중단 명령어가 등록되지 않았습니다!", inData.form));
        }
    }
}
