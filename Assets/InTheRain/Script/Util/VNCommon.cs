using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Text.RegularExpressions;

namespace VNEngine
{
    public class VNCommon
    {

        /// <summary>
        /// Split 데이터의 유무 판단
        /// </summary>
        /// <param name="inSplitArray"></param>
        /// <param name="inBaseValue"></param>
        /// <returns></returns>
        public static string GetSplitValue(string[] inSplitArray,int inIndex, string inBaseValue = "ERROR")
        {
            string value = inBaseValue;
            if (inSplitArray.Length > inIndex)
            {
                value = inSplitArray[inIndex];
            }
            else if (value == "ERROR")
            {
                Debug.LogError(StringHelper.Format("{0} {1}번째 인덱스는 포함되어야합니다!", inSplitArray.ToString(), inIndex));
            }
            return value;
        }

        /// <summary>
        /// 해시 테이블 생성 함수
        /// </summary>
        /// <returns>파라메터로 생성된 해시 테이블.</returns>
        /// <param name="args">파라메터 나열(키, 벨류, 키, 벨류, ...).</param>

        public static Hashtable Hash(params object[] args)
        {
            Hashtable hashTable = new Hashtable(args.Length / 2);
            if (args.Length % 2 != 0)
            {
                Debug.LogError("Hash requires an even number of arguments!");
                return null;
            }
            else
            {
                int i = 0;
                while (i < args.Length - 1)
                {
                    hashTable.Add(args[i], args[i + 1]);
                    i += 2;
                }
                return hashTable;
            }
        }
    }
}
