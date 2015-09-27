using System.Collections.Generic;
using PDD.Views;

namespace PDD.Models
{
    internal static class MenuStructure
    {
        private static readonly List<MenuItem> MenuItems = new List<MenuItem>
        {
            new MenuItem(0, typeof (RulesPage), "Правила", "rules.png"),
            new MenuItem(1, typeof (SignsPage), "Знаки", "signs.png"),
            new MenuItem(2, typeof (MarkingsPage), "Разметка", "road.png"),
            new MenuItem(3, typeof (TrafficLightsPage), "Светофоры", "traffic_lights.png"),
            new MenuItem(4, typeof (MalfunctionPage), "Неисправности", "malefunctions.png"),
            new MenuItem(5, typeof (PointsmanPage), "Регулировщик", "police.png"),
            new MenuItem(6, typeof (PenaltyPage), "Штрафы", "penalties.png"),
            new MenuItem(7, typeof (ContactsPage), "Контакты", "contacts.png"),
            new MenuItem(8, typeof (TestStartPage), "Тесты", "tests.png"),
        };

        public static List<MenuItem> GetMenuItems()
        {
            return MenuItems;
        }
    }
}
