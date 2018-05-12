using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;

namespace VNEngine
{
    public class CharacterParser : Parser
    {
        // 작성중인 캐릭터 정보
        private CharacterData _writeCharacterData = null;

        /// <summary>
        /// 캐릭터 정보 작성 완료로 간주
        /// </summary>
        public override void LineEmpty()
        {
            base.LineEmpty();
            if (_writeCharacterData != null)
            {
                GameDataManager.getInstance._characterDataList.Add(_writeCharacterData);
                _writeCharacterData = null;
            }
        }

        /// <summary>
        /// 캐릭터 정보를 파싱
        /// </summary>
        /// <param name="text"></param>
        public override void Parse()
        {
            base.Parse();
            if (_writeCharacterData == null)
            {
                // 주인공 캐릭터 이름 판별
                if (_readLine[0].CompareTo('[') == 0)
                {
                    StringBuilder speaker = new StringBuilder();
                    for (int i = 0; i < _readLine.Length - 1; i++)
                    {
                        if (_readLine[i] != '[' && _readLine[i] != ']')
                        {
                            speaker.Append(_readLine[i].ToString());
                        }
                    }
                    _writeCharacterData = new CharacterData();
                    _writeCharacterData.name = speaker.ToString();
                }
            }
            else
            {
                string[] valueArray = _readLine.Split(' ');
                if (valueArray[0] == "특징")
                {
                    _writeCharacterData.feature = valueArray[1];
                }
                else if (valueArray[0] == "키")
                {
                    _writeCharacterData.height = valueArray[1];
                }
                else if (valueArray[0] == "경로")
                {
                    _writeCharacterData.path = valueArray[1];
                }
            }
        }
    }
}
