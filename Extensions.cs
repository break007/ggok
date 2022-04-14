using System.Runtime.InteropServices;
using Serilog;

public static class Extensions
{
    public static bool IsEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    /// <summary>
    /// 循环执行方法 直到次数用完 或者方法返回true
    /// </summary>
    /// <param name="action"></param>
    /// <param name="ergodicNumber"></param>
    public static void ErgodicAction(Func<bool> action, int ergodicNumber = 5, int sleepSecond = 1)
    {
        for (int i = 0; i < ergodicNumber; i++)
        {
            Thread.Sleep(sleepSecond * 1000);
            bool isActionSuccess;
            try
            {
                isActionSuccess = action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.ToString());
               isActionSuccess=false;
            }
            if (isActionSuccess)
            {
                break;
            }
        }
    }
}

