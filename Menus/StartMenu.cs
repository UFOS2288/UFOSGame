using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFOSGame.Properties;

namespace UFOSGame.Menus
{
    public class SwitchMenuComponent : MenuComponent
    {
        public Type nextMenu;
        public SwitchMenuComponent(string name, Type menu, ConsoleColor color = ConsoleColor.White)
        {
            data.componentName = name;
            data.color = color;
            nextMenu = menu;
        }
        public override void OnClick(Menu menu)
        {
            Base.SwitchMenu(nextMenu);
        }
        public override void OnInit(Menu menu)
        {
            //data = new MenuComponentData()
            //{
            //    componentName = "Back",
            //    color = ConsoleColor.Gray,
            //};
        }
    }
    public class CloseMenuComponent : MenuComponent
    {
        public CloseMenuComponent(string name, ConsoleColor color = ConsoleColor.White)
        {
            data.componentName = name;
            data.color = color;
        }
        public override void OnClick(Menu menu)
        {
            Base.CloseMenu();
        }
        public override void OnInit(Menu menu)
        {
            //data = new MenuComponentData()
            //{
            //    componentName = "Back",
            //    color = ConsoleColor.Gray,
            //};
        }
    }

    public class StartMenu : Menu
    {
        public override void OnInit()
        {
            base.OnInit();
            data = new MenuData()
            {
                menuLabel = "GAME",
                key = ConsoleKey.Escape,
                menuColor = ConsoleColor.Green,
                openSound = Resources.backpack,
                isCloseable = false,
                components = new List<MenuComponent>()
                {
                    new CloseMenuComponent("Start game!", ConsoleColor.Green),
                }
            };
        }
        public override void ClosedMenu()
        {
            
        }

        public override void OpenedMenu()
        {
            
        }

        public override void UpdateMenu(ref int selectedInMenu)
        {
            MenuScroll(ref selectedInMenu, data.components.Count - 1, Resources.backpackScroll);
            for (int i = 0; i < data.components.Count; i++)
            {
                MenuComponent component = data.components[i];
                Base.WriteLineWithColor(component.data.componentName + (i == selectedInMenu ? "◄" : ""), component.data.color);
                if (i == selectedInMenu && Base.GetKeyDown(ConsoleKey.Spacebar))
                {
                    component.OnClick(this);
                }
            }
        }
    }
}
