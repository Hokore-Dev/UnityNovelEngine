using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PopupLoad : MonoBehaviour
{
    [System.Serializable]
    public class LoadBox
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Button _button;

        private SaveData _saveData;

        public void SetSaveData(SaveData data)
        {
            _saveData = data;
            Texture2D texture = Resources.Load("Background/" + data.backgroundName) as Texture2D;
            _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);
        }

        public void LoadData(int index)
        {
            GameDataManager.getInstance.scriptPlayMode = GameDataManager.EScriptPlayMode.Load;
            GameDataManager.getInstance.ParseDistractorHistory(_saveData.distractorHistory);
            GameDataManager.getInstance.loadLine    = _saveData.readCount;
            GameDataManager.getInstance.readCount   = 0;
            SceneManager.LoadScene("MenuScene");
        }
    }

    [SerializeField]
    LoadBox[] _LoadBox;

    public void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            string path = string.Format("{0}/{1}.dat", Application.temporaryCachePath, GameDataManager.getInstance.savePath + i.ToString());
            SaveData data = FileIOExtension.LoadFromFile<SaveData>(path, path);
            if (data != null)
            {
                _LoadBox[i].SetSaveData(data);
            }
        }
    }

    public void OnLoad(int index)
    {
        _LoadBox[index].LoadData(index);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
