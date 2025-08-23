namespace Models;



public class DataApiTracks
{
    public string preview { get; set; }
}
public class DataApi
{
    public List<DataApiTracks> data { get; set; }
    public int total { get; set; }
    public string next { get; set; }
}