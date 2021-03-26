using System.Text;

namespace InventoryCounter
{
    public class Error
    {
        StringBuilder _errorMessage;

        private const string ErrorPrefix = "ERROR: ";

        public Error(string errorMsg)
        {
            _errorMessage = new StringBuilder(errorMsg);
        }

        public void AddHierarchyToFolderPath(string folderName)
        {
            _errorMessage.Insert(0, folderName + "/");
        }

        public string Print()
        {
            return ErrorPrefix + _errorMessage.ToString();
        }


    }
}
