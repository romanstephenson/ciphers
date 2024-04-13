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

            vigenere.AssociateKeyWordAndDiffieSecret();
            //Console.WriteLine(vigenere.AssociatedKey);

            //associate viginere key and diffie public key and assign to sessions
            // HttpContext.Session.SetString( vigenere.Key, vigenere.DiffieHellmanPublicKey.ToString() );

            //create ciphertext using key from above
            vigenere.CreateCipherText();

            //create md5 hash of the ciphertext
            vigenere.GetMD5Hash();

            //get rsa private key of sender from rsa key generation
            vigenere.RsaPrivateKey = Convert.ToInt32( HttpContext.Session.GetString("SenderRsaPrivateKeyPart1") );

            //set ciphertext to session, we are going to send this to receiver
            if(vigenere.Ciphertext is not null)
            {
                HttpContext.Session.SetString( "RawCypterText", vigenere.Ciphertext );
            }

            //set the hash to session, we are going to set this to receiver
            if(vigenere.Hash is not null)
            {
                HttpContext.Session.SetString( "SenderMd5Hash", vigenere.Hash );
            }
            

            //get rsa private key of sender
            var n = Convert.ToInt32( HttpContext.Session.GetString("SenderRsaPrivateKeyPart2") );


            //assign private key and n for rsa sign
            Rsa SenderSign = new Rsa
            {
                RsaPrivateKey = vigenere.RsaPrivateKey,
                n = n
            };

            List<long> SignedMessage;

            //encode and sign md5hash
            if(vigenere.Hash is not null)
            {
                SignedMessage = SenderSign.RsaLetterEncoder( vigenere.Hash );

                string SignedSerialize = JsonConvert.SerializeObject( SignedMessage );

                HttpContext.Session.SetString( "SignedListRsa", SignedSerialize );

                 //assign the rsa sign to session 
                HttpContext.Session.SetString( "SenderRsaSignedMessage", string.Join("",SignedMessage).ToString());

                
            }
            
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
            //string SenderRsaSignedMessage = HttpContext.Session.GetString( "SenderRsaSignedMessage" ) ;

            /*get rsa public key public from session data*/
            vigenere.RsaPublicKey = Convert.ToInt32( HttpContext.Session.GetString( "SenderRsaPublicKeyPart1" ) );

            /*get rsa public key public from session data*/
            var n = Convert.ToInt32( HttpContext.Session.GetString( "SenderRsaPublicKeyPart2" ) );

            //set ciphertext to session, we are going to send this to receiver
            //vigenere.Ciphertext = HttpContext.Session.GetString( "RawCypterText" );

            /*get rsa sign from session that was serialize to maintain list instantiation , then deserialize and compare to what was entered by user, if match then allow further vigenere decrypt and rsa decrypt*/

            List<long> SignedMessage;

            string SignedSerialize;


            SignedSerialize = HttpContext.Session.GetString( "SignedListRsa" ) ?? "";

            SignedMessage = JsonConvert.DeserializeObject<List<long>>(SignedSerialize);

            //assign private key and n for rsa sign to instance of rsa close / model
            Rsa ReceiverSign = new Rsa
            {
                RsaPublicKey = vigenere.RsaPublicKey,
                n = n
            };

            //set decrypted rsa sign which is the md5 hash
            string ExtractedMd5HashFromRsaSign = ReceiverSign.RsaLetterDecoder(SignedMessage);
            

            // Console.WriteLine(SignedSerialize);

            // Console.WriteLine(ExtractedMd5HashFromRsaSign);

            //generate md5 hash based on ciphertext from vigenere
            vigenere.GetMD5Hash();

            //Console.WriteLine(vigenere.Hash);

            //check if decrypted rsa signature is the same as sender
            if(vigenere.RsaSignature == string.Join("",SignedMessage).ToString())
            {
                //check if ciphertext to hash and sender md5 hash matches
                if( vigenere.VerifyMD5Signature( vigenere.Hash , ExtractedMd5HashFromRsaSign) )
                {

                    //Console.WriteLine("Md5 Hash Matches");

                    //create key based on keyword
                    vigenere.CreateKeyForDecrypt();

                    vigenere.DeAssociateKeyWordAndDiffieSecret();
                    Console.WriteLine("Deassociated key ciphertext: " +vigenere.DeAssociatedKey);
                    
                    //assign ciphertext decrypted from rsa sign to view
                    //@ViewBag.Ciphertext = vigenere.Ciphertext;

                    //generate plaintext from rsa to vigenere 
                    vigenere.CreatePlainText();

                    //update user that message is fine
                    @ViewBag.Md5Verify = "Looks good boss, message has not been tampered with";

                    @ViewBag.Plaintext = vigenere.Plaintext;
                    
                    return View(vigenere);

                }
                else
                    {

                        @ViewBag.Md5Verify = "Message has been tampered with, md5 hash does not match that of sender, please review.";
                        return View();
                    }
            }
            else{
                @ViewBag.CheckRsaSign = "Woah there, that Rsa Signature is wrong. Verify with sender.";
            }
            
        }

        //Console.WriteLine("Model state not valid");
        return View();
    }

}