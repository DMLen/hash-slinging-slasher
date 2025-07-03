namespace hash_slinging_slasher.Models;

public class ImageRecord
{
  public int Id { get; set; }
  public ulong Hash { get; set; }
  public string FileName { get; set; } = string.Empty;
  public string OriginURL { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
}