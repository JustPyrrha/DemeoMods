using System;
using Boardgame;
using UnityEngine;

namespace PyCore.Persistence;

public class PyCorePersistence : MonoBehaviour
{
    private GameContext _context;
    private bool _hasSetVersionInfo;
    private void Awake()
    {
        Console.WriteLine("[PyCore] Persistant GameObject Init.");
        DontDestroyOnLoad(this);
    }

    public void SetGameContext(GameContext context)
    {
        _context = context;
    }
}