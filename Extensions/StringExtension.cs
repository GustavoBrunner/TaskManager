using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Extensions
{
    public static class StringExtensions
    {
        public static string GetFirstWord(this string @str){
            var firstName = @str.Trim().Split(' ')[0];
            return firstName;
        }
    }
}