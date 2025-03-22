using System.Xml.Serialization;
using FluentAssertions;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using RestEase;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public class XmlResponseDeserialiserTest
{
	[XmlRoot("dummy")]
	public class DummyXmlDto
	{
		[XmlElement("token")]
		public int Token;
	}

	public class DummyDto;

	public class DummyIncompatibleDto;

	[Fact]
	public void Constructor_CalledWithNullXmlDeserialiser_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var constructor = () => CreateXmlResponseDeserialiserWith((XmlSerializer) null!);
		constructor.Should().Throw<ArgumentNullException>().WithParameterName("xmlDeserialiser");
	}

	private static XmlResponseDeserialiser<DummyXmlDto, DummyDto> CreateXmlResponseDeserialiserWith(XmlSerializer xmlDeserialiser) =>
		new(xmlDeserialiser, DummyMapper());

	private static IMap<DummyXmlDto, DummyDto> DummyMapper() => MapTestDoubles.Dummy<DummyXmlDto, DummyDto>();

	[Fact]
	public void Constructor_CalledWithNullMapper_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var constructor = () => CreateXmlResponseDeserialiserWith((IMap<DummyXmlDto, DummyDto>) null!);
		constructor.Should().Throw<ArgumentNullException>().WithParameterName("mapper");
	}
	private static XmlResponseDeserialiser<DummyXmlDto, DummyDto> CreateXmlResponseDeserialiserWith(IMap<DummyXmlDto, DummyDto> mapper) =>
		new(DummyXmlDeserialiser(), mapper);

	private static XmlSerializer DummyXmlDeserialiser() => new(typeof(DummyXmlDto));

	[Fact]
	public void Deserialize_CalledWithNullContent_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var deserialiser = CreateXmlResponseDeserialiserWith(DummyMapper());
		deserialiser
			.Invoking(x => x.Deserialize<DummyDto>(null, DummyHttpResponseMessage(), DummyResponseDeserializerInfo()))
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("content");
	}

	private static HttpResponseMessage DummyHttpResponseMessage() => new();

	private static ResponseDeserializerInfo DummyResponseDeserializerInfo() => new();

	[Fact]
	public void Deserialize_CalledWithTypeArgumentIncompatibleWithMappedType_ExpectInvalidOperationException()
	{
		var deserialiser = CreateXmlResponseDeserialiserWith(DummyMapper());
		deserialiser
			.Invoking(x => x.Deserialize<DummyIncompatibleDto>(DummyContent(), DummyHttpResponseMessage(), DummyResponseDeserializerInfo()))
			.Should().Throw<InvalidOperationException>()
			.WithMessage("*incompatible*");
	}

	private static string DummyContent() => "";

	[Fact]
	public void Deserialize_CalledWhenMapperReturnsNull_ExpectInvalidOperationException()
	{
		var mapper = MapTestDoubles.StubFor<DummyXmlDto, DummyDto>(null!);
		var deserialiser = CreateXmlResponseDeserialiserWith(mapper);
		deserialiser
			.Invoking(x => x.Deserialize<DummyDto>(StubContent(), DummyHttpResponseMessage(), DummyResponseDeserializerInfo()))
			.Should().Throw<InvalidOperationException>()
			.WithMessage("*mapper*returned*null*");
	}

	private static string StubContent() => StubContentForToken(AnyToken());

	private static string StubContentForToken(int token) => $"<dummy><token>{token}</token></dummy>";

	private static int AnyToken() => IntegerGenerator.Any();

	[Fact]
	public void Deserialize_Called_ExpectMappedDeserialisedDtoIsReturned()
	{
		var token = AnyToken();
		var mapped = new DummyDto();
		var mapper = MapTestDoubles.StubFor((DummyXmlDto unmapped) => unmapped.Token == token, mapped);
		var deserialiser = CreateXmlResponseDeserialiserWith(mapper);
		var response = deserialiser.Deserialize<DummyDto>(StubContentForToken(token), DummyHttpResponseMessage(), DummyResponseDeserializerInfo());
		response.Should().BeSameAs(mapped);
	}
}
