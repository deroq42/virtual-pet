using System;
using VirtualPetSchool;

namespace VirtualPetSchool.Command.Commands {
    internal class HelpCommand : ICommand {
        private readonly VirtualPetApp _app;

        public HelpCommand(VirtualPetApp app) { 
            this._app = app;
        }

        public void Execute(string[] args) {
            _app.Help();
        }
    }
}
