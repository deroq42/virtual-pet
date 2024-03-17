using System;
using VirtualPetSchool.Model;
using VirtualPetSchool.Model.Simulation;

namespace VirtualPetSchool.Model.Simulation.Simulations {
    internal class MiscSimulation : ISimulation {
        private const int _FATIGUE_PER_DURATION = 17;
        private const int _HYGIENE_PER_DURATION = 17;
        private const int _FUN_PER_DURATION = 10;

        private readonly VirtualPet _pet;

        public MiscSimulation(VirtualPet pet) : base(pet) {
            _pet = pet;
        }

        override public void Simulate() {
            if (_pet.hygiene > 0) {
                _pet.hygiene -= (int)((double)_pet.age / _HYGIENE_PER_DURATION * 20);
                if (_pet.hygiene < 0) {
                    _pet.hygiene = 0;
                }
            }

            if (!_pet.Sleeping) {
                if (_pet.fatigue < 100) {
                    _pet.fatigue += (int)((double)_pet.age / _FATIGUE_PER_DURATION * 20);
                    _pet.CheckFatigue();
                    if (_pet.fatigue > 100) {
                        _pet.fatigue = 100;
                    }
                }
            }

            if (!_pet.Playing) {
                if (_pet.fun > 0) {
                    _pet.fun -= (1 - _pet.age * 4 / 100) * _FUN_PER_DURATION;
                    if (_pet.fun < 0) {
                        _pet.fun = 0;
                    }
                }
            }
        }

        override public int Timeout() {
            return 5 * 1000;
        }
    }
}
