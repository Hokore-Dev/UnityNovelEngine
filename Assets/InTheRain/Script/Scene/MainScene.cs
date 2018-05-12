using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Image _character;

    [SerializeField]
    private Image _fadeBox;

    [SerializeField]
    private Sprite[] _sprCharacter;

    [SerializeField]
    private AudioSource _clickAudio;

    [SerializeField]
    private AudioSource _bgm;

    [SerializeField]
    private float waitTime = 3.0f;

    public DigitalRuby.RainMaker.RainScript2D _rainScript;
    private bool _selected = false;

    private void Awake()
    {
        _fadeBox.gameObject.SetActive(true);
        LeanTween.alphaCanvas(_fadeBox.GetComponent<CanvasGroup>(), 0, 1.5f).setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() => {
                _fadeBox.gameObject.SetActive(false);
        });

        Screen.SetResolution(1280, 720, true);
        Utils.SetResolution(_camera);
        _rainScript.RainStart(10.0f);
        StartCoroutine(Co_Character());

        if (!GameDataManager.getInstance.playBGM)
        {
            _bgm.Play();
            DontDestroyOnLoad(_bgm);
            GameDataManager.getInstance.playBGM = true;
        }
    }

    IEnumerator Co_Character()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            LeanTween.alphaCanvas(_character.GetComponent<CanvasGroup>(), 0, 0.5f).setEase(LeanTweenType.easeInOutSine);

            yield return new WaitForSeconds(0.5f);
            _character.sprite = _sprCharacter[0];
            LeanTween.alphaCanvas(_character.GetComponent<CanvasGroup>(), 1, 0.5f).setEase(LeanTweenType.easeInOutSine);

            yield return new WaitForSeconds(waitTime + 0.5f);
            LeanTween.alphaCanvas(_character.GetComponent<CanvasGroup>(), 0, 0.5f).setEase(LeanTweenType.easeInOutSine);

            yield return new WaitForSeconds(0.5f);
            _character.sprite = _sprCharacter[1];
            LeanTween.alphaCanvas(_character.GetComponent<CanvasGroup>(), 1, 0.5f).setEase(LeanTweenType.easeInOutSine);
        }
    }

    public void OnClick(int idx)
    {
        if (_selected)
            return;

        _clickAudio.Play();

        switch (idx)
        {
            case 0:
                _selected = true;
                _fadeBox.gameObject.SetActive(true);
                _rainScript.RainEnd(2.0f);
                LeanTween.alphaCanvas(_fadeBox.GetComponent<CanvasGroup>(), 1, 3.0f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() =>
                {
                    _bgm.Stop();
                    DestroyObject(_bgm);
                    SceneManager.LoadScene("MenuScene");
                });
                break;
            case 1:
                _selected = true;
                _fadeBox.gameObject.SetActive(true);
                _rainScript.RainEnd(2.0f);
                LeanTween.alphaCanvas(_fadeBox.GetComponent<CanvasGroup>(), 1, 3.0f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() =>
                    {
                        SceneManager.LoadScene("LoadScene");
                    });
                break;
            case 2:
                _selected = true;
                _fadeBox.gameObject.SetActive(true);
                _rainScript.RainEnd(2.0f);
                LeanTween.alphaCanvas(_fadeBox.GetComponent<CanvasGroup>(), 1, 3.0f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() =>
                    {
                        SceneManager.LoadScene("SettingScene");
                    });
                break;
            case 3:
                _selected = true;
                break;
            case 4:
                _selected = true;
                break;
            default:
                break;
        }
    }
}
