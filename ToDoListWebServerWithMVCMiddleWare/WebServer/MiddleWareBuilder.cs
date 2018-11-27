using System;
using System.Collections.Generic;

namespace ToDoListWebServerWithMiddleWare.WebServer
{
    public class MiddleWareBuilder
    {
        private Stack<Type> types = new Stack<Type>();

        public MiddleWareBuilder Use<T>()
        {
            types.Push(typeof(T));
            return this;
        }

        public DelegateMiddleWare Build()
        {
            DelegateMiddleWare first = null;

            while (types.Count > 0)
            {
                IMiddleWare mw = Activator.CreateInstance(types.Pop(), first) as IMiddleWare;

                first = mw.InvokeAsync;
            }

            return first;
        }
    }
}