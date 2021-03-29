﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryCounter

{
    public class FolderSummary
    {
        public float? GrandTotal
        {
            get 
            {
                bool foundAValue = false;
                float temp = 0f;
                if (Folders.TotalWorth != null)
                {
                    temp += (float)Folders.TotalWorth;
                    foundAValue = true;
                }
                if (Pics.TotalWorth != null)
                {
                    temp += (float)Pics.TotalWorth;
                    foundAValue = true;
                }
                if (foundAValue) return temp;
                return null;
            }
        }

        private string _folderName;
        public CsvRecordCollection Folders = new CsvRecordCollection();
        public CsvRecordCollection Pics = new CsvRecordCollection();

        private List<Error> _foldersErrors = new List<Error>();
        private List<Error> _picsErrors = new List<Error>();
        public FolderSummary(string folderName) 
        {
            _folderName = folderName;
        }

        public void AbsorbChildFolder(FolderSummary childFolder)
        {            
            List<Error> childFolderErrors = childFolder.GetAllErrors();
            foreach(Error error in childFolderErrors)
            {
                error.AddHierarchyToFolderPath(childFolder._folderName);
            }
            _foldersErrors.AddRange(childFolderErrors);
            AddFolderRecord(childFolder._folderName, childFolderErrors, childFolder.GrandTotal);
        }

        private void AddFolderRecord(string folderName, List<Error> errors, float? value)
        {
            string[] errorMessages = new string[errors.Count];
            for(int i = 0; i< errors.Count; i++)
            {
                errorMessages[i] = errors[i].Print();
            }
            Folders.AddInventoryRecord(folderName, errorMessages, value);
        }

        public void AddFileInventoryRecord(string ItemName, float? Value = null, string Date = "")
        {
            Pics.AddInventoryRecord(ItemName, Value, Date);
        }


        public void AddFileErrorRecord(string error, Error.Type type)
        {
            Error newError = new Error(error, type);
            _picsErrors.Add(newError);
            Pics.AddErrorRecord(newError.Print());
        }


        // My problem here, is that an Error doesn't even exist for the picture file. Just the CsvRecord marked as an error.
        // When you record a new CsvRecord that is an error, THAT'S the only time when you should be making the new error.
        // NOT inside a GetAllErrors() method (that you should be able to call multiple times)
        // and Not when you're about to absorb the error into its parent

        private List<Error> GetAllErrors()
        {
            List<Error> allErrors = new List<Error>();            
            allErrors.AddRange(_foldersErrors);
            allErrors.AddRange(_picsErrors);
            return allErrors;
        }

        internal List<string> GetPrintableErrors()
        {
            List<string> printableErrors = new List<string>();
            foreach(Error error in GetAllErrors())
            {
                printableErrors.Add(error.Print());
            }
            return printableErrors;
        }
    }
}
