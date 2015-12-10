using Huahua.Control;
using Huahua.Model;
using Huahua.View;
using System;

namespace Huahua
{
    static class Program
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            App app = new App();
            UIMain m = null;

            if (args.Length > 0)
                m = new UIMain(args[0]);
            else
                m = new UIMain();
            
            app.Run(m);
        }
    }
}
