using System.Xml.Linq;

namespace IndDev.Domain.Abstract
{
    public interface IXml
    {
        XDocument CreateSitemapDocument();
        XDocument CreateShopYml();
    }
}