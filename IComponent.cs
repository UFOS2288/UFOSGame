using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFOSGame
{
    public interface IComponent
    {
        void Start(Object sender);
        void Update(Object sender);
        void OnDestroy(Object sender);
    }
}
