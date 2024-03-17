using System;

namespace VirtualPetSchool.Nutrition.Food {
    internal class FoodUtil {
        public static Food? GetFood(string name) {
            if (name == null
                || name.Length == 0) {
                return null;
            }

            if (Enum.TryParse(name, out Food food)) {
                return food;
            }

            return null;
        }

        public static Food[] GetFoods() {
            return (Food[]) Enum.GetValues(typeof(Food));
        }
    }
}
