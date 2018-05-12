using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupSave : MonoBehaviour {
    
    [System.Serializable]
    public class SaveBox
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _image;

        private SaveData _saveData;

        public void SetSaveData(SaveData data)
        {
            _saveData = data;
            Texture2D texture = Resources.Load("Background/" + data.backgroundName) as Texture2D;
            _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);
        }

        public void SaveData(int index, Image background)
        {
            _saveData = new SaveData();
            _image.sprite               = background.sprite;
            _saveData.backgroundName    = background.name;
            _saveData.distractorHistory = GameDataManager.getInstance.DistractorToString();
            _saveData.readCount         = GameDataManager.getInstance.readCount;

            string path = string.Format("{0}/{1}.dat", Application.temporaryCachePath, GameDataManager.getInstance.savePath + index.ToString());
            FileIOExtension.SaveAsFile(_saveData, path, path);
        }
    }

    [SerializeField]
    SaveBox []_saveBox;

    private Image _background;

    public void Init(Image image)
    {
        _background = image;

        for (int i = 0; i < 3; i++)
        {
            string path = string.Format("{0}/{1}.dat", Application.temporaryCachePath, GameDataManager.getInstance.savePath + i.ToString());
            SaveData data = FileIOExtension.LoadFromFile<SaveData>(path, path);
            if (data != null)
            {
                _saveBox[i].SetSaveData(data);
            }
        }
    }
    
    public void OnSave(int index)
    {
        _saveBox[index].SaveData(index, _background);
    }

	public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
