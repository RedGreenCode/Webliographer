namespace Webliographer.Model
{
	/// <summary>
	/// A single web reference (link/URL).
	/// </summary>
	public class WebReference
	{
		public string Domain { get; set; }
		public string Link { get; set; }
		public int Rank { get; set; }
		public string Title { get; set; }
	}
}
