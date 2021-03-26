using NUnit.Framework;
using InventoryCounter;
using System;

namespace InventoryTest
{
    class TotalWorthTests
    {
        [Test]
        public void SumTwoIndividualRecordsTest()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            folderSummary.AddFileInventoryRecord(10.10f, "my Item");
            folderSummary.AddFileInventoryRecord(10.10f, "my Other Item");
            Assert.AreEqual(20.20f, folderSummary.GrandTotal, 0.01f);
        }

        [Test]
        public void ErrorsDontAffectIndividualRecordsSumTest()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            folderSummary.AddFileInventoryRecord(10.10f, "my Item");
            folderSummary.AddFileErrorRecord("my incorrectly formatted item");
            folderSummary.AddFileInventoryRecord(10.10f, "my Other Item");
            folderSummary.AddFileErrorRecord("another badly formatted item");
            Assert.AreEqual(20.20f, folderSummary.GrandTotal, 0.01f);
        }

        [Test]
        public void RollUpChildFolderRecordsAndSumParentFolderTest()
        {
            FolderSummary childFolder1Summary = new FolderSummary("child folder 1");
            childFolder1Summary.AddFileInventoryRecord(10.10f, "my Item");
            childFolder1Summary.AddFileErrorRecord("my incorrectly formatted item");
            FolderSummary childFolder2Summary = new FolderSummary("child folder 2");
            childFolder2Summary.AddFileInventoryRecord(10.10f, "my Other Item");
            childFolder2Summary.AddFileErrorRecord("another badly formatted item");

            FolderSummary parentFolderSummary = new FolderSummary("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolder1Summary);
            parentFolderSummary.AbsorbChildFolder(childFolder2Summary);
            Assert.AreEqual(20.20f, parentFolderSummary.GrandTotal, 0.01f);
        }

        [Test]
        public void PrintingCsvFileDoesntAffectTotal()
        {
            FolderSummary childFolderSummary = new FolderSummary("child folder 1");
            childFolderSummary.AddFileInventoryRecord(10.10f, "my Item");
            childFolderSummary.AddFileErrorRecord("my incorrectly formatted item");
            FolderSummary parentFolderSummary = new FolderSummary("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolderSummary);

            parentFolderSummary.AddFileInventoryRecord(10.10f, "my Other Item");
            parentFolderSummary.AddFileErrorRecord("another badly formatted item");

            CsvFileCreator csvMaker = new CsvFileCreator(Environment.CurrentDirectory, parentFolderSummary.Folders, parentFolderSummary.Pics);
            csvMaker.CreateFolderInventoryCsvFile(); 
                        
            Assert.AreEqual(20.20f, parentFolderSummary.GrandTotal, 0.01f);            
        }
    }
}
