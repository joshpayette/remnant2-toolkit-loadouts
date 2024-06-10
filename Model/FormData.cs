namespace WebSaves.Model;

public class FormData
{
    public IFormFile saveFile { get; set; }
    public string characterSlot { get; set; }
    public string authToken { get; set; }
}
