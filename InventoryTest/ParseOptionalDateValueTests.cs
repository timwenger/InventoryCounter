using InventoryCounter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTest
{
    class ParseOptionalDateValueTests
    {
        [Test]
        public void DateValueDescription_ExpectSuccess()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31 $20 new years drinks", folderSummary);

            CsvRecord record = folderSummary.Files.GetCollectionCopy()[0];
            Assert.AreEqual("2020,12,31", record.Date);
            Assert.AreEqual(20f, record.WorthF, 0.01f);
            Assert.AreEqual("new years drinks", record.Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void ValueDateDescription_ExpectSuccess()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty); 
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
            
            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("$20 2020,12,31 new years drinks", folderSummary);

            CsvRecord record = folderSummary.Files.GetCollectionCopy()[0];
            Assert.AreEqual("2020,12,31", record.Date);
            Assert.AreEqual(20f, record.WorthF, 0.01f);
            Assert.AreEqual("new years drinks", record.Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void OnlyDescription_ExpectSuccess()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31 $20 new years drinks", folderSummary);

            CsvRecord record = folderSummary.Files.GetCollectionCopy()[0];
            Assert.AreEqual("2020,12,31 $20 new years drinks", record.Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void DescriptionIsJustSpaceCharsNoSearchOptions_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("   ", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("   ", Error.Type.noDescription).Print(), errors[0].Description);
        }

        [Test]
        public void NoDescriptionNoSearchOptions_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("", Error.Type.noDescription).Print(), errors[0].Description);
        }

        [Test]
        public void NoDescriptionAllSearchOptions_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty); 
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);            

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31 $20", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,31 $20", Error.Type.noDescription).Print(), errors[0].Description);
        }

        [Test]
        public void InvalidDate_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty); 
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);            

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("2020,1231 $20 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,1231 $20 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void InvalidValue_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31 $$20 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,31 $$20 new years drinks", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void NoSpaceDateValue_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31$20 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,31$20 new years drinks", Error.Type.noSpacingBetweenTerms).Print(), errors[0].Description);
        }

        [Test]
        public void DateMissing_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("$20 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$20 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void ValueMissing_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,31 new years drinks", Error.Type.value).Print(), errors[0].Description);
        }

        [Test]
        public void DateValueWrongOrder_ExpectError()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.value, string.Empty);

            FolderSummary folderSummary = new("folder name");
            RecursiveCounter.ParseFileToRecord("$20 2020,12,31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("$20 2020,12,31 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }
    }
}
