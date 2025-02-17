﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovTextGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Chain chain = new Chain();

            Console.WriteLine("Welcome to Marky Markov's Random Text Generator!");
            Console.WriteLine();
            LoadText("Sample.txt", chain);
            /*
            while (true)
            {

                Console.Write("> ");

                String line = Console.ReadLine();
                if (line == "!")
                    break;

                chain.AddString(line);  // Let the chain process this string
            }
            */
            // Now let's update all the probabilities with the new data
            chain.UpdateProbabilities();

            // Okay now for the fun part


            String word = chain.startingwords[0];
            for (int i = 0; i < 6000; i++)
            {
                Console.WriteLine(chain.GenerateSentence(word));
            }
        }

        static void LoadText(string filename, Chain chain)
        {
            int counter = 0;
            String line;

            string path = Path.Combine(Environment.CurrentDirectory, $"data/{filename}");
            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                chain.AddString(line);
                counter++;
            }

            file.Close();
        }
    }
}
