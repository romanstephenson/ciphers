namespace ciphers.Models;

public class Rsa
{
    public static HashSet<int> prime = new HashSet<int>();
    public int RsaPublicKey;
    public int RsaPrivateKey;
    public int n;
    private Random random = new Random();
 
    // public static void Main()
    // {
    //     PrimeFiller();
    //     GenerateRsaKeys();
    //     string message = "Test Message";
    //     // Uncomment below for manual input
    //     // Console.WriteLine("Enter the message:");
    //     // message = Console.ReadLine();
 
    //     List<int> coded = Encoder(message);
 
    //     Console.WriteLine("Initial message:");
    //     Console.WriteLine(message);
    //     Console.WriteLine("\n\nThe encoded message (encrypted by public key)\n");
    //     Console.WriteLine(string.Join("", coded));
    //     Console.WriteLine("\n\nThe decoded message (decrypted by public key)\n");
    //     Console.WriteLine(Decoder(coded));
    // }
 
    public void PrimeFiller()
    {
        bool[] sieve = new bool[2048];
        for (int i = 0; i < 2048; i++)
        {
            sieve[i] = true;
        }
 
        sieve[0] = false;
        sieve[1] = false;
 
        for (int i = 2; i < 2048; i++)
        {
            for (int j = i * 2; j < 2048; j += i)
            {
                sieve[j] = false;

                //Console.WriteLine( j);
            }
        }
 
        for (int i = 0; i < sieve.Length; i++)
        {
            if (sieve[i])
            {
                prime.Add(i);

                //Console.WriteLine(i);
            }
        }
    }
 
    public int CreateRandomPrimeNumber()
    {
        int k = random.Next(0, prime.Count - 1);
        
        var enumerator = prime.GetEnumerator();

        for (int i = 0; i <= k; i++)
        {
            enumerator.MoveNext();
        }
 
        int ret = enumerator.Current;
        prime.Remove(ret);
        return ret;
    }
 
    public void GenerateRsaKeys()
    {
        int prime1 = CreateRandomPrimeNumber();
        int prime2 = CreateRandomPrimeNumber();
 
        n = prime1 * prime2;
        int fi = (prime1 - 1) * (prime2 - 1);
 
        int e = 2;
        while (true)
        {
            if (GCD(e, fi) == 1)
            {
                break;
            }
            e += 1;
        }
 
        RsaPublicKey = e;
 
        int d = 2;
        while (true)
        {
            if ((d * e) % fi == 1)
            {
                break;
            }
            d += 1;
        }
 
        RsaPrivateKey = d;
    }
    
    public int GCD(int a, int b)
    {
        if (b == 0)
        {
            return a;
        }
        return GCD(b, a % b);
    }
}