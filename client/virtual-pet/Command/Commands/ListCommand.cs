using System;
using VirtualPetSchool;
using VirtualPetSchool.Command;
using VirtualPetSchool.Http;
using VirtualPetSchool.Model;

namespace VirtualPetSchool.Command.Commands {
    internal class ListCommand : ICommand {
        private readonly VirtualPetApp _app;

        public ListCommand(VirtualPetApp app) {
            this._app = app;
        }

        async public void Execute(string[] args) {
            if (_app.IsListing) {
                return;
            }
           
            bool alive = false;
            if (args.Length > 0) {
                if ("alive".Equals(args[0].ToLower())) {
                    alive = true;
                }
            }

            _app.IsListing = true;
            Console.WriteLine("Lade Tiere...");

            HttpResponse response = await HttpUtil.ListPets(alive);
            if (response != null) {
                _app.IsListing = false;
                
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
                    Console.WriteLine("Es ist ein Fehler aufgetreten. Überprüfe die Logs des Servers");
                    return;
                }

                VirtualPet[] pets = response.Data as VirtualPet[];
                if (pets.Length == 0) {
                    Console.WriteLine($"Es wurden keine Tiere gefunden!");
                    return;
                }
             
                string petNames = "";
                foreach (VirtualPet pet in pets) {
                    petNames = petNames + String.Join(", ", pet.name); 
                }
                Console.WriteLine($"Folgende Tiere gibt es: {petNames}");
            }
        }

    }
}
