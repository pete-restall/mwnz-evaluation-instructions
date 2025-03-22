using System.Linq.Expressions;
using NSubstitute;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public static class MapTestDoubles
{
	public static IMap<TFrom, TTo> Dummy<TFrom, TTo>() => Substitute.For<IMap<TFrom, TTo>>();

	public static IMap<TFrom, TTo> StubFor<TFrom, TTo>(TTo mapped)
	{
		var mapper = Substitute.For<IMap<TFrom, TTo>>();
		mapper.MapFrom(Arg.Any<TFrom>()).Returns(mapped);
		return mapper;
	}

	public static IMap<TFrom, TTo> StubFor<TFrom, TTo>(Expression<Predicate<TFrom>> unmapped, TTo mapped)
	{
		var mapper = Substitute.For<IMap<TFrom, TTo>>();
		mapper.MapFrom(Arg.Is(unmapped)).Returns(mapped);
		return mapper;
	}
}
