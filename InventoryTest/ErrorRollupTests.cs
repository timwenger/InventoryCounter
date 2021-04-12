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
            FolderSummary folderSummary = new("my folder");

            folderSummary.AddFileInventoryRecord("my Item", 10.10m);
            folderSummary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);

            List<CsvRecord> allRecords = folderSummary.Files.GetCollectionCopy();
            bool foundMyError = false;
            foreach(CsvRecord record in allRecords)
            {
                if (record.Description == new Error("my incorrectly formatted item", Error.Type.value).Print())
                    foundMyError = true;
            }
            Assert.That(foundMyError == true);
        }

        [Test]
        public void IndividualErrorIsAddedToErrorCollection()
        {
            FolderSummary folderSummary = new("my folder");
            folderSummary.AddFileInventoryRecord("my Item", 10.10m);
            folderSummary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);

            List<CsvRecord> myErrors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.That(myErrors.Count == 1);
            Assert.AreEqual(new Error("my incorrectly formatted item", Error.Type.value).Print(), myErrors[0].Description);
        }


        [Test]
        public void ChildFolderErrorsAreRolledUpIntoParentFolder()
        {
            FolderSummary childFolder1Summary = new("child folder 1");
            childFolder1Summary.AddFileErrorRecord("my first error", Error.Type.value);
            childFolder1Summary.AddFileErrorRecord("my second error", Error.Type.value);
            FolderSummary childFolder2Summary = new("child folder 2");
            childFolder2Summary.AddFileInventoryRecord("my Other Item", 10.10m);
            childFolder2Summary.AddFileErrorRecord("my incorrectly formatted item", Error.Type.value);

            FolderSummary parentFolderSummary = new("parent folder");
            parentFolderSummary.AbsorbChildFolder(childFolder1Summary);
            parentFolderSummary.AbsorbChildFolder(childFolder2Summary);            

            List<CsvRecord> allRecords = parentFolderSummary.Folders.GetCollectionCopy();
            List<string> allDescriptions = new();
            foreach (CsvRecord record in allRecords)
            {
                allDescriptions.Add(record.Description);
            }
            Error e1 = new ("my first error", Error.Type.value);
            e1.AddHierarchyToFolderPath("child folder 1");
            CollectionAssert.Contains(allDescriptions, e1.Print());
            Error e2 = new ("my second error", Error.Type.value);
            e2.AddHierarchyToFolderPath("child folder 1");
            CollectionAssert.Contains(allDescriptions, e2.Print());
            Error e3 = new ("my incorrectly formatted item", Error.Type.value);
            e3.AddHierarchyToFolderPath("child folder 2");
            CollectionAssert.Contains(allDescriptions, e3.Print());                       
        }
    }
}
