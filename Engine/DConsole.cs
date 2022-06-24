using QuakeConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    internal class DConsole
    {
        private EngineGame Engine;
        private ConsoleComponent qConsole
        {
            get
            {
                return Engine.qConsole;
            }
            set
            {
                Engine.qConsole = value;
            }
        }

        internal PythonInterpreter Interpreter;
        internal DConsole(EngineGame engine)
        {
            Engine = engine;

            Interpreter = new PythonInterpreter();
            InitalizeValues();
            Engine.qConsole.Interpreter = Interpreter;
        }

        internal void InitalizeValues()
        {
            Interpreter.AddVariable(@"engine", Engine.Attributes);
        }
    }
}
