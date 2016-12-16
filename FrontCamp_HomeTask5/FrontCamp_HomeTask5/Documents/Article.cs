using System.Collections.Generic;

namespace FrontCamp_HomeTask5.Documents
{
  public class Article
  {
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; }

    public List<int> ListCommnetsId { get; set; }

    public List<int> ListTagsId { get; set; }
  }
}
