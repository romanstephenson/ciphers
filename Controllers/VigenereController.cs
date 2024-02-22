using Microsoft.AspNetCore.Mvc;
using ciphers.Models;

namespace ciphers.Controllers;

public class VigenereController : Controller
{

    [BindProperty]
    public Vigenere cipheredVigenere { get; set; } = default!;

    [HttpGet]
    public IActionResult Cipher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Cipher( Vigenere vigenere)
    {
        cipheredVigenere.Key = vigenere.GenerateKey();
        cipheredVigenere.Ciphertext = vigenere.CipherText(cipheredVigenere.Key);
    
        //Console.WriteLine(cipheredVigenere.Ciphertext);
        ViewData["Ciphertext"] = cipheredVigenere.Ciphertext;

        return View(cipheredVigenere);
    }


    public IActionResult Decipher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Decipher(Vigenere vigenere)
    {
        // Console.WriteLine(vigenere.Ciphertext);
        // Console.WriteLine(vigenere.Key);
        vigenere.Key = vigenere.GenerateKeyForDecrypt();
        cipheredVigenere.Plaintext = vigenere.OriginalText();

        Console.WriteLine(cipheredVigenere.Plaintext);

        ViewData["Plaintext"] = cipheredVigenere.Plaintext;
        
        return View(cipheredVigenere);
    }

}