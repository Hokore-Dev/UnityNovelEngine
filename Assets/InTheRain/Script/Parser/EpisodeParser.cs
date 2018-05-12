using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

namespace VNEngine
{
    public class EpisodeParser : Parser
    {
        // 작성중인 행동 데이터
        private BehaviorData _writeBehaviorData = null;

        public override bool LoadData(string path)
        {
            bool value = base.LoadData(path);
            return value;
        }

        public override void Parse()
        {
            base.Parse();

            switch (_splitArray[0])
            {
                case "/중지": ParseStop(); break;
                case "/실행": ParsePlay(); break;
                case "/로드": ParseLoad(); break;
                case "/액션": ParseAction(); break;
                case "/시스템": ParseSystem(); break;
                case "/파티클": ParseParticle(); break;
                default:
                    ParseDistractor();
                    ParseDialogue();
                    break;
            }
        }

        /// <summary>
        /// 선택지 스크립트
        /// </summary>
        void ParseDistractor()
        {
            // 선택지 스크립트 구분
            if (_readLine.Contains("선택지"))
            {
                if (_readLine.Contains("시작") || _readLine.Contains("종료"))
                {
                    int result = 0;
                    int.TryParse(_splitArray[2], out result);
                    int distractorNumber = 0;

                    if (result != 0)
                    {
                        distractorNumber = int.Parse(_splitArray[2]);
                    }
                    BehaviorData data = new BehaviorData();
                    if (_readLine.Contains("시작"))
                    {
                        data.hashTable.Add("Form", "DISTRACTOR_START");
                    }
                    else if (_readLine.Contains("종료"))
                    {
                        data.hashTable.Add("Form", "DISTRACTOR_END");
                    }
                    if (_readLine.Contains("않음"))
                    {
                        data.hashTable.Add("DistractorNumber", GameDataManager.getInstance.NONE_SELECT_DISTRACTOR.ToString());
                    }
                    else
                    {
                        data.hashTable.Add("DistractorNumber", distractorNumber.ToString());
                    }
                    GameDataManager.getInstance._behaviorList.Add(data);
                }
                else
                {
                    DevelopeLog.LogError(StringHelper.Format("선택지 명령어를 찾을수 없습니다! [{0}]", _readLine));
                }
            }
            // 선택지 메시지
            else if (_readLine[0] == '-')
            {
                if (_writeBehaviorData == null)
                {
                    DevelopeLog.LogError("선택지 정의가 설정되지 않았습니다");
                }
                else
                {
                    if (int.Parse(_writeBehaviorData.hashTable["ValueCount"].ToString()) > 0)
                    {
                        _writeBehaviorData.hashTable["DistractorList"] = StringHelper.Format("{0}{1}", _writeBehaviorData.hashTable["DistractorList"].ToString(), _readLine);
                        _writeBehaviorData.hashTable["ValueCount"] = (int.Parse(_writeBehaviorData.hashTable["ValueCount"].ToString()) - 1).ToString();
                        if (int.Parse(_writeBehaviorData.hashTable["ValueCount"].ToString()) == 0)
                        {
                            GameDataManager.getInstance._behaviorList.Add(_writeBehaviorData);
                            _writeBehaviorData = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 파티클 스크립트
        /// </summary>
        void ParseParticle()
        {
            if (_splitArray[1] == "비")
            {
                if (_splitArray[2] == "시작")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "PARTICLE_RAIN_START", "Time", VNCommon.GetSplitValue(_splitArray, 3, "1.0"))
                    });
                }
                else if (_splitArray[2] == "종료")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "PARTICLE_RAIN_END", "Time", VNCommon.GetSplitValue(_splitArray, 3, "1.0"))
                    });
                }
                else
                {
                    DevelopeLog.LogError("파티클 비 명령어를 찾을수 없습니다! [" + _splitArray.ToString() + "]");
                }
            }
            else
            {
                DevelopeLog.LogError(StringHelper.Format("파티클 명령어를 찾을수 없습니다! [{0}]", _readLine));
            }
        }

        /// <summary>
        /// 시스템 스크립트
        /// </summary>
        void ParseSystem()
        {
            if (_readLine.Contains("스크립트 종료"))
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "SYSTEM_END")
                });
            }
            else if (_readLine.Contains("화면 초기화"))
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "SYSTEM_SCREEN_CLEAR")
                });
            }
            else if (_readLine.Contains("진동"))
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "SYSTEM_VIBRATE")
                });
            }
            else if (_readLine.Contains("대기"))
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "SYSTEM_WAIT", "Time", VNCommon.GetSplitValue(_splitArray,2, "1.0"))
                }); 
            }
            else if (_readLine.Contains("로드 시작"))
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "SYSTEM_START")
                });
            }
            else if (_readLine.Contains("로드 종료"))
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "SYSTEM_END")
                });
            }
            else
            {
                DevelopeLog.LogError(StringHelper.Format("시스템 명령어를 찾을수 없습니다! [{0}]", _readLine));
            }
        }

        /// <summary>
        /// 중지 스크립트
        /// </summary>
        void ParseStop()
        {
            if (_splitArray[1] == "BGM")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "STOP_BGM")
                });
            }
            else
            {
                DevelopeLog.LogError(StringHelper.Format("중지 명령어를 찾을수 없습니다! [{0}]", _readLine));
            }
        }

        /// <summary>
        /// 실행 스크립트
        /// </summary>
        void ParsePlay()
        {
            if (_splitArray[1] == "BGM")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "PLAY_BGM", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Loop", VNCommon.GetSplitValue(_splitArray, 3, "반복안함"),"Delay",VNCommon.GetSplitValue(_splitArray, 4, "0"))
                });
            }
            else if (_splitArray[1] == "SE")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "PLAY_SE", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Delay", VNCommon.GetSplitValue(_splitArray, 3, "0"), "Loop", VNCommon.GetSplitValue(_splitArray, 4, "반복안함"))
                });
            }
            else if (_splitArray[1] == "선택지")
            {
                if (_writeBehaviorData != null)
                {
                    DevelopeLog.LogError("작성중인 선택지 데이터가 있습니다");
                }
                _writeBehaviorData = new BehaviorData();
                _writeBehaviorData.hashTable.Add("Form", "PLAY_DISTRACTOR");
                _writeBehaviorData.hashTable.Add("DistractorList", "");
                _writeBehaviorData.hashTable.Add("ValueCount", VNCommon.GetSplitValue(_splitArray, 2, "2"));
                _writeBehaviorData.hashTable.Add("NotChooseTime", VNCommon.GetSplitValue(_splitArray, 3, "0"));
            }
            else if (_splitArray[1] == "배경")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "PLAY_BACKGROUND", "Name", VNCommon.GetSplitValue(_splitArray, 2))
                });
            }
            else
            {
                DevelopeLog.LogError(StringHelper.Format("실행 명령어를 찾을수 없습니다! [{0}]", _readLine));
            }
        }

        /// <summary>
        /// 로드 스크립트
        /// </summary>
        void ParseLoad()
        {
            if (_splitArray[1] == "BGM")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "LOAD_BGM", "Name", VNCommon.GetSplitValue(_splitArray, 2))
                });
            }
            else if (_splitArray[1] == "SE")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "LOAD_SE", "Name", VNCommon.GetSplitValue(_splitArray, 2))
                });
            }
            else if (_splitArray[1] == "캐릭터")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "LOAD_CHARACTER", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Direction" , VNCommon.GetSplitValue(_splitArray,3))
                });
            }
            else if (_splitArray[1] == "배경")
            {
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "LOAD_BACKGROUND", "Name", VNCommon.GetSplitValue(_splitArray, 2))
                });
            }
            else
            {
                DevelopeLog.LogError(StringHelper.Format("로드 명령어를 찾을수 없습니다! [{0}]", _readLine));
            }
        }

        /// <summary>
        /// 액션 스크립트
        /// </summary>
        void ParseAction()
        {
            #region 캐릭터
            if (_splitArray[1] == "캐릭터")
            {
                BehaviorData data = new BehaviorData();
                data.hashTable = VNCommon.Hash("Name", VNCommon.GetSplitValue(_splitArray, 2), "Direction", VNCommon.GetSplitValue(_splitArray, 3),"Time", VNCommon.GetSplitValue(_splitArray, 5));
                if (VNCommon.GetSplitValue(_splitArray, 4) == "출연")
                {
                    data.hashTable.Add("Form", "ACTION_CHARACTER_MOVE_IN");
                }
                if (VNCommon.GetSplitValue(_splitArray, 4) == "출연")
                {
                    data.hashTable.Add("Form", "ACTION_CHARACTER_MOVE_OUT");
                }
                GameDataManager.getInstance._behaviorList.Add(data);
            }
            #endregion
            #region 화면
            else if (_splitArray[1] == "화면")
            {
                BehaviorData data = new BehaviorData();
                if (VNCommon.GetSplitValue(_splitArray, 2) == "페이드인")
                {
                    data.hashTable.Add("Form", "ACTION_SCENE_FADEIN");
                }
                else if (VNCommon.GetSplitValue(_splitArray, 2) == "페이드아웃")
                {
                    data.hashTable.Add("Form", "ACTION_SCENE_FADEOUT");
                }
                else if (VNCommon.GetSplitValue(_splitArray, 2) == "쉐이크")
                {
                    data.hashTable.Add("Form", "ACTION_SCENE_SHAKE");
                }
                data.hashTable.Add("Time", VNCommon.GetSplitValue(_splitArray, 3, "1.0"));
                GameDataManager.getInstance._behaviorList.Add(data);
            }
            #endregion
            #region 대화창
            else if (_splitArray[1] == "대화창")
            {
                BehaviorData data = new BehaviorData();

                if (VNCommon.GetSplitValue(_splitArray, 2) == "숨김")
                {
                    data.hashTable.Add("Form", "ACTION_CHATBOX_HIDE");
                }
                else if (VNCommon.GetSplitValue(_splitArray, 2) == "표시")
                {
                    data.hashTable.Add("Form", "ACTION_CHATBOX_SHOW");
                }
                data.hashTable.Add("Time", VNCommon.GetSplitValue(_splitArray, 3, "0"));
                GameDataManager.getInstance._behaviorList.Add(data);
            }
            #endregion
            #region 글자
            else if (_splitArray[1] == "글자")
            {
                BehaviorData data = new BehaviorData();
                if (VNCommon.GetSplitValue(_splitArray, 2) == "페이드인")
                {
                    data.hashTable.Add("Form", "ACTION_TEXT_FADEIN");
                }
                else if (VNCommon.GetSplitValue(_splitArray, 2) == "페이드아웃")
                {
                    data.hashTable.Add("Form", "ACTION_TEXT_FADEOUT");
                }
                data.hashTable.Add("Time", VNCommon.GetSplitValue(_splitArray, 3, "1"));
                GameDataManager.getInstance._behaviorList.Add(data);
            }
            #endregion
            #region 선택지
            else if (_splitArray[1] == "선택지")
            {
                if (VNCommon.GetSplitValue(_splitArray, 2) == "페이드아웃")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_DISTRACTOR_FADEOUT", "Time", VNCommon.GetSplitValue(_splitArray, 3 , "1.0"))
                    });
                }
                else
                {
                    DevelopeLog.LogError(StringHelper.Format("액션 선택지 명령어를 찾을수 없습니다! [{0}]", _readLine));
                }
            }
            #endregion
            #region 배경
            else if (_splitArray[1] == "배경")
            {
                if (VNCommon.GetSplitValue(_splitArray, 3) == "페이드인")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_BACKGROUND_FADEIN", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time", VNCommon.GetSplitValue(_splitArray, 4, "1.0"), "Axis", VNCommon.GetSplitValue(_splitArray, 5, "NONE"))
                    });
                }
                else if (VNCommon.GetSplitValue(_splitArray, 3) == "흐려지기")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_BACKGROUND_CLOUD", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time", VNCommon.GetSplitValue(_splitArray, 4, "1.0"), "Axis", VNCommon.GetSplitValue(_splitArray, 5, "NONE"))
                    });
                }
                else if (VNCommon.GetSplitValue(_splitArray, 3) == "맑아지기")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_BACKGROUND_CLEAN", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time", VNCommon.GetSplitValue(_splitArray, 4, "1.0"), "Axis", VNCommon.GetSplitValue(_splitArray, 5, "NONE"))
                    });
                }
                else if (VNCommon.GetSplitValue(_splitArray, 3) == "밀어내기")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_BACKGROUND_PUSH", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time", VNCommon.GetSplitValue(_splitArray, 4, "1.0"), "Direction", VNCommon.GetSplitValue(_splitArray, 5, "오른쪽"))
                    });
                }
                else
                {
                    DevelopeLog.LogError(StringHelper.Format("액션 배경 명령어를 찾을수 없습니다! [{0}]", _readLine));
                }
            }
            #endregion
            #region BGM
            else if (_splitArray[1] == "BGM")
            {
                if (VNCommon.GetSplitValue(_splitArray, 3) == "페이드아웃")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_BGM_FADEOUT", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time", VNCommon.GetSplitValue(_splitArray, 4, "1.0"))
                    });
                }
                else if (VNCommon.GetSplitValue(_splitArray, 3) == "페이드인")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_BGM_FADEIN", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time", VNCommon.GetSplitValue(_splitArray, 4, "1.0"))
                    });
                }
                else
                {
                    DevelopeLog.LogError(StringHelper.Format("액션 BGM 명령어를 찾을수 없습니다! [{0}]", _readLine));
                }
            }
            #endregion
            #region SE
            else if (_splitArray[1] == "SE")
            {
                if (VNCommon.GetSplitValue(_splitArray, 3) == "페이드아웃")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_SE_FADEOUT", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time" , VNCommon.GetSplitValue(_splitArray,4,"1.0"))
                    });
                }
                else if (VNCommon.GetSplitValue(_splitArray, 3) == "페이드인")
                {
                    GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                    {
                        hashTable = VNCommon.Hash("Form", "ACTION_SE_FADEIN", "Name", VNCommon.GetSplitValue(_splitArray, 2), "Time", VNCommon.GetSplitValue(_splitArray, 4, "1.0"))
                    });
                }
                else
                {
                    DevelopeLog.LogError(StringHelper.Format("액션 SE 명령어를 찾을수 없습니다! [{0}]", _readLine));
                }
            }
            #endregion
            else
            {
                DevelopeLog.LogError(StringHelper.Format("액션 명령어를 찾을수 없습니다! [{0}]", _readLine));
            }
        }

        /// <summary>
        /// 대화 스크립트
        /// </summary>
        void ParseDialogue()
        {
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
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "DIALOGUE_SPEAKER", "Speaker", speaker.ToString())
                });
            }
            else if (_readLine[0].CompareTo('@') == 0)
            {
                string talk = _readLine.Substring(1, _readLine.Length - 1);
                GameDataManager.getInstance._behaviorList.Add(new BehaviorData()
                {
                    hashTable = VNCommon.Hash("Form", "DIALOGUE_TALK", "Talk", talk)
                });
            }
        }
    }
}
