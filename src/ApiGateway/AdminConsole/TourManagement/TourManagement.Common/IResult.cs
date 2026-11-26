namespace TourManagement.Common
{
	public interface IResult
	{
		bool IsSuccess { get; }
		bool IsFailure { get; }
		ErrorCode Error { get; }
		string ErrorMessage { get; }
	}

	public interface IResult<T> : IResult
	{
		T Value { get; }
	}
}
