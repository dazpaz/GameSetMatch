namespace DomainDesign.Common.UnitTest
{
	public class AggregateRootTests
	{
		private class TestEvent : IDomainEvent { }

		private class TestAggregate : AggregateRoot<int>
		{
			public TestAggregate() { }
			public void AddEvent(IDomainEvent ev) => RaiseDomainEvent(ev);
		}

		[Fact]
		public void DomainEvents_InitiallyEmpty()
		{
			var agg = new TestAggregate();

			Assert.Empty(agg.DomainEvents);
		}

		[Fact]
		public void RaiseDomainEvent_AddsEvent()
		{
			var agg = new TestAggregate();
			var ev = new TestEvent();

			agg.AddEvent(ev);

			Assert.Single(agg.DomainEvents);
			Assert.Same(ev, agg.DomainEvents[0]);
		}

		[Fact]
		public void ClearDomainEvents_RemovesEvents()
		{
			var agg = new TestAggregate();
			agg.AddEvent(new TestEvent());
			Assert.NotEmpty(agg.DomainEvents);

			agg.ClearDomainEvents();

			Assert.Empty(agg.DomainEvents);
		}
	}
}
