using System;
using System.Reflection;

namespace VirtualPetSchool.Nutrition {
    internal class NutritionUtil {
        public static NutritionalValuesAttribute? GetNutritionalValues<T>(T nutrition) {
            if (nutrition == null) {
                return null;
            }

            Type type = nutrition.GetType();
            MemberInfo[] info = type.GetMember(nutrition.ToString());
            object[] attributes = info[0].GetCustomAttributes(typeof(NutritionalValuesAttribute), false);
            return (attributes.Length > 0
                ? (NutritionalValuesAttribute)attributes[0]
                : null);
        }
    }
}
