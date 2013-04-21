using System.Windows.Media;

namespace ProtoBuilder.Model {
    public class DataTypeView {
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
                    case DataType.Float: ret = new SolidColorBrush(Colors.MediumSlateBlue); break;
                    case DataType.Decimal: ret = new SolidColorBrush(Colors.RoyalBlue); break;
                }
                return ret;
            }
        }
        
        public static uint SizeOfType(DataType dataType) {
            uint ret = 0;

            switch (dataType) {
                case DataType.Byte: ret = 1; break;
                case DataType.DateTime: ret = 8; break;
                case DataType.Double: ret = 8; break;
                case DataType.Int16: ret = 2; break;
                case DataType.Int32: ret = 4; break;
                case DataType.Int64: ret = 8; break;
                case DataType.UInt16: ret = 2; break;
                case DataType.UInt32: ret = 4; break;
                case DataType.UInt64: ret = 8; break;
                case DataType.Float: ret = 4; break;
                case DataType.Decimal: ret = 16; break;
            }

            return ret;
        }
    }
}
