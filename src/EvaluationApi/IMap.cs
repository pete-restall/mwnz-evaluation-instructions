namespace MiddlewareNz.EvaluationApi;

public interface IMap<in TFrom, out TTo>
{
	TTo MapFrom(TFrom unmapped);
}
