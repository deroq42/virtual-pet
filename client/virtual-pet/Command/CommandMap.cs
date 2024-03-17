using System;
using System.Collections.Generic;

namespace VirtualPetSchool.Command {
    internal class CommandMap {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public void Register(string name, ICommand command) {
            _commands.Add(name.ToLower(), command);
        }

        public ICommand? GetCommand(string name) {
            return _commands.GetValueOrDefault(name, null);
        }

        public string GetCommands() {
            return String.Join(", ", _commands.Keys);
        }
    }
}
