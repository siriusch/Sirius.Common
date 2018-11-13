using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sirius.Unicode {
	/// <summary>A Unicode codepoint, which can be anything in the range of 0x000000-0x10FFFF.</summary>
	/// <remarks>This struct is immutable.</remarks>
	public struct Codepoint: IComparable<Codepoint>, IEquatable<Codepoint>, IFormattable, IIncrementable<Codepoint>, IConvertible {
		// Extracted from UnicodeData.txt V11
		private static readonly IReadOnlyDictionary<Codepoint, Codepoint> toUpperSmp = new Dictionary<Codepoint, Codepoint>(113) {
				{new Codepoint(0x10428, false), new Codepoint(0x10400, false)}, // DESERET LETTER LONG I
				{new Codepoint(0x1042A, false), new Codepoint(0x10402, false)}, // DESERET LETTER LONG A
				{new Codepoint(0x1042C, false), new Codepoint(0x10404, false)}, // DESERET LETTER LONG O
				{new Codepoint(0x1042E, false), new Codepoint(0x10406, false)}, // DESERET LETTER SHORT I
				{new Codepoint(0x10430, false), new Codepoint(0x10408, false)}, // DESERET LETTER SHORT A
				{new Codepoint(0x10432, false), new Codepoint(0x1040A, false)}, // DESERET LETTER SHORT O
				{new Codepoint(0x10434, false), new Codepoint(0x1040C, false)}, // DESERET LETTER AY
				{new Codepoint(0x10436, false), new Codepoint(0x1040E, false)}, // DESERET LETTER WU
				{new Codepoint(0x10438, false), new Codepoint(0x10410, false)}, // DESERET LETTER H
				{new Codepoint(0x1043A, false), new Codepoint(0x10412, false)}, // DESERET LETTER BEE
				{new Codepoint(0x1043C, false), new Codepoint(0x10414, false)}, // DESERET LETTER DEE
				{new Codepoint(0x1043E, false), new Codepoint(0x10416, false)}, // DESERET LETTER JEE
				{new Codepoint(0x10440, false), new Codepoint(0x10418, false)}, // DESERET LETTER GAY
				{new Codepoint(0x10442, false), new Codepoint(0x1041A, false)}, // DESERET LETTER VEE
				{new Codepoint(0x10444, false), new Codepoint(0x1041C, false)}, // DESERET LETTER THEE
				{new Codepoint(0x10446, false), new Codepoint(0x1041E, false)}, // DESERET LETTER ZEE
				{new Codepoint(0x10448, false), new Codepoint(0x10420, false)}, // DESERET LETTER ZHEE
				{new Codepoint(0x1044A, false), new Codepoint(0x10422, false)}, // DESERET LETTER EL
				{new Codepoint(0x1044C, false), new Codepoint(0x10424, false)}, // DESERET LETTER EN
				{new Codepoint(0x1044E, false), new Codepoint(0x10426, false)}, // DESERET LETTER OI
				{new Codepoint(0x104D8, false), new Codepoint(0x104B0, false)}, // OSAGE LETTER A
				{new Codepoint(0x104DA, false), new Codepoint(0x104B2, false)}, // OSAGE LETTER AIN
				{new Codepoint(0x104DC, false), new Codepoint(0x104B4, false)}, // OSAGE LETTER BRA
				{new Codepoint(0x104DE, false), new Codepoint(0x104B6, false)}, // OSAGE LETTER EHCHA
				{new Codepoint(0x104E0, false), new Codepoint(0x104B8, false)}, // OSAGE LETTER EIN
				{new Codepoint(0x104E2, false), new Codepoint(0x104BA, false)}, // OSAGE LETTER HYA
				{new Codepoint(0x104E4, false), new Codepoint(0x104BC, false)}, // OSAGE LETTER KA
				{new Codepoint(0x104E6, false), new Codepoint(0x104BE, false)}, // OSAGE LETTER KYA
				{new Codepoint(0x104E8, false), new Codepoint(0x104C0, false)}, // OSAGE LETTER MA
				{new Codepoint(0x104EA, false), new Codepoint(0x104C2, false)}, // OSAGE LETTER O
				{new Codepoint(0x104EC, false), new Codepoint(0x104C4, false)}, // OSAGE LETTER PA
				{new Codepoint(0x104EE, false), new Codepoint(0x104C6, false)}, // OSAGE LETTER SA
				{new Codepoint(0x104F0, false), new Codepoint(0x104C8, false)}, // OSAGE LETTER TA
				{new Codepoint(0x104F2, false), new Codepoint(0x104CA, false)}, // OSAGE LETTER TSA
				{new Codepoint(0x104F4, false), new Codepoint(0x104CC, false)}, // OSAGE LETTER TSHA
				{new Codepoint(0x104F6, false), new Codepoint(0x104CE, false)}, // OSAGE LETTER U
				{new Codepoint(0x104F8, false), new Codepoint(0x104D0, false)}, // OSAGE LETTER KHA
				{new Codepoint(0x104FA, false), new Codepoint(0x104D2, false)}, // OSAGE LETTER ZA
				{new Codepoint(0x10CC0, false), new Codepoint(0x10C80, false)}, // OLD HUNGARIAN LETTER A
				{new Codepoint(0x10CC2, false), new Codepoint(0x10C82, false)}, // OLD HUNGARIAN LETTER EB
				{new Codepoint(0x10CC4, false), new Codepoint(0x10C84, false)}, // OLD HUNGARIAN LETTER EC
				{new Codepoint(0x10CC6, false), new Codepoint(0x10C86, false)}, // OLD HUNGARIAN LETTER ECS
				{new Codepoint(0x10CC8, false), new Codepoint(0x10C88, false)}, // OLD HUNGARIAN LETTER AND
				{new Codepoint(0x10CCA, false), new Codepoint(0x10C8A, false)}, // OLD HUNGARIAN LETTER CLOSE E
				{new Codepoint(0x10CCC, false), new Codepoint(0x10C8C, false)}, // OLD HUNGARIAN LETTER EF
				{new Codepoint(0x10CCE, false), new Codepoint(0x10C8E, false)}, // OLD HUNGARIAN LETTER EGY
				{new Codepoint(0x10CD0, false), new Codepoint(0x10C90, false)}, // OLD HUNGARIAN LETTER I
				{new Codepoint(0x10CD2, false), new Codepoint(0x10C92, false)}, // OLD HUNGARIAN LETTER EJ
				{new Codepoint(0x10CD4, false), new Codepoint(0x10C94, false)}, // OLD HUNGARIAN LETTER AK
				{new Codepoint(0x10CD6, false), new Codepoint(0x10C96, false)}, // OLD HUNGARIAN LETTER EL
				{new Codepoint(0x10CD8, false), new Codepoint(0x10C98, false)}, // OLD HUNGARIAN LETTER EM
				{new Codepoint(0x10CDA, false), new Codepoint(0x10C9A, false)}, // OLD HUNGARIAN LETTER ENY
				{new Codepoint(0x10CDC, false), new Codepoint(0x10C9C, false)}, // OLD HUNGARIAN LETTER OO
				{new Codepoint(0x10CDE, false), new Codepoint(0x10C9E, false)}, // OLD HUNGARIAN LETTER RUDIMENTA OE
				{new Codepoint(0x10CE0, false), new Codepoint(0x10CA0, false)}, // OLD HUNGARIAN LETTER EP
				{new Codepoint(0x10CE2, false), new Codepoint(0x10CA2, false)}, // OLD HUNGARIAN LETTER ER
				{new Codepoint(0x10CE4, false), new Codepoint(0x10CA4, false)}, // OLD HUNGARIAN LETTER ES
				{new Codepoint(0x10CE6, false), new Codepoint(0x10CA6, false)}, // OLD HUNGARIAN LETTER ET
				{new Codepoint(0x10CE8, false), new Codepoint(0x10CA8, false)}, // OLD HUNGARIAN LETTER ETY
				{new Codepoint(0x10CEA, false), new Codepoint(0x10CAA, false)}, // OLD HUNGARIAN LETTER U
				{new Codepoint(0x10CEC, false), new Codepoint(0x10CAC, false)}, // OLD HUNGARIAN LETTER NIKOLSBURG UE
				{new Codepoint(0x10CEE, false), new Codepoint(0x10CAE, false)}, // OLD HUNGARIAN LETTER EV
				{new Codepoint(0x10CF0, false), new Codepoint(0x10CB0, false)}, // OLD HUNGARIAN LETTER EZS
				{new Codepoint(0x10CF2, false), new Codepoint(0x10CB2, false)}, // OLD HUNGARIAN LETTER US
				{new Codepoint(0x118C0, false), new Codepoint(0x118A0, false)}, // WARANG CITI LETTER NGAA
				{new Codepoint(0x118C2, false), new Codepoint(0x118A2, false)}, // WARANG CITI LETTER WI
				{new Codepoint(0x118C4, false), new Codepoint(0x118A4, false)}, // WARANG CITI LETTER YA
				{new Codepoint(0x118C6, false), new Codepoint(0x118A6, false)}, // WARANG CITI LETTER II
				{new Codepoint(0x118C8, false), new Codepoint(0x118A8, false)}, // WARANG CITI LETTER E
				{new Codepoint(0x118CA, false), new Codepoint(0x118AA, false)}, // WARANG CITI LETTER ANG
				{new Codepoint(0x118CC, false), new Codepoint(0x118AC, false)}, // WARANG CITI LETTER KO
				{new Codepoint(0x118CE, false), new Codepoint(0x118AE, false)}, // WARANG CITI LETTER YUJ
				{new Codepoint(0x118D0, false), new Codepoint(0x118B0, false)}, // WARANG CITI LETTER ENN
				{new Codepoint(0x118D2, false), new Codepoint(0x118B2, false)}, // WARANG CITI LETTER TTE
				{new Codepoint(0x118D4, false), new Codepoint(0x118B4, false)}, // WARANG CITI LETTER DA
				{new Codepoint(0x118D6, false), new Codepoint(0x118B6, false)}, // WARANG CITI LETTER AM
				{new Codepoint(0x118D8, false), new Codepoint(0x118B8, false)}, // WARANG CITI LETTER PU
				{new Codepoint(0x118DA, false), new Codepoint(0x118BA, false)}, // WARANG CITI LETTER HOLO
				{new Codepoint(0x118DC, false), new Codepoint(0x118BC, false)}, // WARANG CITI LETTER HAR
				{new Codepoint(0x118DE, false), new Codepoint(0x118BE, false)}, // WARANG CITI LETTER SII
				{new Codepoint(0x16E60, false), new Codepoint(0x16E40, false)}, // MEDEFAIDRIN LETTER M
				{new Codepoint(0x16E62, false), new Codepoint(0x16E42, false)}, // MEDEFAIDRIN LETTER V
				{new Codepoint(0x16E64, false), new Codepoint(0x16E44, false)}, // MEDEFAIDRIN LETTER ATIU
				{new Codepoint(0x16E66, false), new Codepoint(0x16E46, false)}, // MEDEFAIDRIN LETTER KP
				{new Codepoint(0x16E68, false), new Codepoint(0x16E48, false)}, // MEDEFAIDRIN LETTER T
				{new Codepoint(0x16E6A, false), new Codepoint(0x16E4A, false)}, // MEDEFAIDRIN LETTER F
				{new Codepoint(0x16E6C, false), new Codepoint(0x16E4C, false)}, // MEDEFAIDRIN LETTER K
				{new Codepoint(0x16E6E, false), new Codepoint(0x16E4E, false)}, // MEDEFAIDRIN LETTER J
				{new Codepoint(0x16E70, false), new Codepoint(0x16E50, false)}, // MEDEFAIDRIN LETTER B
				{new Codepoint(0x16E72, false), new Codepoint(0x16E52, false)}, // MEDEFAIDRIN LETTER U
				{new Codepoint(0x16E74, false), new Codepoint(0x16E54, false)}, // MEDEFAIDRIN LETTER L
				{new Codepoint(0x16E76, false), new Codepoint(0x16E56, false)}, // MEDEFAIDRIN LETTER HP
				{new Codepoint(0x16E78, false), new Codepoint(0x16E58, false)}, // MEDEFAIDRIN LETTER X
				{new Codepoint(0x16E7A, false), new Codepoint(0x16E5A, false)}, // MEDEFAIDRIN LETTER OE
				{new Codepoint(0x16E7C, false), new Codepoint(0x16E5C, false)}, // MEDEFAIDRIN LETTER R
				{new Codepoint(0x16E7E, false), new Codepoint(0x16E5E, false)}, // MEDEFAIDRIN LETTER AI
				{new Codepoint(0x1E922, false), new Codepoint(0x1E900, false)}, // ADLAM LETTER ALIF
				{new Codepoint(0x1E924, false), new Codepoint(0x1E902, false)}, // ADLAM LETTER LAAM
				{new Codepoint(0x1E926, false), new Codepoint(0x1E904, false)}, // ADLAM LETTER BA
				{new Codepoint(0x1E928, false), new Codepoint(0x1E906, false)}, // ADLAM LETTER PE
				{new Codepoint(0x1E92A, false), new Codepoint(0x1E908, false)}, // ADLAM LETTER RA
				{new Codepoint(0x1E92C, false), new Codepoint(0x1E90A, false)}, // ADLAM LETTER FA
				{new Codepoint(0x1E92E, false), new Codepoint(0x1E90C, false)}, // ADLAM LETTER O
				{new Codepoint(0x1E930, false), new Codepoint(0x1E90E, false)}, // ADLAM LETTER YHE
				{new Codepoint(0x1E932, false), new Codepoint(0x1E910, false)}, // ADLAM LETTER NUN
				{new Codepoint(0x1E934, false), new Codepoint(0x1E912, false)}, // ADLAM LETTER YA
				{new Codepoint(0x1E936, false), new Codepoint(0x1E914, false)}, // ADLAM LETTER JIIM
				{new Codepoint(0x1E938, false), new Codepoint(0x1E916, false)}, // ADLAM LETTER HA
				{new Codepoint(0x1E93A, false), new Codepoint(0x1E918, false)}, // ADLAM LETTER GA
				{new Codepoint(0x1E93C, false), new Codepoint(0x1E91A, false)}, // ADLAM LETTER TU
				{new Codepoint(0x1E93E, false), new Codepoint(0x1E91C, false)}, // ADLAM LETTER VA
				{new Codepoint(0x1E940, false), new Codepoint(0x1E91E, false)}, // ADLAM LETTER GBE
				{new Codepoint(0x1E942, false), new Codepoint(0x1E920, false)} // ADLAM LETTER KPO
		};

		// Extracted from UnicodeData.txt V11
		private static readonly IReadOnlyDictionary<Codepoint, Codepoint> toLowerSmp = new Dictionary<Codepoint, Codepoint>(113) {
				{new Codepoint(0x10400, false), new Codepoint(0x10428, false)}, // DESERET LETTER LONG I
				{new Codepoint(0x10402, false), new Codepoint(0x1042A, false)}, // DESERET LETTER LONG A
				{new Codepoint(0x10404, false), new Codepoint(0x1042C, false)}, // DESERET LETTER LONG O
				{new Codepoint(0x10406, false), new Codepoint(0x1042E, false)}, // DESERET LETTER SHORT I
				{new Codepoint(0x10408, false), new Codepoint(0x10430, false)}, // DESERET LETTER SHORT A
				{new Codepoint(0x1040A, false), new Codepoint(0x10432, false)}, // DESERET LETTER SHORT O
				{new Codepoint(0x1040C, false), new Codepoint(0x10434, false)}, // DESERET LETTER AY
				{new Codepoint(0x1040E, false), new Codepoint(0x10436, false)}, // DESERET LETTER WU
				{new Codepoint(0x10410, false), new Codepoint(0x10438, false)}, // DESERET LETTER H
				{new Codepoint(0x10412, false), new Codepoint(0x1043A, false)}, // DESERET LETTER BEE
				{new Codepoint(0x10414, false), new Codepoint(0x1043C, false)}, // DESERET LETTER DEE
				{new Codepoint(0x10416, false), new Codepoint(0x1043E, false)}, // DESERET LETTER JEE
				{new Codepoint(0x10418, false), new Codepoint(0x10440, false)}, // DESERET LETTER GAY
				{new Codepoint(0x1041A, false), new Codepoint(0x10442, false)}, // DESERET LETTER VEE
				{new Codepoint(0x1041C, false), new Codepoint(0x10444, false)}, // DESERET LETTER THEE
				{new Codepoint(0x1041E, false), new Codepoint(0x10446, false)}, // DESERET LETTER ZEE
				{new Codepoint(0x10420, false), new Codepoint(0x10448, false)}, // DESERET LETTER ZHEE
				{new Codepoint(0x10422, false), new Codepoint(0x1044A, false)}, // DESERET LETTER EL
				{new Codepoint(0x10424, false), new Codepoint(0x1044C, false)}, // DESERET LETTER EN
				{new Codepoint(0x10426, false), new Codepoint(0x1044E, false)}, // DESERET LETTER OI
				{new Codepoint(0x104B0, false), new Codepoint(0x104D8, false)}, // OSAGE LETTER A
				{new Codepoint(0x104B2, false), new Codepoint(0x104DA, false)}, // OSAGE LETTER AIN
				{new Codepoint(0x104B4, false), new Codepoint(0x104DC, false)}, // OSAGE LETTER BRA
				{new Codepoint(0x104B6, false), new Codepoint(0x104DE, false)}, // OSAGE LETTER EHCHA
				{new Codepoint(0x104B8, false), new Codepoint(0x104E0, false)}, // OSAGE LETTER EIN
				{new Codepoint(0x104BA, false), new Codepoint(0x104E2, false)}, // OSAGE LETTER HYA
				{new Codepoint(0x104BC, false), new Codepoint(0x104E4, false)}, // OSAGE LETTER KA
				{new Codepoint(0x104BE, false), new Codepoint(0x104E6, false)}, // OSAGE LETTER KYA
				{new Codepoint(0x104C0, false), new Codepoint(0x104E8, false)}, // OSAGE LETTER MA
				{new Codepoint(0x104C2, false), new Codepoint(0x104EA, false)}, // OSAGE LETTER O
				{new Codepoint(0x104C4, false), new Codepoint(0x104EC, false)}, // OSAGE LETTER PA
				{new Codepoint(0x104C6, false), new Codepoint(0x104EE, false)}, // OSAGE LETTER SA
				{new Codepoint(0x104C8, false), new Codepoint(0x104F0, false)}, // OSAGE LETTER TA
				{new Codepoint(0x104CA, false), new Codepoint(0x104F2, false)}, // OSAGE LETTER TSA
				{new Codepoint(0x104CC, false), new Codepoint(0x104F4, false)}, // OSAGE LETTER TSHA
				{new Codepoint(0x104CE, false), new Codepoint(0x104F6, false)}, // OSAGE LETTER U
				{new Codepoint(0x104D0, false), new Codepoint(0x104F8, false)}, // OSAGE LETTER KHA
				{new Codepoint(0x104D2, false), new Codepoint(0x104FA, false)}, // OSAGE LETTER ZA
				{new Codepoint(0x10C80, false), new Codepoint(0x10CC0, false)}, // OLD HUNGARIAN LETTER A
				{new Codepoint(0x10C82, false), new Codepoint(0x10CC2, false)}, // OLD HUNGARIAN LETTER EB
				{new Codepoint(0x10C84, false), new Codepoint(0x10CC4, false)}, // OLD HUNGARIAN LETTER EC
				{new Codepoint(0x10C86, false), new Codepoint(0x10CC6, false)}, // OLD HUNGARIAN LETTER ECS
				{new Codepoint(0x10C88, false), new Codepoint(0x10CC8, false)}, // OLD HUNGARIAN LETTER AND
				{new Codepoint(0x10C8A, false), new Codepoint(0x10CCA, false)}, // OLD HUNGARIAN LETTER CLOSE E
				{new Codepoint(0x10C8C, false), new Codepoint(0x10CCC, false)}, // OLD HUNGARIAN LETTER EF
				{new Codepoint(0x10C8E, false), new Codepoint(0x10CCE, false)}, // OLD HUNGARIAN LETTER EGY
				{new Codepoint(0x10C90, false), new Codepoint(0x10CD0, false)}, // OLD HUNGARIAN LETTER I
				{new Codepoint(0x10C92, false), new Codepoint(0x10CD2, false)}, // OLD HUNGARIAN LETTER EJ
				{new Codepoint(0x10C94, false), new Codepoint(0x10CD4, false)}, // OLD HUNGARIAN LETTER AK
				{new Codepoint(0x10C96, false), new Codepoint(0x10CD6, false)}, // OLD HUNGARIAN LETTER EL
				{new Codepoint(0x10C98, false), new Codepoint(0x10CD8, false)}, // OLD HUNGARIAN LETTER EM
				{new Codepoint(0x10C9A, false), new Codepoint(0x10CDA, false)}, // OLD HUNGARIAN LETTER ENY
				{new Codepoint(0x10C9C, false), new Codepoint(0x10CDC, false)}, // OLD HUNGARIAN LETTER OO
				{new Codepoint(0x10C9E, false), new Codepoint(0x10CDE, false)}, // OLD HUNGARIAN LETTER RUDIMENTA OE
				{new Codepoint(0x10CA0, false), new Codepoint(0x10CE0, false)}, // OLD HUNGARIAN LETTER EP
				{new Codepoint(0x10CA2, false), new Codepoint(0x10CE2, false)}, // OLD HUNGARIAN LETTER ER
				{new Codepoint(0x10CA4, false), new Codepoint(0x10CE4, false)}, // OLD HUNGARIAN LETTER ES
				{new Codepoint(0x10CA6, false), new Codepoint(0x10CE6, false)}, // OLD HUNGARIAN LETTER ET
				{new Codepoint(0x10CA8, false), new Codepoint(0x10CE8, false)}, // OLD HUNGARIAN LETTER ETY
				{new Codepoint(0x10CAA, false), new Codepoint(0x10CEA, false)}, // OLD HUNGARIAN LETTER U
				{new Codepoint(0x10CAC, false), new Codepoint(0x10CEC, false)}, // OLD HUNGARIAN LETTER NIKOLSBURG UE
				{new Codepoint(0x10CAE, false), new Codepoint(0x10CEE, false)}, // OLD HUNGARIAN LETTER EV
				{new Codepoint(0x10CB0, false), new Codepoint(0x10CF0, false)}, // OLD HUNGARIAN LETTER EZS
				{new Codepoint(0x10CB2, false), new Codepoint(0x10CF2, false)}, // OLD HUNGARIAN LETTER US
				{new Codepoint(0x118A0, false), new Codepoint(0x118C0, false)}, // WARANG CITI LETTER NGAA
				{new Codepoint(0x118A2, false), new Codepoint(0x118C2, false)}, // WARANG CITI LETTER WI
				{new Codepoint(0x118A4, false), new Codepoint(0x118C4, false)}, // WARANG CITI LETTER YA
				{new Codepoint(0x118A6, false), new Codepoint(0x118C6, false)}, // WARANG CITI LETTER II
				{new Codepoint(0x118A8, false), new Codepoint(0x118C8, false)}, // WARANG CITI LETTER E
				{new Codepoint(0x118AA, false), new Codepoint(0x118CA, false)}, // WARANG CITI LETTER ANG
				{new Codepoint(0x118AC, false), new Codepoint(0x118CC, false)}, // WARANG CITI LETTER KO
				{new Codepoint(0x118AE, false), new Codepoint(0x118CE, false)}, // WARANG CITI LETTER YUJ
				{new Codepoint(0x118B0, false), new Codepoint(0x118D0, false)}, // WARANG CITI LETTER ENN
				{new Codepoint(0x118B2, false), new Codepoint(0x118D2, false)}, // WARANG CITI LETTER TTE
				{new Codepoint(0x118B4, false), new Codepoint(0x118D4, false)}, // WARANG CITI LETTER DA
				{new Codepoint(0x118B6, false), new Codepoint(0x118D6, false)}, // WARANG CITI LETTER AM
				{new Codepoint(0x118B8, false), new Codepoint(0x118D8, false)}, // WARANG CITI LETTER PU
				{new Codepoint(0x118BA, false), new Codepoint(0x118DA, false)}, // WARANG CITI LETTER HOLO
				{new Codepoint(0x118BC, false), new Codepoint(0x118DC, false)}, // WARANG CITI LETTER HAR
				{new Codepoint(0x118BE, false), new Codepoint(0x118DE, false)}, // WARANG CITI LETTER SII
				{new Codepoint(0x16E40, false), new Codepoint(0x16E60, false)}, // MEDEFAIDRIN LETTER M
				{new Codepoint(0x16E42, false), new Codepoint(0x16E62, false)}, // MEDEFAIDRIN LETTER V
				{new Codepoint(0x16E44, false), new Codepoint(0x16E64, false)}, // MEDEFAIDRIN LETTER ATIU
				{new Codepoint(0x16E46, false), new Codepoint(0x16E66, false)}, // MEDEFAIDRIN LETTER KP
				{new Codepoint(0x16E48, false), new Codepoint(0x16E68, false)}, // MEDEFAIDRIN LETTER T
				{new Codepoint(0x16E4A, false), new Codepoint(0x16E6A, false)}, // MEDEFAIDRIN LETTER F
				{new Codepoint(0x16E4C, false), new Codepoint(0x16E6C, false)}, // MEDEFAIDRIN LETTER K
				{new Codepoint(0x16E4E, false), new Codepoint(0x16E6E, false)}, // MEDEFAIDRIN LETTER J
				{new Codepoint(0x16E50, false), new Codepoint(0x16E70, false)}, // MEDEFAIDRIN LETTER B
				{new Codepoint(0x16E52, false), new Codepoint(0x16E72, false)}, // MEDEFAIDRIN LETTER U
				{new Codepoint(0x16E54, false), new Codepoint(0x16E74, false)}, // MEDEFAIDRIN LETTER L
				{new Codepoint(0x16E56, false), new Codepoint(0x16E76, false)}, // MEDEFAIDRIN LETTER HP
				{new Codepoint(0x16E58, false), new Codepoint(0x16E78, false)}, // MEDEFAIDRIN LETTER X
				{new Codepoint(0x16E5A, false), new Codepoint(0x16E7A, false)}, // MEDEFAIDRIN LETTER OE
				{new Codepoint(0x16E5C, false), new Codepoint(0x16E7C, false)}, // MEDEFAIDRIN LETTER R
				{new Codepoint(0x16E5E, false), new Codepoint(0x16E7E, false)}, // MEDEFAIDRIN LETTER AI
				{new Codepoint(0x1E900, false), new Codepoint(0x1E922, false)}, // ADLAM LETTER ALIF
				{new Codepoint(0x1E902, false), new Codepoint(0x1E924, false)}, // ADLAM LETTER LAAM
				{new Codepoint(0x1E904, false), new Codepoint(0x1E926, false)}, // ADLAM LETTER BA
				{new Codepoint(0x1E906, false), new Codepoint(0x1E928, false)}, // ADLAM LETTER PE
				{new Codepoint(0x1E908, false), new Codepoint(0x1E92A, false)}, // ADLAM LETTER RA
				{new Codepoint(0x1E90A, false), new Codepoint(0x1E92C, false)}, // ADLAM LETTER FA
				{new Codepoint(0x1E90C, false), new Codepoint(0x1E92E, false)}, // ADLAM LETTER O
				{new Codepoint(0x1E90E, false), new Codepoint(0x1E930, false)}, // ADLAM LETTER YHE
				{new Codepoint(0x1E910, false), new Codepoint(0x1E932, false)}, // ADLAM LETTER NUN
				{new Codepoint(0x1E912, false), new Codepoint(0x1E934, false)}, // ADLAM LETTER YA
				{new Codepoint(0x1E914, false), new Codepoint(0x1E936, false)}, // ADLAM LETTER JIIM
				{new Codepoint(0x1E916, false), new Codepoint(0x1E938, false)}, // ADLAM LETTER HA
				{new Codepoint(0x1E918, false), new Codepoint(0x1E93A, false)}, // ADLAM LETTER GA
				{new Codepoint(0x1E91A, false), new Codepoint(0x1E93C, false)}, // ADLAM LETTER TU
				{new Codepoint(0x1E91C, false), new Codepoint(0x1E93E, false)}, // ADLAM LETTER VA
				{new Codepoint(0x1E91E, false), new Codepoint(0x1E940, false)}, // ADLAM LETTER GBE
				{new Codepoint(0x1E920, false), new Codepoint(0x1E942, false)} // ADLAM LETTER KPO
		};

		/// <summary>The minimum value.</summary>
		public static readonly Codepoint MinValue = new Codepoint(0x000000, false);

		/// <summary>The maximum value.</summary>
		public static readonly Codepoint MaxValue = new Codepoint(0x10FFFF, false);

		/// <summary>Equality operator.</summary>
		/// <param name="x">A Codepoint.</param>
		/// <param name="y">A Codepoint.</param>
		/// <returns>The result of the comparison.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator ==(Codepoint x, Codepoint y) {
			return x.value == y.value;
		}

		/// <summary>Inequality operator.</summary>
		/// <param name="x">A Codepoint.</param>
		/// <param name="y">A Codepoint.</param>
		/// <returns>The result of the comparison.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator !=(Codepoint x, Codepoint y) {
			return x.value != y.value;
		}

		/// <summary>Less-than comparison operator.</summary>
		/// <param name="x">A Codepoint.</param>
		/// <param name="y">A Codepoint.</param>
		/// <returns>The result of the comparison.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator <(Codepoint x, Codepoint y) {
			return x.value < y.value;
		}

		/// <summary>Greater-than comparison operator.</summary>
		/// <param name="x">A Codepoint.</param>
		/// <param name="y">A Codepoint.</param>
		/// <returns>The result of the comparison.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator >(Codepoint x, Codepoint y) {
			return x.value > y.value;
		}

		/// <summary>Greater-than-or-equal comparison operator.</summary>
		/// <param name="x">A Codepoint.</param>
		/// <param name="y">A Codepoint.</param>
		/// <returns>The result of the comparison.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator >=(Codepoint x, Codepoint y) {
			return x.value >= y.value;
		}

		/// <summary>Less-than-or-equal comparison operator.</summary>
		/// <param name="x">A Codepoint.</param>
		/// <param name="y">A Codepoint.</param>
		/// <returns>The result of the comparison.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool operator <=(Codepoint x, Codepoint y) {
			return x.value <= y.value;
		}

		/// <summary>Addition operator.</summary>
		/// <param name="c">A Codepoint to process.</param>
		/// <param name="offset">The offset to add.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If the new <see cref="Codepoint"/> would be in an invalid range.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static Codepoint operator +(Codepoint c, int offset) {
			return new Codepoint(c.value+offset);
		}

		/// <summary>Subtraction operator.</summary>
		/// <param name="c">A Codepoint to process.</param>
		/// <param name="offset">The offset to subtract.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If the new <see cref="Codepoint"/> would be in an invalid range.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static Codepoint operator -(Codepoint c, int offset) {
			return new Codepoint(c.value-offset);
		}

		/// <summary>Explicit cast that converts the given <see cref="Codepoint"/> to a <see cref="char"/>.</summary>
		/// <param name="codepoint">The codepoint to be casted.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static explicit operator char(Codepoint codepoint) {
			return (char)codepoint.value;
		}

		/// <summary>Implicit cast that converts the given <see cref="char"/> to a <see cref="Codepoint"/>.</summary>
		/// <param name="value">The character to be casted.</param>
		/// <returns>The result of the operation.</returns>
		/// <remarks>The <paramref name="value"/> is not checked to be valid.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static implicit operator Codepoint(char value) {
			return new Codepoint(value, false);
		}

		/// <summary>Explicit cast that converts the given <see cref="Codepoint"/> to an <see cref="int"/>.</summary>
		/// <param name="codepoint">The codepoint to be casted.</param>
		/// <returns>The result of the operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static explicit operator int(Codepoint codepoint) {
			return codepoint.value;
		}

		/// <summary>Explicit cast that converts the given <see cref="int"/> to a <see cref="Codepoint"/>.</summary>
		/// <param name="value">The character to be casted.</param>
		/// <returns>The result of the operation.</returns>
		/// <remarks>The <paramref name="value"/> is not checked to be valid.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static explicit operator Codepoint(int value) {
			return new Codepoint(value, false);
		}

		/// <summary>Query if the given codepoint is valid.</summary>
		/// <param name="codepoint">The codepoint to be casted.</param>
		/// <returns><c>true</c> if valid, <c>false</c> if not.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public static bool IsValid(Codepoint codepoint) {
			return IsValidRange(codepoint.value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		private static bool IsValidRange(int value) {
			return ((value >= 0) && (value < 0xD800)) || ((value >= 0xE000) && (value <= 0xFFFD)) || ((value >= 0x010000) && (value <= 0x10FFFF));
		}

		/// <summary>Query if codepoint is uppercase.</summary>
		/// <param name="codepoint">The codepoint to check.</param>
		/// <returns><c>true</c> if it is uppercase, <c>false</c> if not.</returns>
		[Pure]
		public static bool IsUpper(Codepoint codepoint) {
			return codepoint.FitsIntoChar ? char.IsUpper((char)codepoint) : Codepoints.UppercaseSmp.Contains(codepoint);
		}

		/// <summary>Query if codepoint is lowercase.</summary>
		/// <param name="codepoint">The codepoint to check.</param>
		/// <returns><c>true</c> if it is lowercase, <c>false</c> if not.</returns>
		[Pure]
		public static bool IsLower(Codepoint codepoint) {
			return codepoint.FitsIntoChar ? char.IsLower((char)codepoint) : Codepoints.LowercaseSmp.Contains(codepoint);
		}

		/// <summary>Converts a codepoint to an upper invariant.</summary>
		/// <param name="codepoint">The codepoint to be casted.</param>
		/// <returns>Codepoint as a Codepoint.</returns>
		[Pure]
		public static Codepoint ToUpperInvariant(Codepoint codepoint) {
			if (codepoint.FitsIntoChar) {
				return char.ToUpperInvariant((char)codepoint);
			}
			if (toUpperSmp.TryGetValue(codepoint, out var result)) {
				return result;
			}
			return codepoint;
		}

		/// <summary>Converts a codepoint to an upper invariant.</summary>
		/// <param name="codepoint">The codepoint to be casted.</param>
		/// <returns>Codepoint as a Codepoint.</returns>
		[Pure]
		public static Codepoint ToLowerInvariant(Codepoint codepoint) {
			if (codepoint.FitsIntoChar) {
				return char.ToLowerInvariant((char)codepoint);
			}
			if (toLowerSmp.TryGetValue(codepoint, out var result)) {
				return result;
			}
			return codepoint;
		}

		/// <summary>Query if the given codepoint is a combining mark.</summary>
		/// <param name="codepoint">A Codepoint to process.</param>
		/// <returns><c>true</c> if it is a combining mark, <c>false</c> if not.</returns>
		[Pure]
		public static bool IsCombiningMark(Codepoint codepoint) {
			return IsCombiningMark(codepoint.value);
		}

		internal static bool IsCombiningMark(int c) {
			return ((c >= 0x0300) && (c <= 0x036F))
					|| ((c >= 0x1AB0) && (c <= 0x1AFF))
					|| ((c >= 0x1DC0) && (c <= 0x1DFF))
					|| ((c >= 0x20D0) && (c <= 0x20FF))
					|| ((c >= 0xFE20) && (c <= 0xFE2F));
		}

		/// <summary>Parses a Codepoint from a hexadecimal value.</summary>
		/// <exception cref="FormatException">Thrown when the format is incorrect, or the value is not a valid codepoint.</exception>
		/// <param name="hexValue">The hexadecimal value.</param>
		/// <returns>A Codepoint.</returns>
		[Pure]
		public static Codepoint Parse(string hexValue) {
			if (!TryParse(hexValue, out var result)) {
				throw new FormatException();
			}
			return result;
		}

		/// <summary>Attempts to parse a Codepoint from the given string.</summary>
		/// <param name="hexValue">The hexadecimal value.</param>
		/// <param name="result">[out] The result.</param>
		/// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
		public static bool TryParse(string hexValue, out Codepoint result) {
			if ((hexValue.StartsWith("0x") || hexValue.StartsWith("U+") || hexValue.StartsWith("u+"))) {
				hexValue = hexValue.Substring(2);
			}
			if (!int.TryParse(hexValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value) || !IsValidRange(value)) {
				result = default;
				return false;
			}
			result = new Codepoint(value, false);
			return true;
		}

		/// <summary>Created an enumeration of codepoints from an enumeration of UTF-16-chars.</summary>
		/// <remarks>Surrogate pairs in the input are converted to single codepoints.</remarks>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <param name="chars">The input UTF-16 chars.</param>
		/// <returns>The codepoints corresponding to the UTF-16 chars.</returns>
		[Pure]
		public static IEnumerable<Codepoint> FromCharsMany(IEnumerable<char> chars) {
			if (chars == null) {
				throw new ArgumentNullException(nameof(chars));
			}
			using (var enumerator = chars.GetEnumerator()) {
				while (enumerator.MoveNext()) {
					yield return FromCharEnumerator(enumerator);
				}
			}
		}

		/// <summary>Created a single codepoint from an enumeration of UTF-16-chars.</summary>
		/// <remarks>Surrogate pairs in the input are converted to a single codepoint.</remarks>
		/// <exception cref="ArgumentNullException">Thrown when one or more required arguments are <c>null</c></exception>
		/// <exception cref="ArgumentException">The enumeration does not exactly contain the expected sequence of UTF-16-characters (one BMP character or a pair of high- and low surrogate).</exception>
		/// <param name="chars">The input UTF-16 chars.</param>
		/// <returns>The codepoint corresponding to the UTF-16 chars.</returns>
		[Pure]
		public static Codepoint FromChars(IEnumerable<char> chars) {
			if (chars == null) {
				throw new ArgumentNullException(nameof(chars));
			}
			Codepoint value;
			using (var enumerator = chars.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					throw new ArgumentException("Empty enumeration of characters is not a valid codepoint", nameof(chars));
				}
				value = FromCharEnumerator(enumerator);
				if (enumerator.MoveNext()) {
					throw new ArgumentException("Excess characters in enumeration", nameof(chars));
				}
			}
			return value;
		}

		internal static Codepoint FromCharEnumerator(IEnumerator<char> chars) {
			Codepoint value;
			if (char.IsHighSurrogate(chars.Current)) {
				var high = chars.Current;
				if (!chars.MoveNext() || !char.IsLowSurrogate(chars.Current)) {
					throw new ArgumentException("Low surrogate must follow a high surrogate", nameof(chars));
				}
				var low = chars.Current;
				value = new Codepoint(char.ConvertToUtf32(high, low), false);
			} else if (char.IsLowSurrogate(chars.Current)) {
				throw new ArgumentException("Low surrogate cannot be without prior high surrogate", nameof(chars));
			} else {
				value = new Codepoint(chars.Current, false);
			}
			return value;
		}

		private readonly int value;

		internal bool IsValidSingleChar {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ((this.value >= 0) && (this.value <= 0xD7FF)) || ((this.value >= 0xE000) && (this.value <= 0xFFFF));
		}

		internal bool IsValidDoubleChar {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (this.value >= 0x10000) && (this.value <= 0x10FFFF);
		}

		internal Codepoint(int value, bool check) {
			if (check && !IsValidRange(value)) {
				throw new ArgumentOutOfRangeException(nameof(value));
			}
			this.value = value;
		}

		/// <summary>Create a new Codepoint from an integer value.</summary>
		/// <param name="value">The character to be casted.</param>
		/// <remarks>The value must be a valid codepoint. To perform an unchecked conversion, use the cast instead.</remarks>
		public Codepoint(int value): this(value, true) { }

		/// <summary>Gets a value indicating whether the fits into a single UTF-16 character.</summary>
		/// <value><c>true</c> if fits into character, <c>false</c> if not.</value>
		public bool FitsIntoChar => this.value <= 0xFFFF;

		/// <summary>Converts this Codepoint to an integer.</summary>
		/// <returns>This Codepoint as an int.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public int ToInt32() {
			return this.value;
		}

		/// <summary>Converts this Codepoint to an integer.</summary>
		/// <returns>This Codepoint as a byte[].</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public byte[] ToUtf32Bytes() {
			return BitConverter.GetBytes(this.value);
		}

		/// <summary>Converts this Codepoint to a UTF-16-char array.</summary>
		/// <exception cref="UnsupportedCodepointException">Thrown when the codepoint is not valid and cannot be converted to UTF-16 chars.</exception>
		/// <returns>This Codepoint as a char[].</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public char[] ToChars() {
			if (this.IsValidSingleChar) {
				return new[] { GetSingleCharUnchecked() };
			}
			if (this.IsValidDoubleChar) {
				return new[] { GetHighSurrogate(), GetLowSurrogate() };
			}
			throw new UnsupportedCodepointException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		internal char GetSingleCharUnchecked() {
			return unchecked((char)this.value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		internal char GetHighSurrogate() {
			return unchecked((char)(((this.value-0x010000)>>10)|0xD800));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		internal char GetLowSurrogate() {
			return unchecked((char)(((this.value-0x010000)&0x03FF)|0xDC00));
		}

		/// <summary>Converts this Codepoint to an array of UTF-8 bytes.</summary>
		/// <exception cref="UnsupportedCodepointException">Thrown when the codepoint is not valid and cannot be converted to UTF-8.</exception>
		/// <returns>This Codepoint as a byte[].</returns>
		/// <remarks>The length of the array can be from 1 to 4 bytes depending on the codepoint.</remarks>
		[Pure]
		public byte[] ToUtf8Bytes() {
			if (this.value >= 0x0) {
				if (this.value <= 0x00007F) {
					return new[] {
						(byte)this.value
					};
				}
				if (this.value <= 0x0007FF) {
					return new[] {
						(byte)(((this.value>>6)&0x1F)|0xC0),
						(byte)(0x80|(this.value&0x3F))
					};
				}
				if (this.value <= 0x00FFFF) {
					return new[] {
						(byte)(((this.value>>12)&0x0F)|0xE0),
						(byte)(((this.value>>6)&0x3F)|0x80),
						(byte)((this.value&0x3F)|0x80)
					};
				}
				if (this.value <= 0x1FFFFF) {
					return new[] {
						(byte)(0xF0|(0x07&(this.value>>18))),
						(byte)(0x80|(0x3F&(this.value>>12))),
						(byte)(0x80|(0x3F&(this.value>>6))),
						(byte)(0x80|(0x3F&this.value))
					};
				}
			}
			throw new UnsupportedCodepointException();
		}

		/// <summary>Appends the codepoint as UTF-16 representation to a <see cref="StringBuilder"/>.</summary>
		/// <exception cref="UnsupportedCodepointException">Thrown when the codepoint is not valid and cannot be converted to UTF-16.</exception>
		/// <param name="builder">The string builder to append to.</param>
		/// <seealso cref="UnicodeExtensions.Append(StringBuilder,Codepoint)"/>
		/// <seealso cref="UnicodeExtensions.Append(StringBuilder,IEnumerable{Codepoint})"/>
		public void AppendTo(StringBuilder builder) {
			if (this.IsValidSingleChar) {
				builder.Append(GetSingleCharUnchecked());
			} else if (this.IsValidDoubleChar) {
				builder.Append(GetHighSurrogate());
				builder.Append(GetLowSurrogate());
			} else {
				throw new UnsupportedCodepointException();
			}
		}

		/// <summary>
		/// 	Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in
		/// 	the sort order as the other object.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		/// 	A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" />
		/// 	in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the
		/// 	sort order.
		/// </returns>
		public int CompareTo(Codepoint other) {
			return this.value.CompareTo(other.value);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		public bool Equals(Codepoint other) {
			return this.value == other.value;
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		public override bool Equals(object obj) {
			if (obj is Codepoint codepoint) {
				return this.value == codepoint.value;
			}
			return base.Equals(obj);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode() {
			return this.value.GetHashCode();
		}

		/// <summary>Returns a textual representation of the codepoint.</summary>
		/// <returns>A string.</returns>
		public override string ToString() {
			return ToString(null, null);
		}

		/// <summary>Returns a textual representation of the codepoint.</summary>
		/// <param name="format">Describes the format to use.</param>
		/// <returns>A string.</returns>
		public string ToString(string format) {
			return ToString(format, null);
		}

		TypeCode IConvertible.GetTypeCode() {
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider) {
			return this.value > 0;
		}

		char IConvertible.ToChar(IFormatProvider provider) {
			return (char)this;
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider) {
			return (sbyte)this.value;
		}

		byte IConvertible.ToByte(IFormatProvider provider) {
			return (byte)this.value;
		}

		short IConvertible.ToInt16(IFormatProvider provider) {
			return (short)this.value;
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider) {
			return (ushort)this.value;
		}

		int IConvertible.ToInt32(IFormatProvider provider) {
			return this.value;
		}

		uint IConvertible.ToUInt32(IFormatProvider provider) {
			return (uint)this.value;
		}

		long IConvertible.ToInt64(IFormatProvider provider) {
			return this.value;
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider) {
			return (ulong)this.value;
		}

		float IConvertible.ToSingle(IFormatProvider provider) {
			return this.value;
		}

		double IConvertible.ToDouble(IFormatProvider provider) {
			return this.value;
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider) {
			return this.value;
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider) {
			throw new NotSupportedException();
		}

		/// <summary>Returns a textual representation of the codepoint.</summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A string.</returns>
		public string ToString(IFormatProvider formatProvider) {
			return ToString(null, formatProvider);
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
			if (typeof(Codepoint).IsAssignableFrom(conversionType)) {
				return this;
			}
			return ((IConvertible)this.value).ToType(conversionType, provider);
		}

		/// <summary>
		///     Returns a textual representation of the codepoint.
		///     <list type="table">
		///         <listheader>
		///             <term>Format</term>
		///             <description>Description</description>
		///         </listheader>
		///         <item>
		///             <term>x</term>
		///             <description>Two-digit hexadecimal. Only correct for ASCII codepoints.</description>
		///         </item>
		///         <item>
		///             <term>u</term>
		///             <description>Four-digit hexadecimal. Only correct for BMP codepoints.</description>
		///         </item>
		///         <item>
		///             <term>U</term>
		///             <description>Eight-digit hexadecimal.</description>
		///         </item>
		///         <item>
		///             <term>X</term>
		///             <description>Automatic hexadecimal (2, 4 or 8 bytes depending on value).</description>
		///         </item>
		///         <item>
		///             <term>(other)</term>
		///             <description>String with the literal UTF-16-characters of the codepoint.</description>
		///         </item>
		///     </list>
		/// </summary>
		/// <param name="format">
		///     The format to use. -or- A null reference (Nothing in Visual Basic) to use the default format
		///     defined for the type of the <see cref="T:System.IFormattable" /> implementation.
		/// </param>
		/// <param name="formatProvider">
		///     The provider to use to format the value. -or- A null reference (Nothing in Visual Basic) to obtain the numeric
		///     format information from the current locale setting of
		///     the operating system.
		/// </param>
		/// <returns>A string.</returns>
		public string ToString(string format, IFormatProvider formatProvider) {
			switch (format) {
			case "x":
				return string.Format(formatProvider, "0x{0:x2}", this.value);
			case "u":
				return string.Format(formatProvider, "u+{0:x4}", this.value);
			case "U":
				return string.Format(formatProvider, "U+{0:x8}", this.value);
			case "X":
				if (this.value <= 0xFF) {
					goto case "x";
				}
				if (this.value <= 0xFFFF) {
					goto case "u";
				}
				goto case "U";
			}
			if (this.value == -1) {
				return "EOF";
			}
			if (this.IsValidSingleChar) {
				return ((char)this.value).ToString();
			}
			if (this.IsValidDoubleChar) {
				return char.ConvertFromUtf32(this.value);
			}
			return "(Invalid Codepoint)";
		}

		Codepoint IIncrementable<Codepoint>.Increment() {
			return new Codepoint(this.value+1, false);
		}

		Codepoint IIncrementable<Codepoint>.Decrement() {
			return new Codepoint(this.value-1, false);
		}
	}
}
