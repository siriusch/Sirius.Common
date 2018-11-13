using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sirius {
	/// <summary>Some string helper methods which are too usage specific to be declared as extension methods.</summary>
	public static class StringHelper {
		private static readonly Dictionary<char, string> jsEscape = new Dictionary<char, string>(7) {
				{'"', "\\\""},
				{'\'', "\\'"},
				{'/', "\\/"},
				{'\b', "\\b"},
				{'\f', "\\f"},
				{'\n', "\\n"},
				{'\r', "\\r"},
				{'\t', "\\t"}
		};

		private static readonly Regex rxEscapeJs = new Regex(@"[\x00-\x1F\\""']", RegexOptions.Compiled|RegexOptions.CultureInvariant);
		private static readonly Regex rxEscapeJson = new Regex(@"[\x00-\x1F\\""]", RegexOptions.Compiled|RegexOptions.CultureInvariant);
		private static readonly Dictionary<char, string> jsUnescape = jsEscape.ToDictionary(p => p.Value[1], p => p.Key.ToString());
		private static readonly Regex rxUnescapeJs = new Regex(@"\\(u....|x..|[^ux])", RegexOptions.Compiled|RegexOptions.CultureInvariant|RegexOptions.ExplicitCapture|RegexOptions.Singleline);

		/// <summary>Escape JSON string.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A string.</returns>
		[Pure]
		public static string EscapeJsonString(string value) {
			if (value == null) {
				return null;
			}
			return rxEscapeJson.Replace(value, EscapeJsonChar);
		}

		[Pure]
		private static string EscapeJsonChar(Match match) {
			var value = match.Value[0];
			if (jsEscape.TryGetValue(value, out var result)) {
				return result;
			}
			return string.Format(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)value);
		}

		/// <summary>Escape JavaScript string.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A string.</returns>
		[Pure]
		public static string EscapeJavaScriptString(string value) {
			if (value == null) {
				return null;
			}
			return rxEscapeJs.Replace(value, EscapeJavaScriptChar);
		}

		[Pure]
		private static string EscapeJavaScriptChar(Match match) {
			var value = match.Value[0];
			if (jsEscape.TryGetValue(value, out var result)) {
				return result;
			}
			return string.Format(CultureInfo.InvariantCulture, (value <= '\xFF') ? "\\x{0:x2}" : "\\u{0:x4}", (int)value);
		}

		/// <summary>Unescape a JavaScript and JSON string.</summary>
		/// <param name="value">The value.</param>
		/// <returns>A string.</returns>
		[Pure]
		public static string UnescapeJavaScriptString(string value) {
			if (value == null) {
				return null;
			}
			return rxUnescapeJs.Replace(value, UnescapeJavaScriptChar);
		}

		[Pure]
		private static string UnescapeJavaScriptChar(Match match) {
			var value = match.Value[1];
			if ((value == 'u') || (value == 'x')) {
				return ((char)int.Parse(match.Value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToString();
			}
			if (jsUnescape.TryGetValue(value, out var result)) {
				return result;
			}
			return value.ToString();
		}
	}
}
