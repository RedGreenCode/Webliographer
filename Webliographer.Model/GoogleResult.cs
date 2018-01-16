namespace Webliographer.Model
{
	/// <summary>
	/// A single results from a set of Google search results.
	/// </summary>
	public class GoogleResult
	{
		public string domain { get; set; }
		public string id { get; set; }
		public string link { get; set; }
		public string link_type { get; set; }
		public string rank { get; set; }
		public string serp_id { get; set; }
		public string snippet { get; set; }
		public string title { get; set; }
		public string visible_link { get; set; }
	}
}
