using UnityEngine;
using UnityEngine.UI;

public class Character : VNEngine.Resource
{
    private CharacterData _characterData;

    /// <summary>
    /// 캐릭터 등장 액션
    /// </summary>
    /// <param name="inData"></param>
    public void FadeIn(BehaviorData inData)
    {
        GameObject moveCharacter = FindResource(inData.name);
        if (moveCharacter == null)
        {
            Debug.LogError("타겟 캐릭터를 찾지 못했습니다.");
            return;
        }
        moveCharacter.SetActive(true);
        LeanTween.moveLocal(moveCharacter, new Vector2(-400 + (int)inData.direction * 400, 0), inData.time).setEase(LeanTweenType.easeInOutSine);
        LeanTween.alphaCanvas(moveCharacter.GetComponent<CanvasGroup>(), 1, inData.time).setEase(LeanTweenType.easeInOutSine);
    }
    
    /// <summary>
    /// 캐릭터 퇴장 액션
    /// </summary>
    /// <param name="inData"></param>
    public void FadeOut(BehaviorData inData)
    {
        GameObject moveCharacter = FindResource(inData.name);
        if (moveCharacter == null)
        {
            Debug.LogError("타겟 캐릭터를 찾지 못했습니다.");
            return;
        }
        LeanTween.moveLocal(moveCharacter, new Vector2(-900 + (int)inData.direction * 900, 0), inData.time).setEase(LeanTweenType.easeInOutSine);
        LeanTween.alphaCanvas(moveCharacter.GetComponent<CanvasGroup>(), 0, inData.time)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(()=> {
                moveCharacter.gameObject.SetActive(false);
            });
    }


    public void LoadResource(string inResourcePath, BehaviorData inData)
    {
        GameObject gameObj = base.LoadResource(inResourcePath);
        gameObj.transform.position = new Vector2(-900 + (int)inData.direction * 900, 360);

        // TODO @minjun 테스트 코드 삭제 요망
        gameObj.transform.localScale = new Vector2(0.35f, 0.35f);

        _characterData = GameDataManager.getInstance._characterDataList.Find(delegate (CharacterData inCharacterData)
        {
            if (inCharacterData.name == inResourcePath)
                return true;
            return false;
        });
        
        if (_characterData == null)
        {
            Debug.LogWarning("캐릭터 세부 정보가 없습니다!");
        }
    }
}
