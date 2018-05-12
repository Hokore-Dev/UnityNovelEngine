using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Text.RegularExpressions;

public class BehaviorData
{
    public enum EDirection
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public float time
    {
        set
        {
            hashTable["Time"] = value;
        }
        get
        {
            return float.Parse(hashTable["Time"].ToString());
        }
    }
    public string form { get { return hashTable["Form"].ToString(); } }
    public int distractorNumber { get { return int.Parse(hashTable["DistractorNumber"].ToString()); } }
    public float delay { get { return float.Parse(hashTable["Delay"].ToString()); } }
    public string name { get { return hashTable["Name"].ToString(); } }
    public string talk { get { return hashTable["Talk"].ToString(); } }
    public string speaker { get { return hashTable["Speaker"].ToString(); } }
    public string distractorList { get { return hashTable["DistractorList"].ToString(); } }
    public float notChooseDistractorTime { get { return float.Parse(hashTable["NotChooseTime"].ToString()); } }
    public string axis { get { return hashTable["Axis"].ToString(); } }
    public EDirection direction
    {
        get
        {
            string targetValue = hashTable["Direction"].ToString();
            if (targetValue == "왼쪽")
            {
                return EDirection.LEFT;
            }
            else if (targetValue == "가운데")
            {
                return EDirection.CENTER;
            }
            else if (targetValue == "오른쪽")
            {
                return EDirection.RIGHT;
            }
            return EDirection.LEFT;
        }
    }

    public bool loop
    {
        get
        {
            string targetValue = hashTable["Loop"].ToString();
            if (targetValue == "반복")
            {
                return true;
            }
            return false;
        }
    }

    public BehaviorData()
    {
        if (hashTable == null)
        {
            hashTable = new Hashtable();
        }
    }

    /// <summary>
    /// 해당 스크립트 확인
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool ContainForm(string value)
    {
        return form.Contains(value);
    }

    public bool _read = false;
    public Hashtable hashTable;
}