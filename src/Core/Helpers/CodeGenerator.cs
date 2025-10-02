using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Helpers
{
    public static class CodeGenerator
    {
        private static readonly Random _random = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateCode(int length)
        {
            return new string(Enumerable.Range(0, length)
              .Select(_ => _chars[_random.Next(_chars.Length)]).ToArray());
        }
    }
}
