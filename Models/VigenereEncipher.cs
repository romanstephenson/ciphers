using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace ciphers.Models;

public class VigenereEncipher
{
    /*This attribute holds the key generated based on the keyword*/
    public string? Key { get; set; }

    public string? AssociatedKey { get; set; }

    /*This attribute will hold the plaintext value entered by the user or generated based on ciphertext*/
    [Required]
    //[StringLength(1, ErrorMessage = "Plaintext must be greater than 1")]
    public string? Plaintext { get; set; }

    /*This attribute will hold the keyword entered by the user*/
    [Required]
    //[StringLength(1, ErrorMessage = "Keyword must be greater than 1")]
    public string? Keyword { get; set; }

    /*This attribute will hold the ciphertext that is either entered by the user or generated based on plaintext*/
    //[Required]
    //[StringLength(1, ErrorMessage = "Ciphertext must be greater than 1")]
    public string? Ciphertext { get; set; }

    public int RsaPrivateKey { get; set; }

    public int DiffieHellmanPublicKey { get; set; }

    [Required]
    public int DiffieHellmanSecretKey { get; set; }

    public string? Hash { get; set; }

    public string? RsaSignature { get; set; }

    /*This attribute holds any error that may be encountered during processing.*/
    public string? Error { get; set; }

    /*
    This function takes the keyword and plaintext, creates the key to be same length as plaintext.
    It does this by looping based on the length of the plaintext and adding a value from the keyword
    to the key in order until the length is the same as the plaintext.
    //WHITEWHITEWHITEWHITEWHI
    //ZPDXVPAZHSLZBHIWZBKMZNM
    */
    public void CreateKeyToEncrypt()
    {
        /*only proceed if the attributes we need are not empty.*/
        if( Plaintext is not null && Keyword is not null)
        {
            //remove leading, trailing, and space within text
            string PlaintextWithoutSpace = Plaintext.Trim().Replace(" ","").Replace(".","").ToUpper();

            /*
            for vigenere, we must know the length of the keyword so we can ensure it's length
            is never greater than but only equal to the length of the plaintext to be encrypted
            */
            string KeywordWithoutSpace = Keyword.Trim().Replace(" ","").Replace(".","").ToUpper();
            
            /*
            We must know the plaintext without spaces length, since we will check the keyword length against this value. It must not be greater than or less than but can be equal to each other.
            */
            int x = PlaintextWithoutSpace.Length;

            /*
            Now that we have the length of both text without space, we loop through the lenghth of
            the plaintext and add to the keyword to pad it up the length of the plaintext.
            */
            for (int i = 0; ; i++)
            {
                /*Our iterator is reset to 0 if and only if it has reached the length of the plain or ciphertext
                This will restart the key building at the first character in the keyword*/
                if (x == i)
                    i = 0;

                /*we check if the length of keyword is the same length as plaintext, if it is we break and we no longer need to proceed with key building.*/
                if (KeywordWithoutSpace.Length == PlaintextWithoutSpace.Length)
                    break;

                /*For each iteration in the loop, we add the next value from the keyword to the key.*/
                KeywordWithoutSpace += KeywordWithoutSpace[i] ;
            }

            Key = KeywordWithoutSpace;
        }
    }

    public void AssociateKeyWordAndDiffieSecret()
    {
        /*
        1. do a for loop to the length of the key 
        2. multiply the integer value of each letter in the key by the diffiehelman secret value
        3. store the result of the multiplication in a list or array
        4. build a string based on the character represent of each integer in the list or array.
        */
        if(Key is not null)
        {
            for(int i = 0; i < Key.Length; i++ )
            {
                // Console.WriteLine("This is I: " + i);

                // Console.WriteLine("Diffie Secret: " + DiffieHellmanSecretKey);

                int KeyValueIndex =  ( Key[i] * DiffieHellmanSecretKey ) % 26;

                //Console.WriteLine("KeyValueIndex: " + KeyValueIndex);

                KeyValueIndex += 'A';

                //Console.WriteLine("KeyValueIndex: " + KeyValueIndex);

                AssociatedKey += ( char ) KeyValueIndex;

                //Console.WriteLine("Associated Key: " + AssociatedKey);
            }
        }
        
    }

    public void GetMD5Hash()
    {
        if( !string.IsNullOrEmpty(Ciphertext) )
        {
            MD5 Md5Hasher = MD5.Create();

            byte[] data = Md5Hasher.ComputeHash(Encoding.Default.GetBytes(Ciphertext));
            
            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            Hash = sBuilder.ToString();
        }        
    }

    public bool VerifyMD5Signature(string hash, string signature)
    {
        if( 0 == Comparer.Default.Compare(hash, signature) )
        {
            return true;
        }

        return false;
    }

    /*
    This method creates the ciphertext using the key generated above.
    */
    public void CreateCipherText( )
    {
        /*only proceed if the attributes we need are not empty.*/
        if( Plaintext is not null && AssociatedKey is not null)
        {
            /*First, we get the user entered plaintext and remove all leading, trailing and spaces within the text. We also convert to upper case to allow consistence use of upper case
            alphabet across key and plaintext*/
            string PlaintextWithoutSpace = Plaintext.Trim().Replace(" ","").Replace(".","").ToUpper();
            
            /*
            We know that we are encrypting each symbol or character so we must loop as many times as the length of the plaintext.
            THe position of the key that corresponds to the plaintext modulus 26 ( letters in the alphabet we used)
            */
            for (int i = 0; i < PlaintextWithoutSpace.Length; i++)
            {
                // converting in range 0-25 (count of 26 since there are 26 characters in the alphabet we are using)
                int x = ( PlaintextWithoutSpace[ i ] + AssociatedKey[ i ] ) % 26;
        
                /*in ascii alphabet each character is given a integer representation so we get that integer value*/
                x += 'A';
        
                /*ensure the integer value is converted to the upper case representation in the aplhabet and append the character to the ciphertext string we are building. This will assign the value to the attribute of the instance of VigenereCipher which we can access in the controller*/
                Ciphertext += ( char )( x );
            }
        }
    }
}