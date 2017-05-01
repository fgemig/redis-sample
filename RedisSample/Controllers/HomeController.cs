using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisSample.Models;
using System;

namespace RedisSample.Controllers
{
    public class HomeController : Controller
    {

        private readonly IDistributedCache _cache;

        public HomeController(IDistributedCache cache)
        {
            _cache = cache;
        }

        private void AdicionarValorCache(string chave, string valor)
        {
            DistributedCacheEntryOptions opcoes =
                new DistributedCacheEntryOptions();

            opcoes.SetAbsoluteExpiration(DateTime.UtcNow.AddMinutes(30));
            _cache.SetString(chave, valor, opcoes);
        }

        public IActionResult Index()
        {

            ExemploRedisConteudoString();
            ExemploRedisConteudoObjeto();

            return View();
        }

        public void ExemploRedisConteudoString()
        {
            var valorString = _cache.GetString("ValorString");

            if (valorString == null)
            {
                valorString = "The Dark Side of the Moon, 1973";
                AdicionarValorCache("ValorString", valorString);
            }

            ViewBag.ValorString = valorString;
        }

        public void ExemploRedisConteudoObjeto()
        {
            var valorObjeto = _cache.GetString("ValorObjeto");

            Complexo objComplexo = null;

            if (valorObjeto == null)
            {
                objComplexo = new Complexo()
                {
                    ValorString = "Pink Floyd",
                    ValorInteiro = 1965,
                    ValorDecimal = 250000000.00M
                };

                valorObjeto = JsonConvert.SerializeObject(objComplexo);

                AdicionarValorCache("ValorObjeto", valorObjeto);
            }
            else
            {
                objComplexo = JsonConvert.DeserializeObject<Complexo>(valorObjeto);
            }

            ViewBag.ValorComplexo = objComplexo;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
