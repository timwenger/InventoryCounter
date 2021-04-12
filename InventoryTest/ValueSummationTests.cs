using NUnit.Framework;
using InventoryCounter;
using System;

namespace InventoryTest
{
    class ValueSummationTests
    {
        [SetUp]
        public void Init()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);
        }

        [Test]
        public void SumTwoIndividualRecordsTest()
        {
            FolderSummary folderSummary = new("folder name");
            folderSummary.AddFileInventoryRecord("my Item", 10.10m);
            folderSummary.AddFileInventoryRecord("my Other Item", 10.10m);
            Assert.AreEqual(20.20m, folderSummary.GrandTotal);
        }

        [Test]
        public void ErrorsDontAffectIndividualRecordsSumTest()
        {
            FolderSummary folderSummary = new("folder name");
            folderSummary.AddFileInventoryRecord("my Item", 10.10m);
            folderSummary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);
            folderSummary.AddFileInventoryRecord("my Other Item", 10.10m);
            folderSummary.AddFileErrorRecord("another badly formatted item", Error.Type.value);
            Assert.AreEqual(20.20m, folderSummary.GrandTotal);
        }

        [Test]
        public void RollUpChildFolderRecordsAndSumParentFolderTest()
        {
            FolderSummary childFolder1Summary = new("child folder 1");
            childFolder1Summary.AddFileInventoryRecord("my Item", 10.10m);
            childFolder1Summary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);
            FolderSummary childFolder2Summary = new("child folder 2");
            childFolder2Summary.AddFileInventoryRecord("my Other Item", 10.10m);
            childFolder2Summary.AddFileErrorRecord("another badly formatted item", Error.Type.value);

            FolderSummary parentFolderSummary = new("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolder1Summary);
            parentFolderSummary.AbsorbChildFolder(childFolder2Summary);
            Assert.AreEqual(20.20m, parentFolderSummary.GrandTotal);
        }

        [Test]
        public void PrintingCsvFileDoesntAffectTotal()
        {
            FolderSummary childFolderSummary = new("child folder 1");
            childFolderSummary.AddFileInventoryRecord("my Item", 10.10m);
            childFolderSummary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);
            FolderSummary parentFolderSummary = new("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolderSummary);

            parentFolderSummary.AddFileInventoryRecord("my Other Item", 10.10m);
            parentFolderSummary.AddFileErrorRecord("another badly formatted item", Error.Type.value);

            CsvFileCreator csvMaker = new (Environment.CurrentDirectory, parentFolderSummary.Folders, parentFolderSummary.Files);
            csvMaker.CreateFolderInventoryCsvFile(); 
                        
            Assert.AreEqual(20.20m, parentFolderSummary.GrandTotal);            
        }
    }
}
