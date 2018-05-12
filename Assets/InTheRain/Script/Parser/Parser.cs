using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace VNEngine
{
    public abstract class Parser
    {
        protected string _readLine = "";
        protected string[] _splitArray = null;

        public virtual void Init()
        {

        }

        /// <summary>
        /// 스크립트 빌드
        /// </summary>
        /// <param name="text"></param>
        public void BuildScript(string text)
        {
            string[] data = text.Split('\n');
            for (int i = 0; i < data.Length; i++)
            {
                _readLine = data[i].Replace("\r", "");
                if (_readLine == "")
                {
                    LineEmpty();
                    continue;
                }

                _splitArray = _readLine.Split(' ');
                Parse();
            }
            DevelopeLog.Log(StringHelper.Format("빌드 완료 스크립트 {0}줄", data.Length));
            DevelopeLog.Log(StringHelper.Format("Episode Parser Behavior Command : {0}개", GameDataManager.getInstance._behaviorList.Count));
        }

        /// <summary>
        /// 해당 경로의 파일을 로드
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        virtual public bool LoadData(string path)
        {
            TextAsset file = Resources.Load(path) as TextAsset;
            if (file == null)
                return false;

            BuildScript(file.text);
            return true;
        }

        /// <summary>
        /// 파싱 메소드 실행
        /// </summary>
        /// <param name="text"></param>
        virtual public void Parse()
        {
        }

        /// <summary>
        /// 빈 라인일때
        /// </summary>
        virtual public void LineEmpty()
        {

        }
    }
}
