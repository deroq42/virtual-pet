using System;
using System.Text.Json.Serialization;
using VirtualPetSchool.Model.Simulation;
using VirtualPetSchool.Model.Simulation.Simulations;
using VirtualPetSchool.Nutrition;
using VirtualPetSchool.Nutrition.Drink;
using VirtualPetSchool.Nutrition.Food;

namespace VirtualPetSchool.Model {
    internal class VirtualPet {

        public string name { get; set; }
        public DateTime birthDate { get; set; }
        public int hunger { get; set; } = 0;
        public int thirst { get; set; } = 0;
        public int bladder { get; set; } = 0;
        public int poop { get; set; } = 0;
        public int energy { get; set; } = 100;
        public int hygiene { get; set; } = 100;
        public int health { get; set; } = 100;
        public int fun { get; set; } = 100;
        public int fatigue { get; set; } = 0;

        public int age { get; set; }
        public bool alive { get; set; } = true;
        public DateTime? timeOfDeath { get; set; }

        [JsonIgnore]
        public bool Sleeping { get; set; } = false;
        [JsonIgnore]
        public bool Playing { get; set; } = false;

        [JsonIgnore]
        public ISimulation SleepingSimulation { get; set; }
        [JsonIgnore]
        public ISimulation PlayingSimulation { get; set; }
        [JsonIgnore]
        public ISimulation NutritionSimulation { get; set; }
        [JsonIgnore]
        public ISimulation HealthSimulation { get; set; }
        [JsonIgnore]
        public ISimulation AgingSimulation { get; set; }
        [JsonIgnore]
        public ISimulation ToiletSimulation { get; set; }
        [JsonIgnore]
        public ISimulation MiscSimulation { get; set; }

        public VirtualPet() {
            InitSimulations();
        }

        private VirtualPet(string name) {
            this.name = name;
            this.birthDate = DateTime.Now;
            this.alive = true;

            InitSimulations();
        }

        public static VirtualPet Create(string name) {
            return new VirtualPet(name);
        }

        public void Feed(string? foodName) {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (Sleeping) {
                Console.WriteLine($"{name} schläft gerade!");
                return;
            }

            if (foodName == null
                || foodName.Length == 0) {
                Console.WriteLine($"{name} möchte vernünftiges Essen! Folgendes Essen gibt es: {String.Join(", ", FoodUtil.GetFoods())}");
                return;
            }

            Food? food = FoodUtil.GetFood(foodName);
            if (food == null) {
                Console.WriteLine($"Leider gibt es kein {foodName}. Folgendes Essen gibt es: {String.Join(", ", FoodUtil.GetFoods())}");
                return;
            }

            NutritionalValuesAttribute? nutritionalValues = NutritionUtil.GetNutritionalValues(food);
            if (nutritionalValues == null) {
                Console.WriteLine($"Es wurden keine Nährwerte für {foodName} angegeben");
                return;
            }

            this.hunger -= nutritionalValues.Saturation;
            if (hunger > 0) {
                Console.WriteLine($"{name} hat aufgegessen! Er hat nur noch zu {hunger}% Hunger");
            } else if (hunger == 0) {
                Console.WriteLine($"{name} ist satt!");
            } else if (hunger < 0) {
                this.hunger = 0;
                this.health--;
                Console.WriteLine($"{name} ist satt! Aber überfüttere ihn nicht, da er sonst an Gesundheit verliert");
            }

            this.poop += nutritionalValues.Saturation / 2;
            if (poop > 100) {
                this.poop = 0;
            }

            this.energy += nutritionalValues.Energy;
            if (energy > 100) {
                energy = 100;
            }
        }

        public void Drink(string drinkName) {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (Sleeping) {
                Console.WriteLine($"{name} schläft gerade!");
                return;
            }

            if (drinkName == null
                || drinkName.Length == 0) {
                Console.WriteLine($"{name} möchte ein vernünftiges Getränk! Folgende Getränke gibt es: {String.Join(", ", DrinkUtil.GetDrinks())}");
                return;
            }

            Drink? drink = DrinkUtil.GetDrink(drinkName);
            if (drink == null) {
                Console.WriteLine($"Leider gibt es kein {drinkName}. Folgende Getränke gibt es: {String.Join(", ", DrinkUtil.GetDrinks())}");
                return;
            }

            NutritionalValuesAttribute? nutritionalValues = NutritionUtil.GetNutritionalValues(drink);
            if (nutritionalValues == null) {
                Console.WriteLine($"Es wurden keine Nährwerte für {drinkName} angegeben");
                return;
            }

            this.thirst -= nutritionalValues.Saturation;
            if (thirst > 0) {
                Console.WriteLine($"{name} hat ausgetrunken! Er hat nur noch zu {thirst}% Durst");
            } else if (thirst <= 0) {
                Console.WriteLine($"{name} hat keinen Durst mehr!");
                this.thirst = 0;
            }

            this.energy += nutritionalValues.Energy;
            if (energy > 100) {
                this.energy = 100;
            }

            this.bladder += nutritionalValues.Bladder;
            if (bladder > 100) {
                this.bladder = 100;
            }
        }

        public void Pee() {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (Sleeping) {
                Console.WriteLine($"{name} schläft gerade!");
                return;
            }

            if (bladder < 20) {
                Console.WriteLine($"{name} muss noch nicht pinkeln!");
                return;
            }

            Console.WriteLine($"{name} geht pinkeln...");
            Thread.Sleep(bladder * 50);
            this.bladder = 0;
            Console.WriteLine($"{name} ist fertig mit Pipi");
        }

        public void Poo() {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }
           
            if (poop < 30) {
                Console.WriteLine($"{name} muss noch nicht kacken!");
                return;
            }

            Console.WriteLine($"{name} geht kacken...");
            Thread.Sleep(poop * 70);
            this.poop = 0;
            Console.WriteLine($"{name} ist fertig mit Kacken");
        }

        public void Wash() {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (Sleeping) {
                Console.WriteLine($"{name} schläft gerade!");
                return;
            }

            if (hygiene > 80) {
                Console.WriteLine($"{name} muss sich noch nicht waschen!");
                return;
            }

            Console.WriteLine($"{name} wird gewaschen...");
            Thread.Sleep((int) (1000 / hygiene) * 100);
            this.hygiene = 100;
            Console.WriteLine($"{name} ist wieder sauber");
        }

        public void Play() {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (Sleeping) {
                Console.WriteLine($"{name} schläft gerade...");
                return;
            }

            if (Playing) {
                Console.WriteLine($"{name} spielt bereits");
                return;
            }

            this.Playing = true;
            Console.WriteLine($"{name} geht nun spielen...");
        }

        public void Chill() {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (Sleeping) {
                Console.WriteLine($"{name} schläft gerade...");
                return;
            }

            if (!Playing) {
                Console.WriteLine($"{name} spielt gerade nicht!");
                return;
            }

            this.Playing = false;
            Console.WriteLine($"{name} hat aufgehört zu spielen");
        }

        public void Sleep() {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (Sleeping) {
                Console.WriteLine($"{name} schläft bereits!");
                return;
            }

            if (Playing) {
                this.Playing = false;
            }

            this.Sleeping = true;
            Console.WriteLine($"{name} geht jetzt schlafen...");
        }

        public void WakeUp() {
            if (!alive) {
                Console.WriteLine($"{name} ist leider am {timeOfDeath} im Alter von {age} Jahren von uns gegangen! :(");
                return;
            }

            if (!Sleeping) {
                Console.WriteLine($"{name} schläft gerade nicht!");
                return;
            }

            this.Sleeping = false;
            Console.WriteLine($"{name} ist aufgewacht");
        }

        public void Die() {
            this.alive = false;
            this.timeOfDeath = DateTime.Now;
            Console.WriteLine($"{name} ist leider im Alter von {age} Jahren von uns gegangen! :(");
        }

        public void Info() {
            Console.WriteLine("--------");
            Console.WriteLine($"Informationen zu {name}:");
            Console.WriteLine($"Alter: {age}, Geburtstag: {birthDate}");
            Console.WriteLine($"Hunger: {hunger}%, Durst: {thirst}%");
            Console.WriteLine($"Pinkeln: {bladder}%, Kacken: {poop}%");
            Console.WriteLine($"Spaß: {fun}%, Hygiene: {hygiene}%, Müdigkeit: {fatigue}%");
            Console.WriteLine($"Energie: {energy}, Gesundheit: {health}%");
            Console.WriteLine("--------");
        }

        public void CheckHunger() {
            switch (hunger) {
                case 50: case 80: case 90:
                    Console.WriteLine($"{name} hat zu {hunger}% hunger! Füttere ihn");
                    break;

                case 100:
                    Console.WriteLine($"{name} verhungert! Fütter ihn schnell, bevor es zu spät ist");
                    break;
            }
        }

        public void CheckThirst() {
            switch (thirst) {
                case 50: case 80: case 90:
                    Console.WriteLine($"{name} hat zu {thirst}% durst! Gib ihm etwas zu trinken");
                    break;

                case 100:
                    Console.WriteLine($"{name} verdurstet! Gib ihm schnell etwas zu trinken, bevor es zu spät ist");
                    break;
            }
        }

        public void CheckEnergy() {
            switch (energy) {
                case 50: case 30: case 10:
                    Console.WriteLine($"{name} hat nur noch {energy}% Energie!");
                    break;

                case 0:
                    Console.WriteLine($"{name} hat keine Energie mehr! Unternimm schnell etwas dagegen");
                    break;
            }
        }

        public void CheckBladder() {
            switch (bladder) {
                case 50: case 80: case 90:
                    Console.WriteLine($"{name}'s Blase ist zu {bladder}% voll");
                    break;

                case 100:
                    Console.WriteLine($"{name}'s Blase ist voll! Geh mit ihm pinkeln, bevor seine Gesundheit sinkt");
                    break;
            }
        }

        public void CheckPoop() {
            switch (poop) {
                case 50: case 80: case 90:
                    Console.WriteLine($"{name}'s muss zu {poop}% kacken");
                    break;

                case 100:
                    Console.WriteLine($"{name}'s muss dringend kacken! Geh mit ihm kacken, bevor seine Gesundheit sinkt");
                    break;
            }
        }

        public void CheckFatigue() {
            switch (fatigue) {
                case 50:case 80: case 90:
                    Console.WriteLine($"{name} ist zu {fatigue}% müde");
                    break;

                case 100:
                    Console.WriteLine($"{name}'s muss dringend schlafen, bevor seine Gesundheit sinkt!");
                    break;
            }
        }

        private void InitSimulations() {
            this.SleepingSimulation = new SleepingSimulation(this);
            this.PlayingSimulation = new PlayingSimulation(this);
            this.NutritionSimulation = new NutritionSimulation(this);
            this.HealthSimulation = new HealthSimulation(this);
            this.AgingSimulation = new AgingSimulation(this);
            this.ToiletSimulation = new ToiletSimulation(this);
            this.MiscSimulation = new MiscSimulation(this);
        }

        public void StartSimulations() {
            AgingSimulation.Start();
            SleepingSimulation.Start();
            PlayingSimulation.Start();
            NutritionSimulation.Start();
            HealthSimulation.Start();
            ToiletSimulation.Start();
            MiscSimulation.Start();
        }

        public void StopSimulations() {
            AgingSimulation.Stop();
            SleepingSimulation.Stop();
            PlayingSimulation.Stop();
            NutritionSimulation.Stop();
            HealthSimulation.Stop();
            ToiletSimulation.Stop();
            MiscSimulation.Stop();
        }
    }
}
