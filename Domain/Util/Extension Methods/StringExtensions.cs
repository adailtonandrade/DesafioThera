using System;

namespace Domain.Util.ExtensionMethods {
	public static class StringExtensions {

		public static string WithMaxLength(this string value, int maxLength) {
			return value?.Substring(0, Math.Min(value.Length, maxLength));
		}

		public static string Truncate(this string value, int maxLength, string mask = "...") {
			string result = WithMaxLength(value, maxLength);
			if (value.Length > maxLength)
				result += mask;

			return result;
		}
	}
}
