namespace Affine_solver
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            string text = "";
            string lettersText = "";
            while (lettersText.Length < 200)
            {
                Console.Write("Enter encrypted text (at least 200 characters long): ");
                text = Console.ReadLine().ToLower();
                lettersText = string.Join("", text.Where(char.IsLetter).ToArray());
            }

            List<string[]> nonLetters = new();

            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsLetter(text[i]))
                {
                    string[] nonLetterIndex = { text[i].ToString(), i.ToString() };
                    nonLetters.Add(nonLetterIndex);
                }
            }

            List<string> possibleDecryptions = new();
            for (int x = 1; x <= 25; x += 2)
            {
                for (int y = 0; y <= 25; y++)
                {
                    int flag = 0;
                    int x_inv = 0;
                    for (int i = 0; i < 26; i++)
                    {
                        flag = (x * i) % 26;

                        // Check if (a*i)%26 == 1,
                        // then i will be the multiplicative inverse of a
                        if (flag == 1)
                        {
                            x_inv = i;
                        }
                    }

                    Console.WriteLine((y, x_inv).ToString());

                    List<string> decrypted = new();
                    foreach (char letter in lettersText)
                    {
                        int letterNumber = Convert.ToInt32(letter - 97) % 26;
                        int numberDecrypted = ((letterNumber - y) * x_inv) % 26;
                        while (numberDecrypted < 0) { numberDecrypted += 26; }
                        //Console.WriteLine(numberDecrypted.ToString());
                        decrypted.Add(Convert.ToChar(numberDecrypted + 97).ToString());
                    }

                    possibleDecryptions.Add(string.Join("", decrypted));
                }
            }
            double[] scores = new double[possibleDecryptions.Count];
            for (int i = 0; i < possibleDecryptions.Count(); i++) {scores[i] = ChiSquareTest(possibleDecryptions[i]);}
            string decryption = possibleDecryptions[Array.IndexOf(scores, scores.Max())];

            List<string> output = new List<string>();
            foreach (char letter in decryption) { output.Add(letter.ToString()); }
            foreach (var item in nonLetters) { output.Insert(Convert.ToInt32(item[1]), item[0].ToString()); }

            Array.Sort(scores);
            Console.WriteLine();
            Console.WriteLine(string.Join("", output));
            Console.WriteLine(string.Join(",", scores));
            Console.WriteLine(string.Join("", possibleDecryptions[32]));
        }

        public static Dictionary<string, int> TextFrequency(string testText)
        {
            var characterCount = new Dictionary<string, int>() { { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }, { "i", 0 }, { "j", 0 }, { "k", 0 }, { "l", 0 }, { "m", 0 }, { "n", 0 }, { "o", 0 }, { "p", 0 }, { "q", 0 }, { "r", 0 }, { "s", 0 }, { "t", 0 }, { "u", 0 }, { "v", 0 }, { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 } };

            foreach (char c in testText) { characterCount[c.ToString()]++; }
            return characterCount;
        }

        public static double ChiSquareTest(string testText)
        {
            var exspectedFrequencies = new Dictionary<string, double>() { { "e", 11.1607 }, { "a", 8.4966 }, { "r", 7.5809 }, { "i", 7.5448 }, { "o", 7.1635 }, { "t", 6.9509 }, { "n", 6.6544 }, { "s", 5.7351 }, { "l", 5.4893 }, { "c", 4.5388 }, { "u", 3.6308 }, { "d", 3.3844 }, { "p", 3.1671 }, { "m", 3.0129 }, { "h", 3.0034 }, { "g", 2.4705 }, { "b", 2.0720 }, { "f", 1.8121 }, { "y", 1.7779 }, { "w", 1.2899 }, { "k", 1.1016 }, { "v", 1.0074 }, { "x", 0.2902 }, { "z", 0.2722 }, { "j", 0.1965 }, { "q", 0.1962 } };
            Dictionary<string, int> textFrequencies = TextFrequency(testText);

            double score = 0;

            foreach (string d in textFrequencies.Keys)
            {
                string s = d.ToLower();
                double exspectedCount = exspectedFrequencies[s] / 100 * testText.Length;
                score += Math.Pow(textFrequencies[s] - exspectedCount, 2) / exspectedCount;
            }
            return score;
        }
    }
}