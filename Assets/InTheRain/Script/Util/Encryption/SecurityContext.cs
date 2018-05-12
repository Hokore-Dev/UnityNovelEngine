using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace VNFramework
{

    /// <summary>
    /// 여러 타입의 데이터를 메모리상에서 암호화하여 관리하기 위한 클래스.
    /// </summary>

    [Serializable]
    public class SecurityContext
    {
        [Serializable]
        public class SecurityValue
        {
            uint _checksum = 0xFFFFFFFF;

            short _beginCursor = 0; // [0][1][2][3][4][5][6][7]
            short _direction = 0;   // 0 = <- , 1 = ->

            int _byteSize;

            byte[,] _seperatedValueList = new byte[4, 8];
            byte[] _masks = new byte[4];

            public static SecurityValue Create(byte[] inData)
            {
                SecurityValue value = new SecurityValue();
                value.SetValue(inData);
                return value;
            }

            public void SetValue(byte[] inData)
            {
                _byteSize = inData.Length;
                GenerateUniqueMask();
                _beginCursor = (short)UnityEngine.Random.Range(0, _byteSize);
                _direction = (UnityEngine.Random.Range(0, 2) == 0) ? (short)-1 : (short)1;

                _checksum = CRC32.Encrypt(inData);

                for (int i = 0; i < _byteSize; i++)
                {
                    int columnIndex = _beginCursor + (_direction * i);
                    if (columnIndex < 0)
                        columnIndex += _byteSize;
                    else if (columnIndex > 7)
                        columnIndex %= 8;

                    byte inverseByte = (byte)~inData[i];
                    for (int j = 0; j < 4; j++)
                    {
                        int rowIndex = (columnIndex + j) % 4;
                        byte byteValue = (byte)(inverseByte & _masks[j]);
                        _seperatedValueList[rowIndex, columnIndex] = byteValue;
                    }
                }
            }

            public byte[] GetValue()
            {
                byte[] totalValueArr = new byte[_byteSize];

                for (int i = 0; i < _byteSize; i++)
                {
                    int columnIndex = _beginCursor + (_direction * i);
                    if (columnIndex < 0)
                        columnIndex += _byteSize;
                    else if (columnIndex > 7)
                        columnIndex %= 8;

                    byte byteValue = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        int rowIndex = (columnIndex + j) % 4;
                        byteValue |= _seperatedValueList[rowIndex, columnIndex];
                    }

                    totalValueArr[i] = (byte)~byteValue;
                }

                if (_checksum != CRC32.Encrypt(totalValueArr))
                {
                    byte[] zero = new byte[_byteSize];
                    for (int i = 0; i < _byteSize; i++)
                    {
                        zero[i] = 0;
                    }

                    SetValue(zero);
                    return zero;
                }

                return totalValueArr;
            }

            private void GenerateUniqueMask()
            {
                int i = UnityEngine.Random.Range(1, 16);
                byte seed = System.BitConverter.GetBytes(i)[0];

                _masks[0] = (byte)(seed & 0x00aa);
                _masks[1] = (byte)(seed & 0x0055);
                _masks[2] = (byte)(~seed & 0x00aa);
                _masks[3] = (byte)(~seed & 0x0055);
            }
        }

        /// <summary>
        /// 스트링을 암호화 하기 위한 클래스
        /// </summary>

        [Serializable]
        public class SecurityString
        {
            uint _crcKey;
            string _value;

            public SecurityString(string inKey, string inValue)
            {
                Set(inKey, inValue);
            }

            public void Set(string inKey, string inValue)
            {
                _value = inValue;
                _crcKey = CRC32.Encrypt(string.Format("{0}{1}", inKey, _value));
            }

            public string Get(string inKey)
            {
                if (CRC32.Encrypt(string.Format("{0}{1}", inKey, _value)) == _crcKey)
                {
                    return _value;
                }

                return null;
            }
        }

        /* 스트링 외 데이터 저장소 */
        private Dictionary<Type, Dictionary<string, SecurityValue>> _securityKeyDic = new Dictionary<Type, Dictionary<string, SecurityValue>>(); // 타입과 암호화된 값으로 이루어진 딕셔너리.

        /* 스트링 데이터 저장소 */
        private Dictionary<string, SecurityString> _strKeyDic = new Dictionary<string, SecurityString>();

        /// <summary>
        /// 데이터를 바이트 배열로 변환하여 저장
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="inKey">키</param>
        /// <param name="inData">저장할 데이터</param>
        private void SetByte<T>(string inKey, byte[] inData)
        {
            Type type = typeof(T);
            if (!_securityKeyDic.ContainsKey(type))
            {
                return;
            }


            if (_securityKeyDic[type].ContainsKey(inKey))
            {
                _securityKeyDic[type][inKey].SetValue(inData);
            }
            else
            {
                _securityKeyDic[type].Add(inKey, SecurityValue.Create(inData));
            }
        }

        /// <summary>
        /// 바이트 배열로 저장된 데이터를 가져옴
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="inKey">키</param>
        /// <returns></returns>
        private byte[] GetByte<T>(string inKey)
        {
            Type type = typeof(T);
            if (_securityKeyDic.ContainsKey(type) && _securityKeyDic[type].ContainsKey(inKey))
            {
                return _securityKeyDic[type][inKey].GetValue();
            }

            return null;
        }

        /// <summary>
        /// 데이터 삭제
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="inKey">키</param>
        public void Remove<T>(string inKey)
        {
            Type type = typeof(T);
            if (type == typeof(string))
            {
                _strKeyDic.Remove(inKey);
            }
            else if (_securityKeyDic.ContainsKey(type) && _securityKeyDic[type].ContainsKey(inKey))
            {
                _securityKeyDic[type].Remove(inKey);
            }
        }

        /// <summary>
        /// 데이터 테이블에서 입력된 타입과 동일한 데이터를 삭제
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        public void Clear<T>()
        {
            Type type = typeof(T);
            if (type == typeof(string))
            {
                _strKeyDic.Clear();
            }
            else if (_securityKeyDic.ContainsKey(type))
                _securityKeyDic[type].Clear();
        }

        /// <summary>
        /// 데이터 기록
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="key">키</param>
        /// <param name="value">데이터</param>
        public void Set<T>(string key, object value) where T : IConvertible
        {
            System.Type type = typeof(T);

            byte[] byteArr = null;
            if (type == typeof(bool)) { byteArr = BitConverter.GetBytes((bool)value); }
            else if (type == typeof(short)) { byteArr = BitConverter.GetBytes((short)value); }
            else if (type == typeof(ushort)) { byteArr = BitConverter.GetBytes((ushort)value); }
            else if (type == typeof(int)) { byteArr = BitConverter.GetBytes((int)value); }
            else if (type == typeof(uint)) { byteArr = BitConverter.GetBytes((uint)value); }
            else if (type == typeof(long)) { byteArr = BitConverter.GetBytes((long)value); }
            else if (type == typeof(ulong)) { byteArr = BitConverter.GetBytes((ulong)value); }
            else if (type == typeof(float)) { byteArr = BitConverter.GetBytes((float)value); }
            else if (type == typeof(double)) { byteArr = BitConverter.GetBytes((double)value); }
            else if (type == typeof(char)) { byteArr = BitConverter.GetBytes((char)value); }
            else if (type == typeof(string))
            {
                if (_strKeyDic.ContainsKey(key))
                {
                    _strKeyDic[key].Set(key, (string)value);
                }
                else
                {
                    _strKeyDic.Add(key, new SecurityString(key, (string)value));
                }
                return;
            }
            else
            {
                UnityEngine.Debug.Log(string.Format("{0} 타입은 암호화할 수 없습니다.", type));
                return;
            }

            if (!_securityKeyDic.ContainsKey(type))
            {
                _securityKeyDic.Add(type, new Dictionary<string, SecurityValue>());
            }

            if (_securityKeyDic[type].ContainsKey(key))
            {
                _securityKeyDic[type][key].SetValue(byteArr);
            }
            else
            {
                _securityKeyDic[type].Add(key, SecurityValue.Create(byteArr));
            }
        }

        /// <summary>
        /// 데이터 가져오기
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="key">키</param>
        /// <returns>타입에 적합하게 변환된 데이터</returns>
        public T Get<T>(string key) where T : IConvertible
        {
            System.Type type = typeof(T);

            object result = null;

            if (type == typeof(string))
            {
                if (_strKeyDic.ContainsKey(key))
                {
                    result = _strKeyDic[key].Get(key);
                }
            }
            else
            {
                byte[] data = GetByte<T>(key);
                if (data == null)
                    return default(T);

                if (type == typeof(bool)) { result = BitConverter.ToBoolean(data, 0); }
                else if (type == typeof(short)) { result = BitConverter.ToInt16(data, 0); }
                else if (type == typeof(ushort)) { result = BitConverter.ToUInt16(data, 0); }
                else if (type == typeof(int)) { result = BitConverter.ToInt32(data, 0); }
                else if (type == typeof(uint)) { result = BitConverter.ToUInt32(data, 0); }
                else if (type == typeof(long)) { result = BitConverter.ToUInt64(data, 0); }
                else if (type == typeof(ulong)) { result = BitConverter.ToUInt64(data, 0); }
                else if (type == typeof(float)) { result = BitConverter.ToSingle(data, 0); }
                else if (type == typeof(double)) { result = BitConverter.ToDouble(data, 0); }
                else if (type == typeof(char)) { result = BitConverter.ToChar(data, 0); }
                else
                {
                    UnityEngine.Debug.Log(string.Format("{0} 타입은 불러올 수 없습니다."));
                    return default(T);
                }
            }

            if (result != null)
                return (T)result;

            return default(T);
        }

        /// <summary>
        /// 모든 타입의 저장된 값을 클리어.
        /// </summary>
        public void Clear()
        {
            _securityKeyDic.Clear();
            _strKeyDic.Clear();
        }
    }
}
