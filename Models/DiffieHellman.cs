using System.ComponentModel.DataAnnotations;

namespace ciphers.Models;

public class DiffieHellman
{
    /*Sender will choose a prime number*/
    public long SenderPrimeNumber { get; set; }

    /*Sender will also choose a private key value*/
    public long SenderPrivateKey { get; set; }

    /*Senders secret key will be stored here when generated*/
    public long SenderSecretKey { get; set; }

    /*Receiver prime key, which is primitive root of sender prime number*/
    public long ReceiverPrimitiveRootofSender { get; set; }

    /*Receiver chooses private key*/
    public long ReceiverPrivateKey { get; set; }

    /*Receiver secret key*/
    public long ReceiverSecretKey { get; set; }

    public long GenerateKey(long a, long b, long p)
    {

        if(b == 1)
        {
            return a;
        }
        else{
            return ((long)Math.Pow(a,b)) % p;
        }

    } 
}