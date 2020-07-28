//using System.Linq;

namespace Discord_New
{
    public class FormData
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}\r\nValue: {1}\r\n----------------------------\r\n", Name, Value);
        }
    }
}