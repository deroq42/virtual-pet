using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualPetSchool.Command;

namespace VirtualPetSchool.Command.Commands {
    internal class ExitCommand : ICommand {
        public void Execute(string[] args) {
            Environment.Exit(0);
        }
    }
}
