namespace BuberDinner.Infrastructure.UnitTests.Consts
{
    public static partial class Constants
    {
        public static class Menu
        {
            public const string Description = "Menu Description";

            public const string ItemDescription = "Menu Item Descrition";

            public const string ItemName = "Menu Item Name";

            public const string Name = "Menu Name";

            public const string SectionDescription = "Menu Section Description";

            public const string SectionName = "Menu Section Name";

            public static string SectionNameFromIndex(int index) => $"{index}.{SectionName}";
            public static string SectionDescriptionFromIndex(int index) => $"{index}.{SectionDescription}";

            public static string ItemNameFromIndex(int index) => $"{index}.{ItemName}";
            public static string ItemDescriptionFromIndex(int index) => $"{index}.{ItemDescription}";
        }
    }
}
