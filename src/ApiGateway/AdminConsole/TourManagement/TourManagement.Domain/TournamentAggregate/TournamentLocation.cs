namespace TourManagement.Domain.TournamentAggregate
{
	public sealed class TournamentLocation : TextValueObjectBase<TournamentLocation>
	{
		public static readonly int MaxLength = 60;

		public static implicit operator string(TournamentLocation location) => location.Value;
		public static explicit operator TournamentLocation(string location) => Create(location);

		private TournamentLocation(string value) : base(value)
		{
		}

		public static TournamentLocation Create(string location)
		{
			return new TournamentLocation(Validate(location, nameof(location), MaxLength));
		}
	}
}
