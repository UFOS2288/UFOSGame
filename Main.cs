using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UFOSGame.Components;
using UFOSGame.Menus;
using UFOSGame.Objects;
using UFOSGame.Properties;

namespace UFOSGame
{
    public class Manager
    {
        private static Random random = new Random();
        public static string ObjArrayToString(Object[] objs, string sep = ", ")
        {
            string a = "";
            foreach (Object o in objs)
            {
                a += o.data.name + sep;
            }
            return a;
        }
        public static string MultiplyStr(string str, int count)
        {
            string done = "";
            for (int i = 0; i < count; i++)
            {
                done += str;
            }
            return done;
        }
        public static int Random(int min, int max)
        {
            return random.Next(min, max);
        }
        public static string GetLabel(string label, string fill = "-")
        {
            return MultiplyStr(fill, (Base.screenWidth / 2) - (label.Length / 2)) + label + MultiplyStr(fill, (Base.screenWidth / 2) - (label.Length / 2)) + fill;
        }
    }


    public static class Base
    {
        public static int screenWidth = 61;
        public static int screenHeight = 21;
        public static float screenSize = 1f;
        public static int mapBorders = 10;
        public static List<Object> objects = new List<Object>();
        public static List<Object> backpack = new List<Object>();
        public static Menu[] regMenus =
        {
            new StartMenu(),
            new InventoryMenu(),
        };
        public static long time = 0;
        public static Vector cameraPos = Vector.zero;
        public static bool isInMenu = false;

        private static SoundPlayer source = new SoundPlayer();
        private static ConsoleKeyInfo pressedKey;
        private static int selectedInMenu = 0;
        private static Menu openedMenu = null;
        public static void Main(string[] args)
        {
            Init();
            while (true)
            {
                if (!isInMenu)
                    time++;
                pressedKey = Console.ReadKey(true);
                UpdateScreen();
                Thread.Sleep(100);
            }
        }
        private static void Init()
        {
            Console.Title = "Renderer";
            Console.Beep(1000, 500);
            Object.SpawnObject(new PlayerObject(), Vector.zero);
            foreach (var item in regMenus)
            {
                item.OnInit();
            }
            OpenMenu(typeof(StartMenu));
            UpdateScreen();
        }
        public static void UpdateScreen()
        {
            Console.SetWindowSize(screenWidth, screenHeight + 30);
            //UpdateComps
            foreach (Object item in objects)
            {
                foreach (var component in item.data.components)
                {
                    component.Update(item);
                }
            }
            //Render
            Console.Clear();
            for (int y = 0; y < screenHeight; y++)
            {
                for (int x = 0; x < screenWidth; x++)
                {
                    bool isDrawed = false;
                    foreach (var obj in objects)
                    {
                        if (obj.data.position.y == y + cameraPos.y && obj.data.position.x == x + cameraPos.x && obj.data.isInWorld)
                        {
                            if (GetObjectInPosition(new Vector(x + cameraPos.x, y + cameraPos.y), out Object objinpos))
                            {
                                if (objinpos.data.orderInRender > obj.data.orderInRender) continue;
                                Console.ForegroundColor = obj.data.color;
                                Console.Write(obj.data.sprite);
                                Console.ResetColor();
                                isDrawed = true;
                            }
                        }
                    }
                    if (isDrawed) continue;
                    if (ScreenPosToWorldPos(new Vector(x, y)).x > mapBorders ||
                        ScreenPosToWorldPos(new Vector(x, y)).x < -mapBorders ||
                        ScreenPosToWorldPos(new Vector(x, y)).y > mapBorders ||
                        ScreenPosToWorldPos(new Vector(x, y)).y < -mapBorders)
                    {
                        WriteWithColor("▓", ConsoleColor.Red);
                        continue;
                    }
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
            WriteLineWithColor(Manager.GetLabel("TIPS"), ConsoleColor.DarkGray);
            WriteLineWithColor("I - Open inventory", ConsoleColor.DarkGray);
            MenuManager();
            //InventoryManager();
            ItemTipManager();
#if DEBUG
            DebugManager();
#endif
            Console.SetWindowPosition(0, 0);
        }
        private static void MenuManager()
        {
            foreach (object menu in regMenus)
            {
                if (GetKeyDown(((Menu) menu).data.key))
                {
                    OpenMenu(menu.GetType());
                }
            }
            if (isInMenu)
            {
                if (openedMenu.data.isCloseable && GetKeyDown(ConsoleKey.Escape))
                {
                    CloseMenu();
                }
                ConsoleColor mencol = openedMenu.data.menuColor;
                WriteLineWithColor(Manager.GetLabel(openedMenu.data.menuLabel), mencol);
                openedMenu.UpdateMenu(ref selectedInMenu);
                WriteLineWithColor(Manager.MultiplyStr("-", screenWidth), mencol);
            }
        }
        private static void ItemTipManager()
        {
            if (!isInMenu)
            {
                if (GetObjectsInPosition(objects[0].data.position, out Object[] objs))
                {
                    foreach (var item in objs)
                    {
                        if (!item.data.tags.Contains("player") && item.TryGetComponent(out EquipabbleComponent equipabbleComponent))
                        {
                            if (item.data.isInWorld)
                            {
                                WriteLineWithColor(Manager.GetLabel("ITEM ON GROUND", "."), ConsoleColor.Cyan);
                                Console.WriteLine(item.data.name);
                                Console.WriteLine(item.data.desc);
                                WriteLineWithColor(Manager.MultiplyStr(".", screenWidth), ConsoleColor.Cyan);
                            }
                        }
                    }
                }
            }
        }
        private static void DebugManager()
        {
            Console.WriteLine(Manager.MultiplyStr("-", screenWidth));
            Console.WriteLine("DEBUG:");
            Console.WriteLine("Time:" + time);
            Console.WriteLine("Objects:" + Manager.ObjArrayToString(objects.ToArray()));
            Console.WriteLine("PlayerPos:" + objects[0].data.position);
            Console.WriteLine("OpenedMenu:" + ((openedMenu is null) ? "None" : openedMenu.data.menuLabel));
            if (GetKeyDown(ConsoleKey.F1))
            {
                Console.Write("Cmd:");
                switch (Console.ReadLine().ToLower())
                {
                    case "decgen":
                        PlaySound(Resources.cheatCode);
                        MapGenerator.GenerateDecals(new MapDecal[]{new MapDecal(0.6)}, 0, mapBorders);
                        foreach (var item in objects)
                        {
                            Console.WriteLine(item.data.name + " , pos:" + item.data.position);
                        }
                        WriteLineWithColor("Done!", ConsoleColor.Green);
                        break;
                    default:
                        PlaySound(Resources.cheatCodeFail);
                        WriteLineWithColor("Fail!", ConsoleColor.DarkRed);
                        break;
                }
            }
            if (GetKeyDown(ConsoleKey.F4))
            {
                PlaySound(Resources.cheatCode);
                Object.SpawnObject(new ArmorObject(), objects[0].data.position);
                //PickupObject(Object.SpawnObject(new ArmorObject(), objects[0].data.position));
            }
        }
        public static bool GetObjectInPosition(Vector pos, out Object obj)
        {
            foreach (var item in objects)
            {
                if (pos == item.data.position)
                {
                    obj = item;
                    return true;
                }
            }
            obj = null;
            return false;
        }
        public static bool GetObjectsInPosition(Vector pos, out Object[] objs)
        {
            List<Object> objes = new List<Object>();
            foreach (var item in objects)
            {
                if (pos == item.data.position)
                {
                    objes.Add(item);
                }
            }
            objs = objes.ToArray();
            return objes.Count > 0;
        }
        public static bool GetKeyDown(ConsoleKey keydown)
        {
            return pressedKey.Key == keydown;
        }
        public static void SetCameraTarget(Vector pos)
        {
            cameraPos = pos - new Vector(screenWidth / 2, screenHeight / 2);
        }
        public static Vector ScreenPosToWorldPos(Vector screenPos)
        {
            //Console.WriteLine(screenPos + cameraPos);
            return screenPos + cameraPos;
        }
        public static void WriteWithColor(string str, ConsoleColor color, bool isResetColor = true)
        {
            Console.ForegroundColor = color;
            Console.Write(str);
            if (isResetColor)
            Console.ResetColor();
        }
        public static void WriteLineWithColor(string str, ConsoleColor color, bool isResetColor = true)
        {
            WriteWithColor(str + "\n", color, isResetColor);
        }
        public static void DebugWriteLine()
        {
            Console.WriteLine("DEBUGTRIGGA!");
        }
        public static void PlaySound(Stream snd)
        {
            //source = new SoundPlayer(snd);
            source.Stream = snd;
            source.Play();
        }
        public static void PickupObject(Object obj)
        {
            backpack.Add(obj);
            obj.data.isInWorld = false;
        }
        public static void DropObject(Object obj)
        {
            backpack.Remove(obj);
            obj.data.isInWorld = true;
            obj.data.position = objects[0].data.position;
        }
        public static void SwitchMenu(Type menuType)
        {
            CloseMenu();
            OpenMenu(menuType);
        }
        public static void OpenMenu(Type menuType)
        {

            foreach (Menu menu in regMenus)
            {
                if (menu.GetType() == menuType)
                {
                    if (openedMenu == menu)
                    {
                        CloseMenu();
                    }
                    else if(openedMenu == null)
                    {
                        selectedInMenu = 0;
                        menu.OpenedMenu();
                        openedMenu = menu;
                        isInMenu = true;
                    }
                    break;
                }
            }
        }
        public static void CloseMenu()
        {
            selectedInMenu = 0;
            openedMenu.ClosedMenu();
            openedMenu = null;
            isInMenu = false;
        }
    }
}
