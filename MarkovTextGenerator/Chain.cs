﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovTextGenerator
{
    public class Chain
    {
        public Dictionary<String, List<Word>> words;
        private Dictionary<String, int> sums;
        private Random rand;
        public List<string> startingwords;

        public Chain ()
        {
            words = new Dictionary<String, List<Word>>();
            sums = new Dictionary<string, int>();
            rand = new Random(System.Environment.TickCount);
            string[] linesText = System.IO.File.ReadAllLines(@"C:\Users\coolf\OneDrive\Desktop\MarkovTextGenerator\MarkovTextGenerator\Data\Sample.txt");
            startingwords = new List<string>();
            for (int i = 0; i < linesText.Length-1; i++)
            {
                string z = linesText[i];
                string[] wordsText = z.Split(' ');
                startingwords.Add(wordsText[0]);
            }
            
        }

        /// <summary>
        /// Returns a random starting word from the stored list of words
        /// This may not be the best approach.. better may be to actually store
        /// a separate list of actual sentence starting words and randomly choose from that
        /// </summary>
        /// <returns></returns>
        public String GetRandomStartingWord ()
        {
            return words.Keys.ElementAt(rand.Next() % words.Keys.Count);
        }

        // Adds a sentence to the chain
        // You can use the empty string to indicate the sentence will end
        //
        // For example, if sentence is "The house is on fire" you would do the following:
        //  AddPair("The", "house")
        //  AddPair("house", "is")
        //  AddPair("is", "on")
        //  AddPair("on", "fire")
        //  AddPair("fire", "")

        public void AddString (String sentence)
        {
            // TODO: Break sentence up into word pairs
            // TODO: Add each word pair to the chain
            // TODO: The last word of any sentence will be paired up with
            //       an empty string to show that it is the end of the sentence
            String[] wordie = sentence.Split(' ');
            for (int i = 0; i < wordie.Length - 1; i++)
            {
                AddPair(wordie[i], wordie[i + 1]);
            }
                AddPair(wordie[wordie.Length-1], "  ");
        }

        // Adds a pair of words to the chain that will appear in order
        public void AddPair(String word, String word2)
        {
            if (!words.ContainsKey(word))
            {
                sums.Add(word, 1);
                words.Add(word, new List<Word>());
                words[word].Add(new Word(word2));
            }
            else
            {
                bool found = false;
                foreach (Word s in words[word])
                {
                    if (s.ToString() == word2)
                    {
                        found = true;
                        s.Count++;
                        sums[word]++;
                    }
                }

                if (!found)
                {
                    words[word].Add(new Word(word2));
                    sums[word]++;
                }
            }
        }

        /// <summary>
        /// Given a word, randomly chooses the next word.  This should be done
        /// by using the list of words in the words Dictionary.  The provided
        /// code allows you to pick a word from the choices array.  Bear in mind
        /// that each word is not equally likely to occur and has their own probability
        /// of occurring.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public String GetNextWord (String word)
        {
            if (words.ContainsKey(word))
            {
                double t = 0;
                List<Word> choices = words[word];
                double b = rand.NextDouble();

                foreach(var choice in choices)
                {
                    t = t + choice.Probability;
                    if(b < t)
                    {
                        return choice.ToString();
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Generates a full randomly generated sentence based that starts with
        /// startingWord.
        /// </summary>
        /// <param name="startingWord"></param>
        /// <returns></returns>
        public String GenerateSentence(string startingWord)
        {
            int length = startingwords.Count;
            int num = rand.Next(length);
            startingWord = startingwords[num];
            string a = startingWord;
            for (int i = 0; i <= 100; i++)
            {
                String nextWord = GetNextWord(a);
                a = nextWord;
                startingWord = startingWord + " " + nextWord;
                if(a == "  ")
                {
                    break;
                }
            }


            return startingWord;
        }
        
        /// <summary>
        /// Updates the probability of choosing a second word at random
        /// for a chain of words attached to a first word.
        /// Example: If the starting word is "The" and the only word that
        /// you ever see following it is the word "cat", then "cat" would
        /// have a probability of following "The" of 1.0.  Another scenario
        /// would involve sentences like:
        /// The cat loves milk, The cat is my friend, The dog is in the yard
        /// In this scenario with the starting word of "The":
        /// - cat would have a probability of 0.66 (appears 66% of the time)
        /// - dog would have a probability of 0.33 (appears 33% of the time)
        /// </summary>
        public void UpdateProbabilities ()
        {
            foreach (String word in words.Keys)
            {
                double sum = 0;  // Total sum of all the occurrences of each followup word

                // Update the probabilities
                foreach (Word s in words[word])
                {
                    s.Probability = (double)s.Count / sums[word];
                }

            }
        }
    }
}
