using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Webliographer.Import
{
	public class ProcessMarkdown
	{
		/// <summary>
		/// Extract links from a Markdown file and write to a file in minimal TSV format (Title, Link)
		/// </summary>
		public static void MarkdownToMinimalTsv(string inputFilePath, string outputFilePath)
		{
			var split = new[] {"]("};

			Console.WriteLine($"Reading from input file {inputFilePath}");
			var sb = new StringBuilder();
			var lines = File.ReadAllLines(inputFilePath);
			var count = 0;
			foreach (var line in lines)
			{
				// TODO: This regex doesn't work correctly for nested Markdown links.
				// Example: [![Word](https://example.com/image.png)](https://example.com)
				var matches = Regex.Matches(line, "\\[([^\\[\\]]+)\\]\\(([^)]+)");
				foreach (var match in matches)
				{
					var m = match.ToString();
					var mParts = m.Split(split, StringSplitOptions.RemoveEmptyEntries);
					var title = mParts[0].Remove(0, 1);
					var link = mParts[1];
					if (!link.StartsWith("http")) continue;
					sb.AppendLine($"{title}\t{link}");
					count++;
				}
			}
			File.WriteAllText(outputFilePath, sb.ToString());
			Console.WriteLine($"Wrote {count} references to output tsv file {outputFilePath}.");
		}
	}
}
