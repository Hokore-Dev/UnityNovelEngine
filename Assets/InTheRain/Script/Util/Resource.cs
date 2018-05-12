using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace VNEngine
{
    public class Resource : MonoBehaviour
    {
        public enum EResourceType
        {
            Texture = 0,
            Sound   = 1,
        }

        [SerializeField]
        private string _prefabPath;

        [SerializeField]
        private string _folderPath;
        
        [SerializeField]
        private Transform _parentTansform;

        protected EResourceType _resourceType = EResourceType.Texture;
        protected Dictionary<string, GameObject> _resourceList = new Dictionary<string, GameObject>();

        /// <summary>
        /// 리소스를 모두 제거합니다
        /// </summary>
        public void ClearResoure()
        {
            foreach (var item in _resourceList)
            {
                DestroyObject(item.Value);
            }
            _resourceList.Clear();
        }

        /// <summary>
        /// 리소스를 찾는다
        /// </summary>
        /// <param name="inResourceName"></param>
        /// <returns></returns>
        public GameObject FindResource(string inResourceName, bool ignoreError = false)
        {
            GameObject gameObject = null;
            foreach (var item in _resourceList)
            {
                if (item.Key == inResourceName)
                {
                    gameObject = item.Value;
                    break;
                }
            }
            if (gameObject == null && !ignoreError)
            {
                Debug.LogError(StringHelper.Format("{0} 해당 오브젝트를 찾지 못했습니다.", inResourceName));
            }
            return gameObject;
        }

        /// <summary>
        /// 표시되고 있는 리소스를 받아온다
        /// </summary>
        /// <returns></returns>
        public GameObject FindActiveResource()
        {
            GameObject gameObject = null;
            foreach (var item in _resourceList)
            {
                if (item.Value.active == true)
                {
                    gameObject = item.Value;
                    break;
                }
            }
            if (gameObject == null)
            {
                Debug.LogError(StringHelper.Format("액티브 오브젝트를 찾지 못했습니다."));
            }
            return gameObject;
        }

        /// <summary>
        /// 리소스를 표시한다
        /// </summary>
        /// <param name="inResourceName"></param>
        public GameObject ShowResource(string inResourceName)
        {
            GameObject gameObject = null;
            foreach (var item in _resourceList)
            {
                item.Value.SetActive((item.Key == inResourceName) ? true : false);
                if (_resourceType == EResourceType.Texture)
                {
                    item.Value.GetComponent<CanvasGroup>().alpha = (item.Key == inResourceName) ? 1 : 0;
                }
                if (item.Key == inResourceName)
                {
                    gameObject = item.Value;
                    if (_resourceType == EResourceType.Sound)
                    {
                        item.Value.GetComponent<AudioSource>().volume = 0.5f;
                        item.Value.GetComponent<AudioSource>().Play();
                    }
                }
                else
                {
                    if (_resourceType == EResourceType.Sound)
                    {
                        item.Value.GetComponent<AudioSource>().Stop();
                    }
                }
            }

            if (gameObject == null)
            {
                Debug.LogError(StringHelper.Format("{0} 리소스가 로드되지 않았습니다.", inResourceName));
            }
            return gameObject;
        }

        /// <summary>
        /// 모든 리소스를 숨긴다
        /// </summary>
        public void HideAllResource()
        {
            foreach (var item in _resourceList)
            {
                if (_resourceType == EResourceType.Sound)
                {
                    item.Value.GetComponent<AudioSource>().Stop();
                }
                else if (_resourceType == EResourceType.Texture)
                {
                    item.Value.GetComponent<CanvasGroup>().alpha = 0;
                }
                item.Value.SetActive(false);
            }
        }

        /// <summary>
        /// 해당 리소스를 숨긴다
        /// </summary>
        /// <param name="inResourceName"></param>
        public void HideResource(string inResourceName)
        {
            GameObject gameObject = null;
            foreach (var item in _resourceList)
            {
                if (item.Key == inResourceName)
                {
                    gameObject = item.Value;
                    item.Value.SetActive(false);
                    if (_resourceType == EResourceType.Sound)
                    {
                        item.Value.GetComponent<AudioSource>().Stop();
                    }
                    else if (_resourceType == EResourceType.Texture)
                    {
                        item.Value.GetComponent<CanvasGroup>().alpha = 0;
                    }
                    break;
                }
            }

            if (gameObject == null)
            {
                Debug.LogError(StringHelper.Format("{0} 리소스가 숨겨지지 않았습니다.", inResourceName));
            }
        }

        IEnumerator Co_LoadAudioWWW(string inResourceName)
        {
            string realPath = string.Format("{0}{1}", Application.dataPath, @"/Resources/", inResourceName);
            string path = StringHelper.Format(@"file:///{0}{1}{2}.wav", Application.dataPath, @"/Resources/", inResourceName);
            WWW audioWWW = new WWW(path);
            Debug.Log("Importing file " + path);

            WWW www = new WWW("file://" + path);
            while (!www.isDone)
                yield return www;

            Debug.Log("File imported size: " + www.size);

            var gameObj = MonoBehaviour.Instantiate(Resources.Load(_prefabPath)) as GameObject;
            gameObj.name = inResourceName;
            gameObj.transform.SetParent(_parentTansform);
            gameObj.GetComponent<AudioSource>().clip = audioWWW.GetAudioClip(false, true);

            yield return null;
        }

        /// <summary>
        /// 리소스를 로드한다.
        /// </summary>
        /// <param name="inResourceName">리소스 이름</param>
        public GameObject LoadResource(string inResourceName)
        {
            GameObject findObject = FindResource(inResourceName, true);
            if (findObject != null)
                return findObject;

            var obj = Resources.Load(_prefabPath);
            var gameObj = MonoBehaviour.Instantiate(obj) as GameObject;
            gameObj.name = inResourceName;
            gameObj.transform.SetParent(_parentTansform);

            if (_resourceType == EResourceType.Texture)
            {
                Texture2D texture = null;
                if (SceneManager.GetActiveScene().name == "DevelopeTool")
                {
                    string path = StringHelper.Format("{0}{1}{2}.png", Application.dataPath, @"/Resources/", inResourceName);
                    byte[] data = File.ReadAllBytes(path);
                    texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
                    texture.LoadImage(data);
                    texture.name = Path.GetFileNameWithoutExtension(path);
                }
                else
                {
                    texture = Resources.Load(StringHelper.Format("{0}/{1}", _folderPath, inResourceName)) as Texture2D;
                }
                if (texture == null)
                {
                    Debug.LogError(StringHelper.Format("[{0}] 텍스처 리소스를 찾을 수 없습니다.", inResourceName));
                }
                else
                {
                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    gameObj.GetComponent<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 1);
                    gameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
                    gameObj.GetComponent<RectTransform>().position = new Vector2(640, 360);
                    gameObj.GetComponent<CanvasGroup>().alpha = 0;
                }
            }
            else if (_resourceType == EResourceType.Sound)
            {
                AudioClip audioSource = null;
                if (SceneManager.GetActiveScene().name == "DevelopeTool")
                {
                    Co_LoadAudioWWW(inResourceName);
                    //string realPath = string.Format("{0}{1}", Application.dataPath, @"/Resources/", inResourceName);
                    //string path = StringHelper.Format(@"file:///{0}{1}{2}.wav", Application.dataPath, @"/Resources/", inResourceName);
                    //WWW audioWWW = new WWW(path);
                    //audioSource = audioWWW.GetAudioClip(false, true);
                    //audioSource.name = Path.GetFileNameWithoutExtension(path);
                }
                else
                {
                    audioSource = Resources.Load(StringHelper.Format("{0}/{1}", _folderPath, inResourceName)) as AudioClip;
                }
                if (audioSource == null)
                {
                    Debug.LogError(StringHelper.Format("[{0}] 오디오 리소스를 찾을 수 없습니다.", inResourceName));
                }
                else
                {
                    gameObj.GetComponent<AudioSource>().clip = audioSource;
                }
            }
            _resourceList.Add(inResourceName, gameObj);
            return gameObj;
        }
    }
}


