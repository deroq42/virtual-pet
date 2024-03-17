using System;
using VirtualPetSchool.Model;
using VirtualPetSchool.Model.Simulation;

namespace VirtualPetSchool.Model.Simulation.Simulations {
    internal class SleepingSimulation : ISimulation {
        private const int _ENERGY_PER_DURATION = 7;
        private const int _RECOVERY_PER_DURATION = 10;

        private readonly VirtualPet _pet;

        public SleepingSimulation(VirtualPet pet) : base(pet) {
            _pet = pet;
        }

        override public void Simulate() {
            if (_pet.Sleeping) {
                _pet.energy += (1 - _pet.age * 4 / 100) * _ENERGY_PER_DURATION;
                if (_pet.energy >= 100) {
                    _pet.energy = 100;
                }

                _pet.fatigue -= (1 - _pet.age * 4 / 100) * _RECOVERY_PER_DURATION;
                if (_pet.fatigue <= 0) {
                    _pet.fatigue = 0;
                    _pet.Sleeping = false;
                    Console.WriteLine($"{_pet.name} ist ausgeschlafen!");
                }
            }
        }

        override public int Timeout() {
            return 10 * 1000;
        }
    }
}
