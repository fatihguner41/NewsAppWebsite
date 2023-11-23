namespace NewsAppWebsite.Models
{
    public class IndexViewModel
    {
        public string type { get; set; }
        public string name { get; set; }
        public IndexViewModel(String type,String name)
        {
            this.type = type; 
            this.name = name ;
        }
    }
}
