using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace ProtoBuilder.Model {
    public class Segment {
        //  Sort index
        public int OrderId { get; set; }
        //  Basic properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DataTypeView Type { get; set; }
        public bool IsLittleEndian { get; set; }
        public static List<DataTypeView> GetDataTypes() { 
            return (from object value in Enum.GetValues(typeof (DataType))
                    select new DataTypeView {
                                                Type = (DataType) value
                                            }).ToList();
        }
        public uint Size { get; set; }
        public Guid DynamicSizeSegment { get; set; }
        public Visibility IsDynamicSize { 
            get { return Type.Type == DataType.Bytes || Type.Type == DataType.String ? Visibility.Visible : Visibility.Collapsed; } 
        }

        public Segment() {
            Id = Guid.NewGuid();
        }
    }

    public enum DataType { Byte, Int16, Int32, Int64, UInt16, UInt32, UInt64, Double, DateTime, String, Bytes, Float, Decimal}
}
