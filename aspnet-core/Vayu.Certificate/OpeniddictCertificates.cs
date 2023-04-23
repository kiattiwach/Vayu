using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Vayu.Certificate;

public class OpeniddictCertificates
{
    private static string _password = "R318gVEBWHzTJDF3BdBE";
    public static void CreateApiEncryptionCertificate()
    {
        using var algorithm = RSA.Create(keySizeInBits: 2048);

        var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
        var request = new CertificateRequest(
            subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));

        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(10));

        File.WriteAllBytes("api-encryption-certificate.pfx",
            certificate.Export(X509ContentType.Pfx, _password));
    }

    public static void CreateApiSigningCertificate()
    {
        using var algorithm = RSA.Create(keySizeInBits: 2048);

        var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
        var request = new CertificateRequest(
            subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));

        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(10));

        File.WriteAllBytes("api-signing-certificate.pfx",
            certificate.Export(X509ContentType.Pfx, _password));
    }
}