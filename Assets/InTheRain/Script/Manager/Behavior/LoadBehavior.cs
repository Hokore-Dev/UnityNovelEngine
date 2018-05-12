using UnityEngine;
using System.Collections;

public class LoadBehavior : VNEngine.Behavior
{
    protected override void Excute(BehaviorData inData)
    {
        if (inData.ContainForm("CHARACTER"))
        {
            _character.LoadResource(inData.name,inData);
        }
        else if (inData.ContainForm("BGM"))
        {
            _bgm.LoadResource(inData.name);
        }
        else if (inData.ContainForm("BACKGROUND"))
        {
            _background.LoadResource(inData.name);
        }
        else if (inData.ContainForm("SE"))
        {
            _se.LoadResource(inData.name);
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 로드 명령어가 등록되지 않았습니다!", inData.form));
        }
    }
}
