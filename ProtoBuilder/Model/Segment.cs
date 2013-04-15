using System;
using System.Linq;
using System.Collections.Generic;

namespace ProtoBuilder.Model {
    public class Segment {
        //  Sort index
        public int OrderId { get; set; }
        //  Basic properties
        public string Name { get; set; }
        public string Description { get; set; }
        public DataTypeView Type { get; set; }
        public static List<DataTypeView> GetDataTypes() { 
            return (from object value in Enum.GetValues(typeof (DataType))
                    select new DataTypeView {
                                                Type = (DataType) value
                                            }).ToList();
        }
        public uint Size { get; set; }
    }

    public enum DataType { Byte, Int16, Int32, Int64, UInt16, UInt32, UInt64, Double, DateTime, String, Bytes }
}
