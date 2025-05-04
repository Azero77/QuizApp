using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Parser.WordFileParser
{
    public interface IMessager
    {
        void Message(string message);
        event Action<string> Error;
    }
}
