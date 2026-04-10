using System;
using System.Threading;

namespace CybersecurityChatbot
{
    public static class DisplayHelper
    {
        // Print coloured text
        public static void PrintColoured(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // Simulate typing effect character by character
        public static void TypeEffect(string text, ConsoleColor color, int delayMs = 25)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        // Top border line
        public static void PrintBorder()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.ResetColor();
        }

        // Bottom border line
        public static void PrintBorderBottom()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        // Section divider
        public static void PrintDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  ──────────────────────────────────────────────────────────");
            Console.ResetColor();
        }

        // ASCII art logo displayed on startup
        public static void PrintAsciiLogo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
   ██████╗██╗   ██╗██████╗ ███████╗██████╗ 
  ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗
  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝
  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗
  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║
   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
        ██████╗  ██████╗ ████████╗
        ██╔══██╗██╔═══██╗╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔══██╗██║   ██║   ██║   
        ██████╔╝╚██████╔╝   ██║   
        ╚═════╝  ╚═════╝    ╚═╝   ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("    *** CYBERSECURITY AWARENESS ASSISTANT ***");
            Console.WriteLine("       Protecting South African Citizens Online");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
