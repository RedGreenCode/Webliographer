using System;
using System.IO;
using Webliographer.Import;

namespace Webliographer
{
	public class Command
	{
		private string[] _lines;
		private int _pos = -1;
		private string _commandName = string.Empty;

		private string GetArg()
		{
			if (++_pos < _lines.Length)
			{
				return _lines[_pos];
			}
			throw new Exception($"Missing argument for {_commandName} command");
		}

		private void ExecuteCommand()
		{
			try
			{
				switch (_commandName)
				{
					case "jsontotsv": // convert results in JSON format to Webliographer TSV format
						var gs = new ProcessGoogleResults();
						gs.JsonToStandardTsv(GetArg(), GetArg());
						break;
					case "processtsv": // merge two TSVs into a new TSV
						var pt = new ProcessTsv();
						pt.MergeTsv(GetArg(), GetArg(), GetArg());
						break;
					case "processdirectories": // merge all TSVs in a directory into a master.tsv
						var pf = new ProcessFiles();
						pf.ProcessDirectories(GetArg(), GetArg());
						break;
					case "expandtsv": // convert results in minimal TSV format into Webliographer TSV format
						var pgr = new ProcessGoogleResults();
						pgr.MinimalTsvToStandardTsv(GetArg(), GetArg());
						break;
					case "processmd": // extract links from a Markdown file and write to a file in minimal TSV format (Title, Link)
						var pm = new ProcessMarkdown();
						ProcessMarkdown.MarkdownToMinimalTsv(GetArg(), GetArg());
						break;
					default:
						Console.WriteLine($"Unknown command: {_commandName}");
						break;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private void ExecuteCommands()
		{
			while (++_pos < _lines.Length)
			{
				var line = _lines[_pos];
				if (string.IsNullOrWhiteSpace(line) || line[0] == '#') continue;

				_commandName = line.Trim();
				if (_commandName == "$ ExecuteEnd") return;
				ExecuteCommand();
			}
		}

		public void ParseCommandFile(string commandFileName)
		{
			_lines = File.ReadAllLines(commandFileName);
			while (++_pos < _lines.Length)
			{
				var line = _lines[_pos];
				if (string.IsNullOrWhiteSpace(line) || line[0] == '#') continue;

				if (line.Trim() == "$ ExecuteStart") ExecuteCommands();
				if (line.Trim() == "$ ExecuteEnd") break;
			}
		}
	}
}
