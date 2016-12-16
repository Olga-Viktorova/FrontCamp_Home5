using System.Collections.Generic;

namespace FrontCamp_HomeTask5.Documents
{
  public class Author
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public List<int> ListArticlesId{ get; set; }
  }
}
