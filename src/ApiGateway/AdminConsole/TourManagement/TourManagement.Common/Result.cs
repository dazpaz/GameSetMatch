namespace TourManagement.Common
{
	public class Result : IResult
	{
		public bool IsSuccess { get; private set; }
		public bool IsFailure => !IsSuccess;
		public ErrorCode Error { get; private set; }
		public string ErrorMessage { get; private set; }

		protected Result(bool isSuccess, ErrorCode error, string errorMessage)
		{
			if (isSuccess && error != ErrorCode.None)
				throw new InvalidOperationException("A successful result cannot have an error code.");

			if (!isSuccess && error == ErrorCode.None)
				throw new InvalidOperationException("A failure result must have an error code.");

			IsSuccess = isSuccess;
			Error = error;

			if (string.IsNullOrWhiteSpace(errorMessage)) errorMessage = error.ToString();
			ErrorMessage = isSuccess ? string.Empty : errorMessage;
		}
		public static Result Success() => new(true, ErrorCode.None, string.Empty);

		public static Result Failure(ErrorCode error) => new(false, error, string.Empty);

		public static Result Failure(ErrorCode error, string errorMessage) => new(false, error, errorMessage);
	}

	public class Result<T> : Result, IResult<T>
	{
		public T Value { get; private set; }

		protected internal Result(bool isSuccess, ErrorCode error, string errorMessage, T value) : base(isSuccess, error, errorMessage)
		{
			Value = value;
		}

		public static Result<T> Success(T value) => new(true, ErrorCode.None, string.Empty, value);

		public static new Result<T> Failure(ErrorCode error) => new(false, error, string.Empty, default!);

		public static new Result<T> Failure(ErrorCode error, string errorMessage) => new(false, error, errorMessage, default!);
	}
}
