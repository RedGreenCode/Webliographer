using System.Collections.Generic;

namespace Webliographer.Model
{
	/// <summary>
	/// A set of Google search results for a single query.
	/// </summary>
	public class GoogleRoot
	{
		public string effective_query { get; set; }
		public string id { get; set; }
		public string no_results { get; set; }
		public string num_results { get; set; }
		public string num_results_for_query { get; set; }
		public string page_number { get; set; }
		public string query { get; set; }
		public string requested_at { get; set; }
		public string requested_by { get; set; }
		public List<GoogleResult> results { get; set; }
		public string scrape_method { get; set; }
		public string search_engine_name { get; set; }
		public string status { get; set; }
	}
}
