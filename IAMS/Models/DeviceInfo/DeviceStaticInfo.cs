namespace IAMS.Models.DeviceInfo {
    public class DeviceStaticInfo {
        public static readonly Dictionary<int, (string devName, int pointLength)> devType2DbTableAndPointLength = new Dictionary<int, (string, int)>() {
            {0 , ("ems",200)},
            {1 , ("pcc",61)},
            {2 , ("bsm",61)},
            {3 , ("bsu",100)},
            {4 , ("bcu",2000)},
            {5 , ("pcs",300)},
            {6 , ("airlqd",100)},
            {7 , ("thss",20)},
            {8 , ("water",20)},
            {9 , ("gas",20)},
            {10 , ("fire",200)},
            {11 , ("dido",100)}
        };
    }
}
