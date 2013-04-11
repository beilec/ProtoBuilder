using System.Windows.Media;

namespace ProtoBuilder.Model {
    public class Segment {
        //  Sort index
        public int OrderId { get; set; }
        //  Basic properties
        public string Name { get; set; }
        public string Description { get; set; }
        public DataType Type { get; set; }
        public SolidColorBrush TypeColor { 
            get {
                var ret = new SolidColorBrush();
                switch (Type) {
                    case DataType.Byte: ret = new SolidColorBrush(Colors.Magenta); break;
                    case DataType.Bytes: ret = new SolidColorBrush(Colors.DarkCyan); break;
                    case DataType.DateTime: ret = new SolidColorBrush(Colors.Green); break;
                    case DataType.Double: ret = new SolidColorBrush(Colors.DarkMagenta); break;
                    case DataType.Int16: ret = new SolidColorBrush(Colors.Blue); break;
                    case DataType.Int32: ret = new SolidColorBrush(Colors.Blue); break;
                    case DataType.Int64: ret = new SolidColorBrush(Colors.Blue); break;
                    case DataType.UInt16: ret = new SolidColorBrush(Colors.DarkBlue); break;
                    case DataType.UInt32: ret = new SolidColorBrush(Colors.DarkBlue); break;
                    case DataType.UInt64: ret = new SolidColorBrush(Colors.DarkBlue); break;
                    case DataType.String: ret = new SolidColorBrush(Colors.Red); break;
                }
                return ret;
            }
        }
        public uint Size { get; set; }
    }

    public enum DataType { Byte, Int16, Int32, Int64, UInt16, UInt32, UInt64, Double, DateTime, String, Bytes }
}
