namespace Webliographer
{
	class Program
	{
		static void Main(string[] args)
		{
			var c = new Command();
			c.ParseCommandFile("Command.txt");
		}
	}
}
