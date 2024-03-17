using System;
using VirtualPetSchool.Http;
using VirtualPetSchool.Model;
using VirtualPetSchool.Model.Simulation;

namespace VirtualPetSchool.Model.Simulation.Simulations {
    internal class HealthSimulation : ISimulation {
        private readonly VirtualPet _pet;

        public HealthSimulation(VirtualPet pet) : base(pet) {
            _pet = pet;
        }

        override async public void Simulate() {
            List<string> critical = new List<string>();
            if (_pet.hunger >= 100) {
                critical.Add("Hunger");
                _pet.health--;
            }

            if (_pet.thirst >= 100) {
                critical.Add("Durst");
                _pet.health--;
            }

            if (_pet.hygiene <= 0) {
                critical.Add("Hygiene");
                _pet.health--;
            }

            if (_pet.bladder >= 100) {
                critical.Add("Blase");
                _pet.health--;
            }

            if (_pet.poop >= 100) {
                critical.Add("Stuhlgang");
                _pet.health--;
            }

            if (_pet.energy <= 0) {
                critical.Add("Energie");
                _pet.health--;
            }

            if (_pet.fun <= 0) {
                critical.Add("Spaß");
                _pet.health--;
            }

            if (_pet.health < 0) {
                _pet.health = 0;
            }

            if (_pet.health != 100) {
                if (_pet.health % 10 == 0) {
                    switch (_pet.health) {
                        case 0:
                            _pet.Die();
                            await HttpUtil.UpdatePet(_pet);
                            break;

                        default:
                            Console.WriteLine($"{_pet.name} verliert an Gesundheit! Kritische Werte: {string.Join(", ", critical)}. Unternimm etwas, bevor es zuspät ist!");
                            break;

                    }
                }
            }
        }

        override public int Timeout() {
            return 3 * 1000;
        }
    }
}
