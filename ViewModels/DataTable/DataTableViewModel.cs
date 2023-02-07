namespace ViewModels.DataTable
{
    public class DataTableViewModel
    {
        public string title { get; set; }
        public string data { get; set; }
        public string format { get; set; }
        public string formatValue { get; set; }
        public bool orderable { get; set; }
        public string className { get; set; } = "";
    }
}
