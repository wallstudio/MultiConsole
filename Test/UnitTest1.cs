using NUnit.Framework;
using ProgramWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Test
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var outputs = new List<string>();
            var program = new Program(@"""..\..\..\..\TestRunApp\bin\Debug\netcoreapp3.1\TestRunApp.exe"" 12");
            program.OnOutput += l => outputs.Add($"[{DateTime.Now}] {l}");
            program.OnExit += () => outputs.Add($"[{DateTime.Now}] Exited");
            program.Execute().Wait();
        }
    }
}