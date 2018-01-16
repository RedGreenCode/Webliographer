using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Webliographer.Import
{
	public class ProcessFiles
	{
		/// <summary>
		/// Convert JSON results files to TSV files, and then combine multiple TSV files into a single master TSV.
		/// </summary>
		public void ProcessDirectories(string inputDirectory, string outputDirectory)
		{
			var pgr = new ProcessGoogleResults();
			var files = Directory.EnumerateFiles(inputDirectory, "*.json").ToList();
			Console.WriteLine($"Processing {files.Count} JSON files from input directory.");
			foreach (var file in files)
			{
				var fileParts = file.Split('.');

				var sb = new StringBuilder();
				for (var i = 0; i < fileParts.Length - 1; i++)
				{
					sb.Append($"{fileParts[i]}.");
				}
				sb.Append("tsv");
				var outputFileName = sb.ToString();

				pgr.JsonToStandardTsv($"{file}", $"{outputFileName}");
			}
			Console.WriteLine("Finished writing TSV files to input directory.");

			var pt = new ProcessTsv();
			files = Directory.EnumerateFiles(inputDirectory, "*.tsv").OrderBy(filename => filename).ToList();
			Console.WriteLine($"Processing {files.Count} TSV files.");
			var master = $"{outputDirectory}\\master.tsv";
			var first = true;
			foreach (var file in files)
			{
				if (first)
				{
					Console.WriteLine($"Base input TSV file is {file}");
					File.Copy(file, master, true);
					first = false;
				}
				else
				{
					pt.MergeTsv(master, file, master);
				}
			}
			Console.WriteLine($"Files processed to {master}.");
		}
	}
}
