namespace ProtoBuilder.Utils {
    public class Core {
        public static string GetMask(string value) {
            var ret = "";
            for (var i = 0; i < value.Length + 1; i++) {
                ret = string.Format("{0}9", ret);
            }
            return ret;
        }
    }
}
