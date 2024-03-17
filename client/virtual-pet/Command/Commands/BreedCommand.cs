using System;
using VirtualPetSchool.Command;
using VirtualPetSchool;
using VirtualPetSchool.Model;
using VirtualPetSchool.Http;

namespace VirtualPetSchool.Command.Commands {
    internal class BreedCommand : ICommand {
        private readonly VirtualPetApp _app;

        public BreedCommand(VirtualPetApp app) {
            this._app = app;
        }

        async public void Execute(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Es wird ein Argument erwartet! Nutze 'breed <Name>'");
                return;
            }

            if (_app.IsBreeding) {
                return;
            }

            VirtualPet pet = VirtualPet.Create(args[0]);
            _app.IsBreeding = true;
            Console.WriteLine($"{args[0]} wird gezüchtet...");

            HttpResponse response = await HttpUtil.BreedPet(pet);
            if (response != null) {
                _app.IsBreeding = false;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
                    Console.WriteLine("Es ist ein Fehler aufgetreten. Überprüfe die Logs des Servers");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Found) {
                    Console.WriteLine($"Es gibt bereits ein Tier mit dem Namen {pet.name}");
                    return;
                }

                if (_app.CurrentPet != null) {
                    lock (_app.CurrentPet) {
                        _app.CurrentPet.StopSimulations();
                    }
                }
                _app.CurrentPet = response.Data as VirtualPet;
                if (_app.CurrentPet != null) {
                    _app.CurrentPet.StartSimulations();
                }
                Console.WriteLine($"{pet.name} wurde gezüchtet!");
            }
        }
    }
}
