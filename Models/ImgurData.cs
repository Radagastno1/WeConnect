public class ImgurData
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long Datetime { get; set; }
    public string Type { get; set; }
    public bool Animated { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Size { get; set; }
    public int Views { get; set; }
    public int Bandwidth { get; set; }
    public object Vote { get; set; }
    public bool Favorite { get; set; }
    public object Nsfw { get; set; }
    public object Section { get; set; }
    public object Account_url { get; set; }
    public int Account_id { get; set; }
    public bool Is_ad { get; set; }
    public bool In_most_viral { get; set; }
    public bool Has_sound { get; set; }
    public List<object> Tags { get; set; }
    public int Ad_type { get; set; }
    public string Ad_url { get; set; }
    public string Edited { get; set; }
    public bool In_gallery { get; set; }
    public string Deletehash { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
}

public class ImgurResponse
{
    public ImgurData Data { get; set; }
    public bool Success { get; set; }
    public int Status { get; set; }
}
