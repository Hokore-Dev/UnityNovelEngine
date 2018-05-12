
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// Credit Breyer
/// Sourced from - http://forum.unity3d.com/threads/scripts-useful-4-6-scripts-collection.264161/#post-1777407

[RequireComponent(typeof(Text), typeof(RectTransform))]
[AddComponentMenu("UI/Effects/Curved Text")]
public class CurvedText : BaseMeshEffect
{
    public AnimationCurve curveForText = AnimationCurve.Linear(0, 0, 1, 10);
    public float curveMultiplier = 1;
    private RectTransform rectTrans;
    public RectTransform RectTrans
    {
        get
        {
            if (rectTrans == null)
                rectTrans = GetComponent<RectTransform>();

            return rectTrans;
        }

        set
        {
            rectTrans = value;
        }
    }


#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (curveForText[0].time != 0)
        {
            var tmpRect = curveForText[0];
            tmpRect.time = 0;
            curveForText.MoveKey(0, tmpRect);
        }
        RectTrans = GetComponent<RectTransform>();
        if (curveForText[curveForText.length - 1].time != RectTrans.rect.width)
            OnRectTransformDimensionsChange();
    }
#endif
    protected override void Awake()
    {
        base.Awake();
        RectTrans = GetComponent<RectTransform>();
        OnRectTransformDimensionsChange();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        RectTrans = GetComponent<RectTransform>();
        OnRectTransformDimensionsChange();
    }
    public override void ModifyMesh(VertexHelper vh)
    {
        int count = vh.currentVertCount;
        if (!IsActive() || count == 0)
        {
            return;
        }

        for (int i = 0; i < vh.currentVertCount; i += 4)
        {
            List<UIVertex> list = new List<UIVertex>();

            for(int j = i; j < i + 4; ++j)
            {
                UIVertex uiVertex = new UIVertex();
                vh.PopulateUIVertex(ref uiVertex, j);
                uiVertex = SetVertexPosY(uiVertex);
                list.Add(uiVertex);
            }

            float curved_x = 0f;
            if (list[0].position.y > list[1].position.y)
            {
                curved_x = list[0].position.y - list[1].position.y;
                for (int j = 0; j < 4; ++j)
                {
                    UIVertex uiVertex = list[j];
                    uiVertex = SetVertexPosX(uiVertex, (j % 4 == 0 || (j - 1) % 4 == 0) ? curved_x : -curved_x);
                    vh.SetUIVertex(uiVertex, i+j);
                }
            }
            else if (list[0].position.y < list[1].position.y)
            {
                curved_x = list[1].position.y - list[0].position.y;
                for (int j = 0; j < 4; ++j)
                {
                    UIVertex uiVertex = list[j];
                    uiVertex = SetVertexPosX(uiVertex, (j % 4 == 0 || (j - 1) % 4 == 0) ? -curved_x : curved_x);
                    vh.SetUIVertex(uiVertex, i + j);
                }
            }
        }
    }
    protected override void OnRectTransformDimensionsChange()
    {
        var tmpRect = curveForText[curveForText.length - 1];
        tmpRect.time = RectTrans.rect.width;
        curveForText.MoveKey(curveForText.length - 1, tmpRect);
    }

    public UIVertex SetVertexPosY(UIVertex outUIVertex)
    {
        float curved_y = curveForText.Evaluate(RectTrans.rect.width * RectTrans.pivot.x + outUIVertex.position.x) * curveMultiplier;
        outUIVertex.position.y += curved_y;

        return outUIVertex;
    }
    public UIVertex SetVertexPosX(UIVertex outUIVertex, float add_x)
    {
        outUIVertex.position.x += add_x * 0.5f;
        return outUIVertex;
    }
}
