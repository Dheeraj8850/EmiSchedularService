using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name , object key):base($"{name} with id({key}) was not Found.")
        {
            
        }

        public NotFoundException(string message):base (message) 
        {
            
        }
    }
}
