namespace PPlus.Controls
{
    internal class ItemChartBar
    {
        public ItemChartBar(int id)
        {
            Id = id;
        }
        public int Id { get; }
        public string Label { get; set; }
        public double Value { get; set; }
        public Color? ColorBar { get; set; }
    }
}
