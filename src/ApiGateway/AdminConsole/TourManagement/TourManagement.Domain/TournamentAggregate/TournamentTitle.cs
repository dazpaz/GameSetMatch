using Ardalis.GuardClauses;
using Common.DomainDesign;

namespace TourManagement.Domain.TournamentAggregate
{
	public sealed class TournamentTitle : ValueObject<TournamentTitle>
	{
		public string Value { get; }

		private TournamentTitle(string value)
		{
			Value = value;
		}

		public static TournamentTitle Create(string title)
		{
			title = Guard.Against.NullOrWhiteSpace(title);
			return new TournamentTitle(title);
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Value;
		}

		public override string ToString() => Value;
	}
}
