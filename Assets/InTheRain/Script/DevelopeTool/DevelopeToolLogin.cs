using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DevelopeToolLogin : MonoBehaviour
{
    [SerializeField]
    private Image _imgIcon;

    [SerializeField]
    private InputField _license;

    [SerializeField]
    private CanvasGroup _group;

    [SerializeField]
    private CanvasGroup _fadebox;

    public void EditEnd()
    {
        StartCoroutine(IdentifyLicense());
    }

    private void Awake()
    {
        Screen.SetResolution(1280, 500, false);
    }

    private void Start()
    {
        LeanTween.moveLocalY(_imgIcon.gameObject, 50, 1.0f).setEase(LeanTweenType.easeOutSine);
        LeanTween.alphaCanvas(_group, 1, 1.0f).setDelay(0.85f);
    }

    /// <summary>
    /// 라이선스를 받아온다
    /// [보안 이슈로 해결 요망]
    /// </summary>
    /// <returns></returns>
    private IEnumerator IdentifyLicense()
    {
        WWW www = new WWW("http://anioneguild.com/RuriEngine/IdentifyLicense.php");
        while (true)
        {
            if (www.isDone)
            {
                if (_license.text == www.text)
                {
                    _fadebox.gameObject.SetActive(true);
                    LeanTween.alphaCanvas(_fadebox, 1, 0.5f).setOnComplete(() =>
                    {
                        SceneManager.LoadScene("DevelopeTool");
                    });
                }
                else
                {
                    LeanTween.moveLocalX(_license.gameObject, 8, 0.15f).setEase(LeanTweenType.easeShake);
                }
                break;
            }
            else
            {
                yield return null;
            }
        }
    }
}
