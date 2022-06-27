namespace Com.Kana.Service.Upload.Lib.ViewModels.GarmentInternNoteViewModel
{
    public class CategoryDto
    {
        public CategoryDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}