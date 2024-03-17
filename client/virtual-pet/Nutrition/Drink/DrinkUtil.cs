using System;

namespace VirtualPetSchool.Nutrition.Drink {
    internal class DrinkUtil {
        public static Drink? GetDrink(string name) {
            if (name == null
                || name.Length == 0) {
                return null;
            }

            if (Enum.TryParse(name, out Drink drink)) {
                return drink;
            }

            return null;
        }

        public static Drink[] GetDrinks() {
            return (Drink[])Enum.GetValues(typeof(Drink));
        }
    }
}
