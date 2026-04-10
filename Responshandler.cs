using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class ResponseHandler
    {
        // List of keyword groups paired with their responses
        private readonly List<(string[] Keywords, string Response)> _responses;

        public ResponseHandler()
        {
            _responses = new List<(string[], string)>
            {
                (new[] { "how are you", "how are u", "how r you", "how do you feel" },
                    "I'm running at full capacity and ready to help you stay safe online! Keep your guard up out there."),

                (new[] { "your purpose", "what do you do", "what can you do", "purpose" },
                    "I'm the Cybersecurity Awareness Assistant!\n" +
                    "  My purpose is to educate South African citizens on staying safe online.\n" +
                    "  I cover topics like phishing emails, safe password practices, and online threats."),

                (new[] { "what can i ask", "topics", "help", "what can you help", "menu" },
                    "You can ask me about any of the following topics:\n" +
                    "  [1] Password Safety\n" +
                    "  [2] Phishing Emails\n" +
                    "  [3] Safe Browsing\n" +
                    "  [4] Malware & Viruses\n" +
                    "  [5] Social Engineering\n" +
                    "  Type a topic or keyword to get started!"),

                (new[] { "password", "passwords", "password safety", "strong password" },
                    "PASSWORD SAFETY TIPS:\n" +
                    "  * Use at least 12 characters mixing letters, numbers & symbols.\n" +
                    "  * Never reuse the same password across multiple sites.\n" +
                    "  * Use a reputable password manager to store passwords safely.\n" +
                    "  * Enable two-factor authentication (2FA) wherever possible.\n" +
                    "  * Never share your password with ANYONE - not even IT support!"),

                (new[] { "phishing", "phish", "scam email", "fake email", "suspicious email" },
                    "PHISHING AWARENESS:\n" +
                    "  * Be suspicious of urgent emails demanding immediate action.\n" +
                    "  * Always check the sender's actual email address carefully.\n" +
                    "  * Hover over links to reveal the real destination URL.\n" +
                    "  * Legitimate banks or companies NEVER ask for passwords via email.\n" +
                    "  * When in doubt, contact the organisation directly via official channels."),

                (new[] { "browsing", "safe browsing", "internet safety", "online safety", "website" },
                    "SAFE BROWSING TIPS:\n" +
                    "  * Always check for HTTPS and the padlock icon in your browser.\n" +
                    "  * Avoid banking or shopping on public Wi-Fi networks.\n" +
                    "  * Keep your browser and all plugins up to date.\n" +
                    "  * Use a VPN when connecting to public or unsecured networks.\n" +
                    "  * Be mindful of what personal information you share on social media."),

                (new[] { "malware", "virus", "ransomware", "spyware", "trojan" },
                    "MALWARE PROTECTION:\n" +
                    "  * Install a reputable antivirus and keep it updated at all times.\n" +
                    "  * Never download software from untrusted or unknown sources.\n" +
                    "  * Regularly back up your important files to an external drive or cloud.\n" +
                    "  * Be cautious with USB drives from sources you don't know.\n" +
                    "  * Enable automatic OS updates to get the latest security patches."),

                (new[] { "social engineering", "manipulation", "pretexting", "impersonation" },
                    "SOCIAL ENGINEERING AWARENESS:\n" +
                    "  * Attackers often pose as IT support, banks, or authority figures.\n" +
                    "  * Always verify a caller's identity before sharing any information.\n" +
                    "  * Be wary of requests that create urgency, fear, or pressure.\n" +
                    "  * NEVER share your OTP (one-time pin) with anyone - ever!\n" +
                    "  * When something feels off, trust your instincts and hang up."),

                (new[] { "goodbye", "bye", "exit", "quit", "close" },
                    "GOODBYE_SIGNAL"),
            };
        }

        // Match user input against keywords and return the appropriate response
        public string? GetResponse(string userInput)
        {
            // Return null to signal empty/invalid input
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return null;
            }

            string lowerInput = userInput.ToLower().Trim();

            foreach (var (keywords, response) in _responses)
            {
                foreach (string keyword in keywords)
                {
                    if (lowerInput.Contains(keyword))
                    {
                        return response;
                    }
                }
            }

            // Return DEFAULT to signal no matching response was found
            return "DEFAULT";
        }
    }
}