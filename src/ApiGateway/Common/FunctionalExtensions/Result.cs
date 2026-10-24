namespace FunctionalExtensions
{
	public class Result : IResult
	{
		public bool IsSuccess { get; private set; }
		public bool IsFailure => !IsSuccess;
		public string Error { get; private set; }


		protected Result(bool isSuccess, string error)
		{
			if (isSuccess && !string.IsNullOrEmpty(error))
				throw new InvalidOperationException("A successful result cannot have an error message.");

			if (!isSuccess && string.IsNullOrEmpty(error))
				throw new InvalidOperationException("A failure result must have an error message.");

			IsSuccess = isSuccess;
			Error = error;
		}
		public static Result Success() => new Result(true, string.Empty);

		public static Result Failure(string error) => new Result(false, error);
	}

	public class Result<T> : Result, IResult<T>
	{
		public T Value { get; private set; }

		protected internal Result(bool isSuccess, string error, T value) : base(isSuccess, error)
		{
			Value = value;
		}

		public static Result<T> Success(T value) => new Result<T>(true, string.Empty, value);

		public static new Result<T> Failure(string error) => new Result<T>(false, error, default!);
	}
}
