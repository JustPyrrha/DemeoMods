using System;
using System.Linq;
using Boardgame.Modding;
using HarmonyLib;
using PyCore.Persistence;
using UnityEngine;

namespace PyCore;

public static class ModInfo
{
    public static ModdingAPI.ModInformation ModInformation { get; } = new()
    {
        name = "PyCore",
        description = "Utilities for my mods.",
        version = "0.1.0-alpha",
        author = "JustPyrrha",
        isNetworkCompatible = true,
    };
}

// ReSharper disable once UnusedType.Global
public class PyCore : DemeoMod
{
    public override ModdingAPI.ModInformation ModInformation => ModInfo.ModInformation;

    public override void OnEarlyInit()
    {
        Console.WriteLine("[PyCore] Loading Patches...");
        
        var harmony = new Harmony("gay.pyrrha.PyCore");
        harmony.PatchAll();
        
        Console.WriteLine($"[PyCore] Loaded {harmony.GetPatchedMethods().Count()} Patches.");
    }

    public override void Load()
    {
        Console.WriteLine($"[PyCore] v{ModInformation.version}");
        Console.WriteLine("[PyCore] Loading...");
        var persistenceGo = new GameObject("PyCore__Persistence");
        var persistence = persistenceGo.AddComponent<PyCorePersistence>();
        persistence.SetGameContext(gameContext);
        
        Console.WriteLine("[PyCore] Loaded.");
    }
}