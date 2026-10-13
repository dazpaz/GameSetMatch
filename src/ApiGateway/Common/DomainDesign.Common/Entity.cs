namespace DomainDesign.Common
{
	public abstract class Entity<TId> : IEquatable<Entity<TId>>
	{
		/// <summary>
		/// Entity identifier. Protected setter so derived types and ORMs can set it.
		/// Nullable to accommodate reference ids or uninitialized state.
		/// </summary>
		public TId? Id { get; protected set; }

		/// <summary>
		/// Protected parameterless constructor for ORMs.
		/// </summary>
		protected Entity() { }

		/// <summary>
		/// Protected constructor that initializes the Id.
		/// </summary>
		protected Entity(TId id)
		{
			Id = id;
		}

		public override bool Equals(object? otherObject)
		{
			if (otherObject is null) return false;

			if (ReferenceEquals(this, otherObject)) return true;

			if (otherObject.GetType() != GetType()) return false;

			return Equals(otherObject as Entity<TId>);
		}

		public virtual bool Equals(Entity<TId>? otherObject)
		{
			if (otherObject is null) return false;

			if (ReferenceEquals(this, otherObject)) return true;

			if (GetType() != otherObject.GetType()) return false;

			// Use object.Equals to handle null and value-type comparisons safely
			return Equals(Id, otherObject.Id);
		}

		public override int GetHashCode()
		{
			if (Id == null) return base.GetHashCode();

			return EqualityComparer<TId>.Default.GetHashCode(Id);
		}

		public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
		{
			if (ReferenceEquals(left, right)) return true;

			if (left is null || right is null) return false;

			return left.Equals(right);
		}

		public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
		{
			return !(left == right);
		}
	}
}
