using System.Security.Cryptography.Xml;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters.Soap;

internal sealed class SignedXmlWithId : SignedXml
{
  public SignedXmlWithId(XmlDocument xml) : base(xml)
  {
  }

  public SignedXmlWithId(XmlElement xmlElement) : base(xmlElement)
  {
  }

  public override XmlElement GetIdElement(XmlDocument doc, string id)
  {
    // check to see if it's a standard ID reference
    XmlElement idElem = base.GetIdElement(doc, id);

    if (idElem is null)
    {
      var nsManager = new XmlNamespaceManager(doc.NameTable);
      nsManager.AddNamespace(CfdiDescargaMasivaWsNamespaces.WsuPrefix, CfdiDescargaMasivaWsNamespaces.WsuNamespaceUrl);

      idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
    }

    return idElem;
  }
}