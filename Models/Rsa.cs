namespace ciphers.Models;

public class Rsa
{
    public HashSet<long> PrimeNumberHolder = [];
    public long RsaPublicKey { get; set; }
    public long RsaPrivateKey { get; set; }
    public long n { get; set; }
    private readonly Random random = new();
 
    public void GenerateAndPopulatePrimeNumbers()
    {
        bool[] sieve = new bool[250];
        for (int i = 0; i < 250; i++)
        {
            sieve[i] = true;
        }
 
        sieve[0] = false;
        sieve[1] = false;
 
        for (int i = 2; i < 250; i++)
        {
            for (int j = i * 2; j < 250; j += i)
            {
                sieve[j] = false;

                //Console.WriteLine( j);
            }
        }
 
        for (int i = 0; i < sieve.Length; i++)
        {
            if (sieve[i])
            {
                PrimeNumberHolder.Add(i);

                //Console.WriteLine(i);
            }
        }
    }
 
    public long CreateRandomPrimeNumber()
    {
        long CurrentPrime = random.Next(0, PrimeNumberHolder.Count - 1);
        
        var enumerator = PrimeNumberHolder.GetEnumerator();

        for (long i = 0; i <= CurrentPrime; i++)
        {
            enumerator.MoveNext();
        }
 
        long ret = enumerator.Current;

        PrimeNumberHolder.Remove(ret);
        
        return ret;
    }
 
    public void GenerateRsaKeys()
    {
        long PrimeNumber1 = CreateRandomPrimeNumber();
        long PrimeNumber2 = CreateRandomPrimeNumber();
 
        n = PrimeNumber1 * PrimeNumber2;
        long fi = (PrimeNumber1 - 1) * (PrimeNumber2 - 1);
 
        long encrypt = 2;
        while (true)
        {
            if (CalculateGreatestCommonDivisor(encrypt, fi) == 1)
            {
                break;
            }
            encrypt += 1;
        }
 
        RsaPublicKey = encrypt;
 
        long decrypt = 2;
        while (true)
        {
            if ((decrypt * encrypt) % fi == 1)
            {
                break;
            }
            decrypt += 1;
        }
 
        RsaPrivateKey = decrypt;
    }
    
    public long CalculateGreatestCommonDivisor(long a, long b)
    {
        if (b == 0)
        {
            return a;
        }
        return CalculateGreatestCommonDivisor(b, a % b);
    }

    public List<long> RsaLetterEncoder(string message)
    {
        List<long> EncodedMessage = new List<long>();

        //Console.WriteLine(message);

        foreach(char letter in message)
        {
           // Console.WriteLine((int)letter);
            EncodedMessage.Add(RsaEncrypter( (long) letter ));
        }
        //Console.WriteLine(EncodedMessage);
        return EncodedMessage;
    }

    public long RsaEncrypter(long EncodedMessage)
    {
        long PrivateKey = RsaPrivateKey;

        //Console.WriteLine("inside the rsa encrypter:" + EncodedMessage);
        //Console.WriteLine("inside the rsa encrypter:" + PrivateKey);
        long EncryptedMessage = 1;

        while(PrivateKey > 0)
        {
            //Console.WriteLine("PrivateKey:" + PrivateKey);
            EncryptedMessage *= EncodedMessage;
            //Console.WriteLine("inside the rsa encrypter:" + EncryptedMessage + " and " + EncodedMessage);
            //Console.WriteLine(n);
            EncryptedMessage %= n;
            //Console.WriteLine(EncryptedMessage);
            PrivateKey -= 1;

        }

        return EncryptedMessage;
    }

    public string RsaLetterDecoder(List<long> EncodedMessage)
    {
        string DecodedMessage = "";

        foreach(long number in EncodedMessage)
        {
            DecodedMessage += (char) RsaDecrypter(number);
        }

        return DecodedMessage;
    }

    public long RsaDecrypter(long EncryptedMessage)
    {
        long PublicKey = RsaPublicKey;

        long DecryptedMessage = 1;

        while(PublicKey > 0)
        {
            DecryptedMessage *= EncryptedMessage;
            DecryptedMessage %= n;

            PublicKey -= 1;
        }

        return DecryptedMessage;

    }

}