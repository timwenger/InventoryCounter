using System;
using System.Collections.Generic;
using System.IO;

namespace InventoryCounter
{
    public class RecursiveCounter
    {
        /// <summary>
        /// returns null if there were errors writing to CSV file
        /// </summary> 
        public static FolderSummary CountInventoryWorthForThisFolder(string directory)
        {
            FolderSummary folderSummary = new (Path.GetFileName(directory));

            if (!CountJustFoldersInThisFolder(directory, folderSummary))
                return null;
            CountJustFilesInThisFolder(directory, folderSummary);

            CsvFileCreator csvMaker = new (directory, folderSummary.Folders, folderSummary.Files);
            if (!csvMaker.CreateFolderInventoryCsvFile())
                return null;

            return folderSummary;
        }

        /// <summary>
        /// returns false if there were errors writing to CSV file
        /// </summary>  
        private static bool CountJustFoldersInThisFolder(string directory, FolderSummary folderSummary)
        {            
            List<string> folderNames = new ();
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

        private static void GetFileNames(string directory, List<string> fileNames)
        {
            string[] fullFilePaths = Directory.GetFiles(directory, SearchOptions.Instance.FileExtension);
            foreach (string fullFilePath in fullFilePaths)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(fullFilePath));
            }
        }

        private static void CountJustFilesInThisFolder(string directory, FolderSummary folderSummary)
        {
            List<string> fileNames = new ();
            GetFileNames(directory, fileNames);
            foreach (string fileName in fileNames)
            {
                ParseFileToRecord(fileName, folderSummary);
            }
        }

        /// <summary>
        /// public for unit testing
        /// </summary>
        public static void ParseFileToRecord(string fileName, FolderSummary folderSummary)
        {
            string remainingFileName = fileName;
            float? value = null;
            string date = null;
            bool parseResult = true;
            bool thisIsNotTheFirstTerm = false;
            foreach (ChkBx chkBx in SearchOptions.Instance.fNameFormatDict.Keys)
            {                
                switch (chkBx)
                {
                    case ChkBx.date:
                        parseResult = RequireSpacingBeforeNextTerm();
                        if (parseResult && (!(parseResult = FileParsing.ParseDate(ref remainingFileName, out date))))
                            folderSummary.AddFileErrorRecord(fileName, Error.Type.date);
                        break;
                    case ChkBx.value:
                        parseResult = RequireSpacingBeforeNextTerm();
                        if (parseResult && (!(parseResult = FileParsing.ParseValue(ref remainingFileName, out value))))
                            folderSummary.AddFileErrorRecord(fileName, Error.Type.value);
                        break;
                }
                if (!parseResult) break;
            }
            if (parseResult)
                parseResult = RequireSpacingBeforeNextTerm();
            if (parseResult && (!(parseResult = FileParsing.ParseDescription(ref remainingFileName))))
                folderSummary.AddFileErrorRecord(fileName, Error.Type.noDescription);
       

            if (parseResult)
            {
                if (value != null && date != null)
                    folderSummary.AddFileInventoryRecord(remainingFileName, value, date);
                else if (value != null)
                    folderSummary.AddFileInventoryRecord(remainingFileName, value);
                else if (date != null)
                    folderSummary.AddFileInventoryRecord(remainingFileName, Date: date);
                else
                    folderSummary.AddFileInventoryRecord(remainingFileName);
            } 
            
            bool RequireSpacingBeforeNextTerm()
            {
                bool localParseResult = true;
                if (thisIsNotTheFirstTerm && remainingFileName.Length > 0)
                {
                    if (!(localParseResult = FileParsing.FirstCharIsASpace(remainingFileName)))
                        folderSummary.AddFileErrorRecord(fileName, Error.Type.noSpacingBetweenTerms);
                }
                else
                    thisIsNotTheFirstTerm = true;
                return localParseResult;
            }
        }
    }
}

