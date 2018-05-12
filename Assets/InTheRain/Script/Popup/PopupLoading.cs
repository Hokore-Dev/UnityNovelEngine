using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupLoading : MonoBehaviour {

    [SerializeField]
    private Text _txtLoad;

    public void Init()
    {
        GameDataManager.getInstance.nowLoading = true;
        this.GetComponent<CanvasGroup>().alpha = 0;
        LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 1, 1.0f).setOnComplete(()=> {
                StartCoroutine(Co_TextLoad());
        });
    }

    IEnumerator Co_TextLoad()
    {
        while (true)
        {
            _txtLoad.text = "하나 옷 갈아 입는 중";

            yield return new WaitForSeconds(0.2f);
            _txtLoad.text = "하나 옷 갈아 입는 중.";

            yield return new WaitForSeconds(0.2f);

            _txtLoad.text = "하나 옷 갈아 입는 중..";

            yield return new WaitForSeconds(0.2f);

            _txtLoad.text = "하나 옷 갈아 입는 중...";

            yield return new WaitForSeconds(0.2f);

            if (GameDataManager.getInstance.nowLoading == false)
            {
                LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 0, 1.0f).setOnComplete(() => {
                    GameDataManager.getInstance.stopBehavior = false;
                    gameObject.SetActive(false);
                });
                break;
            }
        }
    }
}
