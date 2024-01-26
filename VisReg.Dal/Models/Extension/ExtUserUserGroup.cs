namespace VisReg.Dal.Models.Extension
{
    public class ExtUserUserGroup
    {
        public short USE_Id { get; set; }

        public short USG_Id { get; set; }

        public string USG_Name { get; set; }

        public string USG_Description { get; set; }

        public bool UserIsBelongUserGroup { get; set; }
    }
}
