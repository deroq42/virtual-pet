using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualPetSchool;
using VirtualPetSchool.Command;
using VirtualPetSchool.Http;

namespace VirtualPetSchool.Command.Commands {
    internal class BurnCommand : ICommand {
        private readonly VirtualPetApp _app;

        public BurnCommand(VirtualPetApp app) {
            this._app = app;
        }

        async public void Execute(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Es wird ein Argument erwartet! Nutze 'burn <Name>'");
                return;
            }

            if (_app.IsBurning) {
                return;
            }
            
            string petName = args[0];
            _app.IsBurning = true;
            Console.WriteLine($"{petName} wird verbrannt...");
         
            HttpResponse response = await HttpUtil.DeletePet(petName);
            if (response != null) {
                _app.IsBurning = false;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
                    Console.WriteLine("Es ist ein Fehler aufgetreten. Überprüfe die Logs des Servers");
                    return;

                }
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    Console.WriteLine($"Es wurde kein Tier mit dem Namen {petName} gefunden");
                    return;
                }

                if (_app.CurrentPet != null) {
                    if (_app.CurrentPet.name.Equals(petName)) {
                        lock (_app.CurrentPet) {
                            _app.CurrentPet.StopSimulations();
                            _app.CurrentPet = null;
                        }
                    }
                }
                Console.WriteLine($"{petName} wurde verbrannt");
            }
        }
    }
}
