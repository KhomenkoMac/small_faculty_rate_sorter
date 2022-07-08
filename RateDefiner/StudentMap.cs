using CsvHelper.Configuration;

namespace RateDefiner
{
    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Map(s => s.ListNumber).Index(0).Name("Номер у списку");
            Map(s => s.NSP).Index(1).Name("ПІБ");
            Map(s => s.Points).Index(2).Name("Середній бал за дисципліни");
            Map(s => s.SpecCode).Index(3).Name("Спеціальність");
            Map(s => s.GroupCode).Index(4).Name("Група");
        }
    }
}
