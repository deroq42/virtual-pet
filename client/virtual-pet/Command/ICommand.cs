using System;

namespace VirtualPetSchool.Command {
    internal interface ICommand {
        void Execute(string[] args);
    }
}
