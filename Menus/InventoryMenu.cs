using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFOSGame.Properties;
using UFOSGame;
using UFOSGame.Components;

namespace UFOSGame.Menus
{
    public class InventoryMenu : Menu
    {
        public override void OnInit()
        {
            base.OnInit();
            data = new MenuData()
            {
                menuLabel = "BACKPACK",
                key = ConsoleKey.I,
                menuColor = ConsoleColor.Cyan,
                isCloseable = true,
                openSound = Resources.backpack
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
            MenuScroll(ref selectedInMenu, Base.backpack.Count - 1, Resources.backpackScroll);
            if (Base.GetKeyDown(ConsoleKey.D) && Base.backpack.Count > 0)
            {
                Base.backpack[selectedInMenu].GetComponent<EquipabbleComponent>().OnEquip(Base.objects[0]);
                Base.PlaySound(Resources.backpackSel);
            }
            if (Base.GetKeyDown(ConsoleKey.A) && Base.backpack.Count > 0)
            {
                Base.backpack[selectedInMenu].GetComponent<EquipabbleComponent>().OnDrop(Base.objects[0]);
                Base.DropObject(Base.backpack[selectedInMenu]);
                selectedInMenu = 0;
                Base.PlaySound(Resources.backpackDrop);
            }
            for (int i = 0; i < Base.backpack.Count; i++)
            {
                Console.Write(Base.backpack[i].data.name + " ");
                Base.WriteWithColor(Base.backpack[i].GetComponent<EquipabbleComponent>().isEquipped ? "–Equipped " : "", ConsoleColor.Green);
                Console.WriteLine(selectedInMenu == i ? "◄" : "");
            }

            //TIPS
            try
            {
                Base.WriteLineWithColor(Manager.MultiplyStr(".", Base.screenWidth), ConsoleColor.Cyan);
                Console.WriteLine(Base.backpack[selectedInMenu].data.name);
                Console.WriteLine(Base.backpack[selectedInMenu].data.desc);
                Base.WriteLineWithColor(Manager.MultiplyStr(".", Base.screenWidth), ConsoleColor.Cyan);
            }
            catch
            {
                Console.WriteLine("None");
                Base.WriteLineWithColor(Manager.MultiplyStr(".", Base.screenWidth), ConsoleColor.Cyan);
            }
        }
    }
}
