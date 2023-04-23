# RomanNumerals
Converting numbers to and from roman numerals as easy as II+II=IV.  
Available as a [![NuGet package](https://img.shields.io/nuget/v/Roman-Numerals.svg?style=flat-square)](https://www.nuget.org/packages/Roman-Numerals) package.

## What is does

There are very few features:  
- Convert integers to roman numerals
- Convert roman numerals to integers
- Use roman numeral ASCII representation (I, II, III, IV)
- Use roman numeral Unicode representation (Ⅰ, Ⅱ, Ⅲ, Ⅳ)
- Use roman numeral vinculum extensions (V̅I̅=6000, I̿I̿=200000, etc.)
- Use roman numeral apostrophus (C|Ɔ=1000, |ƆƆ=5000, CC|ƆƆ=10000, |ƆƆƆ=50000, CCC|ƆƆƆ=100000)
- Use roman numeral apostrophus special characters (ↀ=1000, ↁ=5000, ↂ=10000, ↇ=50000, ↈ=100000)
- Use of positive-only combinations (use ⅠⅠⅠⅠ instead of Ⅳ)

## How it works

### Simple way

To display: `uint` to `string` (representing a Roman numeral)
```csharp
Console.WriteLine(RomanNumerals.Convert.ToRomanNumerals(123)); // CXXIII
Console.WriteLine(RomanNumerals.Convert.ToRomanNumerals(227, NumeralFlags.Unicode)); // ⅭⅭⅩⅩⅦ
```
Parsing: `string` to `uint`
```csharp
Console.WriteLine(RomanNumerals.Convert.FromRomanNumerals("IV")); // 4
```
All methods also works as extension methods:
```csharp
using RomanNumerals;
var a1 = Convert.ToRomanNumerals(123);
var a2 = 123.ToRomanNumerals(); // same as above 😍
```

### With options

#### Formatting
Formatting is done using the `NumeralBuilder` class, with several options.
`NumeralBuilder` implements `ICustomFormatter` with the following flags:

| Flag | Effect |
| :-- | :-- |
| `0` | Don’t use negative digits (turns `4` to `IIII` instead of `IV`) |
| `V` or `-` or `=` | Use vinculum notation (turns `1,000` to `Ī` instead of `M`, but also `1,000,000` to `I̿` or `5,000,000` to `V̿`) |
| `'` or `|` | Use apostrophus notation (turns `1,000` to `(|)` instead of `M`, but also `10,000` to `((|))` or `50,000` to `|)))`) |
| `u` | Use Unicode (subset of Unicode Roman numerals) |
| `U` | Use Unicode plus ligatures (full range of Unicode Roman numerals) |
| `A` | Use ASCII, the default case |

#### Parsing
Parsing is done using the `NumeralParser` class (with less options, because it parses all forms at once).

## Some documentation

Read more about roman numerals at 
* https://en.wikipedia.org/wiki/Roman_numerals for general information
* https://en.wikipedia.org/wiki/Numerals_in_Unicode#Roman_numerals for Unicode information
* https://www.unicode.org/charts/PDF/U2150.pdf for Unicode chart
