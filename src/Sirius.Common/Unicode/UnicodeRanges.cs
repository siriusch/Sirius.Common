using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using Sirius.Collections;

namespace Sirius.Unicode {
	public static class UnicodeRanges {
		// ReSharper disable IdentifierTypo
		// ReSharper disable InconsistentNaming
		public const string Letter = "L";
		public const string LowercaseLetter = "Ll";
		public const string UppercaseLetter = "Lu";
		public const string TitlecaseLetter = "Lt";
		public const string CasedLetter = "L&";
		public const string ModifierLetter = "Lm";
		public const string OtherLetter = "Lo";
		public const string Mark = "M";
		public const string NonSpacingMark = "Mn";
		public const string SpacingCombiningMark = "Mc";
		public const string EnclosingMark = "Me";
		public const string Separator = "Z";
		public const string SpaceSeparator = "Zs";
		public const string LineSeparator = "Zl";
		public const string ParagraphSeparator = "Zp";
		public const string Symbol = "S";
		public const string MathSymbol = "Sm";
		public const string CurrencySymbol = "Sc";
		public const string ModifierSymbol = "Sk";
		public const string OtherSymbol = "So";
		public const string Number = "N";
		public const string DecimalDigitNumber = "Nd";
		public const string LetterNumber = "Nl";
		public const string OtherNumber = "No";
		public const string Punctuation = "P";
		public const string DashPunctuation = "Pd";
		public const string OpenPunctuation = "Ps";
		public const string ClosePunctuation = "Pe";
		public const string InitialPunctuation = "Pi";
		public const string FinalPunctuation = "Pf";
		public const string ConnectorPunctuation = "Pc";
		public const string OtherPunctuation = "Po";
		public const string Other = "C";
		public const string Control = "Cc";
		public const string Format = "Cf";
		public const string PrivateUse = "Co";
		public const string Surrogate = "Cs";
		public const string Unassigned = "Cn";
		public const string InBasicLatin = "InBasic_Latin";
		public const string InLatin1Supplement = "InLatin-1_Supplement";
		public const string InLatinExtendedA = "InLatin_Extended-A";
		public const string InLatinExtendedB = "InLatin_Extended-B";
		public const string InIPAExtensions = "InIPA_Extensions";
		public const string InSpacingModifierLetters = "InSpacing_Modifier_Letters";
		public const string InCombiningDiacriticalMarks = "InCombining_Diacritical_Marks";
		public const string InGreekandCoptic = "InGreek_and_Coptic";
		public const string InCyrillic = "InCyrillic";
		public const string InCyrillicSupplementary = "InCyrillic_Supplementary";
		public const string InArmenian = "InArmenian";
		public const string InHebrew = "InHebrew";
		public const string InArabic = "InArabic";
		public const string InSyriac = "InSyriac";
		public const string InThaana = "InThaana";
		public const string InDevanagari = "InDevanagari";
		public const string InBengali = "InBengali";
		public const string InGurmukhi = "InGurmukhi";
		public const string InGujarati = "InGujarati";
		public const string InOriya = "InOriya";
		public const string InTamil = "InTamil";
		public const string InTelugu = "InTelugu";
		public const string InKannada = "InKannada";
		public const string InMalayalam = "InMalayalam";
		public const string InSinhala = "InSinhala";
		public const string InThai = "InThai";
		public const string InLao = "InLao";
		public const string InTibetan = "InTibetan";
		public const string InMyanmar = "InMyanmar";
		public const string InGeorgian = "InGeorgian";
		public const string InHangulJamo = "InHangul_Jamo";
		public const string InEthiopic = "InEthiopic";
		public const string InCherokee = "InCherokee";
		public const string InUnifiedCanadianAboriginalSyllabics = "InUnified_Canadian_Aboriginal_Syllabics";
		public const string InOgham = "InOgham";
		public const string InRunic = "InRunic";
		public const string InTagalog = "InTagalog";
		public const string InHanunoo = "InHanunoo";
		public const string InBuhid = "InBuhid";
		public const string InTagbanwa = "InTagbanwa";
		public const string InKhmer = "InKhmer";
		public const string InMongolian = "InMongolian";
		public const string InLimbu = "InLimbu";
		public const string InTaiLe = "InTai_Le";
		public const string InKhmerSymbols = "InKhmer_Symbols";
		public const string InPhoneticExtensions = "InPhonetic_Extensions";
		public const string InLatinExtendedAdditional = "InLatin_Extended_Additional";
		public const string InGreekExtended = "InGreek_Extended";
		public const string InGeneralPunctuation = "InGeneral_Punctuation";
		public const string InSuperscriptsandSubscripts = "InSuperscripts_and_Subscripts";
		public const string InCurrencySymbols = "InCurrency_Symbols";
		public const string InCombiningDiacriticalMarksforSymbols = "InCombining_Diacritical_Marks_for_Symbols";
		public const string InLetterlikeSymbols = "InLetterlike_Symbols";
		public const string InNumberForms = "InNumber_Forms";
		public const string InArrows = "InArrows";
		public const string InMathematicalOperators = "InMathematical_Operators";
		public const string InMiscellaneousTechnical = "InMiscellaneous_Technical";
		public const string InControlPictures = "InControl_Pictures";
		public const string InOpticalCharacterRecognition = "InOptical_Character_Recognition";
		public const string InEnclosedAlphanumerics = "InEnclosed_Alphanumerics";
		public const string InBoxDrawing = "InBox_Drawing";
		public const string InBlockElements = "InBlock_Elements";
		public const string InGeometricShapes = "InGeometric_Shapes";
		public const string InMiscellaneousSymbols = "InMiscellaneous_Symbols";
		public const string InDingbats = "InDingbats";
		public const string InMiscellaneousMathematicalSymbolsA = "InMiscellaneous_Mathematical_Symbols-A";
		public const string InSupplementalArrowsA = "InSupplemental_Arrows-A";
		public const string InBraillePatterns = "InBraille_Patterns";
		public const string InSupplementalArrowsB = "InSupplemental_Arrows-B";
		public const string InMiscellaneousMathematicalSymbolsB = "InMiscellaneous_Mathematical_Symbols-B";
		public const string InSupplementalMathematicalOperators = "InSupplemental_Mathematical_Operators";
		public const string InMiscellaneousSymbolsandArrows = "InMiscellaneous_Symbols_and_Arrows";
		public const string InCJKRadicalsSupplement = "InCJK_Radicals_Supplement";
		public const string InKangxiRadicals = "InKangxi_Radicals";
		public const string InIdeographicDescriptionCharacters = "InIdeographic_Description_Characters";
		public const string InCJKSymbolsandPunctuation = "InCJK_Symbols_and_Punctuation";
		public const string InHiragana = "InHiragana";
		public const string InKatakana = "InKatakana";
		public const string InBopomofo = "InBopomofo";
		public const string InHangulCompatibilityJamo = "InHangul_Compatibility_Jamo";
		public const string InKanbun = "InKanbun";
		public const string InBopomofoExtended = "InBopomofo_Extended";
		public const string InKatakanaPhoneticExtensions = "InKatakana_Phonetic_Extensions";
		public const string InEnclosedCJKLettersandMonths = "InEnclosed_CJK_Letters_and_Months";
		public const string InCJKCompatibility = "InCJK_Compatibility";
		public const string InCJKUnifiedIdeographsExtensionA = "InCJK_Unified_Ideographs_Extension_A";
		public const string InYijingHexagramSymbols = "InYijing_Hexagram_Symbols";
		public const string InCJKUnifiedIdeographs = "InCJK_Unified_Ideographs";
		public const string InYiSyllables = "InYi_Syllables";
		public const string InYiRadicals = "InYi_Radicals";
		public const string InHangulSyllables = "InHangul_Syllables";
		public const string InPrivateUseArea = "InPrivate_Use_Area";
		public const string InCJKCompatibilityIdeographs = "InCJK_Compatibility_Ideographs";
		public const string InAlphabeticPresentationForms = "InAlphabetic_Presentation_Forms";
		public const string InArabicPresentationFormsA = "InArabic_Presentation_Forms-A";
		public const string InVariationSelectors = "InVariation_Selectors";
		public const string InCombiningHalfMarks = "InCombining_Half_Marks";
		public const string InCJKCompatibilityForms = "InCJK_Compatibility_Forms";
		public const string InSmallFormVariants = "InSmall_Form_Variants";
		public const string InArabicPresentationFormsB = "InArabic_Presentation_Forms-B";
		public const string InHalfwidthandFullwidthForms = "InHalfwidth_and_Fullwidth_Forms";
		public const string InSpecials = "InSpecials";
		// ReSharper restore InconsistentNaming
		// ReSharper restore IdentifierTypo

		private static readonly Dictionary<string, ICollection<UnicodeCategory>> categoriesByName = new Dictionary<string, ICollection<UnicodeCategory>>(StringComparer.OrdinalIgnoreCase) {
				{"L", new[] {UnicodeCategory.LowercaseLetter, UnicodeCategory.UppercaseLetter, UnicodeCategory.TitlecaseLetter, UnicodeCategory.ModifierLetter, UnicodeCategory.OtherLetter}},
				{"Letter", new[] {UnicodeCategory.LowercaseLetter, UnicodeCategory.UppercaseLetter, UnicodeCategory.TitlecaseLetter, UnicodeCategory.ModifierLetter, UnicodeCategory.OtherLetter}},
				{"Ll", new[] {UnicodeCategory.LowercaseLetter}},
				{"Lowercase_Letter", new[] {UnicodeCategory.LowercaseLetter}},
				{"Lu", new[] {UnicodeCategory.UppercaseLetter}},
				{"Uppercase_Letter", new[] {UnicodeCategory.UppercaseLetter}},
				{"Lt", new[] {UnicodeCategory.TitlecaseLetter}},
				{"Titlecase_Letter", new[] {UnicodeCategory.TitlecaseLetter}},
				{"L&", new[] {UnicodeCategory.LowercaseLetter, UnicodeCategory.UppercaseLetter, UnicodeCategory.TitlecaseLetter}},
				{"Cased_Letter", new[] {UnicodeCategory.LowercaseLetter, UnicodeCategory.UppercaseLetter, UnicodeCategory.TitlecaseLetter}},
				{"Lm", new[] {UnicodeCategory.ModifierLetter}},
				{"Modifier_Letter", new[] {UnicodeCategory.ModifierLetter}},
				{"Lo", new[] {UnicodeCategory.OtherLetter}},
				{"Other_Letter", new[] {UnicodeCategory.OtherLetter}},
				{"M", new[] {UnicodeCategory.NonSpacingMark, UnicodeCategory.SpacingCombiningMark, UnicodeCategory.EnclosingMark}},
				{"Mark", new[] {UnicodeCategory.NonSpacingMark, UnicodeCategory.SpacingCombiningMark, UnicodeCategory.EnclosingMark}},
				{"Mn", new[] {UnicodeCategory.NonSpacingMark}},
				{"Non_Spacing_Mark", new[] {UnicodeCategory.NonSpacingMark}},
				{"Mc", new[] {UnicodeCategory.SpacingCombiningMark}},
				{"Spacing_Combining_Mark", new[] {UnicodeCategory.SpacingCombiningMark}},
				{"Me", new[] {UnicodeCategory.EnclosingMark}},
				{"Enclosing_Mark", new[] {UnicodeCategory.EnclosingMark}},
				{"Z", new[] {UnicodeCategory.SpaceSeparator, UnicodeCategory.LineSeparator, UnicodeCategory.ParagraphSeparator}},
				{"Separator", new[] {UnicodeCategory.SpaceSeparator, UnicodeCategory.LineSeparator, UnicodeCategory.ParagraphSeparator}},
				{"Zs", new[] {UnicodeCategory.SpaceSeparator}},
				{"Space_Separator", new[] {UnicodeCategory.SpaceSeparator}},
				{"Zl", new[] {UnicodeCategory.LineSeparator}},
				{"Line_Separator", new[] {UnicodeCategory.LineSeparator}},
				{"Zp", new[] {UnicodeCategory.ParagraphSeparator}},
				{"Paragraph_Separator", new[] {UnicodeCategory.ParagraphSeparator}},
				{"S", new[] {UnicodeCategory.MathSymbol, UnicodeCategory.CurrencySymbol, UnicodeCategory.ModifierSymbol, UnicodeCategory.OtherSymbol}},
				{"Symbol", new[] {UnicodeCategory.MathSymbol, UnicodeCategory.CurrencySymbol, UnicodeCategory.ModifierSymbol, UnicodeCategory.OtherSymbol}},
				{"Sm", new[] {UnicodeCategory.MathSymbol}},
				{"Math_Symbol", new[] {UnicodeCategory.MathSymbol}},
				{"Sc", new[] {UnicodeCategory.CurrencySymbol}},
				{"Currency_Symbol", new[] {UnicodeCategory.CurrencySymbol}},
				{"Sk", new[] {UnicodeCategory.ModifierSymbol}},
				{"Modifier_Symbol", new[] {UnicodeCategory.ModifierSymbol}},
				{"So", new[] {UnicodeCategory.OtherSymbol}},
				{"Other_Symbol", new[] {UnicodeCategory.OtherSymbol}},
				{"N", new[] {UnicodeCategory.DecimalDigitNumber, UnicodeCategory.LetterNumber, UnicodeCategory.OtherNumber}},
				{"Number", new[] {UnicodeCategory.DecimalDigitNumber, UnicodeCategory.LetterNumber, UnicodeCategory.OtherNumber}},
				{"Nd", new[] {UnicodeCategory.DecimalDigitNumber}},
				{"Decimal_Digit_Number", new[] {UnicodeCategory.DecimalDigitNumber}},
				{"Nl", new[] {UnicodeCategory.LetterNumber}},
				{"Letter_Number", new[] {UnicodeCategory.LetterNumber}},
				{"No", new[] {UnicodeCategory.OtherNumber}},
				{"Other_Number", new[] {UnicodeCategory.OtherNumber}},
				{"P", new[] {UnicodeCategory.DashPunctuation, UnicodeCategory.OpenPunctuation, UnicodeCategory.ClosePunctuation, UnicodeCategory.InitialQuotePunctuation, UnicodeCategory.FinalQuotePunctuation, UnicodeCategory.ConnectorPunctuation}},
				{"Punctuation", new[] {UnicodeCategory.DashPunctuation, UnicodeCategory.OpenPunctuation, UnicodeCategory.ClosePunctuation, UnicodeCategory.InitialQuotePunctuation, UnicodeCategory.FinalQuotePunctuation, UnicodeCategory.ConnectorPunctuation}},
				{"Pd", new[] {UnicodeCategory.DashPunctuation}},
				{"Dash_Punctuation", new[] {UnicodeCategory.DashPunctuation}},
				{"Ps", new[] {UnicodeCategory.OpenPunctuation}},
				{"Open_Punctuation", new[] {UnicodeCategory.OpenPunctuation}},
				{"Pe", new[] {UnicodeCategory.ClosePunctuation}},
				{"Close_Punctuation", new[] {UnicodeCategory.ClosePunctuation}},
				{"Pi", new[] {UnicodeCategory.InitialQuotePunctuation}},
				{"Initial_Punctuation", new[] {UnicodeCategory.InitialQuotePunctuation}},
				{"Pf", new[] {UnicodeCategory.FinalQuotePunctuation}},
				{"Final_Punctuation", new[] {UnicodeCategory.FinalQuotePunctuation}},
				{"Pc", new[] {UnicodeCategory.ConnectorPunctuation}},
				{"Connector_Punctuation", new[] {UnicodeCategory.ConnectorPunctuation}},
				{"Po", new[] {UnicodeCategory.OtherPunctuation}},
				{"Other_Punctuation", new[] {UnicodeCategory.OtherPunctuation}},
				{"C", new[] {UnicodeCategory.Control, UnicodeCategory.Format, UnicodeCategory.PrivateUse, UnicodeCategory.Surrogate, UnicodeCategory.OtherNotAssigned}},
				{"Other", new[] {UnicodeCategory.Control, UnicodeCategory.Format, UnicodeCategory.PrivateUse, UnicodeCategory.Surrogate, UnicodeCategory.OtherNotAssigned}},
				{"Cc", new[] {UnicodeCategory.Control}},
				{"Control", new[] {UnicodeCategory.Control}},
				{"Cf", new[] {UnicodeCategory.Format}},
				{"Format", new[] {UnicodeCategory.Format}},
				{"Co", new[] {UnicodeCategory.PrivateUse}},
				{"Private_Use", new[] {UnicodeCategory.PrivateUse}},
				{"Cs", new[] {UnicodeCategory.Surrogate}},
				{"Surrogate", new[] {UnicodeCategory.Surrogate}},
				{"Cn", new[] {UnicodeCategory.OtherNotAssigned}},
				{"Unassigned", new[] {UnicodeCategory.OtherNotAssigned}}
		};

		private static readonly Lazy<Dictionary<UnicodeCategory, RangeSet<Codepoint>>> charSetByCategory = new Lazy<Dictionary<UnicodeCategory, RangeSet<Codepoint>>>(
				() => Codepoints
						.ValidBmp
						.Expand()
						.GroupBy(c => char.GetUnicodeCategory((char)c))
						.ToDictionary(g => g.Key, g => new RangeSet<Codepoint>(g.Condense())),
				LazyThreadSafetyMode.PublicationOnly);

		private static readonly Dictionary<string, RangeSet<Codepoint>> charSetByName = new Dictionary<string, RangeSet<Codepoint>>(StringComparer.OrdinalIgnoreCase) {
				{"InBasic_Latin", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0000', '\u007F'))},
				{"InLatin-1_Supplement", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0080', '\u00FF'))},
				{"InLatin_Extended-A", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0100', '\u017F'))},
				{"InLatin_Extended-B", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0180', '\u024F'))},
				{"InIPA_Extensions", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0250', '\u02AF'))},
				{"InSpacing_Modifier_Letters", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u02B0', '\u02FF'))},
				{"InCombining_Diacritical_Marks", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0300', '\u036F'))},
				{"InGreek_and_Coptic", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0370', '\u03FF'))},
				{"InCyrillic", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0400', '\u04FF'))},
				{"InCyrillic_Supplementary", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0500', '\u052F'))},
				{"InArmenian", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0530', '\u058F'))},
				{"InHebrew", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0590', '\u05FF'))},
				{"InArabic", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0600', '\u06FF'))},
				{"InSyriac", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0700', '\u074F'))},
				{"InThaana", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0780', '\u07BF'))},
				{"InDevanagari", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0900', '\u097F'))},
				{"InBengali", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0980', '\u09FF'))},
				{"InGurmukhi", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0A00', '\u0A7F'))},
				{"InGujarati", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0A80', '\u0AFF'))},
				{"InOriya", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0B00', '\u0B7F'))},
				{"InTamil", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0B80', '\u0BFF'))},
				{"InTelugu", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0C00', '\u0C7F'))},
				{"InKannada", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0C80', '\u0CFF'))},
				{"InMalayalam", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0D00', '\u0D7F'))},
				{"InSinhala", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0D80', '\u0DFF'))},
				{"InThai", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0E00', '\u0E7F'))},
				{"InLao", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0E80', '\u0EFF'))},
				{"InTibetan", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u0F00', '\u0FFF'))},
				{"InMyanmar", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1000', '\u109F'))},
				{"InGeorgian", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u10A0', '\u10FF'))},
				{"InHangul_Jamo", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1100', '\u11FF'))},
				{"InEthiopic", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1200', '\u137F'))},
				{"InCherokee", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u13A0', '\u13FF'))},
				{"InUnified_Canadian_Aboriginal_Syllabics", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1400', '\u167F'))},
				{"InOgham", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1680', '\u169F'))},
				{"InRunic", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u16A0', '\u16FF'))},
				{"InTagalog", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1700', '\u171F'))},
				{"InHanunoo", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1720', '\u173F'))},
				{"InBuhid", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1740', '\u175F'))},
				{"InTagbanwa", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1760', '\u177F'))},
				{"InKhmer", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1780', '\u17FF'))},
				{"InMongolian", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1800', '\u18AF'))},
				{"InLimbu", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1900', '\u194F'))},
				{"InTai_Le", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1950', '\u197F'))},
				{"InKhmer_Symbols", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u19E0', '\u19FF'))},
				{"InPhonetic_Extensions", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1D00', '\u1D7F'))},
				{"InLatin_Extended_Additional", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1E00', '\u1EFF'))},
				{"InGreek_Extended", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u1F00', '\u1FFF'))},
				{"InGeneral_Punctuation", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2000', '\u206F'))},
				{"InSuperscripts_and_Subscripts", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2070', '\u209F'))},
				{"InCurrency_Symbols", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u20A0', '\u20CF'))},
				{"InCombining_Diacritical_Marks_for_Symbols", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u20D0', '\u20FF'))},
				{"InLetterlike_Symbols", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2100', '\u214F'))},
				{"InNumber_Forms", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2150', '\u218F'))},
				{"InArrows", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2190', '\u21FF'))},
				{"InMathematical_Operators", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2200', '\u22FF'))},
				{"InMiscellaneous_Technical", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2300', '\u23FF'))},
				{"InControl_Pictures", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2400', '\u243F'))},
				{"InOptical_Character_Recognition", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2440', '\u245F'))},
				{"InEnclosed_Alphanumerics", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2460', '\u24FF'))},
				{"InBox_Drawing", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2500', '\u257F'))},
				{"InBlock_Elements", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2580', '\u259F'))},
				{"InGeometric_Shapes", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u25A0', '\u25FF'))},
				{"InMiscellaneous_Symbols", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2600', '\u26FF'))},
				{"InDingbats", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2700', '\u27BF'))},
				{"InMiscellaneous_Mathematical_Symbols-A", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u27C0', '\u27EF'))},
				{"InSupplemental_Arrows-A", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u27F0', '\u27FF'))},
				{"InBraille_Patterns", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2800', '\u28FF'))},
				{"InSupplemental_Arrows-B", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2900', '\u297F'))},
				{"InMiscellaneous_Mathematical_Symbols-B", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2980', '\u29FF'))},
				{"InSupplemental_Mathematical_Operators", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2A00', '\u2AFF'))},
				{"InMiscellaneous_Symbols_and_Arrows", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2B00', '\u2BFF'))},
				{"InCJK_Radicals_Supplement", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2E80', '\u2EFF'))},
				{"InKangxi_Radicals", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2F00', '\u2FDF'))},
				{"InIdeographic_Description_Characters", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u2FF0', '\u2FFF'))},
				{"InCJK_Symbols_and_Punctuation", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3000', '\u303F'))},
				{"InHiragana", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3040', '\u309F'))},
				{"InKatakana", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u30A0', '\u30FF'))},
				{"InBopomofo", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3100', '\u312F'))},
				{"InHangul_Compatibility_Jamo", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3130', '\u318F'))},
				{"InKanbun", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3190', '\u319F'))},
				{"InBopomofo_Extended", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u31A0', '\u31BF'))},
				{"InKatakana_Phonetic_Extensions", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u31F0', '\u31FF'))},
				{"InEnclosed_CJK_Letters_and_Months", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3200', '\u32FF'))},
				{"InCJK_Compatibility", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3300', '\u33FF'))},
				{"InCJK_Unified_Ideographs_Extension_A", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u3400', '\u4DBF'))},
				{"InYijing_Hexagram_Symbols", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u4DC0', '\u4DFF'))},
				{"InCJK_Unified_Ideographs", new RangeSet<Codepoint>(Range<Codepoint>.Create('\u4E00', '\u9FFF'))},
				{"InYi_Syllables", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uA000', '\uA48F'))},
				{"InYi_Radicals", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uA490', '\uA4CF'))},
				{"InHangul_Syllables", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uAC00', '\uD7AF'))},
				{"InPrivate_Use_Area", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uE000', '\uF8FF'))},
				{"InCJK_Compatibility_Ideographs", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uF900', '\uFAFF'))},
				{"InAlphabetic_Presentation_Forms", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFB00', '\uFB4F'))},
				{"InArabic_Presentation_Forms-A", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFB50', '\uFDFF'))},
				{"InVariation_Selectors", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFE00', '\uFE0F'))},
				{"InCombining_Half_Marks", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFE20', '\uFE2F'))},
				{"InCJK_Compatibility_Forms", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFE30', '\uFE4F'))},
				{"InSmall_Form_Variants", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFE50', '\uFE6F'))},
				{"InArabic_Presentation_Forms-B", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFE70', '\uFEFF'))},
				{"InHalfwidth_and_Fullwidth_Forms", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFF00', '\uFFEF'))},
				{"InSpecials", new RangeSet<Codepoint>(Range<Codepoint>.Create('\uFFF0', '\uFFFD'))}
		};

		public static RangeSet<Codepoint> FromUnicodeCategory(UnicodeCategory category) {
			return charSetByCategory.Value[category];
		}

		public static RangeSet<Codepoint> FromUnicodeName(string name) {
			lock (charSetByName) {
				if (!charSetByName.TryGetValue(name, out var result)) {
					if (!categoriesByName.TryGetValue(name, out var categories)) {
						throw new ArgumentException(string.Format("Unknown unicode name '{0}'", name), "name");
					}
					result = RangeSet<Codepoint>.Union(categories.Select(FromUnicodeCategory));
					charSetByName.Add(name, result);
				}
				return result;
			}
		}
	}
}
