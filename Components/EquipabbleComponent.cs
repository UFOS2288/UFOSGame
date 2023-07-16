using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFOSGame.Components
{
    public class EquipabbleComponent : IComponent
    {
        public bool isEquipped = false;
        public void OnDestroy(Object sender)
        {
            
        }

        public void Start(Object sender)
        {
            
        }

        public void Update(Object sender)
        {
            
        }
        public void OnEquip(Object sender)
        {
            isEquipped = !isEquipped;
            if (isEquipped)
            {

            }
        }
        public void OnDrop(Object sender)
        {
            isEquipped = false;
        }
    }
}
