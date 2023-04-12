using System;
using Services;
using UnityEngine;
using Gameplay.Space.SpaceObjects.Scriptables;
using Utilities.Mathematics;

namespace Gameplay.Space.Planets
{
    public class PlanetMovement : IDisposable
    {
        private readonly PlanetView _view;
        private readonly Updater _updater;

        private readonly Rigidbody2D _planetRigidbody;
        private readonly Transform _spaceObjectTransform;
        private readonly Vector3 _rotationAxis;
         
        private readonly float _speed;
        private const float SpeedMultiplier = 0.001f;

        public PlanetMovement(PlanetView view, Updater updater, PlanetConfig planetConfig, Transform spaceObjectTransform)
        {
            _view = view;
            _updater = updater;
            _spaceObjectTransform = spaceObjectTransform;
            _planetRigidbody = _view.GetComponent<Rigidbody2D>();
            _speed = (RandomPicker.PickRandomBetweenTwoValues(planetConfig.MinSpeed, planetConfig.MaxSpeed)) * SpeedMultiplier;
            _rotationAxis = RandomPicker.TakeChance(planetConfig.RetrogradeMovementChance) ? Vector3.forward : Vector3.back;

            _updater.SubscribeToFixedUpdate(Move);
        }
       
        public void Dispose()
        {
            _updater.UnsubscribeFromFixedUpdate(Move);
        }

        private void Move()
        {
            RotatePlanetAroundSpaceObject(_planetRigidbody, _spaceObjectTransform.position, _rotationAxis, _speed);
        }
        
        private void RotatePlanetAroundSpaceObject(Rigidbody2D planetRigidBody, Vector3 spaceObjectPosition, Vector3 rotationAxis, float speed)
        {
            Quaternion q = Quaternion.AngleAxis(speed, rotationAxis);
            planetRigidBody.MovePosition(q * (planetRigidBody.transform.position - spaceObjectPosition) + spaceObjectPosition);
            planetRigidBody.MoveRotation(planetRigidBody.transform.rotation * q);
        }
    }
}