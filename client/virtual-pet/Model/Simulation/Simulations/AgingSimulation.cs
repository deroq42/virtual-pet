using System;
using VirtualPetSchool.Http;
using VirtualPetSchool.Model;
using VirtualPetSchool.Model.Simulation;

namespace VirtualPetSchool.Model.Simulation.Simulations {
    internal class AgingSimulation : ISimulation {
        private readonly Random _RANDOM = new Random();

        private readonly VirtualPet _pet;

        public AgingSimulation(VirtualPet pet) : base(pet) {
            _pet = pet;
        }

        override async public void Simulate() {
            _pet.age++;
            if (_pet.age >= 13) {
                if (_RANDOM.Next(100) % 2 == 0) {
                    _pet.Die();
                    await HttpUtil.UpdatePet(_pet);
                }
            }
        }

        override public int Timeout() {
            return 60 * 1000;
        }
    }
}
