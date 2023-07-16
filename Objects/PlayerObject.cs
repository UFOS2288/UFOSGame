using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFOSGame.Components;

namespace UFOSGame.Objects
{
    public class PlayerObject : Object
    {
        public override void OnInit()
        {
            data = new ObjectData()
            {
                name = "PlayerObject",
                desc = "A player.",
                tags = new List<string>() { "player" },
                color = ConsoleColor.Green,
                components = new List<IComponent>() { new PhysicsComponent(), new PlayerComponent() },
                isInWorld = true,
                position = Vector.zero,
                orderInRender = 5,
                sprite = '#'
            };
        }
    }
}
