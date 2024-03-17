using System;
using VirtualPetSchool.Nutrition.Food;
using VirtualPetSchool;
using VirtualPetSchool.Command;

namespace VirtualPetSchool.Command.Commands {
    internal class FeedCommand : ICommand {
        private readonly VirtualPetApp _app;

        public FeedCommand(VirtualPetApp app) {
            this._app = app;
        }

        public void Execute(string[] args) {
            if (_app.CurrentPet == null) {
                Console.WriteLine("Aktuell hast du kein Tier geladen. Nutze 'load <Name>', um ein bestehendes Tier zu laden oder 'create <Name>', um ein neues zu erstellen");
                return;
            }
            
            if (args.Length == 0) {
                Console.WriteLine($"{_app.CurrentPet.name} möchte vernünftiges Essen! Folgendes Essen gibt es: {String.Join(", ", FoodUtil.GetFoods())}");
                return;
            }

            lock (_app.CurrentPet) {
                _app.CurrentPet.Feed(args[0]);
            }
        }
    }
}
