namespace LexiElectronics.Models.ViewModels
{
    public class CreateGUID
    {
        public string guid1 { get; set; }
        public string guid2 { get; set; }
        public string guid3 { get; set; }

        public CreateGUID() {
            guid1 = Guid.NewGuid().ToString();
            guid2 = Guid.NewGuid().ToString();
            guid3 = Guid.NewGuid().ToString();
        }
    }
}
