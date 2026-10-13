namespace DomainDesign.Common.UnitTest
{
	public class EntityTests
	{
		private sealed class TestEntity : Entity<int>
		{
			public TestEntity() : base() { }
			public TestEntity(int id) : base(id) { }
		}

		private sealed class OtherEntity : Entity<int>
		{
			public OtherEntity() : base() { }
			public OtherEntity(int id) : base(id) { }
		}

		[Fact]
		public void Equals_SameReference_ReturnsTrue()
		{
			var e = new TestEntity(1);

			Assert.True(e.Equals(e));

			#pragma warning disable CS1718
			Assert.True(e == e);
			#pragma warning restore CS1718
		}

		[Fact]
		public void Equals_ObjectOverload_Null_ReturnsFalse()
		{
			var e = new TestEntity(1);

			Assert.False(e.Equals((object?)null));
		}

		[Fact]
		public void Equals_DifferentTypesWithSameId_ReturnsFalse()
		{
			var a = new TestEntity(5);
			var b = new OtherEntity(5);

			Assert.False(a.Equals(b));
			Assert.False(a == b);
		}

		[Fact]
		public void Equals_SameId_ReturnsTrue()
		{
			var a = new TestEntity(42);
			var b = new TestEntity(42);

			Assert.True(a.Equals(b));
			Assert.True(a == b);
			Assert.False(a != b);
		}

		[Fact]
		public void Equals_DifferentId_ReturnsFalse()
		{
			var a = new TestEntity(1);
			var b = new TestEntity(2);

			Assert.False(a.Equals(b));
			Assert.False(a == b);
			Assert.True(a != b);
		}

		[Fact]
		public void GetHashCode_WithId_UsesIdHashCode()
		{
			var id = 123;
			var e = new TestEntity(id);

			var expected = EqualityComparer<int>.Default.GetHashCode(id);

			Assert.Equal(expected, e.GetHashCode());
		}

		[Fact]
		public void OperatorEquality_NullHandling()
		{
			TestEntity? left = null;
			TestEntity? right = null;

			Assert.True(left == right);

			left = new TestEntity(1);
			Assert.False(left == right);
			Assert.True(left != right);
		}
	}
}
