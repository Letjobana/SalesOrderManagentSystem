using System;
using System.Net.Http;
using System.Threading.Tasks;
using Imperial.SalesOrder.DataModel;
using Newtonsoft.Json;

public class DogApiService
{
    private readonly HttpClient _httpClient;

    public DogApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string GetRandomDogImageUrl()
    {
        var response = _httpClient.GetAsync("https://dog.ceo/api/breeds/image/random").Result;

        if (response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            var apiResponse = JsonConvert.DeserializeObject<DogApiResponse>(content);

            if (apiResponse.Status == "success")
            {
                return apiResponse.Message;
            }
        }

        throw new Exception("Failed to fetch dog image from the API");
    }
}

