using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Webliographer.Model;

namespace Webliographer.Import
{
	public class ProcessTsv
	{
		
		private readonly Dictionary<string, WebReference> _webReferences = new Dictionary<string, WebReference>();

		/// <summary>
		/// Return a single web reference
		/// </summary>
		private WebReference AddWebReference(string line)
		{
			var lineParts = line.Split('\t');
			var wr = new WebReference
			{
				Domain = lineParts[0],
				Link = lineParts[1]
			};
			if (!wr.Link.StartsWith("http")) return null;
			wr.Rank = int.Parse(lineParts[2]);
			wr.Title = lineParts[3];

			if (_webReferences.ContainsKey(wr.Link)) return null;

			_webReferences.Add(wr.Link, wr);
			return wr;
		}

		/// <summary>
		/// Merge a TSV into an existing master TSV
		/// </summary>
		public void MergeTsv(string inputFileMasterPath, string inputFileNewPath, string outputFilePath)
		{
			var lines = File.ReadAllLines(inputFileMasterPath);
			foreach (var line in lines)
			{
				AddWebReference(line);
			}
			Console.WriteLine($"Read {_webReferences.Count} references from input tsv file {inputFileNewPath}.");

			lines = File.ReadAllLines(inputFileNewPath);
			var newReferenceCount = 0;
			foreach (var line in lines)
			{
				var wr = AddWebReference(line);
				if (wr != null)
				{
					newReferenceCount++;
					Console.WriteLine($"New: {wr.Link}");
				}
			}
			Console.WriteLine($"Read {lines.Length-1} references from new tsv file, and added {newReferenceCount} new references to new tsv file.");

			var sb = new StringBuilder();
			sb.AppendLine("Domain\tLink\tRank\tTitle");
			foreach (var wr in _webReferences.Values)
			{
				sb.AppendLine($"{wr.Domain}\t{wr.Link}\t{wr.Rank}\t{wr.Title}");
			}
			File.WriteAllText(outputFilePath, sb.ToString());
			Console.WriteLine($"Wrote {_webReferences.Count} references to output tsv file {outputFilePath}.");
			Console.WriteLine();
		}
	}
}
