using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotHalma.Interfaces
{
    internal interface IValidatable
    {
        public abstract static bool IsValid(IValidatable? validatable);
    }
}
