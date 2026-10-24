namespace Common.FunctionalExtensions.UnitTests
{
	public class GenericResultTests
	{
		[Fact]
		public void Success_ShouldCreateSuccessfulGenericResult()
		{
			const int value = 42;

			var result = Result<int>.Success(value);

			Assert.True(result.IsSuccess);
			Assert.False(result.IsFailure);
			Assert.Equal(string.Empty, result.Error);
			Assert.Equal(value, result.Value);
		}

		[Fact]
		public void Failure_ShouldCreateFailedGenericResult()
		{
			const string error = "Something went wrong";

			var result = Result<int>.Failure(error);

			Assert.False(result.IsSuccess);
			Assert.True(result.IsFailure);
			Assert.Equal(error, result.Error);
			Assert.Equal(default, result.Value);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Failure_ShouldThrow_WhenErrorIsNullOrEmpty(string? error)
		{
			var exception = Assert.Throws<InvalidOperationException>(() => Result<int>.Failure(error!));

			Assert.Equal("A failure result must have an error message.", exception.Message);
		}

		[Fact]
		public void Constructor_ShouldThrow_WhenSuccessfulGenericResultHasError()
		{
			var exception = Assert.Throws<InvalidOperationException>(() => new TestableGenericResult<int>(true, "error", 1));

			Assert.Equal("A successful result cannot have an error message.", exception.Message);
		}

		private sealed class TestableGenericResult<T> : Result<T>
		{
			public TestableGenericResult(bool isSuccess, string error, T value)
				: base(isSuccess, error, value)
			{
			}
		}
	}
}
