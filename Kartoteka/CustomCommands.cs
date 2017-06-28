using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kartoteka
{
   public static class CustomCommands
    {
         public static void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        public static bool IsFilled(string firststring, string secondstring)
        {
            if ((String.IsNullOrWhiteSpace(firststring)) || (String.IsNullOrEmpty(secondstring)))
            {
                return false;
            }
            else return true;
        }
        public static bool IsFilled(string firststring)
        {
            if (String.IsNullOrWhiteSpace(firststring))
            {
                return false;
            }
            else return true;
        }
    }
}
