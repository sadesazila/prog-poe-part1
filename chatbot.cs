using System;
using System.IO;
using System.Media;
using System.Threading;

namespace CybersecurityChatbot
{
    public class ChatBot
    {
        private readonly ResponseHandler _responseHandler;
        private string _userName = string.Empty;//

        public ChatBot()
        {
            _responseHandler = new ResponseHandler();
        }

        // Main entry point — runs the entire chatbot flow
        public void Run()
        {
            Console.Title = "Cybersecurity Awareness Chatbot";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PlayVoiceGreeting();
            DisplayHelper.PrintAsciiLogo();
            DisplayHelper.PrintBorder();
            GetUserName();
            DisplayHelper.PrintBorderBottom();
            Console.WriteLine();

            StartChatLoop();
        }

        // Play the WAV voice greeting on startup
        private void PlayVoiceGreeting()
        {
            try
            {
                // Build the full path to greeting.wav
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string audioPath = Path.Combine(baseDir, "greeting.wav");

                // Debug line — this prints the path so you can verify it
                Console.WriteLine($"  Looking for audio at: {audioPath}");

                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();
                        player.PlaySync();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("  [Voice greeting file not found - continuing without audio]");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(800);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  [Audio skipped: {ex.Message}]");
                Console.ResetColor();
            }
        }

        // Ask user for their name and personalise all responses
        private void GetUserName()
        {
            Console.WriteLine();
            DisplayHelper.TypeEffect(
                "  Welcome! I am your Cybersecurity Awareness Assistant.",
                ConsoleColor.Green);
            Console.WriteLine();
            DisplayHelper.PrintColoured("  What is your name?", ConsoleColor.Yellow);
            Console.Write("  > ");
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine() ?? string.Empty;
            Console.ResetColor();

            // Input validation for the name field
            if (string.IsNullOrWhiteSpace(input))
            {
                _userName = "User";
                DisplayHelper.PrintColoured(
                    "  [No name entered — I'll call you 'User' for now.]",
                    ConsoleColor.DarkYellow);
            }
            else
            {
                _userName = input.Trim();
            }

            Console.WriteLine();
            DisplayHelper.TypeEffect($"  Hello, {_userName}! Great to meet you.", ConsoleColor.Cyan);
            DisplayHelper.TypeEffect(
                "  I am here to help you stay safe in the digital world.",
                ConsoleColor.Cyan);
            Console.WriteLine();
        }

        // Main conversation loop
        private void StartChatLoop()
        {
            DisplayHelper.PrintColoured(
                $"  {_userName}, you can ask me about cybersecurity topics.",
                ConsoleColor.Green);
            DisplayHelper.PrintColoured(
                "  Type 'help' to see available topics, or 'exit' to quit.",
                ConsoleColor.DarkGreen);
            DisplayHelper.PrintDivider();
            Console.WriteLine();

            bool isRunning = true;

            while (isRunning)
            {
                // Prompt user for input
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  {_userName}: ");
                Console.ForegroundColor = ConsoleColor.White;
                string userInput = Console.ReadLine();
                Console.ResetColor();
                Console.WriteLine();

                // Input validation — empty input check
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    DisplayHelper.PrintColoured(
                        "  Bot: Hmm, it looks like you didn't type anything.",
                        ConsoleColor.Magenta);
                    DisplayHelper.PrintColoured(
                        "       Please type a question or topic you'd like help with.",
                        ConsoleColor.Magenta);
                    Console.WriteLine();
                    continue;
                }

                string response = _responseHandler.GetResponse(userInput);

                if (response == "GOODBYE_SIGNAL")
                {
                    DisplayHelper.TypeEffect(
                        $"  Bot: Goodbye, {_userName}! Stay safe and vigilant online.",
                        ConsoleColor.Cyan);
                    DisplayHelper.PrintBorder();
                    Thread.Sleep(1000);
                    isRunning = false;
                }
                else if (response == "DEFAULT")
                {
                    // Unrecognised input — politely redirect
                    DisplayHelper.PrintColoured(
                        "  Bot: I didn't quite understand that. Could you rephrase?",
                        ConsoleColor.Magenta);
                    DisplayHelper.PrintColoured(
                        "       Try keywords like: passwords, phishing, browsing, or type 'help'.",
                        ConsoleColor.DarkMagenta);
                    Console.WriteLine();
                }
                else
                {
                    // Valid recognised response
                    SimulateTyping();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  Bot: {response}");
                    Console.ResetColor();
                    Console.WriteLine();
                    DisplayHelper.PrintDivider();
                    Console.WriteLine();
                }
            }
        }

        // Adds a realistic typing delay effect before each bot response
        private void SimulateTyping()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Bot is typing");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(350);
                Console.Write(".");
            }
            Thread.Sleep(300);
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}