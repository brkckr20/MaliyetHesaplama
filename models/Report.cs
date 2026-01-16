using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.models
{
    public class Report
    {
        public int Id { get; set; }
        [Display(Name = "Form Adı")]
        public string FormName { get; set; }
        [Display(Name = "Rapor Adı")]
        public string ReportName { get; set; }
        [Display(Name = "Rapor Sorgu")]
        public string Query1 { get; set; }
        public string Query2 { get; set; }
        public string Query3 { get; set; }
        public string Query4 { get; set; }
        public string Query5 { get; set; }
        public string DataSource1 { get; set; }
        public string DataSource2 { get; set; }
        public string DataSource3 { get; set; }
        public string DataSource4 { get; set; }
        public string DataSource5 { get; set; }
        public string FormGroup { get; set; }
        public int AppId { get; set; }
    }
}
