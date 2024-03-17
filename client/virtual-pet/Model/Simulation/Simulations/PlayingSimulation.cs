using System;
using VirtualPetSchool.Model;
using VirtualPetSchool.Model.Simulation;

namespace VirtualPetSchool.Model.Simulation.Simulations {
    internal class PlayingSimulation : ISimulation {
        private const int _FUN_PER_DURATION = 12;
        private const int _ENERGY_PER_DURATION = 18;
        private const int _FATIGUE_PER_DURATION = 17;

        private readonly VirtualPet _pet;

        public PlayingSimulation(VirtualPet pet) : base(pet) {
            _pet = pet;
        }

        override public void Simulate() {
            if (_pet.Playing) {
                _pet.fun += (1 - _pet.age * 4 / 100) * _FUN_PER_DURATION;
                if (_pet.fun > 100) {
                    _pet.fun = 100;
                }

                _pet.fatigue += (int)((double)_pet.age / _FATIGUE_PER_DURATION * 20);
                if (_pet.fatigue > 100) {
                    _pet.fatigue = 100;
                }
                _pet.CheckFatigue();

                _pet.energy -= (int)((double)_pet.age / _ENERGY_PER_DURATION * 20);
                _pet.CheckEnergy();
                if (_pet.energy <= 0) {
                    _pet.energy = 0;
                    _pet.Playing = false;
                }
            }
        }

        override public int Timeout() {
            return 5 * 1000;
        }
    }
}
