using Microsoft.AspNetCore.Mvc;
using ciphers.Models;

namespace ciphers.Controllers;

public class VigenereController : Controller
{

    //[BindProperty]
    //public Vigenere cipheredVigenere { get; set; } = default!;

    [HttpGet]
    public IActionResult Cipher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Cipher( Vigenere vigenere)
    {
        if( ModelState.IsValid )
        {
            vigenere.Key = vigenere.GenerateKey();
            vigenere.Ciphertext = vigenere.CipherText(vigenere.Key);
        
            //Console.WriteLine(cipheredVigenere.Ciphertext);
            @ViewBag.Ciphertext = vigenere.Ciphertext;
            //ViewData["Ciphertext"]

            return View(vigenere);
        }

        return View();
        
    }

    [HttpGet]
    public IActionResult Decipher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Decipher(Vigenere vigenere)
    {
         if( ModelState.IsValid )
        {
            vigenere.Key = vigenere.GenerateKeyForDecrypt();
            vigenere.Plaintext = vigenere.OriginalText();

            //Console.WriteLine(vigenere.Plaintext);

            @ViewBag.Plaintext = vigenere.Plaintext;
            //ViewData["Plaintext"]
            
            return View(vigenere);
        }

        return View();
    }

}