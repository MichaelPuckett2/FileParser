using TextFieldParserFramework.FixedWidth;

namespace TextFieldParserFrameworkTests.Models.FixedWidth
{
    [FixedWidth]
    public class TestFixedWidthModel
    {
        [Range(1, 2)]
        public string Roll { get; set; }
        [Range(3, 4)]
        public string Year { get; set; }
        [Range(7, 3)]
        public string Cycle { get; set; }
        [Range(10, 30)]
        public string ParcelID { get; set; }
        [Range(40, 7)]
        public string AlternateKey { get; set; }
        [Range(47, 8)]
        public string CancelDate { get; set; }
        [Range(55, 2)]
        public string Payment_Type { get; set; }
        [Range(57, 12)]
        public string CFNNumber { get; set; }
        [Range(69, 4)]
        public string Cancel { get; set; }
        [Range(73, 1)]
        public string PropClass { get; set; }
        [Range(74, 4)]
        public string GEDBook { get; set; }
        [Range(78, 1)]
        public string Filler { get; set; }
        [Range(79, 4)]
        public string GEDPage { get; set; }
        [Range(83, 10)]
        public string PNOTLINE06 { get; set; }
        [Range(93, 10)]
        public string PNOTLINE07 { get; set; }
        [Range(103, 30)]
        public string PNOTLINE21 { get; set; }
        [Range(133, 30)]
        public string PNOTLINE31 { get; set; }
        [Range(163, 30)]
        public string PNOTLINE33 { get; set; }
        [Range(193, 30)]
        public string PNOTLINE34 { get; set; }
        [Range(223, 30)]
        public string Owner { get; set; }
        [Range(253, 30)]
        public string CoOwner { get; set; }
        [Range(283, 30)]
        public string ADD01 { get; set; }
        [Range(313, 30)]
        public string ADD02 { get; set; }
        [Range(343, 30)]
        public string ADD03 { get; set; }
        [Range(373, 30)]
        public string ADD04 { get; set; }
        [Range(403, 2)]
        public string TaxDistrict { get; set; }
        [Range(405, 20)]
        public string SitusStreet { get; set; }
        [Range(425, 4)]
        public string SitusStreetType { get; set; }
        [Range(429, 2)]
        public string SitusDirection { get; set; }
        [Range(431, 2)]
        public string SitusDirection2 { get; set; }
        [Range(439, 6)]
        public string SitusNumber { get; set; }
        [Range(439, 1)]
        public string SitusSubNumber { get; set; }
        [Range(440, 5)]
        public string SitusApartment { get; set; }
        [Range(445, 3)]
        public string SitusCityCode { get; set; }
        [Range(448, 4)]
        public string SPEPType { get; set; }
        [Range(452, 10)]
        public string SPEPField1 { get; set; }
        [Range(462, 10)]
        public string SPEPField2 { get; set; }
        [Range(472, 10)]
        public string SPEPField3 { get; set; }
        [Range(482, 10)]
        public string SPEPField4 { get; set; }
        [Range(492, 10)]
        public string SPEPField5 { get; set; }
        [Range(502, 10)]
        public string SPEPField6 { get; set; }
        [Range(512, 10)]
        public string SPEPField7 { get; set; }
        [Range(522, 10)]
        public string SPEPField8 { get; set; }
        [Range(532, 1)]
        public string DocType { get; set; }
        public string UsedFields { get; set; }
        public string IgnoredFields { get; set; }
    }
}