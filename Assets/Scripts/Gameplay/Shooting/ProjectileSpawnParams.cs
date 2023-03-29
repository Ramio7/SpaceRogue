using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Shooting.Scriptables;
using UnityEngine;

namespace Gameplay.Shooting
{
    public sealed class ProjectileSpawnParams
    {
        public Vector2 Position { get; }
        public Quaternion Rotation { get; }
        public EntityType EntityType { get; }
        public ProjectileConfig Config { get; }

        public ProjectileSpawnParams(Vector2 position, Quaternion rotation, EntityType entityType, ProjectileConfig config)
        {
            Position = position;
            Rotation = rotation;
            EntityType = entityType;
            Config = config;
        }

        public void Deconstruct(out Vector2 position, out Quaternion rotation, out ProjectileConfig config, out EntityType entityType)
        {
            position = Position;
            rotation = Rotation;
            config = Config;
            entityType = EntityType;
        }
    }
}