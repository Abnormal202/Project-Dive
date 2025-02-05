﻿using MyBox;
using Phys;
using UnityEngine;

namespace Mechanics {
    public class Breakable : Solid {
        private GameObject _particles;



        private void Awake()
        {
            _particles = (GameObject) Resources.Load("PS_Breakable");
        }

        public override bool Collidable() {
            Resources.Load("PS_Breakable");
            return true;
        }

        public override bool PlayerCollide(PlayerActor p, Vector2 direction) {
            if (p.IsDiving() && direction.y < 0) {
                Break();
                return false;
            }
            
            return base.PlayerCollide(p, direction);
        }

        public override bool IsGround(PhysObj whosAsking) {
            PlayerActor p = whosAsking.GetComponent<PlayerActor>();
            if (p != null) {
                return !p.IsDiving();
            }
            return true;
        }

        public void Break() {
            gameObject.SetActive(false);
            Instantiate(_particles, transform.position, Quaternion.identity);
            //Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
        }
    }
}