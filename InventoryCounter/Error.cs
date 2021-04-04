using System.Text;

namespace InventoryCounter
{
    public class Error
    {
        public enum Type
        {
            value,
            date,
            noDescription
        }

        StringBuilder _errorMessage;
        Type _type;
        private const string ErrorPrefixValue = "ERROR, could not read value: ";
        private const string ErrorPrefixDate = "ERROR, could not read date: ";
        private const string ErrorPrefixNoDescription = "ERROR, no description: ";
        private const string ErrorPrefixUnknown = "ERROR, unknown: ";

        public Error(string errorMsg, Type type)
        {
            _errorMessage = new StringBuilder(errorMsg);
            _type = type;
        }

        public void AddHierarchyToFolderPath(string folderName)
        {
            _errorMessage.Insert(0, folderName + "/");
        }

        public string Print()
        {
            switch (_type)
            {
                case Type.value:
                    return ErrorPrefixValue + _errorMessage.ToString();
                case Type.date:
                    return ErrorPrefixDate + _errorMessage.ToString();
                case Type.noDescription:
                default:
                    return ErrorPrefixUnknown + _errorMessage.ToString();
            }
            
        }


    }
}
