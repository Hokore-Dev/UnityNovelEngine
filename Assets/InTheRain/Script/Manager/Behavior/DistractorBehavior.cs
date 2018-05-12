using UnityEngine;
using System.Collections;

public class DistractorBehavior : VNEngine.Behavior
{
    protected override void Excute(BehaviorData inData)
    {
        if (inData.ContainForm("START"))
        {
            int distractorNumber = inData.distractorNumber;
            int followNumber = GameDataManager.getInstance.followDistactor;
            if (distractorNumber != followNumber)
            {
                GameDataManager.getInstance.readCount = GameDataManager.getInstance.FindDisractorEndArea(distractorNumber);
            }
        }
        else if (inData.ContainForm("END"))
        {
            if (GameDataManager.getInstance.followDistactor == inData.distractorNumber)
            {
                GameDataManager.getInstance.followDistactor = -1;
            }
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 선택지 명령어가 등록되지 않았습니다!", inData.form));
        }
    }
}
