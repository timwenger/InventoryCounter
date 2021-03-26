using NUnit.Framework;
using InventoryCounter;
using System.Collections.Generic;

namespace InventoryTest
{
    class ErrorRollupTests
    {
        [Test]
        public void IndividualErrorIsAddedToCollection()
        {
            FolderSummary folderSummary = new FolderSummary("my folder");

            folderSummary.AddFileInventoryRecord(10.10f, "my Item");
            folderSummary.AddFileErrorRecord("my incorrectly formatted item");

            List<CsvRecord> allRecords = folderSummary.Pics.GetCollectionCopy();
            bool foundMyError = false;
            foreach(CsvRecord record in allRecords)
            {
                if (record.Description == new Error("my incorrectly formatted item").Print())
                    foundMyError = true;
            }
            Assert.That(foundMyError == true);
        }

        [Test]
        public void IndividualErrorIsAddedToErrorCollection()
        {
            FolderSummary folderSummary = new FolderSummary("my folder");
            folderSummary.AddFileInventoryRecord(10.10f, "my Item");
            folderSummary.AddFileErrorRecord("my incorrectly formatted item");

            List<CsvRecord> myErrors = folderSummary.Pics.GetCollectionErrorsCopy();
            Assert.That(myErrors.Count == 1);

            CsvRecord myError = myErrors[0];
            Assert.AreEqual(new Error("my incorrectly formatted item").Print(), myError.Description);
        }


        [Test]
        public void ChildFolderErrorsAreRolledUpIntoParentFolder()
        {
            FolderSummary childFolder1Summary = new FolderSummary("child folder 1");
            childFolder1Summary.AddFileErrorRecord("my first error");
            childFolder1Summary.AddFileErrorRecord("my second error");
            FolderSummary childFolder2Summary = new FolderSummary("child folder 2");
            childFolder2Summary.AddFileInventoryRecord(10.10f, "my Other Item");
            childFolder2Summary.AddFileErrorRecord("my incorrectly formatted item");

            FolderSummary parentFolderSummary = new FolderSummary("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolder1Summary);
            parentFolderSummary.AbsorbChildFolder(childFolder2Summary);            

            List<CsvRecord> allRecords = parentFolderSummary.Folders.GetCollectionCopy();
            List<string> allDescriptions = new List<string>();
            foreach (CsvRecord record in allRecords)
            {
                allDescriptions.Add(record.Description);
            }
            Error e1 = new Error("my first error");
            e1.AddHierarchyToFolderPath("child folder 1");
            CollectionAssert.Contains(allDescriptions, e1.Print());
            Error e2 = new Error("my second error");
            e2.AddHierarchyToFolderPath("child folder 1");
            CollectionAssert.Contains(allDescriptions, e2.Print());
            Error e3 = new Error("my incorrectly formatted item");
            e3.AddHierarchyToFolderPath("child folder 2");
            CollectionAssert.Contains(allDescriptions, e3.Print());                       
        }
    }
}
