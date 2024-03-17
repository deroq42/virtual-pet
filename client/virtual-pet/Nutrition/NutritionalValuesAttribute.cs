using System;

namespace VirtualPetSchool.Nutrition {
    sealed class NutritionalValuesAttribute : Attribute {
        public int Saturation { get; private set; }
        public int Energy { get; private set; }
        public int Bladder { get; private set; }

        public NutritionalValuesAttribute(int saturation, int energy, int bladder){
            Saturation = saturation;
            Energy = energy;
            Bladder = bladder;
        }
    }
}
