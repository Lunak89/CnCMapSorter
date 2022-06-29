using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CnCMapSorter
{
	public static class CnCCopy
	{
		private static readonly List<String> FILE_TYPES = new() { ".INI", ".BIN", ".JSON", ".TGA" };
		private static readonly string PATH_TO_CNC = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\CnCRemastered\Local_Custom_Maps\Tiberian_Dawn\";
		private static readonly int PATH_TO_CNC_LENGTH = PATH_TO_CNC.Length;

		public static void DoCopy()
		{
			Dictionary<String, string> mapsInDirectory = new();

			try
			{
				foreach (string map in Directory.GetFiles(PATH_TO_CNC, "*.INI"))
				{
					using StreamReader sr = new(map);

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

				foreach (var map in mapsInDirectory)
				{
					string author = map.Value;
					string mapNameAndPath = map.Key;
					string mapName = map.Key[PATH_TO_CNC_LENGTH..^4]; //C# 8 lol


					List<char> invalidFileChars = Path.GetInvalidPathChars().ToList();
					invalidFileChars.AddRange(Path.GetInvalidFileNameChars().ToList());


					foreach (var invalidChar in invalidFileChars)
					{
						author = author.Replace(Convert.ToString(invalidChar), "");

						if (string.IsNullOrEmpty(author))
						{
							author = "z_Invalid_Name";
						}
					}
					Directory.CreateDirectory(PATH_TO_CNC + author);

					foreach (var fileType in FILE_TYPES)
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

		public static void MoveAllMapsToMainFolder()
		{

		}

	}
}
