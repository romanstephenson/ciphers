using Microsoft.AspNetCore.Mvc;
using ciphers.Models;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json;

namespace ciphers.Controllers;
/*
Use the symmetric key to encrypt the message, a function to hash the encrypted message then encrypt (sign) with the private key. Do the reverse using the public key for the receiver.
*/

public class VigenereController : Controller
{

    /*
    The controller gets a request once the user clicks the view with name Cipher, a GET request is posted and it simply returns the page with the form for the user to fill in
    */
    [HttpGet]
    public IActionResult Encipher()
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
    public IActionResult Encipher( VigenereEncipher vigenere)
    {
        if( ModelState.IsValid )
        {
            //create key based on keyword
            vigenere.CreateKeyToEncrypt();

            //associate viginere key and diffie public key and assign to sessions
            HttpContext.Session.SetString( vigenere.Key, vigenere.DiffieHellmanPublicKey.ToString());

            //create ciphertext using key from above
            vigenere.CreateCipherText();

            //get rsa private key of sender
            vigenere.RsaPrivateKey = Convert.ToInt32(HttpContext.Session.GetString("SenderRsaPrivateKeyPart1"));

            //get rsa private key of sender
            var n = Convert.ToInt32(HttpContext.Session.GetString("SenderRsaPrivateKeyPart2"));

            //create md5 hash of the ciphertext
            vigenere.GetMD5Hash();

            //assign the hash to session 
            HttpContext.Session.SetString( "SenderSignature", vigenere.Signature);

            //assign private key and n for rsa sign
            Rsa SenderSign = new Rsa
            {
                //Console.WriteLine(vigenere.RsaPrivateKey);

                RsaPrivateKey = vigenere.RsaPrivateKey,
                n = n
            };

            //encode and sign ciphertext
            List<long> SignedMessage = SenderSign.RsaLetterEncoder(vigenere.Ciphertext);

            string SignedSerialize = JsonConvert.SerializeObject(SignedMessage);

            HttpContext.Session.SetString( "SignedListRsa",SignedSerialize);

            //assign the rsa sign to session 
            HttpContext.Session.SetString( "SenderRsaSignedMessage", string.Join("",SignedMessage).ToString());

            @ViewBag.Ciphertext = vigenere.Ciphertext;

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
            
            //assign sign of ciphertext to session
            string SenderRsaSignedMessage = HttpContext.Session.GetString("SenderRsaSignedMessage") ;

            /*get rsa public key public from session data*/
            vigenere.RsaPublicKey = Convert.ToInt32( HttpContext.Session.GetString("SenderRsaPublicKeyPart1") );

            /*get rsa public key public from session data*/
            var n = Convert.ToInt32( HttpContext.Session.GetString("SenderRsaPublicKeyPart2") );


            //assign private key and n for rsa sign to instance of rsa close / model
            Rsa ReceiverSign = new Rsa
            {
                RsaPublicKey = vigenere.RsaPublicKey,
                n = n
            };

            //get rsa sign from session that was serialize to maintain list instantiation , then deserialize and compare to what was entered by user, if match then allow further vigenere decrypt and rsa decrypt
            string SignedSerialize = HttpContext.Session.GetString( "SignedListRsa");

            List<long> SignedMessage = JsonConvert.DeserializeObject<List<long>>(SignedSerialize);


            //set ciphertext to instance of vigenere
            vigenere.Ciphertext = ReceiverSign.RsaLetterDecoder(SignedMessage);


            if( SenderRsaSignedMessage.ToString() == vigenere.RsaSignature.ToString() )
            {
                //create key based on keyword
                vigenere.CreateKeyForDecrypt();
                
                //assign ciphertext decrypted from rsa sign to view
                @ViewBag.Ciphertext = vigenere.Ciphertext;

                //generate plaintext from rsa to vigenere 
                vigenere.CreatePlainText();

                //set plaintext to view
                @ViewBag.Plaintext = vigenere.Plaintext;
                //ViewData["Plaintext"]
                
                return View(vigenere);
            }
            else{

                @ViewBag.RsaSignMismatch = "Rsa Signature does not match that of sender, please review.";
                return View();
            }
        }

        return View();
    }

}