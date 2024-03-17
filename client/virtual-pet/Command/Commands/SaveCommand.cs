using System;
using System.Text;
using VirtualPetSchool;
using VirtualPetSchool.Command;
using VirtualPetSchool.Http;
using VirtualPetSchool.Model;

namespace VirtualPetSchool.Command.Commands {
    internal class SaveCommand : ICommand {
        private readonly VirtualPetApp _app;

        public SaveCommand(VirtualPetApp app) {
            this._app = app;
        }

        async public void Execute(string[] args) {
            if (_app.IsSaving) {
                return;
            }

            VirtualPet? pet = _app.CurrentPet;
            if (pet == null) {
                Console.WriteLine("Aktuell ist kein Tier geladen");
                return;
            }

            _app.IsSaving = true;
            Console.WriteLine($"{pet.name} wird gespeichert...");

            HttpResponse response = await HttpUtil.UpdatePet(pet);
            if (response != null) {
                _app.IsSaving = false;
               
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
                    Console.WriteLine("Es ist ein Fehler aufgetreten. Überprüfe die Logs des Servers");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    Console.WriteLine($"Es gibt kein Tier mit dem Namen '{pet.name}'");
                    return;
                }

                Console.WriteLine($"{pet.name} wurde gespeichert");
            }
        }
    }
}
