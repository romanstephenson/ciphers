using System.ComponentModel.DataAnnotations;

namespace ciphers.Models;

public class VigenereCipher
{
    /*This attribute holds the key generated based on the keyword*/
    public string? Key { get; set; }

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

                //Console.WriteLine(KeywordWithoutSpace[i]);

                /*For each iteration in the loop, we add the next value from the keyword to the key.*/
                KeywordWithoutSpace += KeywordWithoutSpace[i] ;
                //Console.WriteLine(KeywordWithoutSpace);
            }

            Key = KeywordWithoutSpace;
        }
    }

    
    /*
    This method creates the ciphertext using the key generated above.
    */
    public void CreateCipherText( )
    {
        /*only proceed if the attributes we need are not empty.*/
        if( Plaintext is not null && Key is not null)
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
                int x = ( PlaintextWithoutSpace[ i ] + Key[ i ] ) % 26;
                // Console.WriteLine(i);
                // Console.WriteLine(PlaintextWithoutSpace[ i ]);
                // Console.WriteLine(Key[ i ]);
                // Console.WriteLine(x);
        
                /*in ascii alphabet each character is given a integer representation so we get that integer value*/
                x += 'A';
                //Console.WriteLine(x);
        
                /*ensure the integer value is converted to the upper case representation in the aplhabet and append the character to the ciphertext string we are building. This will assign the value to the attribute of the instance of VigenereCipher which we can access in the controller*/
                Ciphertext += ( char )( x );
                //Console.WriteLine("This is char x" + ( char )( x ));
                //Console.WriteLine(Ciphertext);
            }
        }
    }
}