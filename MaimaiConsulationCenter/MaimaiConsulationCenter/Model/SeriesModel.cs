namespace MaimaiConsulationCenter.Model
{
    public class SeriesModel
    {
        public string SeriesName { get; set; }
        public int CurrentValue { get; set; }
        public bool IsGrowing { get; set; }
        public int ChangeRate { get; set; }

    }
}
