using NUnit.Framework;
using InventoryCounter;
using System;

namespace InventoryTest
{
    class TotalWorthTests
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
            FolderSummary folderSummary = new FolderSummary("folder name");
            folderSummary.AddFileInventoryRecord("my Item", 10.10f);
            folderSummary.AddFileInventoryRecord("my Other Item", 10.10f);
            Assert.AreEqual(20.20f, folderSummary.GrandTotal, 0.01f);
        }

        [Test]
        public void ErrorsDontAffectIndividualRecordsSumTest()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            folderSummary.AddFileInventoryRecord("my Item", 10.10f);
            folderSummary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);
            folderSummary.AddFileInventoryRecord("my Other Item", 10.10f);
            folderSummary.AddFileErrorRecord("another badly formatted item", Error.Type.value);
            Assert.AreEqual(20.20f, folderSummary.GrandTotal, 0.01f);
        }

        [Test]
        public void RollUpChildFolderRecordsAndSumParentFolderTest()
        {
            FolderSummary childFolder1Summary = new FolderSummary("child folder 1");
            childFolder1Summary.AddFileInventoryRecord("my Item", 10.10f);
            childFolder1Summary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);
            FolderSummary childFolder2Summary = new FolderSummary("child folder 2");
            childFolder2Summary.AddFileInventoryRecord("my Other Item", 10.10f);
            childFolder2Summary.AddFileErrorRecord("another badly formatted item", Error.Type.value);

            FolderSummary parentFolderSummary = new FolderSummary("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolder1Summary);
            parentFolderSummary.AbsorbChildFolder(childFolder2Summary);
            Assert.AreEqual(20.20f, parentFolderSummary.GrandTotal, 0.01f);
        }

        [Test]
        public void PrintingCsvFileDoesntAffectTotal()
        {
            FolderSummary childFolderSummary = new FolderSummary("child folder 1");
            childFolderSummary.AddFileInventoryRecord("my Item", 10.10f);
            childFolderSummary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);
            FolderSummary parentFolderSummary = new FolderSummary("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolderSummary);

            parentFolderSummary.AddFileInventoryRecord("my Other Item", 10.10f);
            parentFolderSummary.AddFileErrorRecord("another badly formatted item", Error.Type.value);

            CsvFileCreator csvMaker = new CsvFileCreator(Environment.CurrentDirectory, parentFolderSummary.Folders, parentFolderSummary.Files);
            csvMaker.CreateFolderInventoryCsvFile(); 
                        
            Assert.AreEqual(20.20f, parentFolderSummary.GrandTotal, 0.01f);            
        }
    }
}
