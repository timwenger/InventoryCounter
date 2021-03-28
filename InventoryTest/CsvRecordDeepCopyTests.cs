using NUnit.Framework;
using InventoryCounter;
using System.Collections.Generic;

namespace InventoryTest
{
    public class CsvRecordDeepCopyTests
    {
        [Test]
        public void DeepCopyaCsvRecordTest()
        {
            CsvRecord record = new CsvRecord(5f, "originalDescription");
            record.IsErrorRow = true;
            CsvRecord deepCopyOfRecord = record.DeepCopy();
            deepCopyOfRecord.Description = "newDescription";
            deepCopyOfRecord.IsErrorRow = false;
            Assert.That(record.Description == "originalDescription");
            Assert.That(record.worth == 5f);
            Assert.That(record.IsErrorRow == true);
        }

        [Test]
        public void GetCollection_DeepCopyTest()
        {
            FolderSummary folderSummary = new FolderSummary("folder name");
            CsvRecordCollection collection = new CsvRecordCollection();
            folderSummary.AddFileInventoryRecord(65f, "originalDescription");
            folderSummary.AddFileErrorRecord("originalError", Error.Type.value);
            folderSummary.AddFileErrorRecord("originalError", Error.Type.value);
            List<CsvRecord> printout = folderSummary.Pics.GetCollectionCopy();

            //This unit test wants to ensure a deep copy, so that the client can
            // do whatever it likes with the printout, and it will not affect the
            // records saved in the collection. So that test will be repeated here:
            foreach (CsvRecord record in printout)
            {
                record.Description = "blah";
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
                    Assert.That(unaffectedRecord.worth == 65f);
                }                    
            }
            Assert.That(errorCount == 2);                        
        }
    }
}