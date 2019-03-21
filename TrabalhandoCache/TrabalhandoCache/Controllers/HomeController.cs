using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TrabalhandoCache.Models;

namespace TrabalhandoCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly Contexto _contexto;
        private readonly IMemoryCache _cache;

        public HomeController(Contexto contexto, IMemoryCache cache)
        {
            _contexto = contexto;
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DadosComMemoriaCache()
        {
            List<Pessoa> pessoas;

            var opcoesCache = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
            
            if(!_cache.TryGetValue("ListaPessoas", out pessoas))
            {
                pessoas = _contexto.Pessoas.ToList();

                _cache.Set("ListaPessoas", pessoas, opcoesCache);

                ViewBag.Mensagem = "Dados inseridos na memória cache";
            }

            else
            {
                pessoas = _cache.Get("ListaPessoas") as List<Pessoa>;
                ViewBag.Mensagem = "Dados pegos da memória cache";
            }

            return View(pessoas);
        }

        public IActionResult DadosSemMemoriaCache()
        {
            return View(_contexto.Pessoas.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
