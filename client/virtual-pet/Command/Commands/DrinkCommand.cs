using System;
using VirtualPetSchool.Nutrition.Drink;
using VirtualPetSchool;
using VirtualPetSchool.Command;

namespace VirtualPetSchool.Command.Commands {
    internal class DrinkComamnd : ICommand {
        private readonly VirtualPetApp _app;

        public DrinkComamnd(VirtualPetApp app) {
            this._app = app;
        }

        public void Execute(string[] args) {
            if (_app.CurrentPet == null) {
                Console.WriteLine("Aktuell hast du kein Tier geladen. Nutze 'load <Name>', um ein bestehendes Tier zu laden oder 'create <Name>', um ein neues zu erstellen");
                return;
            }

            if (args.Length == 0) {
                Console.WriteLine($"{_app.CurrentPet.name} möchte ein vernünftiges Getränk! Folgende Getränke gibt es: {String.Join(", ", DrinkUtil.GetDrinks())}");
                return;
            }

            lock (_app.CurrentPet) {
                _app.CurrentPet.Drink(args[0]);
            }
        }
    }
}
