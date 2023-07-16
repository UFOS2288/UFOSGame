using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFOSGame
{
    public class ObjectData
    {
        public string name = "Null";
        public string desc = "Null";
        public List<string> tags = new List<string>();
        public Vector position = Vector.zero;
        public char sprite = 'A';
        public ConsoleColor color;
        public List<IComponent> components = new List<IComponent>();
        public bool isInWorld = true;
        public int orderInRender = 0;
    }

    public abstract class Object
    {
        public ObjectData data = new ObjectData();
        public abstract void OnInit();

        public static Object SpawnObject(Object obj, Vector position)
        {
            Base.objects.Add(obj);
            obj.OnInit();
            obj.data.position = position;
            foreach (var item in obj.data.components)
            {
                item.Start(obj);
            }
            return obj;
        }
        public static void DestroyObject(Object obj)
        {
            Base.objects.Remove(obj);
            foreach (var item in obj.data.components)
            {
                item.OnDestroy(obj);
            }
        }
        public T GetComponent<T>()
        {
            foreach (IComponent component in data.components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T) component;
                }
            }
            throw new Exception("Object " + data.name + " does not have " + typeof(T) + " component!");
        }
        public bool TryGetComponent<T>(out T component)
        {
            try
            {
                component = GetComponent<T>();
                return true;
            }
            catch 
            {
                component = default(T);
                return false;
            }
        }

    }
    public struct Vector
    {
        public long x, y;
        public Vector(long xpos, long ypos)
        {
            x = xpos;
            y = ypos;
        }
        public override string ToString()
        {
            return x + ", " + y;
        }
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.x + vector2.x, vector1.y + vector2.y);
        }
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.x - vector2.x, vector1.y - vector2.y);
        }
        public static Vector operator *(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.x * vector2.x, vector1.y * vector2.y);
        }
        public static bool operator ==(Vector vector1, Vector vector2)
        {
            return vector1.x == vector2.x && vector1.y == vector2.y;
        }
        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return vector1.x != vector2.x || vector1.y != vector2.y;
        }
        public readonly static Vector zero = new Vector(0,0);
    }
}
