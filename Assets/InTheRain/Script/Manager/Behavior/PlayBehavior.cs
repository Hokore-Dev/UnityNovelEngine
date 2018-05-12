using UnityEngine;
using System.Collections;

public class PlayBehavior : VNEngine.Behavior
{
    protected override void Excute(BehaviorData inData)
    {
        if (inData.ContainForm("BGM"))
        {
            _bgm.ShowResource(inData.name, inData);
        }
        else if (inData.ContainForm ("SE"))
        {
            _se.ShowResource(inData.name, inData);
        }
        else if (inData.ContainForm("DISTRACTOR"))
        {
            // 선택지 히스토리를 따름
            if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
            {
                GameDataManager.getInstance.followDistactor = GameDataManager.getInstance.distractorHistory[GameDataManager.getInstance.readDistractorHistory];
                if (GameDataManager.getInstance.followDistactor == GameDataManager.getInstance.NONE_SELECT_DISTRACTOR)
                {
                    GameDataManager.getInstance.readCount = GameDataManager.getInstance.FindDisractorStartArea(GameDataManager.getInstance.NONE_SELECT_DISTRACTOR);
                }
                GameDataManager.getInstance.readDistractorHistory++;
            }
            else
            {
                GameDataManager.getInstance.stopBehavior = true;
                _distractor.ClearDistractorList();
                string[] splitMessage = inData.distractorList.Split('-');
                for (int i = 0; i < splitMessage.Length; i++)
                {
                    if (i == 0)
                        continue;
                    _distractor.CreateDistractor(splitMessage[i], inData.notChooseDistractorTime);
                }
                _distractor.RefreshDistractorPosition();
            }
        }
        else if (inData.ContainForm("BACKGROUND"))
        {
            _background.ShowResource(inData.name);
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 실행 명령어가 등록되지 않았습니다!", inData.form));
        }
    }
}
