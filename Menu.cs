using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFOSGame.Properties;

namespace UFOSGame
{
    public class MenuData
    {
        public string menuLabel = "BaseMenu";
        public ConsoleKey key = 0;
        public ConsoleColor menuColor = ConsoleColor.White;
        public Stream openSound = Resources.backpack;
        public bool isCloseable = true;
        public List<MenuComponent> components = new List<MenuComponent>();
    }
    public class MenuComponentData
    {
        public string componentName = "I am menu part";
        public ConsoleColor color = ConsoleColor.White;
    }

    public abstract class MenuComponent
    {
        public MenuComponentData data = new MenuComponentData();
        public override string ToString()
        {
            return data.componentName;
        }
        public abstract void OnClick(Menu menu);
        public abstract void OnInit(Menu menu);
    }

    public abstract class Menu
    {
        public MenuData data = new MenuData();

        public virtual void OnInit()
        {
            foreach (var item in data.components)
            {
                item.OnInit(this);
            }
        }
        public abstract void OpenedMenu();
        public abstract void ClosedMenu();
        public abstract void UpdateMenu(ref int selectedInMenu);
        public void MenuScroll(ref int selectedInMenu, int maxScroll, Stream scrollSound)
        {
            if (Base.GetKeyDown(ConsoleKey.W))
            {
                selectedInMenu--;
                if (selectedInMenu < 0)
                {
                    selectedInMenu = maxScroll;
                }
                Base.PlaySound(scrollSound);
            }
            if (Base.GetKeyDown(ConsoleKey.S))
            {
                selectedInMenu++;
                if (selectedInMenu > maxScroll)
                {
                    selectedInMenu = 0;
                }
                Base.PlaySound(scrollSound);
            }
        }
    }
}
