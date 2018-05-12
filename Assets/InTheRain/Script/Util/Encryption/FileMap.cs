using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VNFramework
{
    /// <summary>
    /// 여러 타입의 벨류를 암호화하여 파일로 저장하거나 불러올 때 사용하는 클래스.
    /// 대용량의 데이터를 파일로 기록하거나 파일로부터 불러오는 용도로 사용하기 때문에 메모리 상에서는 별도의 암호화를 하지 않는다.
    /// </summary>

	[Serializable]
    public class FileMap
    {
        protected Dictionary<System.Type, Dictionary<string, object>> _dic = new Dictionary<Type, Dictionary<string, object>>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inKey"></param>
        /// <param name="inValue"></param>
        public void Set<T>(string inKey, object inValue)
        {
            System.Type type = typeof(T);
            if (!_dic.ContainsKey(type))
            {
                _dic.Add(type, new Dictionary<string, object>());
            }

            if (_dic[type].ContainsKey(inKey))
            {
                _dic[type][inKey] = inValue;
            }
            else
            {
                _dic[type].Add(inKey, inValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return Has<T>(key) ? (T)_dic[typeof(T)][key] : default(T);
        }

        public Dictionary<string, T> GetDictionary<T>()
        {
            System.Type type = typeof(T);
            if (!_dic.ContainsKey(type))
            {
                return null;
            }

            Dictionary<string, T> returnDic = new Dictionary<string, T>();
            foreach (var keyValue in _dic[type])
            {
                returnDic.Add(keyValue.Key, (T)keyValue.Value);
            }

            return returnDic;
        }

        public void SetDictionary<T>(Dictionary<string, T> inDic)
        {
            System.Type type = typeof(T);
            if (_dic.ContainsKey(type))
            {
                _dic[type].Clear();
            }
            else
            {
                _dic.Add(type, new Dictionary<string, object>());
            }

            foreach (var keyValue in inDic)
            {
                _dic[type].Add(keyValue.Key, keyValue.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Has<T>(string key)
        {
            System.Type type = typeof(T);
            if (_dic.ContainsKey(type)
                && _dic[type].ContainsKey(key))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public void Remove<T>(string key)
        {
            if (Has<T>(key))
                _dic[typeof(T)].Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Clear<T>()
        {
            System.Type type = typeof(T);
            if (_dic.ContainsKey(type))
            {
                _dic[type].Clear();
            }
        }

        /// <summary>
        /// 모든 키-벨류 값 삭제.
        /// </summary>
        public void Clear()
        {
            _dic.Clear();
        }
    }
}
