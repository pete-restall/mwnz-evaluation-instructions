using System.Xml.Serialization;

namespace MiddlewareNz.EvaluationApi.Companies;

[XmlRoot("Data")]
public class XmlBackendCompany
{
	[XmlElement("id")]
	public int Id;

	[XmlElement("name")]
	public string Name = "";

	[XmlElement("description")]
	public string Description = "";
}
