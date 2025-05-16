using System.Text.Json;

namespace GenovaAPI.Services;

public class CoinGeckoServices
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<(DateTime time, decimal price)>> GetHistoricalPricesAsync(string symbol, int days)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "BTC", "bitcoin" },
            { "ETH", "ethereum" },
            { "BNB", "binancecoin" },
            { "SOL", "solana" },
            { "XRP", "ripple" },
            { "LTC", "litecoin" }
        };

        if (!map.TryGetValue(symbol, out var apiName))
            return [];
        
        var url = $"https://api.coingecko.com/api/v3/coins/{apiName}/market_chart?vs_currency=usd&days={days}";
        var response = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(response);
        var pricesArray = doc.RootElement.GetProperty("prices");
        
        var result = new List<(DateTime time, decimal price)>();

        foreach (var item in pricesArray.EnumerateArray())
        {
            var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(item[0].GetInt64()).UtcDateTime;
            var price = item[1].GetDecimal();
            result.Add((timestamp, price));
        }
        
        return result;
    }

}