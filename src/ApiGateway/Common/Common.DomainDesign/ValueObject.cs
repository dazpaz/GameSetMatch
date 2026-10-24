namespace Common.DomainDesign
{
	public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
	{
		// Derived types return the sequence of components that define equality
		protected abstract IEnumerable<object?> GetEqualityComponents();

		public override bool Equals(object? obj)
		{
			return Equals(obj as T);
		}

		public virtual bool Equals(T? other)
		{
			if (other is null) return false;

			if (ReferenceEquals(this, other)) return true;

			if (GetType() != other.GetType()) return false;

			using var thisValues = GetEqualityComponents().GetEnumerator();
			using var otherValues = other.GetEqualityComponents().GetEnumerator();

			while (thisValues.MoveNext() && otherValues.MoveNext())
			{
				var a = thisValues.Current;
				var b = otherValues.Current;

				if (a is null ^ b is null) return false;
				if (a != null && !a.Equals(b)) return false;
			}

			return !thisValues.MoveNext() && !otherValues.MoveNext();
		}

		public override int GetHashCode()
		{
			var h = new HashCode();
			foreach (var obj in GetEqualityComponents())
			{
				h.Add(obj);
			}
			return h.ToHashCode();
		}

		public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
		{
			if (ReferenceEquals(left, right)) return true;

			if (left is null || right is null) return false;

			return left.Equals(right as T);
		}

		public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
		{
			return !(left == right);
		}
	}
}