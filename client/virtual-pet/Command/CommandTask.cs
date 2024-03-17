using System;
using VirtualPetSchool;

namespace VirtualPetSchool.Command {
    internal class CommandTask  {
        private readonly VirtualPetApp _app;

        public CommandTask(VirtualPetApp app) {
            this._app = app;
        }

        public void Start() {
            while (true) {
                string[] commandArgs = Console.ReadLine().Split(" ");
                if (commandArgs.Length == 0) {
                    continue;
                }
                
                string commandName = commandArgs[0].ToLower();
                ICommand command = _app.CommandMap.GetCommand(commandName);
                if (command == null) {
                    _app.Help();
                    continue;
                }

                command.Execute(commandArgs.Skip(1).ToArray());
            }
        }
    }
}
