using InventoryCounter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTest
{
    class ParseDateTests
    {
        [SetUp]
        public void Init()
        {
            SearchOptions.Instance.fNameFormatDict.Clear();
            SearchOptions.Instance.fNameFormatDict.Add(ChkBx.date, string.Empty);
        }

        [Test]
        public void RegsFile_ExpectSuccess()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31 new years drinks", folderSummary);
            
            CsvRecord record = folderSummary.Files.GetCollectionCopy()[0];
            Assert.AreEqual("2020,12,31", record.Date); 
            Assert.AreEqual("new years drinks", record.Description);            
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void ValueInDescription_ExpectSuccess()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31 new years drinks - $20", folderSummary);

            CsvRecord record = folderSummary.Files.GetCollectionCopy()[0];
            Assert.AreEqual("2020,12,31", record.Date);
            Assert.AreEqual("new years drinks - $20", record.Description);
            Assert.AreEqual(0, folderSummary.Files.GetCollectionErrorsCopy().Count);
        }

        [Test]
        public void NoDescription_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,31", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,31", Error.Type.noDescription).Print(), errors[0].Description);
        }

        [Test]
        public void YearNotValid_DateObviouslyWrongBecauseTooFarInFuture_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2200,12,31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2200,12,31 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void YearNotValid_DateObviouslyWrongBecauseTooFarInPast_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("1800,12,31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("1800,12,31 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void MonthNotValid_TooBig_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,13,31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,13,31 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void MonthNotValid_NotPaddedWith0_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,3,31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,3,31 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void DayNotValid_TooBig_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,32 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,32 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void DayNotValid_NotPaddedWith0_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,12,2 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,12,2 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void WrongFormat_YearDayMonth_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020,31,12 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020,31,12 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void WrongFormat_DayYearMonth_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("31,2020,12 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("31,2020,12 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void WrongFormat_DayMonthYear_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("31,12,2020 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("31,12,2020 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void SeparatedBySlashes_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020/12/31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020/12/31 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }

        [Test]
        public void SeparatedByPeriods_ExpectError()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            RecursiveCounter.ParseFileToRecord("2020.12.31 new years drinks", folderSummary);

            List<CsvRecord> errors = folderSummary.Files.GetCollectionErrorsCopy();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(new Error("2020.12.31 new years drinks", Error.Type.date).Print(), errors[0].Description);
        }
    }
}
