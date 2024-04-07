using System.ComponentModel.DataAnnotations;

namespace ciphers.Models;

public class DiffieHellman
{
    /*Sender will choose a prime number*/
    public long SenderPrimeNumber { get; set; }

    /*Sender will also choose a private key value*/
    public long SenderPrivateKey { get; set; }

    /**/
    public long SenderPrimitiveRoot { get; set; }

    /*Sender Public Key*/
    public long SenderPublicKey { get; set; }

    /*Senders secret key will be stored here when generated*/
    public long SenderSecretKey { get; set; }


     /*Sender will choose a prime number*/
    public long ReceiverPrimeNumber { get; set; }

    /*Receiver prime key, which is primitive root of sender prime number*/
    //public long ReceiverPrimitiveRoot { get; set; }

    /*Receiver chooses private key*/
    public long ReceiverPrivateKey { get; set; }

    /*Receiver secret key*/
    public long ReceiverSecretKey { get; set; }

    /*Reciever Public Key*/
    public long ReceiverPublicKey { get; set; }

    /*
        takes a and raise it to b then mod by p
        a being prime number
        b being private key
        p being primitive root of a
    */ 
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