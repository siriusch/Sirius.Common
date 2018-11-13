using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using Sirius.Collections;

namespace Sirius.Unicode {
	/// <summary>Unicode ranges.</summary>
	/// <remarks>The current implementation contains only BMP codepoints.</remarks>
	public static class UnicodeRanges {
		// ReSharper disable IdentifierTypo
		// ReSharper disable InconsistentNaming

		///<summary>Unicode Letter range.</summary>
		public static RangeSet<Codepoint> Letter => FromUnicodeName("L");

		///<summary>Unicode LowercaseLetter range.</summary>
		public static RangeSet<Codepoint> LowercaseLetter => FromUnicodeName("Ll");

		///<summary>Unicode UppercaseLetter range.</summary>
		public static RangeSet<Codepoint> UppercaseLetter => FromUnicodeName("Lu");

		///<summary>Unicode TitlecaseLetter range.</summary>
		public static RangeSet<Codepoint> TitlecaseLetter => FromUnicodeName("Lt");

		///<summary>Unicode CasedLetter range.</summary>
		public static RangeSet<Codepoint> CasedLetter => FromUnicodeName("L&");

		///<summary>Unicode ModifierLetter range.</summary>
		public static RangeSet<Codepoint> ModifierLetter => FromUnicodeName("Lm");

		///<summary>Unicode OtherLetter range.</summary>
		public static RangeSet<Codepoint> OtherLetter => FromUnicodeName("Lo");

		///<summary>Unicode Mark range.</summary>
		public static RangeSet<Codepoint> Mark => FromUnicodeName("M");

		///<summary>Unicode NonSpacingMark range.</summary>
		public static RangeSet<Codepoint> NonSpacingMark => FromUnicodeName("Mn");

		///<summary>Unicode SpacingCombiningMark range.</summary>
		public static RangeSet<Codepoint> SpacingCombiningMark => FromUnicodeName("Mc");

		///<summary>Unicode EnclosingMark range.</summary>
		public static RangeSet<Codepoint> EnclosingMark => FromUnicodeName("Me");

		///<summary>Unicode Separator range.</summary>
		public static RangeSet<Codepoint> Separator => FromUnicodeName("Z");

		///<summary>Unicode SpaceSeparator range.</summary>
		public static RangeSet<Codepoint> SpaceSeparator => FromUnicodeName("Zs");

		///<summary>Unicode LineSeparator range.</summary>
		public static RangeSet<Codepoint> LineSeparator => FromUnicodeName("Zl");

		///<summary>Unicode ParagraphSeparator range.</summary>
		public static RangeSet<Codepoint> ParagraphSeparator => FromUnicodeName("Zp");

		///<summary>Unicode Symbol range.</summary>
		public static RangeSet<Codepoint> Symbol => FromUnicodeName("S");

		///<summary>Unicode MathSymbol range.</summary>
		public static RangeSet<Codepoint> MathSymbol => FromUnicodeName("Sm");

		///<summary>Unicode CurrencySymbol range.</summary>
		public static RangeSet<Codepoint> CurrencySymbol => FromUnicodeName("Sc");

		///<summary>Unicode ModifierSymbol range.</summary>
		public static RangeSet<Codepoint> ModifierSymbol => FromUnicodeName("Sk");

		///<summary>Unicode OtherSymbol range.</summary>
		public static RangeSet<Codepoint> OtherSymbol => FromUnicodeName("So");

		///<summary>Unicode Number range.</summary>
		public static RangeSet<Codepoint> Number => FromUnicodeName("N");

		///<summary>Unicode DecimalDigitNumber range.</summary>
		public static RangeSet<Codepoint> DecimalDigitNumber => FromUnicodeName("Nd");

		///<summary>Unicode LetterNumber range.</summary>
		public static RangeSet<Codepoint> LetterNumber => FromUnicodeName("Nl");

		///<summary>Unicode OtherNumber range.</summary>
		public static RangeSet<Codepoint> OtherNumber => FromUnicodeName("No");

		///<summary>Unicode Punctuation range.</summary>
		public static RangeSet<Codepoint> Punctuation => FromUnicodeName("P");

		///<summary>Unicode DashPunctuation range.</summary>
		public static RangeSet<Codepoint> DashPunctuation => FromUnicodeName("Pd");

		///<summary>Unicode OpenPunctuation range.</summary>
		public static RangeSet<Codepoint> OpenPunctuation => FromUnicodeName("Ps");

		///<summary>Unicode ClosePunctuation range.</summary>
		public static RangeSet<Codepoint> ClosePunctuation => FromUnicodeName("Pe");

		///<summary>Unicode InitialPunctuation range.</summary>
		public static RangeSet<Codepoint> InitialPunctuation => FromUnicodeName("Pi");

		///<summary>Unicode FinalPunctuation range.</summary>
		public static RangeSet<Codepoint> FinalPunctuation => FromUnicodeName("Pf");

		///<summary>Unicode ConnectorPunctuation range.</summary>
		public static RangeSet<Codepoint> ConnectorPunctuation => FromUnicodeName("Pc");

		///<summary>Unicode OtherPunctuation range.</summary>
		public static RangeSet<Codepoint> OtherPunctuation => FromUnicodeName("Po");

		///<summary>Unicode Other range.</summary>
		public static RangeSet<Codepoint> Other => FromUnicodeName("C");

		///<summary>Unicode Control range.</summary>
		public static RangeSet<Codepoint> Control => FromUnicodeName("Cc");

		///<summary>Unicode Format range.</summary>
		public static RangeSet<Codepoint> Format => FromUnicodeName("Cf");

		///<summary>Unicode PrivateUse range.</summary>
		public static RangeSet<Codepoint> PrivateUse => FromUnicodeName("Co");

		///<summary>Unicode Surrogate range.</summary>
		public static RangeSet<Codepoint> Surrogate => FromUnicodeName("Cs");

		///<summary>Unicode Unassigned range.</summary>
		public static RangeSet<Codepoint> Unassigned => FromUnicodeName("Cn");

		///<summary>Unicode InBasicLatin range.</summary>
		public static RangeSet<Codepoint> InBasicLatin => FromUnicodeName("InBasic_Latin");

		///<summary>Unicode InLatin1Supplement range.</summary>
		public static RangeSet<Codepoint> InLatin1Supplement => FromUnicodeName("InLatin-1_Supplement");

		///<summary>Unicode InLatinExtendedA range.</summary>
		public static RangeSet<Codepoint> InLatinExtendedA => FromUnicodeName("InLatin_Extended-A");

		///<summary>Unicode InLatinExtendedB range.</summary>
		public static RangeSet<Codepoint> InLatinExtendedB => FromUnicodeName("InLatin_Extended-B");

		///<summary>Unicode InIPAExtensions range.</summary>
		public static RangeSet<Codepoint> InIPAExtensions => FromUnicodeName("InIPA_Extensions");

		///<summary>Unicode InSpacingModifierLetters range.</summary>
		public static RangeSet<Codepoint> InSpacingModifierLetters => FromUnicodeName("InSpacing_Modifier_Letters");

		///<summary>Unicode InCombiningDiacriticalMarks range.</summary>
		public static RangeSet<Codepoint> InCombiningDiacriticalMarks => FromUnicodeName("InCombining_Diacritical_Marks");

		///<summary>Unicode InGreekandCoptic range.</summary>
		public static RangeSet<Codepoint> InGreekandCoptic => FromUnicodeName("InGreek_and_Coptic");

		///<summary>Unicode InCyrillic range.</summary>
		public static RangeSet<Codepoint> InCyrillic => FromUnicodeName("InCyrillic");

		///<summary>Unicode InCyrillicSupplementary range.</summary>
		public static RangeSet<Codepoint> InCyrillicSupplementary => FromUnicodeName("InCyrillic_Supplementary");

		///<summary>Unicode InArmenian range.</summary>
		public static RangeSet<Codepoint> InArmenian => FromUnicodeName("InArmenian");

		///<summary>Unicode InHebrew range.</summary>
		public static RangeSet<Codepoint> InHebrew => FromUnicodeName("InHebrew");

		///<summary>Unicode InArabic range.</summary>
		public static RangeSet<Codepoint> InArabic => FromUnicodeName("InArabic");

		///<summary>Unicode InSyriac range.</summary>
		public static RangeSet<Codepoint> InSyriac => FromUnicodeName("InSyriac");

		///<summary>Unicode InThaana range.</summary>
		public static RangeSet<Codepoint> InThaana => FromUnicodeName("InThaana");

		///<summary>Unicode InDevanagari range.</summary>
		public static RangeSet<Codepoint> InDevanagari => FromUnicodeName("InDevanagari");

		///<summary>Unicode InBengali range.</summary>
		public static RangeSet<Codepoint> InBengali => FromUnicodeName("InBengali");

		///<summary>Unicode InGurmukhi range.</summary>
		public static RangeSet<Codepoint> InGurmukhi => FromUnicodeName("InGurmukhi");

		///<summary>Unicode InGujarati range.</summary>
		public static RangeSet<Codepoint> InGujarati => FromUnicodeName("InGujarati");

		///<summary>Unicode InOriya range.</summary>
		public static RangeSet<Codepoint> InOriya => FromUnicodeName("InOriya");

		///<summary>Unicode InTamil range.</summary>
		public static RangeSet<Codepoint> InTamil => FromUnicodeName("InTamil");

		///<summary>Unicode InTelugu range.</summary>
		public static RangeSet<Codepoint> InTelugu => FromUnicodeName("InTelugu");

		///<summary>Unicode InKannada range.</summary>
		public static RangeSet<Codepoint> InKannada => FromUnicodeName("InKannada");

		///<summary>Unicode InMalayalam range.</summary>
		public static RangeSet<Codepoint> InMalayalam => FromUnicodeName("InMalayalam");

		///<summary>Unicode InSinhala range.</summary>
		public static RangeSet<Codepoint> InSinhala => FromUnicodeName("InSinhala");

		///<summary>Unicode InThai range.</summary>
		public static RangeSet<Codepoint> InThai => FromUnicodeName("InThai");

		///<summary>Unicode InLao range.</summary>
		public static RangeSet<Codepoint> InLao => FromUnicodeName("InLao");

		///<summary>Unicode InTibetan range.</summary>
		public static RangeSet<Codepoint> InTibetan => FromUnicodeName("InTibetan");

		///<summary>Unicode InMyanmar range.</summary>
		public static RangeSet<Codepoint> InMyanmar => FromUnicodeName("InMyanmar");

		///<summary>Unicode InGeorgian range.</summary>
		public static RangeSet<Codepoint> InGeorgian => FromUnicodeName("InGeorgian");

		///<summary>Unicode InHangulJamo range.</summary>
		public static RangeSet<Codepoint> InHangulJamo => FromUnicodeName("InHangul_Jamo");

		///<summary>Unicode InEthiopic range.</summary>
		public static RangeSet<Codepoint> InEthiopic => FromUnicodeName("InEthiopic");

		///<summary>Unicode InCherokee range.</summary>
		public static RangeSet<Codepoint> InCherokee => FromUnicodeName("InCherokee");

		///<summary>Unicode InUnifiedCanadianAboriginalSyllabics range.</summary>
		public static RangeSet<Codepoint> InUnifiedCanadianAboriginalSyllabics => FromUnicodeName("InUnified_Canadian_Aboriginal_Syllabics");

		///<summary>Unicode InOgham range.</summary>
		public static RangeSet<Codepoint> InOgham => FromUnicodeName("InOgham");

		///<summary>Unicode InRunic range.</summary>
		public static RangeSet<Codepoint> InRunic => FromUnicodeName("InRunic");

		///<summary>Unicode InTagalog range.</summary>
		public static RangeSet<Codepoint> InTagalog => FromUnicodeName("InTagalog");

		///<summary>Unicode InHanunoo range.</summary>
		public static RangeSet<Codepoint> InHanunoo => FromUnicodeName("InHanunoo");

		///<summary>Unicode InBuhid range.</summary>
		public static RangeSet<Codepoint> InBuhid => FromUnicodeName("InBuhid");

		///<summary>Unicode InTagbanwa range.</summary>
		public static RangeSet<Codepoint> InTagbanwa => FromUnicodeName("InTagbanwa");

		///<summary>Unicode InKhmer range.</summary>
		public static RangeSet<Codepoint> InKhmer => FromUnicodeName("InKhmer");

		///<summary>Unicode InMongolian range.</summary>
		public static RangeSet<Codepoint> InMongolian => FromUnicodeName("InMongolian");

		///<summary>Unicode InLimbu range.</summary>
		public static RangeSet<Codepoint> InLimbu => FromUnicodeName("InLimbu");

		///<summary>Unicode InTaiLe range.</summary>
		public static RangeSet<Codepoint> InTaiLe => FromUnicodeName("InTai_Le");

		///<summary>Unicode InKhmerSymbols range.</summary>
		public static RangeSet<Codepoint> InKhmerSymbols => FromUnicodeName("InKhmer_Symbols");

		///<summary>Unicode InPhoneticExtensions range.</summary>
		public static RangeSet<Codepoint> InPhoneticExtensions => FromUnicodeName("InPhonetic_Extensions");

		///<summary>Unicode InLatinExtendedAdditional range.</summary>
		public static RangeSet<Codepoint> InLatinExtendedAdditional => FromUnicodeName("InLatin_Extended_Additional");

		///<summary>Unicode InGreekExtended range.</summary>
		public static RangeSet<Codepoint> InGreekExtended => FromUnicodeName("InGreek_Extended");

		///<summary>Unicode InGeneralPunctuation range.</summary>
		public static RangeSet<Codepoint> InGeneralPunctuation => FromUnicodeName("InGeneral_Punctuation");

		///<summary>Unicode InSuperscriptsandSubscripts range.</summary>
		public static RangeSet<Codepoint> InSuperscriptsandSubscripts => FromUnicodeName("InSuperscripts_and_Subscripts");

		///<summary>Unicode InCurrencySymbols range.</summary>
		public static RangeSet<Codepoint> InCurrencySymbols => FromUnicodeName("InCurrency_Symbols");

		///<summary>Unicode InCombiningDiacriticalMarksforSymbols range.</summary>
		public static RangeSet<Codepoint> InCombiningDiacriticalMarksforSymbols => FromUnicodeName("InCombining_Diacritical_Marks_for_Symbols");

		///<summary>Unicode InLetterlikeSymbols range.</summary>
		public static RangeSet<Codepoint> InLetterlikeSymbols => FromUnicodeName("InLetterlike_Symbols");

		///<summary>Unicode InNumberForms range.</summary>
		public static RangeSet<Codepoint> InNumberForms => FromUnicodeName("InNumber_Forms");

		///<summary>Unicode InArrows range.</summary>
		public static RangeSet<Codepoint> InArrows => FromUnicodeName("InArrows");

		///<summary>Unicode InMathematicalOperators range.</summary>
		public static RangeSet<Codepoint> InMathematicalOperators => FromUnicodeName("InMathematical_Operators");

		///<summary>Unicode InMiscellaneousTechnical range.</summary>
		public static RangeSet<Codepoint> InMiscellaneousTechnical => FromUnicodeName("InMiscellaneous_Technical");

		///<summary>Unicode InControlPictures range.</summary>
		public static RangeSet<Codepoint> InControlPictures => FromUnicodeName("InControl_Pictures");

		///<summary>Unicode InOpticalCharacterRecognition range.</summary>
		public static RangeSet<Codepoint> InOpticalCharacterRecognition => FromUnicodeName("InOptical_Character_Recognition");

		///<summary>Unicode InEnclosedAlphanumerics range.</summary>
		public static RangeSet<Codepoint> InEnclosedAlphanumerics => FromUnicodeName("InEnclosed_Alphanumerics");

		///<summary>Unicode InBoxDrawing range.</summary>
		public static RangeSet<Codepoint> InBoxDrawing => FromUnicodeName("InBox_Drawing");

		///<summary>Unicode InBlockElements range.</summary>
		public static RangeSet<Codepoint> InBlockElements => FromUnicodeName("InBlock_Elements");

		///<summary>Unicode InGeometricShapes range.</summary>
		public static RangeSet<Codepoint> InGeometricShapes => FromUnicodeName("InGeometric_Shapes");

		///<summary>Unicode InMiscellaneousSymbols range.</summary>
		public static RangeSet<Codepoint> InMiscellaneousSymbols => FromUnicodeName("InMiscellaneous_Symbols");

		///<summary>Unicode InDingbats range.</summary>
		public static RangeSet<Codepoint> InDingbats => FromUnicodeName("InDingbats");

		///<summary>Unicode InMiscellaneousMathematicalSymbolsA range.</summary>
		public static RangeSet<Codepoint> InMiscellaneousMathematicalSymbolsA => FromUnicodeName("InMiscellaneous_Mathematical_Symbols-A");

		///<summary>Unicode InSupplementalArrowsA range.</summary>
		public static RangeSet<Codepoint> InSupplementalArrowsA => FromUnicodeName("InSupplemental_Arrows-A");

		///<summary>Unicode InBraillePatterns range.</summary>
		public static RangeSet<Codepoint> InBraillePatterns => FromUnicodeName("InBraille_Patterns");

		///<summary>Unicode InSupplementalArrowsB range.</summary>
		public static RangeSet<Codepoint> InSupplementalArrowsB => FromUnicodeName("InSupplemental_Arrows-B");

		///<summary>Unicode InMiscellaneousMathematicalSymbolsB range.</summary>
		public static RangeSet<Codepoint> InMiscellaneousMathematicalSymbolsB => FromUnicodeName("InMiscellaneous_Mathematical_Symbols-B");

		///<summary>Unicode InSupplementalMathematicalOperators range.</summary>
		public static RangeSet<Codepoint> InSupplementalMathematicalOperators => FromUnicodeName("InSupplemental_Mathematical_Operators");

		///<summary>Unicode InMiscellaneousSymbolsandArrows range.</summary>
		public static RangeSet<Codepoint> InMiscellaneousSymbolsandArrows => FromUnicodeName("InMiscellaneous_Symbols_and_Arrows");

		///<summary>Unicode InCJKRadicalsSupplement range.</summary>
		public static RangeSet<Codepoint> InCJKRadicalsSupplement => FromUnicodeName("InCJK_Radicals_Supplement");

		///<summary>Unicode InKangxiRadicals range.</summary>
		public static RangeSet<Codepoint> InKangxiRadicals => FromUnicodeName("InKangxi_Radicals");

		///<summary>Unicode InIdeographicDescriptionCharacters range.</summary>
		public static RangeSet<Codepoint> InIdeographicDescriptionCharacters => FromUnicodeName("InIdeographic_Description_Characters");

		///<summary>Unicode InCJKSymbolsandPunctuation range.</summary>
		public static RangeSet<Codepoint> InCJKSymbolsandPunctuation => FromUnicodeName("InCJK_Symbols_and_Punctuation");

		///<summary>Unicode InHiragana range.</summary>
		public static RangeSet<Codepoint> InHiragana => FromUnicodeName("InHiragana");

		///<summary>Unicode InKatakana range.</summary>
		public static RangeSet<Codepoint> InKatakana => FromUnicodeName("InKatakana");

		///<summary>Unicode InBopomofo range.</summary>
		public static RangeSet<Codepoint> InBopomofo => FromUnicodeName("InBopomofo");

		///<summary>Unicode InHangulCompatibilityJamo range.</summary>
		public static RangeSet<Codepoint> InHangulCompatibilityJamo => FromUnicodeName("InHangul_Compatibility_Jamo");

		///<summary>Unicode InKanbun range.</summary>
		public static RangeSet<Codepoint> InKanbun => FromUnicodeName("InKanbun");

		///<summary>Unicode InBopomofoExtended range.</summary>
		public static RangeSet<Codepoint> InBopomofoExtended => FromUnicodeName("InBopomofo_Extended");

		///<summary>Unicode InKatakanaPhoneticExtensions range.</summary>
		public static RangeSet<Codepoint> InKatakanaPhoneticExtensions => FromUnicodeName("InKatakana_Phonetic_Extensions");

		///<summary>Unicode InEnclosedCJKLettersandMonths range.</summary>
		public static RangeSet<Codepoint> InEnclosedCJKLettersandMonths => FromUnicodeName("InEnclosed_CJK_Letters_and_Months");

		///<summary>Unicode InCJKCompatibility range.</summary>
		public static RangeSet<Codepoint> InCJKCompatibility => FromUnicodeName("InCJK_Compatibility");

		///<summary>Unicode InCJKUnifiedIdeographsExtensionA range.</summary>
		public static RangeSet<Codepoint> InCJKUnifiedIdeographsExtensionA => FromUnicodeName("InCJK_Unified_Ideographs_Extension_A");

		///<summary>Unicode InYijingHexagramSymbols range.</summary>
		public static RangeSet<Codepoint> InYijingHexagramSymbols => FromUnicodeName("InYijing_Hexagram_Symbols");

		///<summary>Unicode InCJKUnifiedIdeographs range.</summary>
		public static RangeSet<Codepoint> InCJKUnifiedIdeographs => FromUnicodeName("InCJK_Unified_Ideographs");

		///<summary>Unicode InYiSyllables range.</summary>
		public static RangeSet<Codepoint> InYiSyllables => FromUnicodeName("InYi_Syllables");

		///<summary>Unicode InYiRadicals range.</summary>
		public static RangeSet<Codepoint> InYiRadicals => FromUnicodeName("InYi_Radicals");

		///<summary>Unicode InHangulSyllables range.</summary>
		public static RangeSet<Codepoint> InHangulSyllables => FromUnicodeName("InHangul_Syllables");

		///<summary>Unicode InPrivateUseArea range.</summary>
		public static RangeSet<Codepoint> InPrivateUseArea => FromUnicodeName("InPrivate_Use_Area");

		///<summary>Unicode InCJKCompatibilityIdeographs range.</summary>
		public static RangeSet<Codepoint> InCJKCompatibilityIdeographs => FromUnicodeName("InCJK_Compatibility_Ideographs");

		///<summary>Unicode InAlphabeticPresentationForms range.</summary>
		public static RangeSet<Codepoint> InAlphabeticPresentationForms => FromUnicodeName("InAlphabetic_Presentation_Forms");

		///<summary>Unicode InArabicPresentationFormsA range.</summary>
		public static RangeSet<Codepoint> InArabicPresentationFormsA => FromUnicodeName("InArabic_Presentation_Forms-A");

		///<summary>Unicode InVariationSelectors range.</summary>
		public static RangeSet<Codepoint> InVariationSelectors => FromUnicodeName("InVariation_Selectors");

		///<summary>Unicode InCombiningHalfMarks range.</summary>
		public static RangeSet<Codepoint> InCombiningHalfMarks => FromUnicodeName("InCombining_Half_Marks");

		///<summary>Unicode InCJKCompatibilityForms range.</summary>
		public static RangeSet<Codepoint> InCJKCompatibilityForms => FromUnicodeName("InCJK_Compatibility_Forms");

		///<summary>Unicode InSmallFormVariants range.</summary>
		public static RangeSet<Codepoint> InSmallFormVariants => FromUnicodeName("InSmall_Form_Variants");

		///<summary>Unicode InArabicPresentationFormsB range.</summary>
		public static RangeSet<Codepoint> InArabicPresentationFormsB => FromUnicodeName("InArabic_Presentation_Forms-B");

		///<summary>Unicode InHalfwidthandFullwidthForms range.</summary>
		public static RangeSet<Codepoint> InHalfwidthandFullwidthForms => FromUnicodeName("InHalfwidth_and_Fullwidth_Forms");

		///<summary>Unicode InSpecials range.</summary>
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
				{"P", new[] {UnicodeCategory.DashPunctuation, UnicodeCategory.OpenPunctuation, UnicodeCategory.ClosePunctuation, UnicodeCategory.InitialQuotePunctuation, UnicodeCategory.FinalQuotePunctuation, UnicodeCategory.ConnectorPunctuation, UnicodeCategory.OtherPunctuation}},
				{"Punctuation", new[] {UnicodeCategory.DashPunctuation, UnicodeCategory.OpenPunctuation, UnicodeCategory.ClosePunctuation, UnicodeCategory.InitialQuotePunctuation, UnicodeCategory.FinalQuotePunctuation, UnicodeCategory.ConnectorPunctuation, UnicodeCategory.OtherPunctuation}},
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

		/// <summary>Gets a Unicode range by <see cref="UnicodeCategory"/>.</summary>
		/// <param name="category">The category.</param>
		/// <returns>A range set of codepoints.</returns>
		public static RangeSet<Codepoint> FromUnicodeCategory(UnicodeCategory category) {
			return charSetByCategory.Value[category];
		}

		/// <summary>Gets a Unicode range by name.</summary>
		/// <param name="name">The name.</param>
		/// <returns>A range set of codepoints.</returns>
		public static RangeSet<Codepoint> FromUnicodeName(string name) {
			lock (charSetByName) {
				if (!charSetByName.TryGetValue(name, out var result)) {
					if (!categoriesByName.TryGetValue(name, out var categories)) {
						throw new ArgumentException(string.Format("Unknown unicode name '{0}'", name), "name");
					}
					result = RangeOperations<Codepoint>.Union(categories.Select(FromUnicodeCategory));
					charSetByName.Add(name, result);
				}
				return result;
			}
		}
	}
}
