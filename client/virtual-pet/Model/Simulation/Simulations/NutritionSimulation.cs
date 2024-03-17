using System;
using VirtualPetSchool.Model;
using VirtualPetSchool.Model.Simulation;

namespace VirtualPetSchool.Model.Simulation.Simulations {
    internal class NutritionSimulation : ISimulation {
        private const int _HUNGER_PER_DURATION = 9;
        private const int _THIRST_PER_DURATION = 10;
        private readonly Random _RANDOM = new Random();

        private readonly VirtualPet _pet;

        public NutritionSimulation(VirtualPet pet) : base(pet) {
            _pet = pet;
        }

        override public void Simulate() {
            if (_pet.hunger < 100) {
                _pet.hunger += (1 - _pet.age * 4 / 100) * _HUNGER_PER_DURATION;
                _pet.CheckHunger();
                if (_pet.hunger >= 100) {
                    _pet.hunger = 100;
                }
            }

            if (_pet.thirst < 100) {
                _pet.thirst += (1 - _pet.age * 4 / 100) * _THIRST_PER_DURATION;
                _pet.CheckThirst();
                if (_pet.thirst >= 100) {
                    _pet.thirst = 100;
                }
            }

            if (_pet.energy > 0) {
                if (_RANDOM.Next(100) % 2 == 0) {
                    _pet.energy--;
                    _pet.CheckEnergy();
                }
            }
        }

        override public int Timeout() {
            return 5 * 1000;
        }
    }
}
