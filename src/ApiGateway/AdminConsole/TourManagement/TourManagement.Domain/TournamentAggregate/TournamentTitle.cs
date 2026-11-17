using Ardalis.GuardClauses;
using Common.DomainDesign;

namespace TourManagement.Domain.TournamentAggregate
{
	public sealed class TournamentTitle : ValueObject<TournamentTitle>
	{
		public static readonly int MaxLength = 60;

		public static implicit operator string(TournamentTitle title) => title.Value;
		public static explicit operator TournamentTitle(string title) => Create(title);

		public string Value { get; }

		private TournamentTitle(string value)
		{
			Value = value;
		}

		public static TournamentTitle Create(string title)
		{
			Guard.Against.NullOrWhiteSpace(title, nameof(title));
			title = title.Trim(); 
			Guard.Against.OutOfRange(title.Length, nameof(title), 1, MaxLength);
	
			return new TournamentTitle(title);
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Value;
		}

		public override string ToString() => Value;
	}
}
