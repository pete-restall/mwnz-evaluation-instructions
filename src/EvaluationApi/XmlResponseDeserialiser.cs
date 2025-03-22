using System.Xml.Serialization;
using RestEase;

namespace MiddlewareNz.EvaluationApi;

public class XmlResponseDeserialiser<TXml, TResponse> : ResponseDeserializer
{
	private readonly XmlSerializer xmlDeserialiser;
	private readonly IMap<TXml, TResponse> mapper;

	public XmlResponseDeserialiser(XmlSerializer xmlDeserialiser, IMap<TXml, TResponse> mapper)
	{
		this.xmlDeserialiser = xmlDeserialiser ?? throw new ArgumentNullException(nameof(xmlDeserialiser));
		this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public override T Deserialize<T>(string? content, HttpResponseMessage response, ResponseDeserializerInfo info)
	{
		if (!typeof(T).IsAssignableTo(typeof(TResponse)))
		{
			throw new InvalidOperationException(
				$"Cannot deserialise incompatible response; type={typeof(TResponse)}, xmlType={typeof(TXml)}, incompatibleType={typeof(T)}");
		}

		using var stringReader = new StringReader(content ?? throw new ArgumentNullException(nameof(content)));
		var deserialised = (TXml) (
			this.xmlDeserialiser.Deserialize(stringReader)
			?? throw new InvalidOperationException($"XML deserialiser returned null; type={typeof(T)}, xmlType={typeof(TXml)}"));

		return (T) (object) (
			this.mapper.MapFrom(deserialised)
			?? throw new InvalidOperationException($"Mapper returned null; type={typeof(T)}, xmlType={typeof(TXml)}, mapper={this.mapper}"));
	}
}
