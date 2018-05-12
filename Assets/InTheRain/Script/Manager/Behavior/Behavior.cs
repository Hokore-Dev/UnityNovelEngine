using UnityEngine;
using System.Collections;

namespace VNEngine
{
    public class Behavior : MonoBehaviour
    {
        // 리소스 데이터 집합
        protected Background    _background;
        protected Distractor    _distractor;
        protected Character     _character;
        protected Dialogue      _dialogue;
        protected BGM           _bgm;
        protected SE            _se;

        public string LoadKey = string.Empty;

        public void Init()
        {
            _background = this.transform.parent.GetComponent<GameEngine>().background;
            _distractor = this.transform.parent.GetComponent<GameEngine>().distractor;
            _character  = this.transform.parent.GetComponent<GameEngine>().character;
            _dialogue   = this.transform.parent.GetComponent<GameEngine>().dialogue;
            _bgm        = this.transform.parent.GetComponent<GameEngine>().bgm;
            _se         = this.transform.parent.GetComponent<GameEngine>().se;
        }

        public bool ExcuteBehavior(BehaviorData inData)
        {
            if (inData.ContainForm(LoadKey))
            {
                Excute(inData);
                return true;
            }
            return false;
        }

        virtual protected void Excute(BehaviorData inData)
        { }

        virtual public void Clear() { }
    }
}
