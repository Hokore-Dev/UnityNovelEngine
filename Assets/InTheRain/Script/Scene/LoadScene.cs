using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadScene : MonoBehaviour {

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Image _fadeBox;

    [SerializeField]
    private AudioSource _audioSource;

	// Use this for initialization
	void Start () {
        Utils.SetResolution(_camera);
        LeanTween.alphaCanvas(_fadeBox.GetComponent<CanvasGroup>(), 0, 1.5f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() =>
                    {
                        _fadeBox.gameObject.SetActive(false);
                    });
    }

    public void OnClick()
    {
        _audioSource.Play();
        _fadeBox.gameObject.SetActive(true);
        LeanTween.alphaCanvas(_fadeBox.GetComponent<CanvasGroup>(), 1, 1.5f)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                SceneManager.LoadScene("MainScene");
            });
    }
}
