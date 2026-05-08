// ============================================================
// ResponseHandler.cs
// Author: Sade Sazila
// Date: May 2026
// Purpose: Handles keyword recognition, random responses,
//          memory recall, sentiment detection and conversation flow
// Reference: IIE (2024). PROG6221 Course Notes. The Independent
//            Institute of Education.
// ============================================================

using System;
using System.Collections.Generic;

namespace CyberBot
{
    public class ResponseHandler
    {
        // Memory storage - stores user name, favourite topic and last topic discussed
        // Satisfies Requirement 5: Memory and Recall
        private string _userName = string.Empty;
        private string _favouriteTopic = string.Empty;
        private string _lastTopic = string.Empty;

        // Random number generator for selecting varied responses
        // Satisfies Requirement 3: Random Responses
        private readonly Random _random = new Random();

        // Dictionary mapping keywords to a list of possible responses
        // Using Dictionary<string, List<string>> satisfies Requirement 8: Code Optimisation
        // Also satisfies Requirement 2: Keyword Recognition
        private readonly Dictionary<string, List<string>> _keywordResponses =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
                {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                    "A good password has at least 12 characters with a mix of letters, numbers and symbols.",
                    "Never reuse the same password across multiple sites - use a password manager instead.",
                    "Enable two-factor authentication (2FA) wherever possible for an extra layer of security."
                },
                ["phishing"] = new List<string>
                {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "Always check the sender's actual email address - scammers use addresses that look almost correct.",
                    "Hover over links before clicking to reveal the real destination URL.",
                    "Legitimate banks or companies will NEVER ask for your password via email."
                },
                ["privacy"] = new List<string>
                {
                    "Review the privacy settings on your social media accounts regularly.",
                    "Be mindful of what personal information you share online - once it is out, it is hard to take back.",
                    "Use a VPN when connecting to public Wi-Fi to protect your privacy.",
                    "Check app permissions on your phone - many apps request more access than they need."
                },
                ["scam"] = new List<string>
                {
                    "If something feels too good to be true online, it probably is. Trust your instincts.",
                    "Never send money or personal details to someone you have only met online.",
                    "Scammers often create urgency - slow down and verify before acting.",
                    "Report scams to the South African Police Service (SAPS) or the SAFPS."
                },
                ["malware"] = new List<string>
                {
                    "Install a reputable antivirus and keep it updated at all times.",
                    "Never download software from untrusted or unknown sources.",
                    "Regularly back up your important files to an external drive or cloud storage.",
                    "Enable automatic OS updates to get the latest security patches."
                },
                ["browsing"] = new List<string>
                {
                    "Always check for HTTPS and the padlock icon before entering personal details.",
                    "Avoid banking or shopping on public Wi-Fi networks.",
                    "Keep your browser and all plugins up to date.",
                    "Be careful about which browser extensions you install - some can spy on you."
                },
                ["social engineering"] = new List<string>
                {
                    "Attackers often pose as IT support, banks, or authority figures - always verify identity.",
                    "Be wary of requests that create urgency, fear, or pressure.",
                    "Never share your OTP (one-time pin) with anyone - ever!",
                    "When something feels off, trust your instincts."
                }
            };

        // Sentiment keyword lists for detecting user mood
        // Satisfies Requirement 6: Sentiment Detection
        private readonly List<string> _worriedKeywords =
            new List<string> { "worried", "scared", "afraid", "nervous", "anxious", "stressed", "fear" };

        private readonly List<string> _frustratedKeywords =
            new List<string> { "frustrated", "angry", "annoyed", "confused", "lost", "dont understand", "don't understand" };

        private readonly List<string> _curiousKeywords =
            new List<string> { "curious", "interested", "want to know", "tell me more", "explain", "how does", "what is" };

        // Follow-up trigger phrases to maintain conversation flow
        // Satisfies Requirement 4: Conversation Flow
        private readonly List<string> _followUpTriggers =
            new List<string> { "tell me more", "explain more", "give me another", "more tips", "another tip", "go on", "continue" };

        // ------------------------------------------------------------
        // SetUserName: stores the user name for personalised responses
        // ------------------------------------------------------------
        public void SetUserName(string name)
        {
            _userName = name;
        }

        // ------------------------------------------------------------
        // GetResponse: main method called by MainWindow to process input
        // Returns a string response based on user input
        // ------------------------------------------------------------
        public string GetResponse(string userInput)
        {
            // Input validation - Requirement 7: Error Handling
            if (string.IsNullOrWhiteSpace(userInput))
                return "It looks like you did not type anything. Please ask me a cybersecurity question!";

            string lower = userInput.ToLower().Trim();

            // Check for exit commands
            if (lower == "exit" || lower == "quit" || lower == "bye" || lower == "goodbye")
                return "GOODBYE_SIGNAL";

            // Check for help or menu request
            if (lower.Contains("help") || lower.Contains("topics") || lower.Contains("menu"))
                return GetHelpMenu();

            // Check for follow-up requests - Requirement 4: Conversation Flow
            if (IsFollowUp(lower))
            {
                if (!string.IsNullOrEmpty(_lastTopic) && _keywordResponses.ContainsKey(_lastTopic))
                    return "Sure! Here is another tip on " + _lastTopic + ":\n\n" + GetRandomResponse(_lastTopic);

                return "I am not sure which topic to continue. Could you mention a topic like password, phishing or privacy?";
            }

            // Detect sentiment before responding - Requirement 6
            string sentimentPrefix = DetectSentiment(lower);

            // Match keywords in user input - Requirement 2: Keyword Recognition
            foreach (var keyword in _keywordResponses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    _lastTopic = keyword;

                    // First time this topic is mentioned - store as favourite topic
                    // Requirement 5: Memory and Recall
                    if (string.IsNullOrEmpty(_favouriteTopic))
                    {
                        _favouriteTopic = keyword;
                        string memoryNote = "\n\nI will remember that you are interested in " + keyword + "!";
                        return sentimentPrefix + GetRandomResponse(keyword) + memoryNote;
                    }

                    // Personalise response if this matches their favourite topic
                    string recall = (_favouriteTopic == keyword)
                        ? "\n\nAs someone interested in " + _favouriteTopic + ", this tip is especially relevant for you, " + _userName + "!"
                        : string.Empty;

                    return sentimentPrefix + GetRandomResponse(keyword) + recall;
                }
            }

            // Default response for unrecognised input - Requirement 7: Error Handling
            return "I am not sure I understand. Can you try rephrasing? Try keywords like: password, phishing, privacy, scam, malware, or browsing.";
        }

        // ------------------------------------------------------------
        // GetRandomResponse: selects a random response from the list
        // for a given keyword - Requirement 3: Random Responses
        // ------------------------------------------------------------
        private string GetRandomResponse(string keyword)
        {
            var list = _keywordResponses[keyword];
            return list[_random.Next(list.Count)];
        }

        // ------------------------------------------------------------
        // DetectSentiment: checks input for emotional keywords and
        // returns an empathetic prefix - Requirement 6: Sentiment Detection
        // ------------------------------------------------------------
        private string DetectSentiment(string input)
        {
            foreach (string word in _worriedKeywords)
                if (input.Contains(word))
                    return "It is completely understandable to feel that way, " + _userName + ". Let me help ease your concerns.\n\n";

            foreach (string word in _frustratedKeywords)
                if (input.Contains(word))
                    return "I hear you, " + _userName + ". Let me try to make this clearer for you.\n\n";

            foreach (string word in _curiousKeywords)
                if (input.Contains(word))
                    return "Great question, " + _userName + "! Curiosity is the first step to staying safe online.\n\n";

            return string.Empty;
        }

        // ------------------------------------------------------------
        // IsFollowUp: checks if the user input is a follow-up request
        // Requirement 4: Conversation Flow
        // ------------------------------------------------------------
        private bool IsFollowUp(string input)
        {
            foreach (string trigger in _followUpTriggers)
                if (input.Contains(trigger))
                    return true;
            return false;
        }

        // ------------------------------------------------------------
        // GetHelpMenu: returns the list of available topics
        // ------------------------------------------------------------
        private string GetHelpMenu()
        {
            return "Here are the topics I can help you with:\n\n" +
                   "[1] Password Safety      - type: password\n" +
                   "[2] Phishing Emails      - type: phishing\n" +
                   "[3] Privacy              - type: privacy\n" +
                   "[4] Scams                - type: scam\n" +
                   "[5] Malware              - type: malware\n" +
                   "[6] Safe Browsing        - type: browsing\n" +
                   "[7] Social Engineering   - type: social engineering\n\n" +
                   "You can also type 'tell me more' to get another tip on the last topic!";
        }
    }
}