using GenovaAPI.Models;
using GenovaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenovaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController  : ControllerBase
{

    private readonly BinanceService _binanceService;
    public CryptoController()
    {
        _binanceService = new BinanceService(); 
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest()
    {
        var symbols = new List<string> { "BTC", "ETH", "LTC" };
        
        var tasks = symbols.Select(async symbol =>
        {

            var price = await _binanceService.GetCryptoPriceAsync(symbol);
            return new CryptoResult
            {
                Symbol = symbol,
                Price = price
            };

        });
        var result = await Task.WhenAll(tasks);
        return Ok(result); 
    }

    [HttpGet("history/{symbol}")]
    public async Task<IActionResult> GetHistoru(string symbol, [FromQuery] int day = 7)
    {
        var coinGeckoService = new CoinGeckoServices();
        var data = await coinGeckoService.GetHistoricalPricesAsync(symbol, day);
        
        var result = data.Select(p => new PricePoint
        {
            Time = p.time,
            Price = p.price
        }).ToList();
        
        return Ok(result);
    }
}