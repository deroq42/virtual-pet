using System;
using VirtualPetSchool.Model;
using VirtualPetSchool.Command;
using VirtualPetSchool.Command.Commands;

namespace VirtualPetSchool {
    internal class VirtualPetApp {
        private readonly CommandMap _commandMap;
        public CommandMap CommandMap { get { return _commandMap; } }
        private readonly CommandTask _commandTask;

        public VirtualPet? CurrentPet { get; set; }
        public bool IsLoading { get; set; } = false;
        public bool IsBreeding { get; set; } = false;
        public bool IsBurning { get; set; } = false;
        public bool IsSaving { get; set; } = false;
        public bool IsListing { get; set; } = false;

        public VirtualPetApp() {
            this._commandMap = new CommandMap();
            this._commandTask = new CommandTask(this);

            _commandMap.Register("help", new HelpCommand(this));
            _commandMap.Register("load", new LoadCommand(this));
            _commandMap.Register("breed", new BreedCommand(this));
            _commandMap.Register("burn", new BurnCommand(this));
            _commandMap.Register("save", new SaveCommand(this));
            _commandMap.Register("list", new ListCommand(this));
            _commandMap.Register("exit", new ExitCommand());
            _commandMap.Register("feed", new FeedCommand(this));
            _commandMap.Register("drink", new DrinkComamnd(this));
            _commandMap.Register("sleep", new SleepCommand(this));
            _commandMap.Register("wakeup", new WakeUpCommand(this));
            _commandMap.Register("play", new PlayCommand(this));
            _commandMap.Register("chill", new ChillCommand(this));
            _commandMap.Register("wash", new WashCommand(this));
            _commandMap.Register("pee", new PeeCommand(this));
            _commandMap.Register("poo", new PooCommand(this));
            _commandMap.Register("info", new InfoCommand(this));

            // On Exit
            AppDomain.CurrentDomain.ProcessExit += (s, e) => DispatchCommand("save", []);
        }

        public void Run() {
            Console.WriteLine("Hi! Möchtest du ein bestehendes Tier laden oder ein neues erstellen?");
            Console.WriteLine("Um ein bestehendes zu laden, nutze 'load <Name>'. Um eins zu züchten, nutze 'breed <Name>'");
            Console.WriteLine("Für weitere Befehle, nutze 'help'");
            
            _commandTask.Start();
        }

        public void Help() {
            Console.WriteLine($"Ungültiger Befehl! Folgende Befehle gibt es: {_commandMap.GetCommands()}");
        }

        public void DispatchCommand(string name, string[] args) {
            ICommand? command = _commandMap.GetCommand(name);
            if (command == null) {
                Console.WriteLine("Manual execute of command '" + name + "' failed: Not found");
                return;
            }

            command.Execute(args);
        }

        public static void Main(string[] args) {
            new VirtualPetApp().Run();
        }
    }
}