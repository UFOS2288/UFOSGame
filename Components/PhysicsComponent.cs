using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFOSGame.Properties;

namespace UFOSGame.Components
{
    public class PhysicsComponent : IComponent
    {
        public bool isHaveCollider = true;
        public void Move(Object sender, Vector direction, bool isPlaySound = false)
        {
            if (Base.GetObjectInPosition(sender.data.position + direction, out Object obj))
            {
                if (!((sender.data.position + direction).x > Base.mapBorders ||
                    (sender.data.position + direction).x < -Base.mapBorders ||
                    (sender.data.position + direction).y > Base.mapBorders ||
                    (sender.data.position + direction).y < -Base.mapBorders))
                {
                    if (obj.TryGetComponent(out PhysicsComponent physicsComponent))
                    {
                        if (!physicsComponent.isHaveCollider)
                            sender.data.position += direction;
                    }
                    else
                    {
                        sender.data.position += direction;
                    }
                }
            }
            else
            {
                if (!((sender.data.position + direction).x > Base.mapBorders ||
                    (sender.data.position + direction).x < -Base.mapBorders ||
                    (sender.data.position + direction).y > Base.mapBorders ||
                    (sender.data.position + direction).y < -Base.mapBorders))
                {
                    sender.data.position += direction;
                }
            }
            if (isPlaySound)
                Base.PlaySound(Resources.step);
                //Console.Beep(5000, 100);
            //Base.UpdateScreen();
        }

        public void OnDestroy(Object sender)
        {
            
        }

        public void Start(Object sender)
        {
            
        }

        public void Update(Object sender)
        {
            foreach (Object obj in Base.objects)
            {
                if (obj.data.position == sender.data.position && obj != sender && obj.data.isInWorld && !sender.data.tags.Contains("player") && obj.TryGetComponent(out PhysicsComponent physicsComponent))
                {
                    if (physicsComponent.isHaveCollider == this.isHaveCollider)
                        sender.data.position += new Vector(Manager.Random(-1,1), Manager.Random(-1, 1));
                }
            }
        }
    }
}
