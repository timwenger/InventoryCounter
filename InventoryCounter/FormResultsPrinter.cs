using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace InventoryCounter
{
    public struct GuiResultsObjects
    {
        public Label ResultLabel { get; set; }
        public TextBox MessagesBox { get; set; }
    }

    internal static class ResultPrinter
    {
        private enum Type
        {
            unsuccessful,
            successWithReturnedValue,
            successWithNoReturnedValue,
            directoryDNE,
            foundNothing,
            parseError
        }

        internal static void PrintResults(string topDirectory, GuiResultsObjects resultsObjs)
        {
            if (Directory.Exists(topDirectory))
            {
                FolderSummary result = RecursiveCounter.CountInventoryWorthForThisFolder(topDirectory);
                if (result == null)
                {
                    Print(Type.unsuccessful, 0f, resultsObjs);
                    return;
                }
                List<string> errors = result.GetPrintableErrors();
                if (errors.Count == 0 && !SearchOptions.Instance.SearchForValues)
                    Print(Type.successWithNoReturnedValue, resultsObjs);
                else if (errors.Count == 0 && result.GrandTotal > 0)
                    Print(Type.successWithReturnedValue, result.GrandTotal, resultsObjs);
                else if (errors.Count == 0)
                    Print(Type.foundNothing, 0f, resultsObjs);
                else
                    Print(Type.parseError, errors, resultsObjs);
            }
            else
                Print(Type.directoryDNE, 0f, resultsObjs);
        }


        private static void Print(Type result, float grandTotal, GuiResultsObjects resultObjs)
        {
            switch (result)
            {
                case Type.unsuccessful:
                    resultObjs.ResultLabel.ForeColor = System.Drawing.Color.Red;
                    resultObjs.ResultLabel.Text = "Inventory count stopped prematurely.";
                    resultObjs.MessagesBox.Visible = false;
                    break;
                case Type.successWithReturnedValue:
                    resultObjs.ResultLabel.ForeColor = System.Drawing.Color.Green;
                    resultObjs.ResultLabel.Text = "Inventory Count Successful. Total value = " + grandTotal.ToString("C2"); // format $99,999.99
                    resultObjs.MessagesBox.Visible = false;
                    break;
                case Type.directoryDNE:
                    resultObjs.ResultLabel.ForeColor = System.Drawing.Color.Red;
                    resultObjs.ResultLabel.Text = "Directory does not exist.";
                    resultObjs.MessagesBox.Visible = false;
                    break;
                case Type.foundNothing:
                    resultObjs.ResultLabel.ForeColor = System.Drawing.Color.Orange;
                    resultObjs.ResultLabel.Text = "Did not find any inventory";
                    resultObjs.MessagesBox.Visible = false;
                    break;
            }
        }

        private static void Print(Type result, GuiResultsObjects resultObjs)
        {
            switch (result)
            {
                case Type.successWithNoReturnedValue:
                    resultObjs.ResultLabel.ForeColor = System.Drawing.Color.Green;
                    resultObjs.ResultLabel.Text = "Inventory Count Successful";
                    resultObjs.MessagesBox.Visible = false;
                    break;
            }
        }

        private static void Print(Type result, List<string> messages, GuiResultsObjects resultObjs)
        {
            switch (result)
            {
                case Type.parseError:
                    resultObjs.ResultLabel.ForeColor = System.Drawing.Color.Orange;
                    resultObjs.ResultLabel.Text = "Could not read the names of 1 or more files:";
                    resultObjs.MessagesBox.Visible = true;
                    PrintMessages(messages, resultObjs);
                    break;
            }
        }

        private static void PrintMessages(List<string> messages, GuiResultsObjects resultObjs)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                resultObjs.MessagesBox.AppendText(messages[i]);
                if (i != messages.Count - 1)
                    resultObjs.MessagesBox.AppendText(Environment.NewLine);
            }
        }
    }
}
