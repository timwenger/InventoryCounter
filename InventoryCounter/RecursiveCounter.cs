using System;
using System.Collections.Generic;
using System.IO;

namespace InventoryCounter
{
    public class RecursiveCounter
    {        
        public static FolderSummary CountInventoryWorthForThisFolder(string directory)
        {
            FolderSummary folderSummary = new FolderSummary(Path.GetFileName(directory));

            if (!CountJustFoldersInThisFolder(directory, folderSummary))
                return null;
            CountJustPicturesInThisFolder(directory, folderSummary);

            CsvFileCreator csvMaker = new CsvFileCreator(directory, folderSummary.Folders, folderSummary.Pics);
            if (!csvMaker.CreateFolderInventoryCsvFile())
                return null;

            return folderSummary;
        }

        /// <summary>
        /// returns false if there were errors writing to CSV file
        /// </summary>  
        private static bool CountJustFoldersInThisFolder(string directory, FolderSummary folderSummary)
        {            
            List<string> folderNames = new List<string>();
            GetFolderNamesInThisFolder(directory, folderNames);

            foreach (string folder in folderNames)
            {
                FolderSummary individualFolder = CountInventoryWorthForThisFolder(folder); //RECURSIVE!
                if (individualFolder == null)
                    return false;
                folderSummary.AbsorbChildFolder(individualFolder);
            }
            return true;
        }

        private static void GetFolderNamesInThisFolder(string parentDirectory, List<string> childDirectoryNames)
        {
            childDirectoryNames.AddRange(Directory.GetDirectories(parentDirectory));
        }

        private static void GetPictureFileNames(string directory, List<string> fileNames)
        {
            string[] fullFilePaths = Directory.GetFiles(directory, SearchOptions.Instance.FileExtension);
            foreach (string fullFilePath in fullFilePaths)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(fullFilePath));
            }
        }

        private static void CountJustPicturesInThisFolder(string directory, FolderSummary folderSummary)
        {
            List<string> pictureFileNames = new List<string>();
            GetPictureFileNames(directory, pictureFileNames);
            foreach (string fileName in pictureFileNames)
            {
                ParsePicFileToRecord(fileName, folderSummary);
            }
        }

        /// <summary>
        /// public for unit testing
        /// </summary>
        public static void ParsePicFileToRecord(string fileName, FolderSummary folderSummary)
        {
            foreach (ChkBx chkBx in SearchOptions.Instance.fNameFormatDict.Keys)
            {
                bool parseResult = true;
                switch (chkBx)
                {
                    case ChkBx.date:
                        //parseResult = ParseDate(fileNameIn, out fileNameRemainder, out date)
                        break;
                    case ChkBx.value:

                        break;
                }
                if (!parseResult) break;
            }
            //then parse the remaining file name as the description

            string worth = string.Empty;
            string recordDescription = string.Empty;
            string firstChar = fileName.Substring(0, 1);
            int indexOfSpaceCharAfterPrice = fileName.IndexOf(" ");
            bool parseSuccess = true;

            if (firstChar == "$" && indexOfSpaceCharAfterPrice > 0)
            {
                worth = fileName[0..indexOfSpaceCharAfterPrice]; // range instead of substring()
                recordDescription = fileName[(indexOfSpaceCharAfterPrice + 1)..];
                //if (IsWorthAFloat(worth) && IsWorthADollarAmount(worth))                
                if (IsWorthADollarAmount(worth))
                    parseSuccess = true;
                else
                    parseSuccess = false;
            }
            else
                parseSuccess = false;
            if (parseSuccess)
            {
                float worthFloat = float.Parse(worth.Substring(1));
                folderSummary.AddFileInventoryRecord(worthFloat, recordDescription);
            }
            else
                folderSummary.AddFileErrorRecord(fileName);
        }

        const int firstAsciiNumber = 48;
        const int LastAsciiNumber = 57;
        const int periodAsciiNumber = 46; 
        private static bool IsWorthADollarAmount(string appraisal)
        {
            //must have no decimals, or exactly 2 decimals.
            //the only character other than numerics is a "."
            bool foundPeriod = false;
            string value = appraisal.Substring(1);
            if (value.Length == 0) return false;
            int numDecimals = 0;
            for(int i = 0; i< value.Length; i++)
            {
                // check what kind of character it is
                bool isDigit = false; 
                int asciiVal = value[i];
                if (asciiVal == periodAsciiNumber)
                {
                    if (i == 0) // first char of appraisal cannot be a decimal.
                        return false;
                    foundPeriod = true;
                }
                else if (asciiVal >= firstAsciiNumber && asciiVal <= LastAsciiNumber)
                    isDigit = true;
                else // if not a decimal or a digit, the appraisal is not a dollar amount
                    return false;

                if (foundPeriod && isDigit)
                    numDecimals++;
            }
            if (foundPeriod && numDecimals != 2)
                return false;
            
            return true;
        }
    }
}

