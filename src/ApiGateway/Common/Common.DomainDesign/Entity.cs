namespace Common.DomainDesign
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
			if (Equals(id, default(TId)))
			{
				throw new ArgumentException("The ID cannot be the type's default value.", nameof(id));
			}
			Id = id;
		}

		public virtual bool Equals(Entity<TId>? otherObject)
		{
			if (otherObject == null) return false;
			if (Id is null) return false;
			return Id.Equals(otherObject.Id);
		}

		public override bool Equals(object? otherObject)
		{
			if (otherObject is not Entity<TId> other) return false;
			if (ReferenceEquals(this, other)) return true;
			if (GetType() != other.GetType()) return false;
			if (Equals(Id, default(TId)) || Equals(other.Id, default(TId))) return false;
			if (Id is null) return false;
			return Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			return (GetType().ToString() + Id).GetHashCode();
		}

		public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
		{
			if (left is null && right is null) return true;

			if (left is null || right is null) return false;

			return left.Equals(right);
		}

		public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
		{
			return !(left == right);
		}
	}
}
