namespace TourManagement.Domain.TournamentAggregate
{
	public sealed class TournamentTitle : TextValueObjectBase<TournamentTitle>
	{
		public static readonly int MaxLength = 60;

		public static implicit operator string(TournamentTitle title) => title.Value;
		public static explicit operator TournamentTitle(string title) => Create(title);

		private TournamentTitle(string value) : base(value)
		{
		}

		public static TournamentTitle Create(string title)
		{
			return new TournamentTitle(Validate(title, nameof(title), MaxLength));
		}
	}
}
