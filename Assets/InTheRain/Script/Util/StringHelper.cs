using System.Text;

/// <summary>
/// string.Format()은 포멧 문자열 처리를 위해 매우 빈번하게 사용되지만 
/// 100 bytes GC 대상 할당이 발생하는 단점이 있으므로 매 프레임 처리하는 상황에서는 지양해야 한다.
/// 따라서 stringBuilder를 이용한 format접근이 용이하다 
/// </summary>
public static class StringHelper
{
    static StringBuilder sb = new StringBuilder();

    public static string Format(string format, params object[] args)
    {
        sb.Length = 0;
        sb.AppendFormat(format, args);
        return sb.ToString();
    }
}
