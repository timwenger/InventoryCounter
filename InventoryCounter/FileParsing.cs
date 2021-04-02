using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryCounter
{
    class FileParsing
    {
        /// <summary>
        /// Parses a dollar value from a string.
        /// </summary>
        /// <param name="parsableStr">the string to being parsing. Modifies the caller's variable to 
        /// contain any leftover string which was not part of the parsed value.
        /// returns the original string if parse is unsuccessful.</param>
        /// <param name="value"> parsed value. Zero if parse is unsuccessful.</param>
        /// <returns>true if it parsed successfully</returns>
        public static bool ParseValue(ref string parsableStr, out float? value)
        {
            string worth = string.Empty;
            int indexOfSpaceCharAfterPrice = parsableStr.IndexOf(" ");
            bool parseResult = true;

            if (indexOfSpaceCharAfterPrice > 0)
            {
                worth = parsableStr[0..indexOfSpaceCharAfterPrice]; // range instead of substring()
                parseResult = IsADollarAmount(worth);
            }
            else
                parseResult = false;

            if (parseResult)
            {
                value = float.Parse(worth.Substring(1));
                parsableStr = parsableStr.Substring(indexOfSpaceCharAfterPrice + 1);
            }
            else
                value = 0f;
            return parseResult;
        }



        const int PeriodAsciiNumber = 46;
        const int DollarAsciiNumber = 36;
        private static bool IsADollarAmount(string value)
        {
            //must have no decimals, or exactly 2 decimals.
            //the only character other than numerics is a "."
            bool foundPeriod = false;
            int numDecimalDigits = 0;
            if ((value.Length <= 1) ||
                (value[0] != DollarAsciiNumber))
                return false;

            for (int i = 1; i < value.Length; i++)
            {
                bool isDigit = false;
                int asciiVal = value[i];

                if (asciiVal == PeriodAsciiNumber)
                {
                    if (i == 1) // first char after '$' cannot be a decimal.
                        return false;
                    foundPeriod = true;
                }
                else if (IsADigit(asciiVal))
                    isDigit = true;
                else // if not a decimal or a digit, the value is not a dollar amount
                    return false;

                if (foundPeriod && isDigit)
                    numDecimalDigits++;
            }
            if (foundPeriod && numDecimalDigits != 2)
                return false;

            return true;
        }

        const int FirstAsciiNumber = 48;
        const int LastAsciiNumber = 57;
        private static bool IsADigit(int asciiVal)
        {
            return asciiVal >= FirstAsciiNumber && asciiVal <= LastAsciiNumber;
        }

        /// <summary>
        /// Parses a date from a string in the format yyyy,mm,dd.
        /// </summary>
        /// <param name="parsableStr">the string to being parsing. Modifies the caller's variable to 
        /// contain any leftover string which was not part of the parsed value.
        /// returns the original string if parse is unsuccessful.</param>
        /// <param name="date"> parsed date. String.Empty if unsuccessful.</param>
        /// <returns>true if it parsed successfully</returns>
        public static bool ParseDate(ref string parsableStr, out string date)
        {

            int indexOfSpaceCharAfterPrice = parsableStr.IndexOf(" ");
            bool parseResult = true;

            if (indexOfSpaceCharAfterPrice > 0)
            {
                date = parsableStr[0..indexOfSpaceCharAfterPrice]; // range instead of substring()
                parseResult = IsADate(date);
            }
            else
            {
                parseResult = false;
                date = string.Empty;
            }

            if (parseResult)
            {
                parsableStr = parsableStr.Substring(indexOfSpaceCharAfterPrice + 1);
            }
            return parseResult;
        }

        const int CommaAsciiNumber = 44;
        private static bool IsADate(string date)
        {
            if (date.Length != 10) return false;
            bool parseResult = IsAYear(date.Substring(0, 4));
            parseResult &= date[4] == CommaAsciiNumber;
            parseResult &= IsAMonth(date.Substring(5, 2));
            parseResult &= date[7] == CommaAsciiNumber;
            parseResult &= IsADay(date.Substring(8, 2));
            return parseResult;
        }

        private static bool IsAYear(string year)
        {
            if (year.Length != 4) return false;
            if (!int.TryParse(year, out int yearInt)) return false;
            if (yearInt > 2100 || yearInt < 1900) return false;
            return true;
        }

        private static bool IsAMonth(string month)
        {
            if (month.Length != 2) return false;
            if (!int.TryParse(month, out int monthInt)) return false;
            if (monthInt > 12 || monthInt < 0) return false;
            return true;
        }

        private static bool IsADay(string day)
        {
            if (day.Length != 2) return false;
            if (!int.TryParse(day, out int dayInt)) return false;
            if (dayInt > 31 || dayInt < 0) return false;
            return true;
        }
    }
}
