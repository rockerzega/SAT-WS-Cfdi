using DescargaMasiva.DescargaMasiva.Application.Ports.Out;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;

namespace DescargaMasiva.DescargaMasiva.Adapters;

public class PfxGeneratorAdapter: IPfxGenerator
{
  public byte[] GenerarPfx(string certificate, string privateKey, string password)
  {
    // Decodificar el certificado desde Base64 a binario
    byte[] certBytes = Convert.FromBase64String(certificate);
    var certParser = new X509CertificateParser();
    var cert = certParser.ReadCertificate(certBytes);

    // Decodificar la clave privada cifrada desde Base64 a binario
    byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

    // Desencriptar la clave privada usando la contraseña
    // Corregido: Uso de PrivateKeyFactory.DecryptKey
    AsymmetricKeyParameter privateKeyParam = DecryptPrivateKey(privateKeyBytes, password);

    // Crear el archivo PKCS12 (.pfx) con la misma contraseña
    // Corregido: Uso de Pkcs12Store.CreateEmpty()
    byte[] pfxBytes = CreatePkcs12(cert, privateKeyParam, password);
    return pfxBytes;
  }

  private AsymmetricKeyParameter DecryptPrivateKey(byte[] privateKeyBytes, string password)
  {
    try
    {
      return PrivateKeyFactory.DecryptKey(password.ToCharArray(), privateKeyBytes);
    }
    catch (Exception ex)
    {
      // Manejo de errores: Si la clave no es una PKCS#8 cifrada, o la contraseña es incorrecta.
      throw new Exception("Error al desencriptar la clave privada (verifique el formato y la contraseña).", ex);
    }
  }

  private byte[] CreatePkcs12(X509Certificate cert, AsymmetricKeyParameter privateKey, string password)
  {
    var pkcs12Store = new Pkcs12StoreBuilder().Build();

    // El alias de la clave/certificado
    string alias = cert.SubjectDN.ToString(); 
        
    // Añadir el certificado y la clave privada al almacén PKCS12
    var certEntry = new X509CertificateEntry(cert);
        
    // La clave privada es el KeyEntry, el certificado asociado va en la cadena.
    pkcs12Store.SetKeyEntry(alias, new AsymmetricKeyEntry(privateKey), new[] { certEntry });

    // Convertir a PFX (.pfx) y devolver el resultado en un array de bytes
    using (var memoryStream = new MemoryStream())
    {
      // Usamos SecureRandom para la generación del PFX
      pkcs12Store.Save(memoryStream, password.ToCharArray(), new SecureRandom());
      return memoryStream.ToArray();
    }
  }
}