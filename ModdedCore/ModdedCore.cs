using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Boardgame.Modding;
using Mono.Cecil;
using UnityEngine;

namespace ModdedCore;

// ReSharper disable once UnusedType.Global
public class ModdedCore : DemeoMod
{
    
    private static bool _loaded;

    private static void Load(string libsDir)
    {
        if(_loaded) return; // Dont try to load it again.
    
        foreach (var file in Directory.GetFiles(libsDir, "0Harmony.dll", SearchOption.AllDirectories))
        {
            Console.WriteLine("[ModdedCore] Looking for Harmony DLL");
            if (!CheckDLL(file)) continue;
            Console.WriteLine($"[ModdedCore] Found Harmony DLL: {file}");
            Assembly.LoadFrom(file);
            _loaded = true;
            return;
        }
    }
    
    private static bool CheckDLL(string path)
    {
        try
        {
            var assemblyDef = AssemblyDefinition.ReadAssembly(path);
            return assemblyDef is { MainModule: not null };
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public override void Load()
    {
        // Do Nothing
    }

    public override void OnEarlyInit()
    {
        if(Environment.GetCommandLineArgs().Contains("--modded.console"))
            ConsoleApi.SetupConsole();
        
        // ModLoader does this to load the mod itself so we are safe to do this here.
        // ReSharper disable once AssignNullToNotNullAttribute
        Load(Path.Combine(Path.GetDirectoryName(Application.dataPath), "Libs"));
    }

    public override ModdingAPI.ModInformation ModInformation { get; } = new ()
    {
        name = "ModdedCore",
        description = "Basic modding utilities.",
        version = "0.0.1",
        author = "JustPyrrha",
        isNetworkCompatible = true
    };
}