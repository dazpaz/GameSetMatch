using Common.DomainDesign;

namespace TourManagement.Domain.TournamentAggregate
{
	public abstract class TextValueObjectBase<T>(string value) : ValueObject<T> where T : TextValueObjectBase<T>
	{
		public string Value { get; } = value;

		protected static string Validate(string value, string paramName, int maxLength)
		{
			var name = char.ToUpperInvariant(paramName[0]) + paramName[1..];
			TournamentRules.ThrowIfInvalid(TournamentRules.CheckText(value, name, maxLength), paramName);

			return value.Trim();
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Value;
		}

		public override string ToString() => Value;
	}
}
