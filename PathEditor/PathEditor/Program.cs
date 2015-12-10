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
            UIMain m = new UIMain();
            app.Run(m);
        }
    }
}
