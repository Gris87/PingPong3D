//
//  CustomResxImporter.cs
//
// Creates or rewrites a .txt file for each .resx file in a subfolder called 
// GeneratedAssets whenever the .resx changes
//
// Copyright (c) 2013 Niklas Borglund. All rights reserved.
// @NiklasBorglund

using UnityEditor;
using UnityEngine;
using System.IO;

public class CustomResxImporter : AssetPostprocessor 
{
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (asset.EndsWith(".resx"))
            {
                string filePath = asset.Substring(0, asset.Length - Path.GetFileName(asset).Length) + "Generated Assets/";
                string newFileName = filePath + Path.GetFileNameWithoutExtension(asset) + ".txt";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
				
				//Delete the file if it already exists
				if(File.Exists(newFileName))
				{
					File.Delete(newFileName);	
				}

                StreamReader reader = new StreamReader(asset);
                string fileData = reader.ReadToEnd();
                reader.Close();

                FileStream resourceFile = new FileStream(newFileName, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(resourceFile);
                writer.Write(fileData);
                writer.Close();
                resourceFile.Close();

                AssetDatabase.Refresh(ImportAssetOptions.Default);
            }
        }
    }

}
