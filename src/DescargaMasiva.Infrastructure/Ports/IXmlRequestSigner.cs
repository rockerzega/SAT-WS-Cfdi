using System.Xml;
namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IXmlRequestSigner
{
  XmlElement SignRequest(XmlElement xml, byte[] pfx, string password);

  XmlElement SignAuthenticationRequest(
    XmlElement xml,
    byte[] pfx,
    string password,
    string referenceUri,
    XmlElement securityTokenReferenceElement);
}