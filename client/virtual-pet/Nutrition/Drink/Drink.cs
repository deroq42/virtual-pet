using System;

namespace VirtualPetSchool.Nutrition.Drink {
    internal enum Drink {
        [NutritionalValues(30, 10, 20)]
        Water,
        [NutritionalValues(10, 70, 50)]
        Coffee
    }
}
