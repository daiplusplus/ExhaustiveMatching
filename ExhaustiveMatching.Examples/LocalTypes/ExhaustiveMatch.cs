using System.ComponentModel;
using System.Diagnostics.Contracts;
using System;

namespace ExhaustiveMatching
{
	internal static class ExhaustiveMatch
	{
		public static ExhaustiveMatchFailedException Failed()
		{
			return new ExhaustiveMatchFailedException();
		}

		public static ExhaustiveMatchFailedException Failed<T>(T value)
		{
			return new ExhaustiveMatchFailedException(typeof(T), value);
		}

        [Pure]
        public static Exception Failed<T>(string paramName, T value)
        {
            if (paramName is null) throw new ArgumentNullException(nameof(paramName));

            if (value == null)
                return new ArgumentNullException(paramName);

            if (value is Enum enumValue)
            {
                var enumType = enumValue.GetType();
                if (!enumType.IsEnumDefined(enumValue))
                {
                    switch (enumValue.GetTypeCode())
                    {
                        case TypeCode.Byte:
                        case TypeCode.SByte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                            // Guaranteed to be convertible to Int32
                            break;
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                            // Check if too large for Int32 and manually format if needed
                            if (enumValue.CompareTo(Enum.ToObject(enumType, int.MaxValue)) > 0)
                                return CreateInvalidEnumArgumentException<T>(paramName, enumValue, enumType);
                            break;
                        case TypeCode.Int64:
                            // Check if too large or small for Int32 and manually format if needed
                            if (enumValue.CompareTo(Enum.ToObject(enumType, int.MinValue)) < 0
                                || enumValue.CompareTo(Enum.ToObject(enumType, int.MaxValue)) > 0)
                                return CreateInvalidEnumArgumentException<T>(paramName, enumValue, enumType);
                            break;
                        default:
                            throw new NotSupportedException(
                                $"Enum with type code {enumValue.GetTypeCode()} not supported.");
                    }

                    return new InvalidEnumArgumentException(paramName, Convert.ToInt32(enumValue), enumType);
                }
            }

            return new InvalidOperationException(paramName);
        }

        private static InvalidEnumArgumentException CreateInvalidEnumArgumentException<T>(string paramName, Enum enumValue, Type enumType)
            => new InvalidEnumArgumentException(
                $"The value of argument '{paramName}' ({enumValue:D}) is invalid for Enum type '{enumType.Name}'.");
	}
}
