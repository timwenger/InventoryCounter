using NUnit.Framework;
using InventoryCounter;
using System.Collections.Generic;

namespace InventoryTest
{
    public class ParseValueTests
    {
        [SetUp]
        public void Init()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);
        }

        [Test]
        public void RegsFile_ExpectSuccess()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            
            RecursiveCounter.ParseFileToRecord("$10 apple", folderSummary);
            Assert.AreEqual(10f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual("apple", folderSummary.Files.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void ValueIncludingCents_ExpectSuccess()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$10.10 apple", folderSummary);
            Assert.AreEqual(10.10f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual("apple", folderSummary.Files.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void DescriptionHasValueRelatedCharacters_ExpectSuccess()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$10 apple, originally bought for $2.99/lb", folderSummary);
            Assert.AreEqual(10f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual("apple, originally bought for $2.99/lb", folderSummary.Files.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void HasNoDescription_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("$10", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$10", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void DoesntStartwithDollarSign_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("10 apple", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("10 apple", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void ValueHasCommaInIt_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("$10,000 apple", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$10,000 apple", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void Has1Decimal_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("$10.1 apple", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$10.1 apple", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void TooManyDecimals_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("$10.108 apple", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$10.108 apple", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void StartsWithDecimal_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("$.10 apple", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();            
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$.10 apple", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void BadSpacing_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("$ 10 apple", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$ 10 apple", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void NoSpacing_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("$10apple", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$10apple", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void HasUnexpectedDate_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,30 apple $10", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,30 apple $10", Error.Type.value).Print(), errors[0].Description);
        }

        
    }
}