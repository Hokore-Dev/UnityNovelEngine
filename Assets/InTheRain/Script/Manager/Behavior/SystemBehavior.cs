using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SystemBehavior : VNEngine.Behavior
{
    private bool _endScript = false;

    [SerializeField]
    private PopupLoading _popupLoading;

    protected override void Excute(BehaviorData inData)
    {
        if (inData.ContainForm("SCREEN_CLEAR"))
        {
            _dialogue.name          = string.Empty;
            _dialogue.talkMessage   = string.Empty;
        }
        else if (inData.ContainForm("START"))
        {
            _popupLoading.gameObject.SetActive(true);
            _popupLoading.Init();
        }
        else if (inData.ContainForm("END"))
        {
            //GameDataManager.getInstance.stopBehavior = true;
            GameDataManager.getInstance.nowLoading = false;
            
        }
        else if (inData.ContainForm("WAIT"))
        {
            if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
            {
                inData.time = 0;
            }
            GameDataManager.getInstance.behaviorDelayTime = inData.time;
        }
        else if (inData.ContainForm("VIBRATE"))
        {
#if UNITYPLATFORM == UNITY_ANDROID && (!UNITY_EDITOR)
            //Handheld.Vibrate();
#endif
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 시스템 명령어가 등록되지 않았습니다!", inData.form));
        }
    }
}
