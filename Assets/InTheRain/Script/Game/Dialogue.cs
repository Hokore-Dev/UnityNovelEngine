using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private Text txtName;

    [SerializeField]
    private Text txtTalk;

    // 현재 말하고 있는 캐릭터 이름
    public string characterName { set { txtName.text = value; } get { return txtName.text; } }

    // 대화 메시지
    public string talkMessage   { set { txtTalk.text = value; } get { return txtTalk.text; } }

    private int messageCount = 0;                       // 출력 메시지 길이
    private string printMessage = string.Empty;         // 출력하는 메시지
    private bool bPrintMessage = false;                 // 출력 중인지 검사

    System.Action _callback;                // 콜백
    private CircleOutline _circleOutline = null;

    public void Awake()
    {
        _circleOutline = txtTalk.GetComponent<CircleOutline>();
    }

    public void Clear()
    {
        characterName   = string.Empty;
        talkMessage     = string.Empty;
        bPrintMessage   = false;
        messageCount    = 0;
        CancelInvoke();
    }

    /// <summary>
    /// 메시지 출력을 시작한다.
    /// </summary>
    /// <param name="message">출력 메시지</param>
    /// <param name="startTime">시작 시간</param>
    /// <param name="repeatTime">반복 시간</param>
    /// <returns></returns>
    public bool PrintMessage(string message, float startTime, float repeatTime, System.Action callback = null)
    {
        if (GameDataManager.getInstance.showChatBox)
        {
            txtTalk.transform.position = new Vector2(640 + 34.5f, -274 + 360);
            txtTalk.color = Color.black;
            txtTalk.alignment = TextAnchor.UpperLeft;
        }
        else
        {
            txtTalk.transform.position = new Vector2(640, -274 + 360);
            txtTalk.color = Color.white;
            txtTalk.alignment = TextAnchor.UpperCenter;
        }

        _circleOutline.enabled = !GameDataManager.getInstance.showChatBox;

        _callback = callback;
        if (!bPrintMessage)
        {
            bPrintMessage = true;
            printMessage = message;
            GameDataManager.getInstance.behaviorDelayTime = printMessage.Length * repeatTime + startTime;
            InvokeRepeating("PrintMessageFunction", startTime, repeatTime);
        }
        return !bPrintMessage;
    }

    /// <summary>
    /// 메시지 출력 Invoke 함수
    /// </summary>
    private void PrintMessageFunction()
    {
        string copyMessage = "";
        messageCount++;
        if (messageCount > printMessage.Length)
        {
            messageCount = 0;
            bPrintMessage = false;
            CancelInvoke("PrintMessageFunction");
            if (_callback != null)
            {
                _callback();
            }
        }
        else
        {
            copyMessage = printMessage.Substring(0, messageCount);
            txtTalk.text = copyMessage;
        }
    }
}
