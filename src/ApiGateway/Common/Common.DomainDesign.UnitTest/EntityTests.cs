namespace Common.DomainDesign.UnitTest
{
	public class EntityTests
	{
		private class TestEntityA : Entity<int>
		{
			public TestEntityA() : base() { }
			public TestEntityA(int id) : base(id) { }
		}

		private class TestEntityB : Entity<int>
		{
			public TestEntityB() : base() { }
			public TestEntityB(int id) : base(id) { }
		}

		private class TestEntityGuid : Entity<Guid>
		{
			public TestEntityGuid() : base() { }
			public TestEntityGuid	(Guid id) : base(id) { }
		}

		[Fact]
		public void Constructor_WithValidId_SetsId()
		{
			var e = new TestEntityA(5);
			Assert.Equal(5, e.Id);
		}

		[Fact]
		public void ParameterlessConstructor_SetsIdToDefault()
		{
			var e1 = new TestEntityGuid();
			var e2 = new TestEntityA();

			Assert.Equal(default, e1.Id);
			Assert.Equal(default, e2.Id);
		}

		[Fact]
		public void Constructor_WithDefaultId_Throws()
		{
			Assert.Throws<ArgumentException>(() => new TestEntityA(0));
		}

		[Fact]
		public void Equals_SameTypeAndId_AreEqual()
		{
			var a = new TestEntityA(7);
			var b = new TestEntityA(7);

			Assert.True(a.Equals(b));
			Assert.True(a == b);
			Assert.False(a != b);
			Assert.Equal(a.GetHashCode(), b.GetHashCode());
		}

		[Fact]
		public void Equals_CompareWithNull_ReturnsFalse()
		{
			var a = new TestEntityA(1);
			TestEntityA? b = null;

			Assert.False(a.Equals(b));
			Assert.False(a == b);
			Assert.False(b == a);
			Assert.True(a != b);
		}

		[Fact]
		public void Equals_WhenBothAreNull_ReturnsTrue()
		{
			TestEntityA? a = null;
			TestEntityA? b = null;

			Assert.True(a == b);
			Assert.False(a != b);
		}

		[Fact]
		public void Equals_DifferentTypeSameId_AreEqual()
		{
			var a = new TestEntityA(3);
			var b = new TestEntityB(3);

			// The typed Equals(Entity<TId>) implementation compares Ids only, so
			// different subclasses with the same id are considered equal.
			Assert.True(a.Equals(b));
			Assert.True(a == b);
			Assert.False(a != b);
		}

		[Fact]
		public void EqualsObject_OtherObjectIsDifferentType_ReturnsFalse()
		{
			var a = new TestEntityA(3);
			var b = new TestEntityB(3);

			Assert.False(a.Equals((object)b));
		}

		[Fact]
		public void EqualsObject_OtherObjectIsNotEntityOfRightType_ReturnsFalse()
		{
			var a = new TestEntityA(3);
			var b = new TestEntityGuid();

			Assert.False(a.Equals(b));
		}

		[Fact]
		public void EqualsObject_EqualObject_ReturnsTrue()
		{
			var a = new TestEntityA(3);
			var b = new TestEntityA(3);

			Assert.True(a.Equals((object)b));
		}

		[Fact]
		public void EqualsObject_DifferntId_ReturnsFalse()
		{
			var a = new TestEntityA(3);
			var b = new TestEntityA(4);

			Assert.False(a.Equals((object)b));
		}

		[Fact]
		public void EqualsObject_SameObject_ReturnsTrue()
		{
			var a = new TestEntityA(3);
			var b = a;

			Assert.True(a.Equals((object)b));
		}

		[Fact]
		public void EqualsObject_EntityHasDefaultId_ReturnsFalse()
		{
			var a = new TestEntityA();
			var b = new TestEntityA(3);

			Assert.False(a.Equals((object)b));
			Assert.False(b.Equals((object)a));
		}

		[Fact]
		public void EqualsObject_BothEntitiesHaveDefaultId_ReturnsFalse()
		{
			var a = new TestEntityA();
			var b = new TestEntityA();
			Assert.False(a.Equals((object)b));
		}

		[Fact]
		public void GetHashCode_DifferentTypesSameId_ReturnsDifferentHashCodes()
		{
			var a = new TestEntityA(10);
			var b = new TestEntityB(10);
			Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
		}

		[Fact]
		public void GetHashCode_SameTypesDifferentId_ReturnsDifferentHashCodes()
		{
			var a = new TestEntityA(10);
			var b = new TestEntityA(20);
			Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
		}

		[Fact]
		public void GetHashCode_SameIdAndType_ReturnsSameHashCode()
		{
			var a = new TestEntityA(10);
			var b = new TestEntityA(10);
			Assert.Equal(a.GetHashCode(), b.GetHashCode());
		}
	}
}