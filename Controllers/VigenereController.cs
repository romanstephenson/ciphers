using Microsoft.AspNetCore.Mvc;
using ciphers.Models;

namespace ciphers.Controllers;

public class VigenereController : Controller
{

    //[BindProperty]
    //public Vigenere cipheredVigenere { get; set; } = default!;

    /*
    The controller gets a request once the user clicks the view with name Cipher, a GET request is posted and it simply returns the page with the form for the user to fill in
    */
    [HttpGet]
    public IActionResult Cipher()
    {
        return View();
    }

    /*
    When the user enters the plaintext to be encrypted and the key, that data is added
    to an instance of the Vigenere model and is sent via POST message to the controller.
    That object is passed into this function.

    We check if the instance of vigenere is valid then proceed to call the generate key and generate cipher text method.

    Once complete, the ciphertext is assigned the attribute in the Vigenere class, as is the key based on keyword and we return a instance of Vigenere model back to the view for use in display
    */
    [HttpPost]
    public IActionResult Cipher( VigenereCipher vigenere)
    {
        if( ModelState.IsValid )
        {
            vigenere.CreateKeyToEncrypt();
            vigenere.CreateCipherText();
        
            //Console.WriteLine(cipheredVigenere.Ciphertext);
            @ViewBag.Ciphertext = vigenere.Ciphertext;
            //ViewData["Ciphertext"]

            return View(vigenere);
        }

        return View();
        
    }


    /*
    The controller gets a request once the user clicks the view with name Decipher, a GET request is posted and it simply returns the page with the form for the user to fill in
    */
    [HttpGet]
    public IActionResult Decipher()
    {
        return View();
    }

    /*
    When the user enters the ciphertext to be decrypted and the key, that data is added
    to an instance of the Vigenere model and is sent via POST message to the controller.
    That object is passed into this function.

    We check if the instance of vigenere is valid then proceed to call the generate key for decrypt method and generate Original text method.

    Once complete, the plaintext is assigned the attribute in the Vigenere class, as is the key based on keyword and we return a instance of Vigenere model back to the view for use in display
    */
    [HttpPost]
    public IActionResult Decipher(VigenereDecipher vigenere)
    {
         if( ModelState.IsValid )
        {
            vigenere.CreateKeyForDecrypt();
            vigenere.CreatePlainText();

            //Console.WriteLine(vigenere.Plaintext);

            @ViewBag.Plaintext = vigenere.Plaintext;
            //ViewData["Plaintext"]
            
            return View(vigenere);
        }

        return View();
    }

}