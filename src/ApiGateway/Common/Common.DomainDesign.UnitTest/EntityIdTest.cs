namespace Common.DomainDesign.UnitTest
{
	public sealed class EntityIdTest
	{
		private sealed class TestEntityId : EntityId<TestEntityId>
		{
			public TestEntityId() : base() { }
			public TestEntityId(Guid id) : base(id) { }
		}

		[Fact]
		public void New_entityId_has_non_empty_id()
		{
			var id = new TestEntityId();
			Assert.False(id.Id == Guid.Empty);
		}

		[Fact]
		public void Construct_with_empty_guid_throws()
		{
			Assert.Throws<ArgumentException>(() => new TestEntityId(Guid.Empty));
		}

		[Fact]
		public void Two_entityIds_with_same_guid_are_equal()
		{
			var guid = Guid.NewGuid();
			var a = new TestEntityId(guid);
			var b = new TestEntityId(guid);

			Assert.True(a.Equals(b));
			Assert.True(a == b);
			Assert.False(a != b);
			Assert.Equal(a.GetHashCode(), b.GetHashCode());
		}

		[Fact]
		public void Entities_with_different_guids_are_not_equal()
		{
			var a = new TestEntityId();
			var b = new TestEntityId();

			// very unlikely to collide
			Assert.False(a.Equals(b));
			Assert.False(a == b);
			Assert.True(a != b);
		}

		[Fact]
		public void Equals_with_null_returns_false()
		{
			var a = new TestEntityId();
			TestEntityId? b = null;
			Assert.False(a.Equals(b));
			Assert.False(a.Equals((object?)null));
		}

		[Fact]
		public void ToString_returns_guid_string()
		{
			var guid = Guid.NewGuid();
			var a = new TestEntityId(guid);
			Assert.Equal(guid.ToString(), a.ToString());
		}
	}
}
