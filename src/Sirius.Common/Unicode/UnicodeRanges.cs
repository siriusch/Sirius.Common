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
		public static RangeSet<Codepoint> Letter => FromUnicodeName("L");
		public static RangeSet<Codepoint> LowercaseLetter => FromUnicodeName("Ll");
		public static RangeSet<Codepoint> UppercaseLetter => FromUnicodeName("Lu");
		public static RangeSet<Codepoint> TitlecaseLetter => FromUnicodeName("Lt");
		public static RangeSet<Codepoint> CasedLetter => FromUnicodeName("L&");
		public static RangeSet<Codepoint> ModifierLetter => FromUnicodeName("Lm");
		public static RangeSet<Codepoint> OtherLetter => FromUnicodeName("Lo");
		public static RangeSet<Codepoint> Mark => FromUnicodeName("M");
		public static RangeSet<Codepoint> NonSpacingMark => FromUnicodeName("Mn");
		public static RangeSet<Codepoint> SpacingCombiningMark => FromUnicodeName("Mc");
		public static RangeSet<Codepoint> EnclosingMark => FromUnicodeName("Me");
		public static RangeSet<Codepoint> Separator => FromUnicodeName("Z");
		public static RangeSet<Codepoint> SpaceSeparator => FromUnicodeName("Zs");
		public static RangeSet<Codepoint> LineSeparator => FromUnicodeName("Zl");
		public static RangeSet<Codepoint> ParagraphSeparator => FromUnicodeName("Zp");
		public static RangeSet<Codepoint> Symbol => FromUnicodeName("S");
		public static RangeSet<Codepoint> MathSymbol => FromUnicodeName("Sm");
		public static RangeSet<Codepoint> CurrencySymbol => FromUnicodeName("Sc");
		public static RangeSet<Codepoint> ModifierSymbol => FromUnicodeName("Sk");
		public static RangeSet<Codepoint> OtherSymbol => FromUnicodeName("So");
		public static RangeSet<Codepoint> Number => FromUnicodeName("N");
		public static RangeSet<Codepoint> DecimalDigitNumber => FromUnicodeName("Nd");
		public static RangeSet<Codepoint> LetterNumber => FromUnicodeName("Nl");
		public static RangeSet<Codepoint> OtherNumber => FromUnicodeName("No");
		public static RangeSet<Codepoint> Punctuation => FromUnicodeName("P");
		public static RangeSet<Codepoint> DashPunctuation => FromUnicodeName("Pd");
		public static RangeSet<Codepoint> OpenPunctuation => FromUnicodeName("Ps");
		public static RangeSet<Codepoint> ClosePunctuation => FromUnicodeName("Pe");
		public static RangeSet<Codepoint> InitialPunctuation => FromUnicodeName("Pi");
		public static RangeSet<Codepoint> FinalPunctuation => FromUnicodeName("Pf");
		public static RangeSet<Codepoint> ConnectorPunctuation => FromUnicodeName("Pc");
		public static RangeSet<Codepoint> OtherPunctuation => FromUnicodeName("Po");
		public static RangeSet<Codepoint> Other => FromUnicodeName("C");
		public static RangeSet<Codepoint> Control => FromUnicodeName("Cc");
		public static RangeSet<Codepoint> Format => FromUnicodeName("Cf");
		public static RangeSet<Codepoint> PrivateUse => FromUnicodeName("Co");
		public static RangeSet<Codepoint> Surrogate => FromUnicodeName("Cs");
		public static RangeSet<Codepoint> Unassigned => FromUnicodeName("Cn");
		public static RangeSet<Codepoint> InBasicLatin => FromUnicodeName("InBasic_Latin");
		public static RangeSet<Codepoint> InLatin1Supplement => FromUnicodeName("InLatin-1_Supplement");
		public static RangeSet<Codepoint> InLatinExtendedA => FromUnicodeName("InLatin_Extended-A");
		public static RangeSet<Codepoint> InLatinExtendedB => FromUnicodeName("InLatin_Extended-B");
		public static RangeSet<Codepoint> InIPAExtensions => FromUnicodeName("InIPA_Extensions");
		public static RangeSet<Codepoint> InSpacingModifierLetters => FromUnicodeName("InSpacing_Modifier_Letters");
		public static RangeSet<Codepoint> InCombiningDiacriticalMarks => FromUnicodeName("InCombining_Diacritical_Marks");
		public static RangeSet<Codepoint> InGreekandCoptic => FromUnicodeName("InGreek_and_Coptic");
		public static RangeSet<Codepoint> InCyrillic => FromUnicodeName("InCyrillic");
		public static RangeSet<Codepoint> InCyrillicSupplementary => FromUnicodeName("InCyrillic_Supplementary");
		public static RangeSet<Codepoint> InArmenian => FromUnicodeName("InArmenian");
		public static RangeSet<Codepoint> InHebrew => FromUnicodeName("InHebrew");
		public static RangeSet<Codepoint> InArabic => FromUnicodeName("InArabic");
		public static RangeSet<Codepoint> InSyriac => FromUnicodeName("InSyriac");
		public static RangeSet<Codepoint> InThaana => FromUnicodeName("InThaana");
		public static RangeSet<Codepoint> InDevanagari => FromUnicodeName("InDevanagari");
		public static RangeSet<Codepoint> InBengali => FromUnicodeName("InBengali");
		public static RangeSet<Codepoint> InGurmukhi => FromUnicodeName("InGurmukhi");
		public static RangeSet<Codepoint> InGujarati => FromUnicodeName("InGujarati");
		public static RangeSet<Codepoint> InOriya => FromUnicodeName("InOriya");
		public static RangeSet<Codepoint> InTamil => FromUnicodeName("InTamil");
		public static RangeSet<Codepoint> InTelugu => FromUnicodeName("InTelugu");
		public static RangeSet<Codepoint> InKannada => FromUnicodeName("InKannada");
		public static RangeSet<Codepoint> InMalayalam => FromUnicodeName("InMalayalam");
		public static RangeSet<Codepoint> InSinhala => FromUnicodeName("InSinhala");
		public static RangeSet<Codepoint> InThai => FromUnicodeName("InThai");
		public static RangeSet<Codepoint> InLao => FromUnicodeName("InLao");
		public static RangeSet<Codepoint> InTibetan => FromUnicodeName("InTibetan");
		public static RangeSet<Codepoint> InMyanmar => FromUnicodeName("InMyanmar");
		public static RangeSet<Codepoint> InGeorgian => FromUnicodeName("InGeorgian");
		public static RangeSet<Codepoint> InHangulJamo => FromUnicodeName("InHangul_Jamo");
		public static RangeSet<Codepoint> InEthiopic => FromUnicodeName("InEthiopic");
		public static RangeSet<Codepoint> InCherokee => FromUnicodeName("InCherokee");
		public static RangeSet<Codepoint> InUnifiedCanadianAboriginalSyllabics => FromUnicodeName("InUnified_Canadian_Aboriginal_Syllabics");
		public static RangeSet<Codepoint> InOgham => FromUnicodeName("InOgham");
		public static RangeSet<Codepoint> InRunic => FromUnicodeName("InRunic");
		public static RangeSet<Codepoint> InTagalog => FromUnicodeName("InTagalog");
		public static RangeSet<Codepoint> InHanunoo => FromUnicodeName("InHanunoo");
		public static RangeSet<Codepoint> InBuhid => FromUnicodeName("InBuhid");
		public static RangeSet<Codepoint> InTagbanwa => FromUnicodeName("InTagbanwa");
		public static RangeSet<Codepoint> InKhmer => FromUnicodeName("InKhmer");
		public static RangeSet<Codepoint> InMongolian => FromUnicodeName("InMongolian");
		public static RangeSet<Codepoint> InLimbu => FromUnicodeName("InLimbu");
		public static RangeSet<Codepoint> InTaiLe => FromUnicodeName("InTai_Le");
		public static RangeSet<Codepoint> InKhmerSymbols => FromUnicodeName("InKhmer_Symbols");
		public static RangeSet<Codepoint> InPhoneticExtensions => FromUnicodeName("InPhonetic_Extensions");
		public static RangeSet<Codepoint> InLatinExtendedAdditional => FromUnicodeName("InLatin_Extended_Additional");
		public static RangeSet<Codepoint> InGreekExtended => FromUnicodeName("InGreek_Extended");
		public static RangeSet<Codepoint> InGeneralPunctuation => FromUnicodeName("InGeneral_Punctuation");
		public static RangeSet<Codepoint> InSuperscriptsandSubscripts => FromUnicodeName("InSuperscripts_and_Subscripts");
		public static RangeSet<Codepoint> InCurrencySymbols => FromUnicodeName("InCurrency_Symbols");
		public static RangeSet<Codepoint> InCombiningDiacriticalMarksforSymbols => FromUnicodeName("InCombining_Diacritical_Marks_for_Symbols");
		public static RangeSet<Codepoint> InLetterlikeSymbols => FromUnicodeName("InLetterlike_Symbols");
		public static RangeSet<Codepoint> InNumberForms => FromUnicodeName("InNumber_Forms");
		public static RangeSet<Codepoint> InArrows => FromUnicodeName("InArrows");
		public static RangeSet<Codepoint> InMathematicalOperators => FromUnicodeName("InMathematical_Operators");
		public static RangeSet<Codepoint> InMiscellaneousTechnical => FromUnicodeName("InMiscellaneous_Technical");
		public static RangeSet<Codepoint> InControlPictures => FromUnicodeName("InControl_Pictures");
		public static RangeSet<Codepoint> InOpticalCharacterRecognition => FromUnicodeName("InOptical_Character_Recognition");
		public static RangeSet<Codepoint> InEnclosedAlphanumerics => FromUnicodeName("InEnclosed_Alphanumerics");
		public static RangeSet<Codepoint> InBoxDrawing => FromUnicodeName("InBox_Drawing");
		public static RangeSet<Codepoint> InBlockElements => FromUnicodeName("InBlock_Elements");
		public static RangeSet<Codepoint> InGeometricShapes => FromUnicodeName("InGeometric_Shapes");
		public static RangeSet<Codepoint> InMiscellaneousSymbols => FromUnicodeName("InMiscellaneous_Symbols");
		public static RangeSet<Codepoint> InDingbats => FromUnicodeName("InDingbats");
		public static RangeSet<Codepoint> InMiscellaneousMathematicalSymbolsA => FromUnicodeName("InMiscellaneous_Mathematical_Symbols-A");
		public static RangeSet<Codepoint> InSupplementalArrowsA => FromUnicodeName("InSupplemental_Arrows-A");
		public static RangeSet<Codepoint> InBraillePatterns => FromUnicodeName("InBraille_Patterns");
		public static RangeSet<Codepoint> InSupplementalArrowsB => FromUnicodeName("InSupplemental_Arrows-B");
		public static RangeSet<Codepoint> InMiscellaneousMathematicalSymbolsB => FromUnicodeName("InMiscellaneous_Mathematical_Symbols-B");
		public static RangeSet<Codepoint> InSupplementalMathematicalOperators => FromUnicodeName("InSupplemental_Mathematical_Operators");
		public static RangeSet<Codepoint> InMiscellaneousSymbolsandArrows => FromUnicodeName("InMiscellaneous_Symbols_and_Arrows");
		public static RangeSet<Codepoint> InCJKRadicalsSupplement => FromUnicodeName("InCJK_Radicals_Supplement");
		public static RangeSet<Codepoint> InKangxiRadicals => FromUnicodeName("InKangxi_Radicals");
		public static RangeSet<Codepoint> InIdeographicDescriptionCharacters => FromUnicodeName("InIdeographic_Description_Characters");
		public static RangeSet<Codepoint> InCJKSymbolsandPunctuation => FromUnicodeName("InCJK_Symbols_and_Punctuation");
		public static RangeSet<Codepoint> InHiragana => FromUnicodeName("InHiragana");
		public static RangeSet<Codepoint> InKatakana => FromUnicodeName("InKatakana");
		public static RangeSet<Codepoint> InBopomofo => FromUnicodeName("InBopomofo");
		public static RangeSet<Codepoint> InHangulCompatibilityJamo => FromUnicodeName("InHangul_Compatibility_Jamo");
		public static RangeSet<Codepoint> InKanbun => FromUnicodeName("InKanbun");
		public static RangeSet<Codepoint> InBopomofoExtended => FromUnicodeName("InBopomofo_Extended");
		public static RangeSet<Codepoint> InKatakanaPhoneticExtensions => FromUnicodeName("InKatakana_Phonetic_Extensions");
		public static RangeSet<Codepoint> InEnclosedCJKLettersandMonths => FromUnicodeName("InEnclosed_CJK_Letters_and_Months");
		public static RangeSet<Codepoint> InCJKCompatibility => FromUnicodeName("InCJK_Compatibility");
		public static RangeSet<Codepoint> InCJKUnifiedIdeographsExtensionA => FromUnicodeName("InCJK_Unified_Ideographs_Extension_A");
		public static RangeSet<Codepoint> InYijingHexagramSymbols => FromUnicodeName("InYijing_Hexagram_Symbols");
		public static RangeSet<Codepoint> InCJKUnifiedIdeographs => FromUnicodeName("InCJK_Unified_Ideographs");
		public static RangeSet<Codepoint> InYiSyllables => FromUnicodeName("InYi_Syllables");
		public static RangeSet<Codepoint> InYiRadicals => FromUnicodeName("InYi_Radicals");
		public static RangeSet<Codepoint> InHangulSyllables => FromUnicodeName("InHangul_Syllables");
		public static RangeSet<Codepoint> InPrivateUseArea => FromUnicodeName("InPrivate_Use_Area");
		public static RangeSet<Codepoint> InCJKCompatibilityIdeographs => FromUnicodeName("InCJK_Compatibility_Ideographs");
		public static RangeSet<Codepoint> InAlphabeticPresentationForms => FromUnicodeName("InAlphabetic_Presentation_Forms");
		public static RangeSet<Codepoint> InArabicPresentationFormsA => FromUnicodeName("InArabic_Presentation_Forms-A");
		public static RangeSet<Codepoint> InVariationSelectors => FromUnicodeName("InVariation_Selectors");
		public static RangeSet<Codepoint> InCombiningHalfMarks => FromUnicodeName("InCombining_Half_Marks");
		public static RangeSet<Codepoint> InCJKCompatibilityForms => FromUnicodeName("InCJK_Compatibility_Forms");
		public static RangeSet<Codepoint> InSmallFormVariants => FromUnicodeName("InSmall_Form_Variants");
		public static RangeSet<Codepoint> InArabicPresentationFormsB => FromUnicodeName("InArabic_Presentation_Forms-B");
		public static RangeSet<Codepoint> InHalfwidthandFullwidthForms => FromUnicodeName("InHalfwidth_and_Fullwidth_Forms");
		public static RangeSet<Codepoint> InSpecials => FromUnicodeName("InSpecials");
		// ReSharper restore InconsistentNaming

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
		// ReSharper restore IdentifierTypo

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
