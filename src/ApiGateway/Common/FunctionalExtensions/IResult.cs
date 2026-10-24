namespace FunctionalExtensions
{
	public interface IResult
	{
		bool IsSuccess { get; }
		bool IsFailure { get; }
		string Error { get; }
	}

	public interface IResult<T> : IResult
	{
		T Value { get; }
	}
}
