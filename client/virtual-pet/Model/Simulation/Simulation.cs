using System;
using System.Threading.Tasks;
using VirtualPetSchool.Model;

namespace VirtualPetSchool.Model.Simulation {
    internal abstract class ISimulation {
        protected readonly Thread _thread;
        protected bool _running = false;

        protected ISimulation(VirtualPet pet) {
            this._thread = new Thread(new ThreadStart(() => {
                while (pet.alive) {
                    Simulate();
                    Thread.Sleep(Timeout());
                }
            }));
        }

        public void Start() {
            if (!_running) {
                _thread.Start();
                _running = true;
            }
        }

        public void Stop() {
            if (_running) {
                _thread.Interrupt();
                _running = false;
            }
        }

        public abstract void Simulate();
        public abstract int Timeout();
    }
}
