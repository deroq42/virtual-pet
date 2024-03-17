using System;
using VirtualPetSchool;
using VirtualPetSchool.Command;

namespace VirtualPetSchool.Command.Commands {
    internal class WashCommand : ICommand {
        private readonly VirtualPetApp _app;

        public WashCommand(VirtualPetApp app) {
            this._app = app;
        }

        public void Execute(string[] args) {
            if (_app.CurrentPet == null) {
                Console.WriteLine("Aktuell hast du kein Tier geladen. Nutze 'load <Name>', um ein bestehendes Tier zu laden oder 'create <Name>', um ein neues zu erstellen");
                return;
            }

            lock (_app.CurrentPet) {
                _app.CurrentPet.Wash();
            }
        }
    }
}
