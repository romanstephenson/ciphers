using Microsoft.AspNetCore.Mvc;
using ciphers.Models;

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
            diffieHellman.SenderPublicKey = diffieHellman.GenerateKey(diffieHellman.SenderPrimitiveRoot,diffieHellman.SenderPrivateKey,diffieHellman.SenderPrimeNumber );

            /*generate sender secret key*/
            if(diffieHellman.ReceiverPublicKey != 0)
            {

                // Console.WriteLine(diffieHellman.ReceiverPublicKey);
                // Console.WriteLine(diffieHellman.SenderPublicKey);
                // Console.WriteLine(diffieHellman.SenderPrimitiveRoot);

                diffieHellman.SenderSecretKey = diffieHellman.GenerateKey(diffieHellman.ReceiverPublicKey,diffieHellman.SenderPrivateKey,diffieHellman.SenderPrimeNumber);

                @ViewBag.SenderSecretKey = diffieHellman.SenderSecretKey;

            }
            
            // /*assign sender public key to session data*/
            // HttpContext.Session.SetString("DiffieSenderPublicKey",diffieHellman.SenderPublicKey.ToString());

            // /*assign primitive root to session data*/
            // HttpContext.Session.SetString("DiffieSenderPrimitiveRoot",diffieHellman.SenderPrimitiveRoot.ToString());

            // /*assign prime number for sender to session data*/
            // HttpContext.Session.SetString("DiffieSenderPrimeNumber", diffieHellman.SenderPrimeNumber.ToString());

            @ViewBag.SenderPublicKey = diffieHellman.SenderPublicKey;

            
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
            // /*get primitive root for sender from session and sender public key*/
            // SenderPrimRoot = Convert.ToInt64(HttpContext.Session.GetString("DiffieSenderPrimitiveRoot"));

            // /*get sender public key from session data*/
            // SenderPublicKey = Convert.ToInt64(HttpContext.Session.GetString("DiffieSenderPublicKey") );

            // /*get sender prime number from session data*/
            // SenderPrimeNumber = Convert.ToInt64(HttpContext.Session.GetString("DiffieSenderPrimeNumber") );

            /*generate public key for reciever*/
            diffieHellman.ReceiverPublicKey = diffieHellman.GenerateKey(diffieHellman.ReceiverPrimitiveRoot,diffieHellman.ReceiverPrivateKey, diffieHellman.ReceiverPrimeNumber );

             /*generate sender secret key*/
            if(diffieHellman.SenderPublicKey != 0)
            {

                // Console.WriteLine("Sender pub" + diffieHellman.SenderPublicKey);
                // Console.WriteLine("Rec Priv" +diffieHellman.ReceiverPrivateKey);
                // Console.WriteLine("Rec Prim" +diffieHellman.ReceiverPrimitiveRoot);

                diffieHellman.ReceiverSecretKey = diffieHellman.GenerateKey(diffieHellman.SenderPublicKey,diffieHellman.ReceiverPrivateKey,diffieHellman.ReceiverPrimeNumber);

                //Console.WriteLine("Rec Secret:" + diffieHellman.ReceiverSecretKey);

                @ViewBag.ReceiverSecretKey = diffieHellman.ReceiverSecretKey;

            }
            
            @ViewBag.ReceiverPublicKey = diffieHellman.ReceiverPublicKey;

            return View(diffieHellman);
        }
        return View();
    }

}