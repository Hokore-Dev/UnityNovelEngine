using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {
    public const float SCREEN_WIDTH = 1280f;
    public const float SCREEN_HEIGHT = 720f;

    /// <summary>
    /// 스크린 크기에 맞게 화면 리사이즈
    /// </summary>
    public static void SetResolution(Camera inCamera)
    {
        if (Screen.width / Screen.height < SCREEN_WIDTH / SCREEN_HEIGHT)
        {
            float width = (SCREEN_HEIGHT * 0.5f) / SCREEN_HEIGHT * SCREEN_WIDTH;
            inCamera.orthographicSize = width / Screen.width * Screen.height;
        }
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

    /// <summary>
    /// 같은 타입의 두 벨류를 상호 교환해주는 함수.
    /// </summary>
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    public static float[] ConvertByteArrayToFloat(byte[] bytes)
    {
        //if (bytes.Length % 4 != 0) throw new System.ArgumentException();

        float[] floats = new float[bytes.Length / 4];
        for (int i = 0; i < floats.Length; i++)
        {
            floats[i] = System.BitConverter.ToSingle(bytes, i * 4);
        }
        return floats;
    }
}
