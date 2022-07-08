using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RateDefiner
{
    class Program
    {
        private static readonly string csvFileName = "output.csv";
        private static readonly string csv_output_path;

        private static readonly string pdfFileName = "input.pdf";
        private static readonly string pdf_input_path;

        static Program()
        {
            csv_output_path = $"./{csvFileName}";
            pdf_input_path = $"./{pdfFileName}";
        }

        static void Main(string[] args)
        {
            string text = PDFExtractor.PDFText(pdf_input_path);
            text = text.Replace("\r\n", "|");

            var regex = new Regex(@"(?<NSP>[А-Яа-яёЁЇїІіЄєҐґ`\-]*\s[А-Яа-яёЁЇїІіЄєҐґ`\-]*\s[А-Яа-яЇїІіЄєҐґ`\-]*)\s(?<streamAbrev>[А-Я]{2})-(?<streamNumber>\d{1})(?<groupNumber>\d{1})(?<magisterAbrev>[мпн]*)\s(?<specCode>[\d]{3})\s(?<listNumber>\d{1,2})(?:\s|[\W]{1})(?<points>\d{1,}\.\d{2})");
            var rows = regex.Matches(text);

            var facultyStudentRateTable = ConvertToStudentsTable(rows);

            var facultyStreams = from student in facultyStudentRateTable
                                 orderby student.Points descending
                                 group student by new Tuple<string, int>(student.StreamAbreviation, student.StreamNumber);

            ExportStudentsToCSV(facultyStreams);

            Console.WriteLine("Succeed!");
        }

        private static void ExportStudentsToCSV(IQueryable<IGrouping<Tuple<string, int>, Student>> facultyStreams)
        {
            using var writer = new StreamWriter(csv_output_path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<StudentMap>();

            csv.WriteHeader<Student>();
            csv.NextRecord();

            facultyStreams.ToList().ForEach(g =>
            {
                g.ToList().ForEach(s =>
                {
                    csv.WriteRecord(s);
                    csv.NextRecord();
                });
                csv.NextRecord();
            });
        }

        static IQueryable<Student> ConvertToStudentsTable(MatchCollection rows) // giving a user a convinient collection to manipulate
        {
            return rows.Select(m => new Student()
            {
                ListNumber = int.Parse(m.Groups["listNumber"].Value),
                NSP = m.Groups["NSP"].Value,
                StreamAbreviation = m.Groups["magisterAbrev"] is null ? m.Groups["streamAbrev"].Value : m.Groups["streamAbrev"].Value + m.Groups["magisterAbrev"],
                StreamNumber = int.Parse(m.Groups["streamNumber"].Value),
                GroupNumber = int.Parse(m.Groups["groupNumber"].Value),
                Points = double.Parse(m.Groups["points"].Value),
                SpecCode = int.Parse(m.Groups["specCode"].Value)
            }).AsQueryable();
        }
    }
}
