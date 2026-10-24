namespace Common.FunctionalExtensions.UnitTests
{
	public class ResultTests
	{
		[Fact]
		public void Success_ShouldCreateSuccessfulResult()
		{
			var result = Result.Success();

			Assert.True(result.IsSuccess);
			Assert.False(result.IsFailure);
			Assert.Equal(string.Empty, result.Error);
		}

		[Fact]
		public void Failure_ShouldCreateFailedResult()
		{
			const string error = "Something went wrong";

			var result = Result.Failure(error);

			Assert.False(result.IsSuccess);
			Assert.True(result.IsFailure);
			Assert.Equal(error, result.Error);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Failure_ShouldThrow_WhenErrorIsNullOrEmpty(string? error)
		{
			var exception = Assert.Throws<InvalidOperationException>(() => Result.Failure(error!));

			Assert.Equal("A failure result must have an error message.", exception.Message);
		}

		[Fact]
		public void Constructor_ShouldThrow_WhenSuccessfulResultHasError()
		{
			var exception = Assert.Throws<InvalidOperationException>(() => new TestableResult(true, "error"));

			Assert.Equal("A successful result cannot have an error message.", exception.Message);
		}

		private sealed class TestableResult : Result
		{
			public TestableResult(bool isSuccess, string error)
				: base(isSuccess, error)
			{
			}

		}
	}
}
