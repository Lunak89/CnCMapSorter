﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CnCMapSorter
{
	public static class MoveCnCMapFiles
	{
		private static readonly List<string> FILE_TYPES = new() { ".INI", ".BIN", ".JSON", ".TGA" };
		private static readonly string PATH_TO_CNC = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\CnCRemastered\Local_Custom_Maps\Tiberian_Dawn\";
		private static readonly int PATH_TO_CNC_LENGTH = PATH_TO_CNC.Length;
		private static readonly List<char> invalidFileChars = new();
		private static readonly Dictionary<string, string> mapsInDirectory = new();



		public static void IntoNamedSubFolders()
		{
			InitializeInvalidFileChars();

			try
			{
				GetAllMapsInMainFolder();
				CreateDirectoryAndMoveFile();

			}
			catch (Exception e)
			{
				Debug.WriteLine("The file could not be moved:");
				Debug.WriteLine(e.Message);
			}


		}
		public static void RevertToMainDirectory()
		{
			var folders = Directory.GetDirectories(PATH_TO_CNC);

			foreach (var folder in folders)
			{
				foreach (var file in Directory.GetFiles(folder))
				{
					//Just an idea File.Move(file, $"{PATH_TO_CNC}{author}\\{mapName}{fileType}", true);
				}
			}
		}


		private static void InitializeInvalidFileChars()
		{
			invalidFileChars.AddRange(Path.GetInvalidPathChars().ToList());
			invalidFileChars.AddRange(Path.GetInvalidFileNameChars().ToList()); // should be possible to declare in one line
		}

		private static void GetAllMapsInMainFolder()
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
						author = ("" + line.Split("=")?.GetValue(1)).Trim();
						mapsInDirectory.Add(key: map, value: author);
					}
				}
			}
		}

		private static void CreateDirectoryAndMoveFile()
		{
			foreach (var map in mapsInDirectory)
			{
				string author = map.Value;
				string mapName = map.Key[PATH_TO_CNC_LENGTH..^4]; //C# 8 lol


				SanitizeString(ref author);

				Directory.CreateDirectory(PATH_TO_CNC + author);

				foreach (var fileType in FILE_TYPES)
				{
					try
					{
						File.Move($"{PATH_TO_CNC}{mapName}{fileType}", $"{PATH_TO_CNC}{author}\\{mapName}{fileType}", true);
					}
					catch (DirectoryNotFoundException e)
					{
						Debug.WriteLine("Folder not Found: " + author);
						Debug.WriteLine($"{PATH_TO_CNC}{mapName}{fileType}");
						Debug.WriteLine($"{PATH_TO_CNC}{author}\\{mapName}{fileType}");
						Debug.WriteLine(e.Message);
					}
				}


			}
		}



		private static void SanitizeString(ref string author)
		{
			foreach (var invalidChar in invalidFileChars)
			{
				author = author.Replace(Convert.ToString(invalidChar), "");

				if (string.IsNullOrEmpty(author))
				{
					author = "z_Invalid_Name";
				}
			}
		}



	}
}
