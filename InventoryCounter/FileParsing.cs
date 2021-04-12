using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryCounter
{
    class FileParsing
    {        
        /// <summary>
        /// Parses a dollar value from a string.
        /// The dollar amount must have either no decimals, or exactly 2 decimals. 
        /// The first character must be $, then the only character allowed other than numerics is a "."
        /// </summary>
        /// <param name="parsableStr">the string to being parsing. Modifies the caller's variable to 
        /// contain any leftover string which was not part of the parsed value.
        /// returns the original string if parse is unsuccessful.</param>
        /// <param name="value"> parsed value. Zero if parse is unsuccessful.</param>
        /// <returns>true if it parsed successfully</returns>
        public static bool ParseValue(ref string parsableStr, out float? value)
        {
            string workingStr = parsableStr.TrimStart();
            bool foundPeriod = false;
            value = 0f;
            int numDecimalDigits = 0;

            if ((workingStr.Length <= 1) ||
                (workingStr[0] != DollarAsciiNumber))
                return false;

            int parseLocation;
            for (parseLocation = 1; parseLocation < workingStr.Length; parseLocation++)
            {
                bool isDigit = false;
                int asciiVal = workingStr[parseLocation];

                if (asciiVal == PeriodAsciiNumber)
                {
                    if (parseLocation == 1) // first char after '$' must be a digit.
                        return false;
                    if (foundPeriod) // if already found a period, we have reached the end of the dollar amount
                        break;
                    foundPeriod = true;
                }
                else if (IsADigit(asciiVal))
                    isDigit = true;
                else // if not a decimal or a digit, the char is not part of a dollar amount.
                {
                    if (parseLocation == 1) // first char after '$' must be a digit.
                        return false;
                    break; // reached the end of the dollar amount
                }

                if (foundPeriod && isDigit)
                    numDecimalDigits++;
            }
            // at this point, we've concluded that:
            // the string is at least 2 chars long,
            // the first char is a $
            // the next char is a digit

            // we have either stopped reading the string at the end of the dollar amount, or the end of the string

            if (foundPeriod && numDecimalDigits != 2)
                return false;

            // at this point, we are confident that there is a valid dollar amount in the string, so we can parse it to a float:
            value = float.Parse(workingStr[1..parseLocation]);
            parsableStr = workingStr[parseLocation..];

            return true;
        }

        private const int SpaceAsciiNumber = 32;
        private const int DollarAsciiNumber = 36;
        private const int CommaAsciiNumber = 44;
        private const int PeriodAsciiNumber = 46; 
        private const int FirstAsciiNumber = 48;
        private const int LastAsciiNumber = 57;
        
        
        
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
            string workingStr = parsableStr.TrimStart();
            int strLen = workingStr.Length;
            if (strLen < LengthOfADateString)
            {
                date = string.Empty;
                return false;
            }
            date = workingStr.Substring(0, LengthOfADateString);
                        
            bool parseResult = IsADate(date);

            if (parseResult)
            {
                if (strLen == LengthOfADateString)
                    parsableStr = string.Empty;
                else
                    parsableStr = workingStr[LengthOfADateString..];
            }
            else
                date = string.Empty;
            return parseResult;
        }

        private const int LengthOfADateString = 10;        
        private static bool IsADate(string date)
        {
            if (date.Length != LengthOfADateString) return false;
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
            foreach (int c in year) // ensure no whitespace
                if (!IsADigit(c)) return false;
            if (!int.TryParse(year, out int yearInt)) return false;
            if (yearInt > 2100 || yearInt < 1900) return false;
            return true;
        }

        private static bool IsAMonth(string month)
        {
            if (month.Length != 2) return false;
            foreach (int c in month) // ensure no whitespace
                if (!IsADigit(c)) return false;
            if (!int.TryParse(month, out int monthInt)) return false;
            if (monthInt > 12 || monthInt < 0) return false;
            return true;
        }

        private static bool IsADay(string day)
        {
            if (day.Length != 2) return false;
            foreach(int c in day) // ensure no whitespace
                if (!IsADigit(c)) return false;
            if (!int.TryParse(day, out int dayInt)) return false;
            if (dayInt > 31 || dayInt < 0) return false;
            return true;
        }

        /// <summary>
        /// Parses a description from a string
        /// </summary>
        /// <param name="description"> returns the original string if not parsable. 
        /// Otherwise returns the description without any leading whitespace.</param>
        /// <returns>Returns true if there is a description. That is, if the string is not empty 
        /// after removing leading whitespace.</returns>
        public static bool ParseDescription(ref string description)
        {
            string workingStr = description.TrimStart();
            if (workingStr.Length == 0)
                return false;
            description = workingStr;
            return true;
        }

        public static bool FirstCharIsASpace(string str)
        {
            return str[0] == SpaceAsciiNumber;
        }
    }
}
