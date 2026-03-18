using System.Xml;
using System.Security.Cryptography.X509Certificates;
using DescargaMasiva.DescargaMasiva.Domain.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Security;

public sealed class X509AuthRequestSigner : IAuthRequestSigner
{
  private readonly X509Certificate2 _certificate;

  public X509AuthRequestSigner(X509Certificate2 certificate)
  {
    _certificate = certificate;
  }

  public XmlElement Sign(
    XmlElement elementToSign,
    string referenceUri,
    XmlElement securityTokenReference)
  {
    return SignedXmlHelper.SignAuthenticationRequest(
      elementToSign,
      _certificate,
      referenceUri,
      securityTokenReference);
  }

  public string GetCertificateBase64()
  {
    return Convert.ToBase64String(_certificate.RawData);
  }
}