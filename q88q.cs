using System;
using FluentScheduler;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

public class q8qserver
{

    static RestClient client = new RestClient(baseUrl: "https://www.q88q.cyou/");

    public  void Run()
    {
        #region 初始化请求环境
        string email = Environment.GetEnvironmentVariable("email");
        string passwd = Environment.GetEnvironmentVariable("passwd");

        #endregion
        
        #region 默认先进行一次登录
        Login(email, passwd);
        CheckIn();
        #endregion
        
        #region 初始化任务调度
        ///每3天登录
        JobManager.AddJob(() =>
        {
            Login(email, passwd);
        }, s => s.ToRunEvery(3).Days().At(0, 1));
        //每一天签到
        JobManager.AddJob(CheckIn, s => s.ToRunEvery(1).Days().At(0, 1));
        #endregion

       
    }
    public void Login(string? email, string? passwd)
    {
        Extensions.ErgodicAction(() =>
        {
            var request = new RestRequest("/auth/login", Method.Post);
            request.AddParameter("email", email);
            request.AddParameter("passwd", passwd);
            var response = client.ExecutePostAsync(request).Result;
            client.CookieContainer.Add(response?.Cookies);
            var jsondata = JsonConvert.DeserializeObject<Result>(response?.Content);
            Log.Information(jsondata.msg);
            Notify.Send("q88q签到", jsondata.msg);
            return true;
        });
    }
    public void CheckIn()
    {
        Extensions.ErgodicAction(() =>
        {
            var request = new RestRequest("/user/checkin", Method.Post);
            var response = client.ExecutePostAsync(request).Result;
            var res = response.Content;
            var jsondata = JsonConvert.DeserializeObject<Result>(res);
            Log.Information(jsondata.msg);
             Notify.Send("q88q签到", jsondata.msg);
            return true;
        });

    }
    ~q8qserver()
    {
        client.Dispose();
    }
}