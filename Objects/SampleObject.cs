using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFOSGame.Components;

namespace UFOSGame.Objects
{
    public class SampleObject : Object
    {
        public override void OnInit()
        {
            data = new ObjectData()
            {
                name = "Sample",
                desc = "sample",
                color = ConsoleColor.White,
                components = new List<IComponent>() { },
                isInWorld = true,
                position = Vector.zero,
                sprite = '&',
                orderInRender = 0,
            };
        }
    }
}
