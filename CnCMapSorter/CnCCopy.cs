using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CnCMapSorter
{
	public static class CnCCopy
	{

		/*
		 * Folder: C:\Users\Lunak\Documents\CnCRemastered\Local_Custom_Maps\Tiberian_Dawn
		Mapdata.ini
			Author

		CreateFOlder "Author"

		Get Name of File

		Move Files NOF.
					BIN
					INI
					JSON
					TGA

*/
		public static void DoCopy()
		{
			string PATH_TO_CNC = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CnCRemastered\Local_Custom_Maps\Tiberian_Dawn\";
			int pathLength = PATH_TO_CNC.Length;
			List<String> fileTypes = new() { ".INI", ".BIN", ".JSON", ".TGA" };

			string[] allMaps = Directory.GetFiles(PATH_TO_CNC, "*.INI");

			try
			{

				Dictionary<String, string> mapsInDirectory = new();


				foreach (string map in allMaps)
				{
					using (StreamReader sr = new(map))
					{

						string? line;
						string author;

						while ((line = sr.ReadLine()) != null)
						{
							if (line.StartsWith("Author="))
							{
								author = line.Split("=")[1];
								mapsInDirectory.Add(key: map, value: author);
							}
						}

					}
				}

				foreach (var map in mapsInDirectory)
				{
					string author = map.Value;
					string mapNameAndPath = map.Key;
					string mapName = map.Key[pathLength..^4]; //C# 8 lol


					List<char> invalidFileChars = Path.GetInvalidPathChars().ToList();
					invalidFileChars.Concat(Path.GetInvalidFileNameChars().ToList());



					//Regex auf nur erlaubte Zeichen

					foreach (var invalidChar in invalidFileChars)
					{


						author = author.Replace(invalidChar.ToString(), "");
					}
					Directory.CreateDirectory(PATH_TO_CNC + author);

					foreach (var fileType in fileTypes)
					{
						File.Move($"{PATH_TO_CNC}{mapName}{fileType}", $"{PATH_TO_CNC}{author}\\{mapName}{fileType}", true);
					}


				}

			}
			catch (Exception e)
			{
				Debug.WriteLine("The file could not be read:");
				Debug.WriteLine(e.Message);
			}


		}

	}
}
