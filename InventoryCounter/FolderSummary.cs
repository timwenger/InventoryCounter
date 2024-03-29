﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryCounter

{
    public class FolderSummary
    {
        public decimal GrandTotal
        {
            get { return Folders.TotalWorth + Files.TotalWorth; }
        }

        private readonly string _folderName;
        public CsvRecordCollection Folders = new ();
        public CsvRecordCollection Files = new ();

        private readonly List<Error> _foldersErrors = new ();
        private readonly List<Error> _filesErrors = new ();

        private static bool PrintValues { get { return SearchOptions.Instance.SearchForValues; } }

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
            if(PrintValues)
            AddFolderRecord(childFolder._folderName, childFolderErrors, childFolder.GrandTotal);
            else
                AddFolderRecord(childFolder._folderName, childFolderErrors);
        }

        private void AddFolderRecord(string folderName, List<Error> errors, decimal? value = null)
        {
            string[] errorMessages = new string[errors.Count];
            for(int i = 0; i< errors.Count; i++)
            {
                errorMessages[i] = errors[i].Print();
            }
            Folders.AddInventoryRecord(folderName, errorMessages, value);
        }

        public void AddFileInventoryRecord(string ItemName, decimal? Value = null, string Date = "")
        {
            Files.AddInventoryRecord(ItemName, Value, Date);
        }


        public void AddFileErrorRecord(string error, Error.Type type)
        {
            Error newError = new (error, type);
            _filesErrors.Add(newError);
            Files.AddErrorRecord(newError.Print());
        }

        private List<Error> GetAllErrors()
        {
            List<Error> allErrors = new ();            
            allErrors.AddRange(_foldersErrors);
            allErrors.AddRange(_filesErrors);
            return allErrors;
        }

        internal List<string> GetPrintableErrors()
        {
            List<string> printableErrors = new ();
            foreach(Error error in GetAllErrors())
            {
                printableErrors.Add(error.Print());
            }
            return printableErrors;
        }
    }
}
