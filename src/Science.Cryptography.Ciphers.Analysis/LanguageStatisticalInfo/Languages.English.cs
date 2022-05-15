using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

public static partial class Languages
{
	public static readonly LanguageStatisticalInfo English = new(
		"English",
		"en",
		WellKnownAlphabets.English,
		RelativeFrequenciesOfLetters: new RelativeCharacterFrequencies(new Dictionary<char, double>(IgnoreCaseCharComparer.Instance)
		{
			{ 'A', 0.08167 },
			{ 'B', 0.01492 },
			{ 'C', 0.02782 },
			{ 'D', 0.04253 },
			{ 'E', 0.12702 },
			{ 'F', 0.02228 },
			{ 'G', 0.02015 },
			{ 'H', 0.06094 },
			{ 'I', 0.06966 },
			{ 'J', 0.00153 },
			{ 'K', 0.00772 },
			{ 'L', 0.04025 },
			{ 'M', 0.02406 },
			{ 'N', 0.06749 },
			{ 'O', 0.07507 },
			{ 'P', 0.01929 },
			{ 'Q', 0.00095 },
			{ 'R', 0.05987 },
			{ 'S', 0.06327 },
			{ 'T', 0.09056 },
			{ 'U', 0.02758 },
			{ 'V', 0.00978 },
			{ 'W', 0.02360 },
			{ 'X', 0.00150 },
			{ 'Y', 0.01975 },
			{ 'Z', 0.00074 },
		}),
		RelativeFrequenciesOfFirstLettersOfWords: new RelativeCharacterFrequencies(new Dictionary<char, double>(IgnoreCaseCharComparer.Instance)
		{
			{ 'A', 0.11602 },
			{ 'B', 0.04702 },
			{ 'C', 0.03511 },
			{ 'D', 0.02670 },
			{ 'E', 0.02007 },
			{ 'F', 0.03779 },
			{ 'G', 0.01950 },
			{ 'H', 0.07232 },
			{ 'I', 0.06286 },
			{ 'J', 0.00597 },
			{ 'K', 0.00590 },
			{ 'L', 0.02705 },
			{ 'M', 0.04383 },
			{ 'N', 0.02365 },
			{ 'O', 0.06264 },
			{ 'P', 0.02545 },
			{ 'Q', 0.00173 },
			{ 'R', 0.01653 },
			{ 'S', 0.07755 },
			{ 'T', 0.16671 },
			{ 'U', 0.01487 },
			{ 'V', 0.00649 },
			{ 'W', 0.06753 },
			{ 'X', 0.00017 },
			{ 'Y', 0.01620 },
			{ 'Z', 0.00034 },
		}),
		RelativeNGramFrequencies: new Dictionary<int, RelativeStringFrequencies>()
		{
			{
				2,
				new RelativeStringFrequencies(new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
				{
					{"TH", 0.02705698d},
					{"HE", 0.02328545d},
					{"IN", 0.020275534d},
					{"ER", 0.017838136d},
					{"AN", 0.016136243d},
					{"RE", 0.014089222d},
					{"ES", 0.013198142d},
					{"ON", 0.01316225d},
					{"ST", 0.012492322d},
					{"NT", 0.011725158d},
					{"EN", 0.011329747d},
					{"AT", 0.011164d},
					{"ED", 0.010787831d},
					{"ND", 0.010682918d},
					{"TO", 0.010664622d},
					{"OR", 0.010574431d},
					{"EA", 0.010020474d},
					{"TI", 0.009918455d},
					{"AR", 0.009794637d},
					{"TE", 0.009781351d},
					{"NG", 0.008919108d},
					{"AL", 0.00883683d},
					{"IT", 0.008773685d},
					{"AS", 0.008735606d},
					{"IS", 0.008637575d},
					{"HA", 0.008318866d},
					{"ET", 0.007602123d},
					{"SE", 0.007292169d},
					{"OU", 0.007195042d},
					{"OF", 0.007062905d},
					{"LE", 0.007026448d},
					{"SA", 0.006956346d},
					{"VE", 0.006780783d},
					{"RO", 0.006759923d},
					{"RA", 0.006624591d},
					{"RI", 0.006390801d},
					{"HI", 0.006358587d},
					{"NE", 0.006320737d},
					{"ME", 0.006299012d},
					{"DE", 0.006250933d},
					{"CO", 0.006183235d},
					{"TA", 0.006046906d},
					{"EC", 0.005960924d},
					{"SI", 0.005957003d},
					{"LL", 0.005697536d},
					{"SO", 0.005527966d},
					{"NA", 0.005445612d},
					{"LI", 0.005386327d},
					{"LA", 0.005360229d},
					{"EL", 0.005340325d},
					{"MA", 0.005048042d},
					{"DI", 0.00501234d},
					{"IC", 0.004964796d},
					{"RT", 0.004961939d},
					{"NS", 0.004927334d},
					{"RS", 0.004911339d},
					{"IO", 0.004905072d},
					{"OM", 0.00487177d},
					{"CH", 0.00465591d},
					{"OT", 0.004645572d},
					{"CA", 0.004609196d},
					{"CE", 0.004579795d},
					{"HO", 0.004562545d},
					{"BE", 0.004502293d},
					{"TT", 0.004478931d},
					{"FO", 0.004376321d},
					{"TS", 0.004376032d},
					{"SS", 0.004374453d},
					{"NO", 0.004369462d},
					{"EE", 0.004277843d},
					{"EM", 0.00419629d},
					{"AC", 0.004140646d},
					{"IL", 0.004134383d},
					{"DA", 0.004066497d},
					{"NI", 0.004035982d},
					{"UR", 0.004010454d},
					{"WA", 0.003894148d},
					{"SH", 0.003878962d},
					{"EI", 0.003706392d},
					{"AM", 0.003694613d},
					{"TR", 0.003658825d},
					{"DT", 0.00364459d},
					{"US", 0.00363064d},
					{"LO", 0.003606811d},
					{"PE", 0.003601493d},
					{"UN", 0.003523878d},
					{"NC", 0.003518541d},
					{"WI", 0.00351817d},
					{"UT", 0.003500629d},
					{"AD", 0.003440517d},
					{"EW", 0.003417199d},
					{"OW", 0.003378815d},
					{"GE", 0.003335938d},
					{"EP", 0.003243284d},
					{"AI", 0.003231847d},
					{"LY", 0.003177989d},
					{"OL", 0.003174395d},
					{"FT", 0.003167362d},
					{"OS", 0.003144279d},
					{"EO", 0.00312761d},
					{"EF", 0.003064717d},
					{"PR", 0.003050599d},
					{"WE", 0.003049197d},
					{"DO", 0.003034212d},
					{"MO", 0.002995001d},
					{"ID", 0.002982517d},
					{"IE", 0.002892039d},
					{"MI", 0.002814196d},
					{"PA", 0.002791016d},
					{"FI", 0.0027737d},
					{"PO", 0.002756055d},
					{"CT", 0.002749399d},
					{"WH", 0.00274111d},
					{"IR", 0.002701436d},
					{"AY", 0.002664911d},
					{"GA", 0.002599319d},
					{"SC", 0.002497761d},
					{"KE", 0.002463079d},
					{"EV", 0.002445351d},
					{"SP", 0.002444568d},
					{"IM", 0.002438508d},
					{"OP", 0.002418859d},
					{"DS", 0.002412021d},
					{"LD", 0.002369398d},
					{"UL", 0.002352721d},
					{"OO", 0.002351655d},
					{"SU", 0.002319775d},
					{"IA", 0.00231307d},
					{"GH", 0.002284946d},
					{"PL", 0.00226918d},
					{"EB", 0.002252199d},
					{"IG", 0.002204045d},
					{"VI", 0.002169232d},
					{"IV", 0.002111231d},
					{"WO", 0.002106008d},
					{"YO", 0.00210181d},
					{"RD", 0.002087273d},
					{"TW", 0.00206059d},
					{"BA", 0.002050694d},
					{"AG", 0.002037235d},
					{"RY", 0.002032442d},
					{"AB", 0.002029446d},
					{"LS", 0.002006289d},
					{"SW", 0.002005776d},
					{"AP", 0.001978182d},
					{"FE", 0.001972488d},
					{"TU", 0.00196051d},
					{"CI", 0.001953246d},
					{"FA", 0.001932859d},
					{"HT", 0.001931384d},
					{"FR", 0.001928568d},
					{"AV", 0.001916892d},
					{"EG", 0.001916332d},
					{"GO", 0.001893725d},
					{"BO", 0.001889952d},
					{"BU", 0.001876279d},
					{"TY", 0.001852146d},
					{"MP", 0.001811966d},
					{"OC", 0.001768438d},
					{"OD", 0.001759942d},
					{"EH", 0.001748131d},
					{"YS", 0.001743617d},
					{"EY", 0.001741008d},
					{"RM", 0.001706237d},
					{"OV", 0.001699768d},
					{"GT", 0.0016993d},
					{"YA", 0.001674222d},
					{"CK", 0.001666253d},
					{"GI", 0.001642676d},
					{"RN", 0.001633771d},
					{"GR", 0.001616502d},
					{"RC", 0.001612825d},
					{"BL", 0.001605189d},
					{"LT", 0.001576566d},
					{"YT", 0.001552718d},
					{"OA", 0.001515732d},
					{"YE", 0.001503033d},
					{"OB", 0.001436709d},
					{"DB", 0.001412243d},
					{"FF", 0.00140734d},
					{"SF", 0.001404675d},
					{"RR", 0.001363561d},
					{"DU", 0.00135549d},
					{"KI", 0.001344631d},
					{"UC", 0.001327987d},
					{"IF", 0.001327531d},
					{"AF", 0.001318779d},
					{"DR", 0.001318619d},
					{"CL", 0.001314301d},
					{"EX", 0.001306475d},
					{"SM", 0.001290608d},
					{"PI", 0.001285626d},
					{"SB", 0.001284348d},
					{"CR", 0.001275251d},
					{"TL", 0.001249532d},
					{"OI", 0.001234149d},
					{"RU", 0.001232747d},
					{"UP", 0.001227287d},
					{"BY", 0.001209972d},
					{"TC", 0.001201819d},
					{"NN", 0.001198137d},
					{"AK", 0.001188057d},
					{"SL", 0.001148211d},
					{"NF", 0.001144817d},
					{"UE", 0.001139614d},
					{"DW", 0.001134752d},
					{"AU", 0.001129515d},
					{"PP", 0.001127023d},
					{"UG", 0.001117526d},
					{"RL", 0.001110801d},
					{"RG", 0.001074422d},
					{"BR", 0.001068673d},
					{"CU", 0.001064734d},
					{"UA", 0.001061485d},
					{"DH", 0.001060506d},
					{"RK", 0.001038683d},
					{"YI", 0.001031703d},
					{"LU", 0.001018226d},
					{"UM", 0.001015169d},
					{"BI", 0.001007478d},
					{"NY", 0.001004431d},
					{"NW", 0.000974987d},
					{"QU", 0.000964223d},
					{"OG", 0.000962767d},
					{"SN", 0.000961579d},
					{"MB", 0.000953201d},
					{"VA", 0.000950799d},
					{"DF", 0.000932877d},
					{"DD", 0.000925337d},
					{"MS", 0.000907201d},
					{"GS", 0.000906697d},
					{"AW", 0.000906301d},
					{"NH", 0.00090548d},
					{"PU", 0.000892237d},
					{"HR", 0.000888734d},
					{"SD", 0.000888561d},
					{"TB", 0.000882365d},
					{"PT", 0.000881675d},
					{"NM", 0.000878079d},
					{"DC", 0.000874738d},
					{"GU", 0.000871489d},
					{"TM", 0.000869507d},
					{"MU", 0.000868576d},
					{"NU", 0.000863203d},
					{"MM", 0.000862719d},
					{"NL", 0.000854042d},
					{"EU", 0.000849681d},
					{"WN", 0.000844012d},
					{"NB", 0.00083316d},
					{"RP", 0.000829806d},
					{"DM", 0.000819797d},
					{"SR", 0.000812605d},
					{"UD", 0.000809304d},
					{"UI", 0.000805129d},
					{"RF", 0.000794665d},
					{"OK", 0.000785724d},
					{"YW", 0.000781444d},
					{"TF", 0.00077899d},
					{"IP", 0.000774404d},
					{"RW", 0.000774261d},
					{"RB", 0.000773847d},
					{"OH", 0.000752674d},
					{"KS", 0.000746355d},
					{"DP", 0.000727324d},
					{"FU", 0.000725904d},
					{"YC", 0.000723395d},
					{"TP", 0.000710068d},
					{"MT", 0.00070672d},
					{"DL", 0.000705563d},
					{"NK", 0.000703772d},
					{"CC", 0.000699908d},
					{"UB", 0.00069167d},
					{"RH", 0.000686544d},
					{"NP", 0.00068641d},
					{"JU", 0.000676394d},
					{"FL", 0.000668537d},
					{"DN", 0.000656901d},
					{"KA", 0.00065517d},
					{"PH", 0.00065339d},
					{"HU", 0.000641015d},
					{"JO", 0.00062934d},
					{"LF", 0.000624987d},
					{"YB", 0.00062366d},
					{"RV", 0.000622656d},
					{"OE", 0.000605049d},
					{"IB", 0.000600917d},
					{"IK", 0.000597837d},
					{"YP", 0.000597083d},
					{"GL", 0.000595909d},
					{"LP", 0.000588317d},
					{"YM", 0.000581915d},
					{"LB", 0.000569755d},
					{"HS", 0.000569369d},
					{"DG", 0.00056477d},
					{"GN", 0.000561137d},
					{"EK", 0.000557717d},
					{"NR", 0.000553541d},
					{"PS", 0.000549715d},
					{"TD", 0.000542656d},
					{"LC", 0.000538389d},
					{"SK", 0.000536961d},
					{"YF", 0.000533112d},
					{"YH", 0.000529881d},
					{"VO", 0.000521097d},
					{"AH", 0.000514617d},
					{"DY", 0.000512945d},
					{"LM", 0.000512592d},
					{"SY", 0.000512073d},
					{"NV", 0.000507509d},
					{"YD", 0.000490813d},
					{"FS", 0.000473486d},
					{"SG", 0.000472643d},
					{"YR", 0.000467595d},
					{"YL", 0.000465745d},
					{"WS", 0.000459914d},
					{"MY", 0.000450757d},
					{"OY", 0.000447002d},
					{"KN", 0.000440282d},
					{"IZ", 0.000431486d},
					{"XP", 0.00042568d},
					{"LW", 0.000424782d},
					{"TN", 0.000412134d},
					{"KO", 0.000406556d},
					{"AA", 0.000398032d},
					{"JA", 0.000396094d},
					{"ZE", 0.000395426d},
					{"FC", 0.000363262d},
					{"GW", 0.000362614d},
					{"TG", 0.000353839d},
					{"XT", 0.000349196d},
					{"FH", 0.000348649d},
					{"LR", 0.000348068d},
					{"JE", 0.000343965d},
					{"YN", 0.000343573d},
					{"GG", 0.000339557d},
					{"GF", 0.000338864d},
					{"EQ", 0.000337972d},
					{"HY", 0.000334507d},
					{"KT", 0.000333937d},
					{"HC", 0.00033326d},
					{"BS", 0.000326001d},
					{"HW", 0.00032451d},
					{"HN", 0.000320055d},
					{"CS", 0.000319511d},
					{"HM", 0.000312896d},
					{"NJ", 0.000310522d},
					{"HH", 0.000307576d},
					{"WT", 0.000300938d},
					{"GC", 0.000300533d},
					{"LH", 0.000294637d},
					{"EJ", 0.000290693d},
					{"FM", 0.000289379d},
					{"DV", 0.000286431d},
					{"LV", 0.000286367d},
					{"WR", 0.0002837d},
					{"GP", 0.000281029d},
					{"FP", 0.000277477d},
					{"GB", 0.0002739d},
					{"GM", 0.000272543d},
					{"HL", 0.000270452d},
					{"LK", 0.00026923d},
					{"CY", 0.000264866d},
					{"MC", 0.000254786d},
					{"YG", 0.000242611d},
					{"XI", 0.000236981d},
					{"HB", 0.000234499d},
					{"FW", 0.000232626d},
					{"GY", 0.00022659d},
					{"HP", 0.000226323d},
					{"MW", 0.000216835d},
					{"PM", 0.000215356d},
					{"ZA", 0.000214869d},
					{"LG", 0.000214256d},
					{"IW", 0.000213236d},
					{"XA", 0.000209094d},
					{"FB", 0.000205395d},
					{"SV", 0.000203991d},
					{"GD", 0.000203461d},
					{"IX", 0.000203361d},
					{"AJ", 0.000201257d},
					{"KL", 0.000195718d},
					{"HF", 0.000192937d},
					{"HD", 0.000191658d},
					{"AE", 0.0001887d},
					{"SQ", 0.000185088d},
					{"DJ", 0.000184862d},
					{"FY", 0.000182687d},
					{"AZ", 0.000177691d},
					{"LN", 0.000173981d},
					{"AO", 0.000173345d},
					{"FD", 0.000172989d},
					{"KW", 0.000166423d},
					{"MF", 0.000165371d},
					{"MH", 0.000164395d},
					{"SJ", 0.00016291d},
					{"UF", 0.00016232d},
					{"TV", 0.000161455d},
					{"XC", 0.000161419d},
					{"YU", 0.000160844d},
					{"BB", 0.000159375d},
					{"WW", 0.000156011d},
					{"OJ", 0.000152882d},
					{"AX", 0.000152823d},
					{"MR", 0.000152775d},
					{"WL", 0.000152119d},
					{"XE", 0.000151232d},
					{"KH", 0.000150341d},
					{"OX", 0.000150337d},
					{"UO", 0.000150298d},
					{"ZI", 0.00014894d},
					{"FG", 0.000147488d},
					{"IH", 0.000141227d},
					{"TK", 0.000141146d},
					{"II", 0.000140404d},
					{"IU", 0.000133364d},
					{"TJ", 0.000129384d},
					{"MN", 0.000129135d},
					{"WY", 0.000128037d},
					{"KY", 0.000127956d},
					{"KF", 0.000124266d},
					{"FN", 0.000123577d},
					{"UY", 0.000123021d},
					{"PW", 0.000122663d},
					{"DK", 0.000121584d},
					{"RJ", 0.000119829d},
					{"UK", 0.00011907d},
					{"KR", 0.000117254d},
					{"KU", 0.000117161d},
					{"WM", 0.000116945d},
					{"KM", 0.000112304d},
					{"MD", 0.000111265d},
					{"ML", 0.000110665d},
					{"EZ", 0.000107644d},
					{"KB", 0.000105885d},
					{"WC", 0.000103696d},
					{"WD", 0.000100054d},
					{"HG", 9.93511E-05d},
					{"BT", 9.90433E-05d},
					{"ZO", 9.80582E-05d},
					{"KC", 9.71333E-05d},
					{"PF", 9.67057E-05d},
					{"YV", 9.51607E-05d},
					{"PC", 9.25754E-05d},
					{"PY", 9.16132E-05d},
					{"WB", 9.13063E-05d},
					{"YK", 9.06432E-05d},
					{"CP", 8.8555E-05d},
					{"YJ", 8.75735E-05d},
					{"KP", 8.68737E-05d},
					{"PB", 8.54128E-05d},
					{"CD", 8.28919E-05d},
					{"JI", 8.26934E-05d},
					{"UW", 8.1573E-05d},
					{"UH", 7.84762E-05d},
					{"WF", 7.77528E-05d},
					{"YY", 7.70035E-05d},
					{"WP", 7.44071E-05d},
					{"BC", 7.40912E-05d},
					{"AQ", 7.28628E-05d},
					{"CB", 6.89279E-05d},
					{"IQ", 6.74437E-05d},
					{"CM", 6.61271E-05d},
					{"MG", 6.594E-05d},
					{"DQ", 6.55193E-05d},
					{"BJ", 6.53561E-05d},
					{"TZ", 6.47546E-05d},
					{"KD", 6.42863E-05d},
					{"PD", 6.31716E-05d},
					{"FJ", 6.24091E-05d},
					{"CF", 6.18922E-05d},
					{"NZ", 6.16219E-05d},
					{"CW", 5.94925E-05d},
					{"FV", 5.6586E-05d},
					{"VY", 5.39027E-05d},
					{"FK", 5.29367E-05d},
					{"OZ", 5.2856E-05d},
					{"ZZ", 5.11722E-05d},
					{"IJ", 5.06757E-05d},
					{"LJ", 5.04985E-05d},
					{"NQ", 5.02811E-05d},
					{"UV", 4.9039E-05d},
					{"XO", 4.8836E-05d},
					{"PG", 4.88267E-05d},
					{"HK", 4.86537E-05d},
					{"KG", 4.8395E-05d},
					{"VS", 4.71987E-05d},
					{"HV", 4.5683E-05d},
					{"BM", 4.43574E-05d},
					{"HJ", 4.39178E-05d},
					{"CN", 4.34876E-05d},
					{"GV", 4.31941E-05d},
					{"CG", 4.19946E-05d},
					{"WU", 4.18313E-05d},
					{"GJ", 4.09209E-05d},
					{"XH", 3.85278E-05d},
					{"GK", 3.78874E-05d},
					{"TQ", 3.67961E-05d},
					{"CQ", 3.64342E-05d},
					{"RQ", 3.62924E-05d},
					{"BH", 3.57272E-05d},
					{"XS", 3.56944E-05d},
					{"UZ", 3.55531E-05d},
					{"WK", 3.44495E-05d},
					{"XU", 3.41186E-05d},
					{"UX", 3.34898E-05d},
					{"BD", 3.27816E-05d},
					{"BW", 3.24202E-05d},
					{"WG", 3.2351E-05d},
					{"MV", 3.1524E-05d},
					{"MJ", 3.10497E-05d},
					{"PN", 3.04443E-05d},
					{"XM", 2.94839E-05d},
					{"OQ", 2.83703E-05d},
					{"BV", 2.777E-05d},
					{"XW", 2.75945E-05d},
					{"KK", 2.74763E-05d},
					{"BP", 2.66322E-05d},
					{"ZU", 2.62569E-05d},
					{"RZ", 2.62323E-05d},
					{"XF", 2.61396E-05d},
					{"MK", 2.56794E-05d},
					{"ZH", 2.48926E-05d},
					{"BN", 2.45425E-05d},
					{"ZY", 2.44838E-05d},
					{"HQ", 2.3413E-05d},
					{"WJ", 2.29954E-05d},
					{"IY", 2.2747E-05d},
					{"DZ", 2.26723E-05d},
					{"VR", 2.22972E-05d},
					{"ZS", 2.19681E-05d},
					{"XY", 2.18146E-05d},
					{"CV", 2.17903E-05d},
					{"XB", 2.1748E-05d},
					{"XR", 2.08241E-05d},
					{"UJ", 2.03898E-05d},
					{"YQ", 2.03401E-05d},
					{"VD", 1.97984E-05d},
					{"PK", 1.91986E-05d},
					{"VU", 1.91553E-05d},
					{"JR", 1.86098E-05d},
					{"ZL", 1.85099E-05d},
					{"SZ", 1.84638E-05d},
					{"YZ", 1.81033E-05d},
					{"LQ", 1.78413E-05d},
					{"KJ", 1.77645E-05d},
					{"BF", 1.74259E-05d},
					{"NX", 1.73085E-05d},
					{"QA", 1.70039E-05d},
					{"QI", 1.69715E-05d},
					{"KV", 1.69246E-05d},
					{"ZW", 1.59258E-05d},
					{"WV", 1.47845E-05d},
					{"UU", 1.45794E-05d},
					{"VT", 1.45491E-05d},
					{"VP", 1.44716E-05d},
					{"XD", 1.3899E-05d},
					{"GQ", 1.38178E-05d},
					{"XL", 1.37797E-05d},
					{"VC", 1.36499E-05d},
					{"CZ", 1.33932E-05d},
					{"LZ", 1.32545E-05d},
					{"ZT", 1.31714E-05d},
					{"WZ", 1.22189E-05d},
					{"SX", 1.17885E-05d},
					{"ZB", 1.17138E-05d},
					{"VL", 1.13392E-05d},
					{"PV", 1.11248E-05d},
					{"FQ", 1.09858E-05d},
					{"PJ", 1.08792E-05d},
					{"ZM", 1.06458E-05d},
					{"VW", 1.05473E-05d},
					{"CJ", 9.60332E-06d},
					{"ZC", 9.49024E-06d},
					{"BG", 9.36975E-06d},
					{"JS", 9.09455E-06d},
					{"XG", 9.08599E-06d},
					{"RX", 8.93914E-06d},
					{"HZ", 8.5719E-06d},
					{"XX", 8.10614E-06d},
					{"VM", 8.09967E-06d},
					{"XN", 8.0326E-06d},
					{"QW", 8.01757E-06d},
					{"JP", 7.98311E-06d},
					{"VN", 7.65056E-06d},
					{"ZD", 7.60986E-06d},
					{"ZR", 7.55875E-06d},
					{"FZ", 7.21209E-06d},
					{"XV", 7.19613E-06d},
					{"ZP", 7.02778E-06d},
					{"VH", 6.98476E-06d},
					{"VB", 6.75096E-06d},
					{"ZF", 6.62746E-06d},
					{"GZ", 6.59416E-06d},
					{"TX", 6.51137E-06d},
					{"VF", 6.49611E-06d},
					{"DX", 6.33954E-06d},
					{"QB", 6.31503E-06d},
					{"BK", 6.24241E-06d},
					{"ZG", 6.09811E-06d},
					{"VG", 5.9168E-06d},
					{"JC", 5.72832E-06d},
					{"ZK", 5.61084E-06d},
					{"ZN", 5.60599E-06d},
					{"UQ", 5.40826E-06d},
					{"JM", 5.1659E-06d},
					{"VV", 5.16382E-06d},
					{"JD", 5.0653E-06d},
					{"MQ", 4.93926E-06d},
					{"JH", 4.84722E-06d},
					{"QS", 4.82109E-06d},
					{"JT", 4.71956E-06d},
					{"JB", 4.48183E-06d},
					{"FX", 4.46633E-06d},
					{"PQ", 4.30306E-06d},
					{"MZ", 4.22536E-06d},
					{"YX", 3.91871E-06d},
					{"QT", 3.91154E-06d},
					{"WQ", 3.75683E-06d},
					{"JJ", 3.71983E-06d},
					{"JW", 3.71936E-06d},
					{"LX", 3.57691E-06d},
					{"GX", 3.41757E-06d},
					{"JN", 3.34218E-06d},
					{"ZV", 3.31604E-06d},
					{"MX", 3.29546E-06d},
					{"JK", 3.23002E-06d},
					{"KQ", 3.21568E-06d},
					{"XK", 3.15694E-06d},
					{"JF", 2.92313E-06d},
					{"QM", 2.84797E-06d},
					{"QH", 2.83826E-06d},
					{"JL", 2.80958E-06d},
					{"JG", 2.78045E-06d},
					{"VK", 2.65233E-06d},
					{"VJ", 2.64377E-06d},
					{"KZ", 2.58827E-06d},
					{"QC", 2.46686E-06d},
					{"XJ", 2.45807E-06d},
					{"PZ", 2.24253E-06d},
					{"QL", 2.22079E-06d},
					{"QO", 2.17246E-06d},
					{"JV", 2.064E-06d},
					{"QF", 2.03E-06d},
					{"QD", 2.00688E-06d},
					{"BZ", 1.88061E-06d},
					{"HX", 1.74047E-06d},
					{"ZJ", 1.65744E-06d},
					{"PX", 1.57581E-06d},
					{"QP", 1.4019E-06d},
					{"QE", 1.39219E-06d},
					{"QR", 1.38178E-06d},
					{"ZQ", 1.33507E-06d},
					{"JY", 1.3235E-06d},
					{"BQ", 1.27494E-06d},
					{"XQ", 1.25251E-06d},
					{"CX", 1.22568E-06d},
					{"KX", 1.1755E-06d},
					{"WX", 1.08184E-06d},
					{"QY", 1.05385E-06d},
					{"QV", 9.74069E-07d},
					{"QN", 8.8064E-07d},
					{"VX", 7.38184E-07d},
					{"BX", 6.98638E-07d},
					{"JZ", 6.61174E-07d},
					{"VZ", 6.08909E-07d},
					{"QG", 5.93646E-07d},
					{"QQ", 5.7792E-07d},
					{"ZX", 5.69595E-07d},
					{"XZ", 4.81484E-07d},
					{"QK", 4.6784E-07d},
					{"VQ", 3.44116E-07d},
					{"QJ", 3.10352E-07d},
					{"QX", 1.76914E-07d},
					{"JX", 1.72752E-07d},
					{"JQ", 1.6697E-07d},
					{"QZ", 6.47529E-08d },
				})
			}
		}
	);
}
