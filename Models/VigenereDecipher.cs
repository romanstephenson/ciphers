using System.ComponentModel.DataAnnotations;

namespace ciphers.Models;

public class VigenereDecipher
{
    /*This attribute holds the key generated based on the keyword*/
    public string? Key { get; set; }

    /*This attribute will hold the plaintext value entered by the user or generated based on ciphertext*/
    //[Required]
    //[StringLength(1, ErrorMessage = "Plaintext must be greater than 1")]
    public string? Plaintext { get; set; }


    /*This attribute will hold the keyword entered by the user*/
    [Required]
    //[StringLength(1, ErrorMessage = "Keyword must be greater than 1")]
    public string? Keyword { get; set; }

    /*This attribute will hold the ciphertext that is either entered by the user or generated based on plaintext*/
    [Required]
    //[StringLength(1, ErrorMessage = "Ciphertext must be greater than 1")]
    public string? Ciphertext { get; set; }

    /*This attribute holds any error that may be encountered during processing.*/
    public string? Error { get; set; }

    
    /*
    This function takes the keyword and ciphertext, creates the key to be same length as ciphertext.
    It does this by looping based on the length of the ciphertext and adding a value from the keyword
    to the key in order until the length is the same as the ciphertext.
    */
    public void CreateKeyForDecrypt()
    {
        /*only proceed if the attributes we need are not empty.*/
        if( Ciphertext is not null && Keyword is not null)
        {
            //remove leading, trailing, and space within text
            string CipherTextWithoutSpace = Ciphertext.Trim().Replace(" ","").Replace(".","").ToUpper();

            /*
            for vigenere, we must know the length of the keyword so we can ensure it's length
            is never greater than but only equal to the length of the plaintext to be encrypted
            */
            string KeywordWithoutSpace = Keyword.Trim().Replace(" ","").Replace(".","").ToUpper();

            /*
            We must know the ciphertext without spaces length, since we will check the keyword length against this value. It must not be greater than or less than but can be equal to each other.
            */
            int x = CipherTextWithoutSpace.Length;

            /*
            Now that we have the length of both text without space, we loop through the lenghth of
            the ciphertext and add to the keyword to pad it up the length of the ciphertext.
            */
            for (int i = 0; ; i++)
            {
                /*Our iterator is reset to 0 if and only if it has reached the length of the plain or ciphertext
                This will restart the key building at the first character in the keyword*/
                if (x == i)
                    i = 0;

                /*For each iteration we check if the length of keyword is the same length as plaintext, if it is we break and we no longer need to proceed with key building.*/
                if (KeywordWithoutSpace.Length == CipherTextWithoutSpace.Length)
                    break;

                KeywordWithoutSpace += KeywordWithoutSpace[i] ;
            }

            Key = KeywordWithoutSpace;
        }
    }
    
    
    /*This function decrypts the encrypted text and returns the original plain text*/
    public void CreatePlainText()
    { 
        /*only proceed if the attributes we need are not empty.*/
        if( Ciphertext is not null && Key is not null)
        {
            /*First, we get the user entered Ciphertext and remove all leading, trailing and spaces within the text. We also convert to upper case to allow consistent use of upper case
            alphabet across key and Ciphertext*/
            string CipherTextWithoutSpace = Ciphertext.Trim().Replace(" ", "").Replace(".","").ToUpper();
            string KeywordWithoutSpace = Key.Trim().Replace(" ", "").Replace(".","").ToUpper();

            /*
            Now we know the key and the ciphertext is the same length we need to define a iterator i and loop through until the 
            */
            for (int i = 0 ; i < CipherTextWithoutSpace.Length && i < KeywordWithoutSpace.Length; i++)
            {
                // converting in range 0-25 (count of 26 since there are 26 characters in the alphabet we are using)
                int x = ( CipherTextWithoutSpace[i] - KeywordWithoutSpace[i] + 26 ) %26;
        
                /*in ascii alphabet each character is given a integer representation so we get that integer value*/
                x += 'A';

                /*ensure the integer value is converted to the upper case representation in the aplhabet and append the character to the Plaintext string we are building. This will assign the value to the attribute of the instance of VigenereDecipher which we can access in the controller*/
                Plaintext += ( char ) ( x) ;
            }
        }
    }
}