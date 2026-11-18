using Ardalis.GuardClauses;
using Common.DomainDesign;

namespace TourManagement.Domain.TournamentAggregate
{
	public abstract class TextValueObjectBase<T>(string value) : ValueObject<T> where T : TextValueObjectBase<T>
	{
		private const string AllowedCharactersPattern = @"^[\p{L}\p{N} .,'&:()!/-]+$";

		public string Value { get; } = value;

		protected static string Validate(string value, string paramName, int maxLength)
		{
			Guard.Against.NullOrWhiteSpace(value, paramName);
			value = value.Trim();
			Guard.Against.OutOfRange(value.Length, paramName, 1, maxLength);
			Guard.Against.InvalidFormat(value, paramName, AllowedCharactersPattern);

			return value;
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Value;
		}

		public override string ToString() => Value;
	}
}
