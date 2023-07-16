using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFOSGame.Components;

namespace UFOSGame.Objects
{
    public class ArmorObject : Object
    {
        public override void OnInit()
        {
            data = new ObjectData()
            {
                name = "Armor",
                desc = "Armor is protects you.",
                color = ConsoleColor.Cyan,
                components = new List<IComponent>() {
                new EquipabbleComponent(),
                new PhysicsComponent()
                {
                    isHaveCollider = false
                }
            },
                isInWorld = true,
                position = Vector.zero,
                sprite = '&'
            };
        }
    }
}
