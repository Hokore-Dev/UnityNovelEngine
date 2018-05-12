using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/Effects/CircleOutline")]
public class CircleOutline : ModifiedShadow
{
    [SerializeField] int m_circleCount = 2;
    [SerializeField] int m_firstSample = 4;
    [SerializeField] int m_sampleIncrement = 2;

    [SerializeField] bool m_glowEffect = false;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        circleCount = m_circleCount;
        firstSample = m_firstSample;
        sampleIncrement = m_sampleIncrement;
    }
#endif

    public int circleCount
    {
        get
        {
            return m_circleCount;
        }

        set
        {
            m_circleCount = Mathf.Max(value, 1);
            if (graphic != null)
                graphic.SetVerticesDirty();
        }
    }

    public int firstSample
    {
        get
        {
            return m_firstSample;
        }

        set
        {
            m_firstSample = Mathf.Max(value, 2);
            if (graphic != null)
                graphic.SetVerticesDirty();
        }
    }

    public int sampleIncrement
    {
        get
        {
            return m_sampleIncrement;
        }

        set
        {
            m_sampleIncrement = Mathf.Max(value, 1);
            if (graphic != null)
                graphic.SetVerticesDirty();
        }
    }

    public bool glowEffect
    {
        get
        {
            return m_glowEffect;
        }
        set
        {
            m_glowEffect = value;
            if (graphic != null)
                graphic.SetVerticesDirty();
        }
    }

    public override void ModifyVertices(List<UIVertex> verts)
    {
        if (!IsActive())
            return;

        var total = (m_firstSample * 2 + m_sampleIncrement * (m_circleCount - 1)) * m_circleCount / 2;
        verts.Capacity = verts.Count * (total + 1);
        var original = verts.Count;
        var count = 0;
        var sampleCount = m_firstSample;
        var dx = effectDistance.x / circleCount;
        var dy = effectDistance.y / circleCount;
        for (int i = 1; i <= m_circleCount; i++)
        {
            var rx = dx * i;
            var ry = dy * i;
            var radStep = 2 * Mathf.PI / sampleCount;
            var rad = (i % 2) * radStep * 0.5f;
            for (int j = 0; j < sampleCount; j++)
            {
                var next = count + original;

                Color applyColor = effectColor;

                if (m_glowEffect)
                {
                    // NOTE @sangmoon 중심에서 멀어진 x, y평균값으로 alpha값을 감소 시킴 
                    applyColor.a = ((1.0f - (rx / effectDistance.x)) + (1.0f - (ry / effectDistance.y))) * 0.5f * applyColor.a;
                }

                ApplyShadow(verts, applyColor, count, next, rx * Mathf.Cos(rad), ry * Mathf.Sin(rad));

                count = next;
                rad += radStep;
            }
            sampleCount += m_sampleIncrement;
        }
    }
}