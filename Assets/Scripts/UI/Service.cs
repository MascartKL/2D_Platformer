using System.IO;
using System.Text;

namespace Assets.Scripts.UI
{
	public class Service
	{
		string path = Directory.GetCurrentDirectory();

		public void writeTofile(string note)
		{
			path += @"/SwordInfo.json";
			using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
			{
				sw.WriteLine(note);
			}
		}
	}
}
