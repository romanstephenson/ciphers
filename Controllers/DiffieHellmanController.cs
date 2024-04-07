using Microsoft.AspNetCore.Mvc;
using ciphers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ciphers.Controllers;

public class DiffieHellmanController : Controller
{
    [HttpGet]
    public IActionResult DiffieHellmanExchange()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DiffieHellmanExchange(DiffieHellman diffieHellman)
    {
        if(ModelState.IsValid)
        {
            /*generate public key for sender*/
            diffieHellman.SenderPublicKey = diffieHellman.GenerateKey(diffieHellman.SenderPrimeNumber,diffieHellman.SenderPrivateKey,diffieHellman.SenderPrimitiveRoot );

            //assigned sender public key to session data
            HttpContext.Session.SetString("DiffieSenderPublicKey",diffieHellman.SenderPublicKey.ToString());

            //Console.WriteLine(diffieHellman.SenderPublicKey);

            @ViewBag.SenderPublicKey = diffieHellman.SenderPublicKey;

            /*generate public key for reciever*/
            diffieHellman.ReceiverPublicKey = diffieHellman.GenerateKey(diffieHellman.ReceiverPrimeNumber,diffieHellman.ReceiverPrivateKey,diffieHellman.SenderPrimitiveRoot );

            //assign reciever public key to session data
            HttpContext.Session.SetString("DiffieRecieverPublicKey",diffieHellman.ReceiverPublicKey.ToString());

            //Console.WriteLine(diffieHellman.RecieverPublicKey);

            @ViewBag.RecieverPublicKey = diffieHellman.ReceiverPublicKey;

            //sender secret key
            diffieHellman.ReceiverSecretKey = diffieHellman.GenerateKey(diffieHellman.SenderPublicKey,diffieHellman.ReceiverPrivateKey,diffieHellman.SenderPrimitiveRoot);


            //reciever secret key
            diffieHellman.SenderSecretKey = diffieHellman.GenerateKey(diffieHellman.ReceiverPublicKey,diffieHellman.SenderPrivateKey,diffieHellman.SenderPrimitiveRoot);

            @ViewBag.ReceiverSecretKey = diffieHellman.ReceiverSecretKey;
            @ViewBag.SenderSecretKey = diffieHellman.SenderSecretKey;

            return View(diffieHellman);
        }
        
        return View();
    }

    
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
            /*generate public key for sender*/
            diffieHellman.SenderPublicKey = diffieHellman.GenerateKey(diffieHellman.SenderPrimeNumber,diffieHellman.SenderPrivateKey,diffieHellman.SenderPrimitiveRoot );

            //assign sender public key to session data
            HttpContext.Session.SetString("DiffieSenderPublicKey",diffieHellman.SenderPublicKey.ToString());

            //assign primitive root to session data
            HttpContext.Session.SetString("DiffieSenderPrimitiveRoot",diffieHellman.SenderPrimitiveRoot.ToString());

            //Console.WriteLine(diffieHellman.SenderPublicKey);

            //sender secret key
            diffieHellman.SenderSecretKey = diffieHellman.GenerateKey(diffieHellman.SenderPublicKey,diffieHellman.ReceiverPrivateKey,diffieHellman.SenderPrimitiveRoot);

            @ViewBag.SenderPublicKey = diffieHellman.SenderPublicKey;

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
        long SenderPrimRoot;
        long SenderPublicKey;

        if(ModelState.IsValid)
        {   
            //get primitive root for sender from session and sender public key
            SenderPrimRoot = Convert.ToInt64(HttpContext.Session.GetString("DiffieSenderPrimitiveRoot"));

            //assign sender public key to session data
            SenderPublicKey = Convert.ToInt64(HttpContext.Session.GetString("DiffieSenderPublicKey") );

            /*generate public key for reciever*/
            diffieHellman.ReceiverPublicKey = diffieHellman.GenerateKey(diffieHellman.ReceiverPrimeNumber,diffieHellman.ReceiverPrivateKey, SenderPrimRoot );

            //assign reciever public key to session data
            HttpContext.Session.SetString("DiffieRecieverPublicKey",diffieHellman.ReceiverPublicKey.ToString());

            //Console.WriteLine(diffieHellman.RecieverPublicKey);

            //reciever secret key
            diffieHellman.ReceiverSecretKey = diffieHellman.GenerateKey(SenderPublicKey , diffieHellman.ReceiverPrivateKey, SenderPrimRoot);
            
            @ViewBag.ReceiverSecretKey = diffieHellman.ReceiverSecretKey;
            @ViewBag.ReceiverPublicKey = diffieHellman.ReceiverPublicKey;
        }
        return View();
    }

}