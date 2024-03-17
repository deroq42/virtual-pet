using System;
using VirtualPetSchool.Model;
using VirtualPetSchool.Model.Simulation;

namespace VirtualPetSchool.Model.Simulation.Simulations {
    internal class ToiletSimulation : ISimulation {
        private const int _BLADDER_PER_DURATION = 20;
        private const int _POOP_PER_DURATION = 25;

        private readonly VirtualPet _pet;

        public ToiletSimulation(VirtualPet pet) : base(pet) {
            _pet = pet;
        }

        override public void Simulate() {
            if (_pet.bladder < 100) {
                _pet.bladder += Math.Max(1, (int)((double)_pet.age / _BLADDER_PER_DURATION * 17));
                _pet.CheckBladder();
                if (_pet.bladder >= 100) {
                    _pet.bladder = 100;
                }
            }

            if (_pet.poop < 100) {
                _pet.poop += Math.Max(1, (int)((double)_pet.age / _POOP_PER_DURATION * 20));
                _pet.CheckPoop();
                if (_pet.poop >= 100) {
                    _pet.poop = 100;
                }
            }
        }

        override public int Timeout() {
            return 5 * 1000;
        }
    }
}
