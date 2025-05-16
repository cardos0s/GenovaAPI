namespace GenovaAPI.Services;

public class BinanceService
{
    private readonly HttpClient _httpClient = new();

    public async Task<decimal?> GetCryptoPriceAsync(string symbol)
    {
        var fullSymbol = $"{symbol}USDT".ToUpper();
        var url = $"https://api.binance.com/api/v3/ticker/price?symbol={fullSymbol}";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<BinancePriceResponse>(url);
            return response?.price;
        }
        catch (Exception e)
        {
            Console.WriteLine("Não foi possível acessar");
            return null;
        }
    }
}

public class BinancePriceResponse
{ 
    public decimal price { get; set; }
    public string symbol { get; set; }  = string.Empty;
}
        
    
    
