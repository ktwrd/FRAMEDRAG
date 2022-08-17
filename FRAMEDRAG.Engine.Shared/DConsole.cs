using Microsoft.Xna.Framework;
using QuakeConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class DConsole
    {
        private EngineGame Engine;
        public ConsoleComponent qConsole
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

        public PythonInterpreter Interpreter;
        public DConsole(EngineGame engine)
        {
            Engine = engine;

            Interpreter = new PythonInterpreter();
            InitalizeValues();
            Engine.qConsole.Interpreter = Interpreter;
        }
        public void InitalizeValues()
        {
            Interpreter.AddVariable(@"engine", Engine.Attributes);
            Interpreter.AddType(typeof(Color));
        }
    }
}
