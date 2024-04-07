using Microsoft.AspNetCore.Mvc;
using ciphers.Models;

namespace ciphers.Controllers;

public class RsaController: Controller
{
    [HttpGet]
    public IActionResult RsaExchange()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RsaExchange(Rsa rsa)
    {
        if(ModelState.IsValid)
        {
            rsa.PrimeFiller();
            rsa.GenerateRsaKeys();
            @ViewBag.SenderRsaPublicKey = rsa.RsaPublicKey;
            @ViewBag.SenderRsaPrivateKey = rsa.RsaPrivateKey;
            @ViewBag.N = rsa.n;

            //rsa.PrimeFiller();
            rsa.GenerateRsaKeys();
            @ViewBag.RecieverRsaPublicKey = rsa.RsaPublicKey;
            @ViewBag.RecieverRsaPrivateKey = rsa.RsaPrivateKey;
            @ViewBag.N = rsa.n;

            return View(rsa);
        }
        
        return View();
    }
}