using NUnit.Framework;
using InventoryCounter;
using System.Collections.Generic;

namespace InventoryTest
{
    public class CsvRecordDeepCopyTests
    {
        //Todo: This class is doing a deep copy... but the 3 main properties of interest are private set, so 
        // they cannot be changed (which would make it clear whether it was a true deep copy).
        // Therefore, it would be ideal to test this a different way. Like have a private method record.Destroy??
        // That's a bit weird to have production methods used only for unit testing.

        [Test]
        public void DeepCopyaCsvRecordTest()
        {
            CsvRecord record = new CsvRecord("originalDescription", 5f, "2019,09,29");
            record.IsErrorRow = true;
            CsvRecord deepCopyOfRecord = record.DeepCopy();
            deepCopyOfRecord.IsErrorRow = false;
            Assert.That(record.Description == "originalDescription");
            Assert.That(record.WorthF == 5f);
            Assert.That(record.Date == "2019,09,29");
            Assert.That(record.IsErrorRow == true);
        }

        [Test]
        public void GetCollection_DeepCopyTest()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            CsvRecordCollection collection = new CsvRecordCollection();
            folderSummary.AddFileInventoryRecord("originalDescription", 65f);
            folderSummary.AddFileErrorRecord("originalError", Error.Type.value);
            folderSummary.AddFileErrorRecord("originalError", Error.Type.value);
            List<CsvRecord> printout = folderSummary.Pics.GetCollectionCopy();

            //This unit test wants to ensure a deep copy, so that the client can
            // do whatever it likes with the printout, and it will not affect the
            // records saved in the collection. So that test will be repeated here:
            foreach (CsvRecord record in printout)
            {
                record.IsErrorRow = true;
            }
            List<CsvRecord> anotherPrintout = folderSummary.Pics.GetCollectionCopy();

            int errorCount = 0;
            foreach(CsvRecord unaffectedRecord in anotherPrintout)
            {                
                if (unaffectedRecord.IsErrorRow)
                {
                    Assert.AreEqual(new Error("originalError", Error.Type.value).Print(), unaffectedRecord.Description);                    
                    errorCount++;
                }
                else
                {
                    Assert.That(unaffectedRecord.Description == "originalDescription"); 
                    Assert.That(unaffectedRecord.WorthF == 65f);
                }                    
            }
            Assert.That(errorCount == 2);                        
        }
    }
}