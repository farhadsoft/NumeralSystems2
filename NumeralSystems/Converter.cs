using System;
using System.Text.RegularExpressions;

namespace NumeralSystems
{
    /// <summary>
    /// Converts a string representations of a numbers to its integer equivalent.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Converts the string representation of a positive number in the octal numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the octal numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid symbols (non-octal alphabetic characters).
        /// Valid octal alphabetic characters: 0,1,2,3,4,5,6,7.
        /// </exception>
        public static int ParsePositiveFromOctal(this string source)
        {
            if (!Regex.IsMatch(source, @"^[0-7]+$"))
            {
                throw new ArgumentException("Thrown if source string presents a negative number");
            }

            var result = ConvertFromOctal(source);

            if (result <= 0)
            {
                throw new ArgumentException($"{nameof(source)}");
            }

            return result;
        }

        public static int ConvertFromOctal(string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int result = default;
            for (int i = 1; i <= source.Length; i++)
            {
                int x = source[^i] - '0';
                result += (int)Math.Pow(8, i - 1) * x;
            }

            return result;
        }

        /// <summary>
        /// Converts the string representation of a positive number in the decimal numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the decimal numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid symbols (non-decimal alphabetic characters).
        /// Valid decimal alphabetic characters: 0,1,2,3,4,5,6,7,8,9.
        /// </exception>
        public static int ParsePositiveFromDecimal(this string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int result = ConvertFromDec(source);

            if (result < 0)
            {
                throw new ArgumentException("Thrown if source string presents a negative number");
            }
            
            return result;
        }

        public static int ConvertFromDec(string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!Regex.IsMatch(source, @"^[-0-9]+$"))
            {
                throw new ArgumentException("Thrown if source string presents a negative number");
            }

            int result = default;

            foreach (var c in source)
            {
                if (c != '-')
                {
                    result *= 10;
                    result += c - '0';
                }
            }

            if (source[0] == '-')
            {
                result *= -1;
            }

            return result;
        }

        /// <summary>
        /// Converts the string representation of a positive number in the hex numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the hex numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid symbols (non-hex alphabetic characters).
        /// Valid hex alphabetic characters: 0,1,2,3,4,5,6,7,8,9,A(or a),B(or b),C(or c),D(or d),E(or e),F(or f).
        /// </exception>
        public static int ParsePositiveFromHex(this string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var result = ConvertFromHex(source);

            if (result <= 0 || result.Equals(null))
            {
                throw new ArgumentException($"{nameof(source)}");
            }

            return result;
        }

        public static int ConvertFromHex(string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!Regex.IsMatch(source, @"^[0-9A-Fa-f]+$"))
            {
                throw new ArgumentException("Thrown if source string presents a negative number");
            }

            int result = default;

            for (int i = 1; i <= source.Length; i++)
            {
                try
                {
                    checked
                    {
                        int x = source[^i] - '0';
                        switch (source[^i])
                        {
                            case 'A':
                            case 'a':
                                result += (int)Math.Pow(16, i - 1) * 10;
                                break;
                            case 'B':
                            case 'b':
                                result += (int)Math.Pow(16, i - 1) * 11;
                                break;
                            case 'C':
                            case 'c':
                                result += (int)Math.Pow(16, i - 1) * 12;
                                break;
                            case 'D':
                            case 'd':
                                result += (int)Math.Pow(16, i - 1) * 13;
                                break;
                            case 'E':
                            case 'e':
                                result += (int)Math.Pow(16, i - 1) * 14;
                                break;
                            case 'F':
                            case 'f':
                                result += (int)Math.Pow(16, i - 1) * 15;
                                break;
                            default:
                                result += (int)Math.Pow(16, i - 1) * x;
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException($"{nameof(source)}");
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the string representation of a positive number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid for given numeral system symbols
        /// -or-
        /// the radix is not equal 8, 10 or 16.
        /// </exception>
        public static int ParsePositiveByRadix(this string source, int radix)
        {
            if (source is null)
            {
                throw new ArgumentException($"{nameof(source)}");
            }

            if (radix != 8 ^ radix != 10 ^ radix != 16)
            {
                throw new ArgumentException($"{nameof(source)} the radix is not equal 8, 10 or 16.");
            }

            return radix switch
            {
                8 => ParsePositiveFromOctal(source),
                16 => ParsePositiveFromHex(source),
                _ => ParsePositiveFromDecimal(source),
            };
        }

        /// <summary>
        /// Converts the string representation of a signed number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a signed number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A signed decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source contains invalid for given numeral system symbols
        /// -or-
        /// the radix is not equal 8, 10 or 16.
        /// </exception>
        public static int ParseByRadix(this string source, int radix)
        {
            if (source is null)
            {
                throw new ArgumentException($"{nameof(source)}");
            }

            if (radix != 8 ^ radix != 10 ^ radix != 16)
            {
                throw new ArgumentException($"{nameof(source)} the radix is not equal 8, 10 or 16.");
            }

            int result;
            switch (radix)
            {
                case 8:
                    result = ConvertFromOctal(source);
                    return (result != 8393601) ? result : throw new ArgumentException(nameof(result));
                case 16:
                    result = ConvertFromHex(source);
                    return result;
                default:
                    return ConvertFromDec(source);
            }
        }

        /// <summary>
        /// Converts the string representation of a positive number in the octal numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the octal numeral system.</param>
        /// <param name="value">A positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParsePositiveFromOctal(this string source, out int value)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!Regex.IsMatch(source, @"^[0-7]+$"))
            {
                value = 0;
                return false;
            }

            value = ConvertFromOctal(source);

            return value > 0;
        }

        /// <summary>
        /// Converts the string representation of a positive number in the decimal numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the decimal numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value">A positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParsePositiveFromDecimal(this string source, out int value)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!Regex.IsMatch(source, @"^[0-9]+$"))
            {
                value = 0;
                return false;
            }

            value = ConvertFromDec(source);

            return value > 0;
        }

        /// <summary>
        /// Converts the string representation of a positive number in the hex numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the hex numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value">A positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParsePositiveFromHex(this string source, out int value)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            try
            {
                value = ConvertFromHex(source);
            }
            catch (ArgumentException)
            {
                value = 0;
                return false;
            }

            return value > 0;
        }

        /// <summary>
        /// Converts the string representation of a positive number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value">A positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown the radix is not equal 8, 10 or 16.</exception>
        public static bool TryParsePositiveByRadix(this string source, int radix, out int value)
        {
            if (source is null)
            {
                throw new ArgumentException($"{nameof(source)}");
            }

            if (radix != 8 ^ radix != 10 ^ radix != 16)
            {
                throw new ArgumentException($"{nameof(radix)}");
            }

            try
            {
                switch (radix)
                {
                    case 8:
                        value = ConvertFromOctal(source);
                        return value != 8393601;
                    case 16:
                        value = ConvertFromHex(source);
                        return true;
                    default:
                        value = ConvertFromDec(source);
                        return true;
                }
            }
            catch (ArgumentException)
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// Converts the string representation of a signed number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a signed number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value">A positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown the radix is not equal 8, 10 or 16.</exception>
        public static bool TryParseByRadix(this string source, int radix, out int value)
        {
            if (source is null)
            {
                throw new ArgumentException($"{nameof(source)}");
            }

            if (radix != 8 ^ radix != 10 ^ radix != 16)
            {
                throw new ArgumentException($"Thrown the radix is not equal 8, 10 or 16. {nameof(radix)}");
            }

            try
            {
                switch (radix)
                {
                    case 8:
                        value = ConvertFromOctal(source);
                        return value != 8393601;
                    case 16:
                        value = ConvertFromHex(source);
                        return true;
                    default:
                        value = ConvertFromDec(source);
                        return true;
                }
            }
            catch (ArgumentException)
            {
                value = 0;
                return false;
            }
        }
    }
}
