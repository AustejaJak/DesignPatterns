using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Bloons
{
    public interface IBloonPrototype
    {
        Bloon CloneToType(Type targetType);
    }
}
