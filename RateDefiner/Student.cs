namespace RateDefiner
{
    public class Student
    {
        public int ListNumber { get; set; }
        public string NSP { get; set; }
        public string StreamAbreviation { get; set; }
        public int StreamNumber { get; set; }
        public int GroupNumber { get; set; }
        public int SpecCode { get; set; }
        public double Points { get; set; }
        public string GroupCode => StreamAbreviation + "-" + StreamNumber + GroupNumber;
    }
}
