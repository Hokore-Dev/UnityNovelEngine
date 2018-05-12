using UnityEngine;
using System.Collections;

public class CharacterData {

    public string name { get; set; }
    public string height { get; set; }
    public string feature { get; set; }
    public string path { get; set; }

    public CharacterData()
    {
        name = "설정되지 않음";
        height = "설정되지 않음";
        feature = "설정되지 않음";
        path = "설정되지 않음";
    }
}
