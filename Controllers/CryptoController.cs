using GenovaAPI.Models;
using GenovaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenovaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController  : ControllerBase
{  
    private static readonly List<CryptoItem> _cryptoItems = new();

    private readonly BinanceService _binanceService;
    public CryptoController()
    {
        _binanceService = new BinanceService(); 
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest()
    {
        var list = new List<CryptoItem>
        {
            new CryptoItem
            {
                Icon = "https://cryptologos.cc/logos/bitcoin-btc-logo.png",
                ActionTitle = "Bought Bitcoin",
                FiatValue = "$145.00",
                CryptoValue = "0.0034 BTC",
                TimeAgo = "2h ago"
            },
            new CryptoItem
            {
                Icon = "https://cryptologos.cc/logos/ethereum-eth-logo.png",
                ActionTitle = "Bought Ethereum",
                FiatValue = "$115.00",
                CryptoValue = "0.0013 ETH",
                TimeAgo = "1h ago"
            }
        };

        return Ok(list);
    }

    [HttpGet("history/{symbol}")]
    public async Task<IActionResult> GetHistory(string symbol, [FromQuery] int day = 7)
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
    
    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(_cryptoItems);
    }
    
    [HttpPost("add")]
    public IActionResult AddCrypto([FromBody] CryptoItem item)
    {
        if (item == null)
            return BadRequest("Item inválido.");

        _cryptoItems.Add(item);
        return Ok(item);
    }
}