using RestSharp;
using Serilog;

public class Notify
{
    static RestClient client = new RestClient();
    public static void Send(string title, string content)
    {
        var request = new RestRequest("https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=c4de16aa-91a8-496b-ae54-2c5d48bdc86d");
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(new { msgtype = "text", text = new { content = title+"："+content } });
        var response = client.ExecutePostAsync(request);
        Log.Information(response.Result.Content);

    }
}
