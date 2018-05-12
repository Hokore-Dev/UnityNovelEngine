using UnityEngine;
using System.Collections;

public class DialogueBehavior : VNEngine.Behavior
{
    protected override void Excute(BehaviorData inData)
    {
        if (inData.ContainForm("SPEAKER"))
        {
            _dialogue.characterName = inData.speaker;
        }
        else if (inData.ContainForm("TALK"))
        {
            string talk = inData.talk.Replace("\\n", "\n");
            if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Load ||
                GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Skip)
            {
                // 대화를 바로 실행한다
                _dialogue.talkMessage = talk;
            }
            else
            {
                _dialogue.PrintMessage(talk, 0, 0.05f);
            }
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 대화 명령어가 등록되지 않았습니다!", inData.form));
        }
    }
}