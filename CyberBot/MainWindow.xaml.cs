// ============================================================
// MainWindow.xaml.cs
// Author: Sade Sazila
// Date: May 2026
// Purpose: Code-behind for the WPF GUI. Handles user interaction,
//          chat display, voice greeting and delegate usage.
// Reference: IIE (2024). PROG6221 Course Notes. The Independent
//            Institute of Education.
// ============================================================

using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberBot
{
    // Delegate definition - satisfies Requirement: Use delegates to solve a programming problem
    // This delegate is used to process and display bot responses
    public delegate void BotResponseDelegate(string response);

    public partial class MainWindow : Window
    {
        // ── Fields ───────────────────────────────────────────────────
        private readonly ResponseHandler _responseHandler;
        private string _userName = string.Empty;
        private bool _nameEntered = false;

        // Delegate instance used to handle bot responses
        private BotResponseDelegate _botResponseHandler;

        // ────────────────────────────────────────────────────────────
        // Constructor: initialises components and sets up the chatbot
        // ────────────────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();

            // Initialise the response handler
            _responseHandler = new ResponseHandler();

            // Assign the delegate to the DisplayBotMessage method
            // Satisfies Requirement: Use delegates to solve a programming problem
            _botResponseHandler = new BotResponseDelegate(DisplayBotMessage);

            // Play voice greeting on startup
            PlayVoiceGreeting();

            // Show welcome message and ask for name
            DisplayBotMessage("Welcome to the Cybersecurity Awareness Chatbot!");
            DisplayBotMessage("I am here to help you stay safe in the digital world.");
            DisplayBotMessage("Please tell me your name to get started.");
        }

        // ────────────────────────────────────────────────────────────
        // PlayVoiceGreeting: plays the WAV greeting file on startup
        // Carried over from Part 1 - Requirement 1: GUI Implementation
        // ────────────────────────────────────────────────────────────
        private void PlayVoiceGreeting()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string audioPath = Path.Combine(basePath, "greeting.wav");

                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();
                        player.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently handle audio errors - app continues without sound
                Console.WriteLine("Audio error: " + ex.Message);
            }
        }

        // ────────────────────────────────────────────────────────────
        // SendButton_Click: fires when user clicks the SEND button
        // ────────────────────────────────────────────────────────────
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        // ────────────────────────────────────────────────────────────
        // UserInputBox_KeyDown: fires when user presses Enter key
        // ────────────────────────────────────────────────────────────
        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ProcessInput();
        }

        // ────────────────────────────────────────────────────────────
        // ClearButton_Click: clears the chat display
        // ────────────────────────────────────────────────────────────
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            DisplayBotMessage("Chat cleared. How can I help you, " + _userName + "?");
        }

        // ────────────────────────────────────────────────────────────
        // ProcessInput: main method that handles all user input
        // ────────────────────────────────────────────────────────────
        private void ProcessInput()
        {
            string input = UserInputBox.Text.Trim();

            // Input validation - Requirement 7: Error Handling
            if (string.IsNullOrWhiteSpace(input))
                return;

            // Display user message in chat
            DisplayUserMessage(input);
            UserInputBox.Clear();

            // First input is always the user's name
            if (!_nameEntered)
            {
                _userName = input;
                _responseHandler.SetUserName(_userName);
                _nameEntered = true;

                // Update memory status bar - Requirement 5: Memory and Recall
                UpdateMemoryStatus("Name stored: " + _userName);

                // Use delegate to display response - Requirement: Delegates
                _botResponseHandler.Invoke("Nice to meet you, " + _userName + "! I am your Cybersecurity Awareness Assistant.");
                _botResponseHandler.Invoke("Type 'help' to see all available topics, or just ask me anything about cybersecurity!");
                return;
            }

            // Get response from ResponseHandler
            string response = _responseHandler.GetResponse(input);

            // Handle goodbye signal
            if (response == "GOODBYE_SIGNAL")
            {
                // Use delegate to display goodbye - Requirement: Delegates
                _botResponseHandler.Invoke("Goodbye, " + _userName + "! Stay safe and vigilant online.");
                SendButton.IsEnabled = false;
                UserInputBox.IsEnabled = false;
                return;
            }

            // Use delegate to display the bot response - Requirement: Delegates
            _botResponseHandler.Invoke(response);

            // Update memory status bar if topic was discussed
            UpdateMemoryStatus("Name: " + _userName + " | Last topic: " + input);
        }

        // ────────────────────────────────────────────────────────────
        // DisplayBotMessage: adds a bot message bubble to the chat
        // Called via delegate - Requirement: Delegates
        // ────────────────────────────────────────────────────────────
        private void DisplayBotMessage(string message)
        {
            // Outer container aligned to the left
            var container = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 4, 60, 4)
            };

            // Bot label
            var label = new TextBlock
            {
                Text = "CyberBot",
                Foreground = new SolidColorBrush(Color.FromRgb(233, 69, 96)),
                FontSize = 10,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(4, 0, 0, 2)
            };

            // Message bubble
            var bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(15, 52, 96)),
                CornerRadius = new CornerRadius(0, 8, 8, 8),
                Padding = new Thickness(12, 8, 12, 8)
            };

            var text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap
            };

            bubble.Child = text;
            container.Children.Add(label);
            container.Children.Add(bubble);
            ChatPanel.Children.Add(container);

            // Auto scroll to bottom
            ChatScrollViewer.ScrollToBottom();
        }

        // ────────────────────────────────────────────────────────────
        // DisplayUserMessage: adds a user message bubble to the chat
        // ────────────────────────────────────────────────────────────
        private void DisplayUserMessage(string message)
        {
            // Outer container aligned to the right
            var container = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(60, 4, 0, 4)
            };

            // User label
            var label = new TextBlock
            {
                Text = string.IsNullOrEmpty(_userName) ? "You" : _userName,
                Foreground = new SolidColorBrush(Color.FromRgb(168, 168, 179)),
                FontSize = 10,
                FontFamily = new FontFamily("Consolas"),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 4, 2)
            };

            // Message bubble
            var bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(233, 69, 96)),
                CornerRadius = new CornerRadius(8, 0, 8, 8),
                Padding = new Thickness(12, 8, 12, 8)
            };

            var text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap
            };

            bubble.Child = text;
            container.Children.Add(label);
            container.Children.Add(bubble);
            ChatPanel.Children.Add(container);

            // Auto scroll to bottom
            ChatScrollViewer.ScrollToBottom();
        }

        // ────────────────────────────────────────────────────────────
        // UpdateMemoryStatus: updates the memory status bar text
        // Requirement 5: Memory and Recall
        // ────────────────────────────────────────────────────────────
        private void UpdateMemoryStatus(string status)
        {
            MemoryStatusText.Text = "Memory: " + status;
        }
    }
}