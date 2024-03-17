using System;
using VirtualPetSchool.Http;
using VirtualPetSchool.Model;

namespace VirtualPetSchool.Command.Commands {
    internal class LoadCommand : ICommand {
        private readonly VirtualPetApp _app;

        public LoadCommand(VirtualPetApp app) {
            this._app = app;
        }

        async public void Execute(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Es wird ein Argument erwartet! Nutze 'load <Name>'");
                return;
            }

            if (_app.IsLoading) {
                return;
            }

            string petName = args[0];
            _app.IsLoading = true;
            Console.WriteLine($"{petName} wird geladen...");

            HttpResponse response = await HttpUtil.LoadPet(petName);
            if (response != null) {
                _app.IsLoading = false;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
                    Console.WriteLine("Es ist ein Fehler aufgetreten. Überprüfe die Logs des Servers");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    Console.WriteLine($"Es gibt kein Tier mit dem Namen {petName}");
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
                Console.WriteLine($"{petName} wurde geladen");
            }
        }
    }
}
