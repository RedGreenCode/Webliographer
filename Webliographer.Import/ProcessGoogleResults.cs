using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Webliographer.Model;

namespace Webliographer.Import
{
	public class ProcessGoogleResults
	{
		/// <summary>
		/// Parse JSON object to C# object
		/// </summary>
		private IEnumerable<GoogleRoot> ParseFile(string fileName)
		{
			var str = File.ReadAllText(fileName);
			return JsonConvert.DeserializeObject<List<GoogleRoot>>(str);
		}

		/// <summary>
		/// Convert search results in JSON format to Webliographer TSV format
		/// </summary>
		public void JsonToStandardTsv(string inputFilePath, string outputFilePath)
		{
			Console.WriteLine($"Reading from input file {inputFilePath}");
			var root = ParseFile(inputFilePath);
			var sb = new StringBuilder();
			sb.AppendLine("Domain\tLink\tRank\tTitle");
			var rank = 1;
			var count = 0;
			foreach (var page in root)
			{
				foreach (var r in page.results)
				{
					sb.AppendLine($"{r.domain}\t{r.link}\t{rank++}\t{r.title}");
					count++;
				}
			}
			File.WriteAllText(outputFilePath, sb.ToString());
			Console.WriteLine($"Wrote {count} results to output file {outputFilePath}");
		}

		/// <summary>
		/// Convert search results in minimal TSV format (Title, Link) to Webliographer TSV format
		/// </summary>
		public void MinimalTsvToStandardTsv(string inputFilePath, string outputFilePath)
		{
			Console.WriteLine($"Reading from input file {inputFilePath}");
			var sb = new StringBuilder();
			sb.AppendLine("Domain\tLink\tRank\tTitle");
			var lines = File.ReadAllLines(inputFilePath);
			var rank = 1;
			foreach (var line in lines)
			{
				var lineParts = line.Split('\t');
				var title = lineParts[0];
				var link = lineParts[1];
				var uri = new Uri(link);
				sb.AppendLine($"{uri.Host}\t{link}\t{rank++}\t{title}");
			}
			File.WriteAllText(outputFilePath, sb.ToString());
			Console.WriteLine($"Wrote {rank-1} references to output tsv file {outputFilePath}.");
		}
	}
}
