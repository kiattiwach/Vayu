using Vayu.Certificate;

class Program
{
    static async Task Main(string[] args)
    {
        OpeniddictCertificates.CreateApiEncryptionCertificate();
        OpeniddictCertificates.CreateApiSigningCertificate();
        
        Console.WriteLine("Certificate Finished...");
    }
}