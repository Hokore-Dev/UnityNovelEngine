using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using VNFramework;

public class FileIOExtension {

    public static byte[] Serialize(object inObject)
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter f = new BinaryFormatter();
        f.Serialize(ms, inObject);

        byte[] arr = ms.ToArray();
        ms.Close();
        return arr;
    }

    public static T Deserialize<T>(byte[] inByte)
    {
        MemoryStream ms = new MemoryStream(inByte);
        BinaryFormatter f = new BinaryFormatter();
        T output = (T)f.Deserialize(ms);
        ms.Close();
        return output;
    }

    public static T Copy<T>(T inObject)
    {
        return Deserialize<T>(Serialize(inObject));
    }

    /// <summary>
    /// 파일 저장하기.
    /// </summary>
    /// <param name="inObject">타겟 오브젝트</param>
    /// <param name="inFilePath">저장할 위치</param>
    /// <param name="inSecretKey">암호화키</param>
    /// <returns></returns>
    public static bool SaveAsFile(object inObject, string inFilePath, string inSecretKey)
    {
        if (inObject == null)
        {
            Debug.Log("저장할 데이터가 없습니다.");
            return false;
        }

        string folderPath = inFilePath.Substring(0, inFilePath.LastIndexOf("/"));
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        try
        {
            MemoryStream ms = new MemoryStream();

            BinaryFormatter f = new BinaryFormatter();
            f.Serialize(ms, inObject);

            byte[] byteArr = AES.EncryptFromStream(ms.ToArray(), inSecretKey);

            ms.Close();

            FileStream fs = new FileStream(inFilePath, FileMode.Create);
            fs.Write(byteArr, 0, byteArr.Length);
            fs.Close();
        }
        catch (System.IO.IOException e)
        {
            Debug.Log(string.Format("{0} 파일 저장에 실패하였습니다.({1})", inFilePath, e.ToString()));
            return false;
        }

        Debug.Log(inFilePath + " 파일 저장을 완료하였습니다.");

        return true;
    }

    /// <summary>
    /// 파일 불러오기.
    /// </summary>
    /// <param name="inObject">타겟 오브젝트</param>
    /// <param name="inFilePath">저장할 위치</param>
    /// <param name="inSecretKey">암호화키</param>
    /// <returns>성공 여부</returns>
    public static T LoadFromFile<T>(string inFilePath, string inSecretKey)
    {
        if (!System.IO.File.Exists(inFilePath))
        {
            Debug.Log(inFilePath + "의 파일이 존재하지 않습니다.");
            return default(T);
        }

        try
        {
            FileStream fs = new FileStream(inFilePath, FileMode.Open);
            byte[] byteArr = new byte[fs.Length];
            fs.Read(byteArr, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();

            byte[] result = AES.DecryptFromStream(byteArr, inSecretKey);
            MemoryStream ms = new MemoryStream(result);
            BinaryFormatter f = new BinaryFormatter();
            T output = (T)f.Deserialize(ms);
            ms.Close();
            return output;
        }
        catch (System.IO.IOException e)
        {
            Debug.Log(e.ToString());
            return default(T);
        }
    }

    public static bool RemoveFile(string inFilePath)
    {
        if (!System.IO.File.Exists(inFilePath))
        {
            Debug.Log(inFilePath + "의 파일이 존재하지 않습니다.");
            return false;
        }

        File.Delete(inFilePath);
        return true;
    }
}
