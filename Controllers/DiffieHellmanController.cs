using Microsoft.AspNetCore.Mvc;
using ciphers.Models;

namespace ciphers.Controllers;

public class DiffieHellmanController : Controller
{
    [HttpGet]
    public IActionResult DiffieHellmanSender()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DiffieHellmanSender(DiffieHellman diffieHellman)
    {
        if(ModelState.IsValid)
        {
            long a = diffieHellman.GenerateKey(diffieHellman.ReceiverPrimitiveRootofSender, diffieHellman.SenderPrivateKey, diffieHellman.SenderPrimeNumber);

            long b = diffieHellman.GenerateKey(diffieHellman.ReceiverPrimitiveRootofSender, diffieHellman.ReceiverPrivateKey, diffieHellman.SenderPrimeNumber);

            diffieHellman.SenderSecretKey = diffieHellman.GenerateKey(b,diffieHellman.SenderPrivateKey,diffieHellman.SenderPrimeNumber);


            @ViewBag.SenderSecretKey = diffieHellman.SenderSecretKey;

            return View(diffieHellman);
        }
        
        return View();
    }


    [HttpGet]
    public IActionResult DiffieHellmanReceiver()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DiffieHellmanReceiver(DiffieHellman diffieHellman)
    {
        if(ModelState.IsValid)
        {
            return View(diffieHellman);
        }
        
        return View();
    }
}