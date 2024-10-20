namespace MaimaiConsulationCenter.Model
{
    public class CategoryItemModel
    {
        public CategoryItemModel(string name, bool state = false)
        {
            CategoryName = name;
            IsSelected = state;
        }
        public bool IsSelected { get; set; }
        public string CategoryName { get; set; }
    }
}
