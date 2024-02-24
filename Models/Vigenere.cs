namespace ciphers.Models;

public class Vigenere
{
    public string? Key { get; set; }
    public string? Plaintext { get; set; }

    public string? Keyword { get; set; }
    public string? Ciphertext { get; set; }

    // This function generates the key in
    // a cyclic manner until it's length isi'nt
    // equal to the length of original text
    public string GenerateKey()
    {
        //remove leading, trailing, and space within text
        string OriginalText = Plaintext.Trim().Replace(" ","").ToUpper();

        int x = OriginalText.Length;

        string OriginalKeyword = Keyword.Trim().Replace(" ","").ToUpper();

        for (int i = 0; ; i++)
        {
            if (x == i)
                i = 0;

            if (OriginalKeyword.Length == OriginalText.Length)
                break;

            OriginalKeyword += OriginalKeyword[i] ;
        }

        return OriginalKeyword;
    }

    // This function generates the key in
    // a cyclic manner until it's length isi'nt
    // equal to the length of original text
    public string GenerateKeyForDecrypt()
    {
        //remove leading, trailing, and space within text
        string OriginalCiphertext = Ciphertext.Trim().Replace(" ","").ToUpper();

        int x = OriginalCiphertext.Length;

        string OriginalKeyword = Keyword.Trim().Replace(" ","").ToUpper();

        for (int i = 0; ; i++)
        {
            if (x == i)
                i = 0;

            if (OriginalKeyword.Length == OriginalCiphertext.Length)
                break;

            OriginalKeyword += OriginalKeyword[i] ;
        }

        return OriginalKeyword;
    }
    
    // This function returns the encrypted text
    // generated with the help of the key
    public string CipherText( String key)
    {
        string OriginalText = Plaintext.Trim().Replace(" ","");
        
        for (int i = 0; i < OriginalText.Length; i++)
        {
            // converting in range 0-25
            int x = ( OriginalText[ i ] + key[ i ] ) % 26;
    
            // convert into alphabets(ASCII)
            x += 'A';
    
            Ciphertext += ( char )( x );
        }

        return Ciphertext;
    }
    
    // This function decrypts the encrypted text
    // and returns the original text
    public string OriginalText()
    { 
        string OriginalCipertext = Ciphertext.Trim().Replace(" ", "").ToUpper();
        string OriginalKeyword = Key.Trim().Replace(" ", "").ToUpper();

        // Console.WriteLine(OriginalCipertext);
        // Console.WriteLine(OriginalKeyword);
        for (int i = 0 ; i < OriginalCipertext.Length && i < OriginalKeyword.Length; i++)
        {
            // converting in range 0-25
            int x = (OriginalCipertext[i] - OriginalKeyword[i] + 26) %26;
    
            // convert into alphabets(ASCII)
            x += 'A';
            //Console.WriteLine(x);
            Plaintext+=(char)(x);
        }

        return Plaintext;
    }
}