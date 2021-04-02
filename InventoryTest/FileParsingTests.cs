using NUnit.Framework;
using InventoryCounter;
using System.Collections.Generic;

namespace InventoryTest
{
    public class FileParsingTests
    {
        [SetUp]
        public void Init()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);
        }

        [Test]
        public void RegsFile_ExpcetSuccess()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            
            RecursiveCounter.ParseFileToRecord("$10 apple", folderSummary);
            Assert.AreEqual(10f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual("apple", folderSummary.Files.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void ValueIncludingCents_ExpcetSuccess()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$10.10 apple", folderSummary);
            Assert.AreEqual(10.10f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual("apple", folderSummary.Files.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void DescriptionHasValueRelatedCharacters_ExpcetSuccess()
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
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void DoesntStartwithDollarSign_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("10 apple", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void ValueHasCommaInIt_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$10,000 apple", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void Has1Decimal_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$10.1 apple", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void TooManyDecimals_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$10.108 apple", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void StartsWithDecimal_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$.10 apple", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void BadSpacing_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$ 10 apple", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void NoSpacing_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("$10apple", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void WrongOrder_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParseFileToRecord("2020,12,30 apple $10", folderSummary);
            //Assert.AreEqual(0f, folderSummary.Files.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        
    }
}