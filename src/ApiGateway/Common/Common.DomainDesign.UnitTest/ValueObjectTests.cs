namespace Common.DomainDesign.UnitTest
{
	public class ValueObjectTests
	{
		private sealed class TestValueObject(int number, string? text) : ValueObject<TestValueObject>
		{
			public int Number { get; } = number;
			public string? Text { get; } = text;

			protected override IEnumerable<object?> GetEqualityComponents()
			{
				yield return Number;
				yield return Text;
			}
		}

		private sealed class AnotherValueObject(int number, string? text) : ValueObject<AnotherValueObject>
		{
			public int Number { get; } = number;
			public string? Text { get; } = text;

			protected override IEnumerable<object?> GetEqualityComponents()
			{
				yield return Number;
				yield return Text;
			}
		}

		[Fact]
		public void Value_objects_with_same_components_are_equal()
		{
			var a = new TestValueObject(1, "x");
			var b = new TestValueObject(1, "x");

			Assert.True(a.Equals(b));
			Assert.True(a == b);
			Assert.False(a != b);
			Assert.Equal(a.GetHashCode(), b.GetHashCode());
			Assert.True(((object)a).Equals(b));
		}

		[Fact]
		public void Value_objects_with_different_components_are_not_equal()
		{
			var a = new TestValueObject(1, "x");
			var b = new TestValueObject(2, "x");

			Assert.False(a.Equals(b));
			Assert.False(a == b);
			Assert.True(a != b);
		}

		[Fact]
		public void Value_objects_with_null_components_are_handled()
		{
			var a = new TestValueObject(1, null);
			var b = new TestValueObject(1, null);

			Assert.True(a.Equals(b));
			Assert.Equal(a.GetHashCode(), b.GetHashCode());
		}

		[Fact]
		public void Reference_equality_returns_true()
		{
			var a = new TestValueObject(5, "ref");
			var b = a;

			Assert.True(a.Equals(b));
			Assert.True(a == b);
		}

		[Fact]
		public void Different_types_with_same_components_are_not_equal()
		{
			var a = new TestValueObject(3, "t");
			var b = new AnotherValueObject(3, "t");

			// Even though components match, types differ so equality should be false
			Assert.False(a.Equals(b));
		}

		[Fact]
		public void Equals_with_null_returns_false()
		{
			var a = new TestValueObject(1, "x");
			TestValueObject? b = null;

			Assert.False(a.Equals(b));
			Assert.False(a == b);
			Assert.True(a != b);
		}
	}
}
