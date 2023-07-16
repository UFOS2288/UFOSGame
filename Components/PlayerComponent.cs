using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using UFOSGame.Properties;

namespace UFOSGame.Components
{
    public class PlayerComponent : IComponent
    {
        private PhysicsComponent physics;
        public void OnDestroy(Object sender)
        {
            
        }

        public void Start(Object sender)
        {
            physics = sender.GetComponent<PhysicsComponent>();
        }

        public void Update(Object sender)
        {
            //ResourceManager resourceManager = new ResourceManager();
            if (!Base.isInMenu)
            {
                if (Base.GetKeyDown(ConsoleKey.W))
                {
                    physics.Move(sender, new Vector(0, -1), true);
                }
                if (Base.GetKeyDown(ConsoleKey.D))
                {
                    physics.Move(sender, new Vector(1, 0), true);
                }
                if (Base.GetKeyDown(ConsoleKey.S))
                {
                    physics.Move(sender, new Vector(0, 1), true);
                }
                if (Base.GetKeyDown(ConsoleKey.A))
                {
                    physics.Move(sender, new Vector(-1, 0), true);
                }
                if (Base.GetKeyDown(ConsoleKey.Spacebar))
                {
                    if (Base.GetObjectsInPosition(sender.data.position, out Object[] objs))
                    {
                        foreach (var item in objs)
                        {
                            if (!item.data.tags.Contains("player") && item.TryGetComponent(out EquipabbleComponent equipabbleComponent) && item.data.isInWorld)
                            {
                                Base.PickupObject(item);
                                Base.PlaySound(Resources.backpackPickup);
                            }
                        }
                    }
                }
                Base.SetCameraTarget(sender.data.position);
            }
        }
    }
}
