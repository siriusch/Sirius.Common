using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sirius {
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

		public static string EscapeJsonString(string value) {
			if (value == null) {
				return null;
			}
			return rxEscapeJson.Replace(value, EscapeJsonChar);
		}

		private static string EscapeJsonChar(Match match) {
			var value = match.Value[0];
			string result;
			if (jsEscape.TryGetValue(value, out result)) {
				return result;
			}
			return string.Format(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)value);
		}

		public static string EscapeJavaScriptString(string value) {
			if (value == null) {
				return null;
			}
			return rxEscapeJs.Replace(value, EscapeJavaScriptChar);
		}

		private static string EscapeJavaScriptChar(Match match) {
			var value = match.Value[0];
			string result;
			if (jsEscape.TryGetValue(value, out result)) {
				return result;
			}
			return string.Format(CultureInfo.InvariantCulture, (value <= '\xFF') ? "\\x{0:x2}" : "\\u{0:x4}", (int)value);
		}

		public static string UnescapeJavaScriptString(string value) {
			if (value == null) {
				return null;
			}
			return rxUnescapeJs.Replace(value, UnescapeJavaScriptChar);
		}

		private static string UnescapeJavaScriptChar(Match match) {
			var value = match.Value[1];
			if ((value == 'u') || (value == 'x')) {
				return ((char)int.Parse(match.Value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToString();
			}
			string result;
			if (jsUnescape.TryGetValue(value, out result)) {
				return result;
			}
			return value.ToString();
		}
	}
}
