//	-----------------------------------------------------------
//	How to use Emum BitWise Extension

//	SomeType value = SomeType.Grapes;

//	bool isGrapes = value.Is(SomeType.Grapes); //true
//	bool hasGrapes = value.Has(SomeType.Grapes); //true

//	value = value.Add(SomeType.Oranges);
//	value = value.Add(SomeType.Apples);
//	value = value.Remove(SomeType.Grapes);

//	bool hasOranges = value.Has(SomeType.Oranges); //true
//	bool isApples = value.Is(SomeType.Apples); //false
//	bool hasGrapes = value.Has(SomeType.Grapes); //false
//	-----------------------------------------------------------


namespace Application.Extensions;


public static class EnumerationExtensions
{
	public static bool Has<T>(this Enum type, T value) {
		try {
			return ((int)(object)type & (int)(object)value!) == (int)(object)value;
		}
		catch {
			return false;
		}
	}

	public static bool Is<T>(this Enum type, T value) {
		try {
			return (int)(object)type == (int)(object)value!;
		}
		catch {
			return false;
		}
	}

	public static T Add<T>(this Enum type, T value) {
		try {
			return (T)(object)((int)(object)type | (int)(object)value!);
		}
		catch (Exception ex) {
			throw new ArgumentException($"Could not append value from enumerated type '{typeof(T).Name}'.", ex);
		}
	}

	public static T Remove<T>(this Enum type, T value) {
		try {
			return (T)(object)((int)(object)type & ~(int)(object)value!);
		}
		catch (Exception ex) {
			throw new ArgumentException($"Could not remove value from enumerated type '{typeof(T).Name}'.", ex);
		}
	}
}
