using UnityEngine;
using System.Collections;

public class ActionBehavior : VNEngine.Behavior
{
    private FadeAction  _fadeAction;
    private ChatAction  _chatBoxAction;
    private ShakeAction _shakeAction;

    private void Awake()
    {
        _fadeAction     = GetComponent<FadeAction>();
        _chatBoxAction  = GetComponent<ChatAction>();
        _shakeAction    = GetComponent<ShakeAction>();
    }

    public override void Clear()
    {
        base.Clear();
        _fadeAction.Clear();
    }

    protected override void Excute(BehaviorData inData)
    {
        // 액션을 바로 실행한다
        if (GameDataManager.getInstance.scriptPlayMode == GameDataManager.EScriptPlayMode.Load)
        {
            inData.time = 0;
        }

        bool applyDelayTime = true;
        if (inData.ContainForm("SCENE_FADEIN"))
        {
            _fadeAction.FadeIn(inData.time);
        }
        else if (inData.ContainForm("SCENE_FADEOUT"))
        {
            _fadeAction.FadeOut(inData.time);
        }
        else if (inData.ContainForm("CHATBOX_SHOW"))
        {
            _chatBoxAction.ShowChatBox(inData.time);
        }
        else if (inData.ContainForm("CHATBOX_HIDE"))
        {
            _chatBoxAction.HideChatBox(inData.time);
        }
        else if (inData.ContainForm("TEXT_FADEIN"))
        {
            _chatBoxAction.ChatFadeIn(inData.time);
        }
        else if (inData.ContainForm("TEXT_FADEOUT"))
        {
            _chatBoxAction.ChatFadeOut(inData.time);
        }
        else if (inData.ContainForm("SCENE_SHAKE"))
        {
            _shakeAction.ScreenShake(5, inData.time, 50);
        }
        else if (inData.ContainForm("CHARACTER_MOVE_IN"))
        {
            _character.FadeIn(inData);
        }
        else if (inData.ContainForm("CHARACTER_MOVE_OUT"))
        {
            _character.FadeOut(inData);
        }
        else if (inData.ContainForm("DISTRACTOR_FADEOUT"))
        {
            _distractor.VanishDistractorButton(inData.time);
        }
        else if (inData.ContainForm("SE_FADEOUT"))
        {
            applyDelayTime = false;
            _se.FadeOut(inData.name, inData.time);
        }
        else if (inData.ContainForm("SE_FADEIN"))
        {
            applyDelayTime = false;
            _se.FadeIn(inData.name, inData.time);
        }
        else if (inData.ContainForm("BGM_FADEOUT"))
        {
            applyDelayTime = false;
            _bgm.FadeOut(inData.name, inData.time);
        }
        else if (inData.ContainForm("BGM_FADEIN"))
        {
            applyDelayTime = false;
            _bgm.FadeIn(inData.name, inData.time);
        }
        else if (inData.ContainForm("BACKGROUND_CLOUD"))
        {
            _fadeAction.CloudFadeIn(inData.time);
        }
        else if (inData.ContainForm("BACKGROUND_CLEAN"))
        {
            _fadeAction.CloudFadeOut(inData.time);
        }
        else if (inData.ContainForm("BACKGROUND_PUSH"))
        {
            _background.Push(inData.name, inData.direction, inData.time);
        }
        else if (inData.ContainForm("BACKGROUND_FADEIN"))
        {
            if (inData.axis == "NONE")
            {
                _background.FadeIn(inData.name, inData.time);
            }
            else if (inData.axis == "X축")
            {
                _background.ShowResource(inData.name);
                _fadeAction.AxisXFadeIn(inData.time);
            }
        }
        else
        {
            Debug.LogError(StringHelper.Format("[{0}] 액션 명령어가 등록되지 않았습니다!", inData.form));
        }
        GameDataManager.getInstance.behaviorDelayTime = (applyDelayTime) ? inData.time : 0;
    }
}
