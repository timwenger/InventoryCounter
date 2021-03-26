using NUnit.Framework;
using InventoryCounter;
using System.Collections.Generic;

namespace InventoryTest
{
    public class FileParsingTests
    {
        [Test]
        public void GoodFile()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            
            RecursiveCounter.ParsePicFileToRecord("$10 apple", folderSummary);
            Assert.AreEqual(10f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual("apple", folderSummary.Pics.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void ValueIncludingCents()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$10.10 apple", folderSummary);
            Assert.AreEqual(10.10f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual("apple", folderSummary.Pics.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void DescriptionHasValueRelatedCharacters()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$10 apple, originally bought for $2.99/lb", folderSummary);
            Assert.AreEqual(10f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual("apple, originally bought for $2.99/lb", folderSummary.Pics.GetCollectionCopy()[0].Description);
            Assert.AreEqual(0, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void HasNoDescription()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$10", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void DoesntStartwithDollarSign()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("10 apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void ValueHasCommaInIt()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$10,000 apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void Has1Decimal()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$10.1 apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void TooManyDecimals()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$10.108 apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void StartsWithDecimal()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$.10 apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void BadSpacing()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$ 10 apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void NoSpacing()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("$10apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void WrongOrder()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");

            RecursiveCounter.ParsePicFileToRecord("2020,12,30 $10 apple", folderSummary);
            Assert.AreEqual(0f, folderSummary.Pics.TotalWorth, 0.01f);
            Assert.AreEqual(1, folderSummary.Pics.GetCollectionErrorsCopy().Count);
        }

        
    }
}