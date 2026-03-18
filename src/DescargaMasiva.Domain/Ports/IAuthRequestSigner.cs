using System.Xml;

namespace DescargaMasiva.DescargaMasiva.Domain.Ports;

public interface IAuthRequestSigner
{
  XmlElement Sign(
    XmlElement elementToSign,
    string referenceUri,
    XmlElement securityTokenReference);
    
  string GetCertificateBase64();
}