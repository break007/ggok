using System.Runtime.InteropServices;
using Serilog;

public static class Extensions
{
    public static bool IsEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    public static string GetEnvironmentVariable(string key)
    {
        string str = Environment.GetEnvironmentVariable(key, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.Process);
        return str;

    }
    public static int? GetEnvironmentVariableInt(string key)
    {
        try
        {
            return int.Parse(GetEnvironmentVariable(key));
        }
        catch (Exception)
        {
            return null;
        }
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
                Log.Error(ex, "执行方法出错");
                isActionSuccess = false;
            }
            if (isActionSuccess)
            {
                break;
            }
        }
    }
}

