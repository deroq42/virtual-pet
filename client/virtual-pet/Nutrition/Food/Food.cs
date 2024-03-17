using System;
using VirtualPetSchool.Nutrition;

namespace VirtualPetSchool.Nutrition.Food {
    internal enum Food {
        [NutritionalValues(40, 30, 0)]
        Steak,
        [NutritionalValues(10, 20, 0)]
        Ice
    }
}
