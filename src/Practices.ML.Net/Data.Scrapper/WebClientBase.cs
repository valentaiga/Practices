using System.Net;

namespace Data.Scrapper;

public abstract class WebClientBase
{
    protected readonly HttpClient HttpClient;

    protected WebClientBase()
    {
        const string baseAddress = "https://www.hltv.org/";
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress),
            Timeout = TimeSpan.FromSeconds(3),
            DefaultRequestHeaders =
            {
                // { ":authority", "www.hltv.org" },
                // { ":method", "GET" },
                // { ":scheme", "https" },
                { "referer", "https://www.hltv.org/results" },
                { "user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36" },
                { "sec-ch-ua", "\"Chromium\";v=\"106\", \"Google Chrome\";v=\"106\", \"Not;A=Brand\";v=\"99\"" },
            }, 
        };
    }

    protected void EnsureSuccessResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Response is not success. Reason: {response.ReasonPhrase}");
    }
}