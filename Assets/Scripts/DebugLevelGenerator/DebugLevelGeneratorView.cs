﻿using Gameplay.Enemy.Scriptables;
using Gameplay.Space.Generator;
using Gameplay.Space.SpaceObjects.Scriptables;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DebugLevelGenerator
{
    public sealed class DebugLevelGeneratorView : MonoBehaviour
    {
        [field: SerializeField, Header("Settings")] public SpaceView SpaceView { get; private set; }
        [field: SerializeField] public SpaceConfig SpaceConfig { get; private set; }
        [field: SerializeField] public StarSpawnConfig StarSpawnConfig { get; private set; }
        [field: SerializeField] public LegacyEnemySpawnConfig LegacyEnemySpawnConfig{ get; private set; }

        [field: SerializeField, Header("Stars")] public Tilemap StarTilemap { get; private set; }
        [field: SerializeField ] public TileBase StarTileBase { get; private set; }
    }
}