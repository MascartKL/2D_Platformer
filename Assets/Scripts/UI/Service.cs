using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class Service
	{
		string path = Directory.GetCurrentDirectory();

		public void writeTofile(string note)
		{
			path = "b:/Repo_g/2dplatform/Assets/SwordInfo.json";
			using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
			{
                
                sw.WriteLine(note);
                sw.Close(); 
			}
            
		}

        public void Readfile()
        {
            path = "b:/Repo_g/2dplatform/Assets/SwordInfo.json";
            StreamReader sr = new StreamReader(path);
            
            if (!File.Exists(path))
            {
                Debug.Log("not found");
            }

            Debug.Log(sr.ReadToEnd());

            var not = sr.ReadToEnd();
 
            Debug.Log(not);
            sr.Close();

           // var computer = JsonConvert.DeserializeObject<AgentDto>(file);

            //sr.ReadLine();

        }
    }
}
