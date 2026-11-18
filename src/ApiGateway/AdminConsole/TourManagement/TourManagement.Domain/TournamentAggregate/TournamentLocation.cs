using Ardalis.GuardClauses;
using Common.DomainDesign;

namespace TourManagement.Domain.TournamentAggregate
{
	public sealed class TournamentLocation : ValueObject<TournamentLocation>
	{
		public static readonly int MaxLength = 60;
		private const string AllowedCharactersPattern = @"^[\p{L}\p{N} .,'&:()!/-]+$";

		public static implicit operator string(TournamentLocation title) => title.Value;
		public static explicit operator TournamentLocation(string location) => Create(location);

		public string Value { get; }

		private TournamentLocation(string value)
		{
			Value = value;
		}

		public static TournamentLocation Create(string location)
		{
			Guard.Against.NullOrWhiteSpace(location, nameof(location));
			location = location.Trim();
			Guard.Against.OutOfRange(location.Length, nameof(location), 1, MaxLength);
			Guard.Against.InvalidFormat(location, nameof(location), AllowedCharactersPattern);

			return new TournamentLocation(location);
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Value;
		}

		public override string ToString() => Value;
	}
}
