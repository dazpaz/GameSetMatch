using System.Text.RegularExpressions;
using TourManagement.Common;

namespace TourManagement.Domain.TournamentAggregate
{
	public static class TournamentRules
	{
		private const string AllowedCharactersPattern = @"^[\p{L}\p{N} .,'&:()!/-]+$";

		public static string? CheckTitle(string? title) => CheckText(title, "Title", TournamentTitle.MaxLength);

		public static string? CheckLocation(string? location) => CheckText(location, "Location", TournamentLocation.MaxLength);

		public static string? CheckId(Guid id) => id == Guid.Empty ? "Id cannot be empty" : null;

		public static string? CheckLevel(TournamentLevel level) => Enum.IsDefined(level) ? null : "Invalid tournament level.";

		public static string? CheckSurface(Surface surface) => Enum.IsDefined(surface) ? null : "Invalid surface.";

		public static string? CheckEvents(bool mensSingles, bool womensSingles, bool mensDoubles,
			bool womensDoubles, bool mixedDoubles)
		{
			var anyEvents = mensSingles || womensSingles || mensDoubles || womensDoubles || mixedDoubles;
			return anyEvents ? null : "At least one event must be included.";
		}

		public static string? CheckDetails(string? title, string? location, TournamentLevel level,
			Surface surface, bool mensSingles, bool womensSingles, bool mensDoubles,
			bool womensDoubles, bool mixedDoubles)
		{
			return CheckTitle(title)
				?? CheckLocation(location)
				?? CheckLevel(level)
				?? CheckSurface(surface)
				?? CheckEvents(mensSingles, womensSingles, mensDoubles, womensDoubles, mixedDoubles);
		}

		internal static string? CheckText(string? value, string name, int maxLength)
		{
			if (string.IsNullOrWhiteSpace(value)) return name + " cannot be blank";

			var trimmed = value.Trim();
			if (trimmed.Length > maxLength) return $"{name} cannot be longer than {maxLength} characters";
			if (!Regex.IsMatch(trimmed, AllowedCharactersPattern)) return $"{name} contains invalid characters";

			return null;
		}

		internal static void ThrowIfInvalid(string? error, string paramName)
		{
			if (error is not null) throw new ArgumentException(error, paramName);
		}
	}
}
