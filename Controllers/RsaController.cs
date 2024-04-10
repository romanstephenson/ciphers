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
            rsa.GenerateAndPopulatePrimeNumbers();
            rsa.GenerateRsaKeys();
            @ViewBag.SenderRsaPublicKey = rsa.RsaPublicKey;
            @ViewBag.SenderRsaPrivateKey = rsa.RsaPrivateKey;
            @ViewBag.N = rsa.n;

            //rsa.GenerateAndPopulatePrimeNumbers();
            rsa.GenerateRsaKeys();
            @ViewBag.RecieverRsaPublicKey = rsa.RsaPublicKey;
            @ViewBag.RecieverRsaPrivateKey = rsa.RsaPrivateKey;
            @ViewBag.N = rsa.n;

            return View(rsa);
        }
        
        return View();
    }

    [HttpGet]
    public IActionResult RsaSender()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RsaSender(Rsa rsa)
    {
        if(ModelState.IsValid)
        {
            rsa.GenerateAndPopulatePrimeNumbers();
            rsa.GenerateRsaKeys();
            @ViewBag.SenderRsaPublicKey = rsa.RsaPublicKey;
            @ViewBag.SenderRsaPrivateKey = rsa.RsaPrivateKey;
            @ViewBag.N = rsa.n;

            /*Set public to session data to make it available to receiver*/
            HttpContext.Session.SetString("SenderRsaPublicKeyPart1",rsa.RsaPublicKey.ToString());
            HttpContext.Session.SetString("SenderRsaPublicKeyPart2",rsa.n.ToString());

            /*Set private to session data to make it available to receiver*/
            HttpContext.Session.SetString("SenderRsaPrivateKeyPart1",rsa.RsaPrivateKey.ToString());
            HttpContext.Session.SetString("SenderRsaPrivateKeyPart2",rsa.n.ToString());

            return View(rsa);
        }
        
        return View();
    }

    [HttpGet]
    public IActionResult RsaReceiver()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RsaReceiver(Rsa rsa)
    {
        if(ModelState.IsValid)
        {
            rsa.GenerateAndPopulatePrimeNumbers();
            rsa.GenerateRsaKeys();
            @ViewBag.ReceiverRsaPublicKey = rsa.RsaPublicKey;
            @ViewBag.ReceiverRsaPrivateKey = rsa.RsaPrivateKey;
            @ViewBag.N = rsa.n;

             /*Set public to session data to make it available to sender*/
            HttpContext.Session.SetString("ReceiverRsaPublicKeyPart1",rsa.RsaPublicKey.ToString());
            HttpContext.Session.SetString("ReceiverRsaPublicKeyPart2",rsa.n.ToString());
            
             /*Set private to session data to make it available to receiver*/
            HttpContext.Session.SetString("ReceiverRsaPrivateKey",rsa.RsaPrivateKey.ToString() + ","+rsa.n);
            

            return View(rsa);
        }
        
        return View();
    }
}