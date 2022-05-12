using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BidlingAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                #region Startup
                Console.WriteLine(
                    "0|STOP - Closes the application\n" +
                    "1|MOST USED WORDS - Finds the most used words\n" +
                    "2|LONGEST NAME - Finds the longest bidling name\n" +
                    "3|ALPHABETICAL - Orders bidling names alphabetically\n" +
                    //"4|SYLLABLE COUNT - Orders bidling names by their estimated syllable count\n"
                    "4|B - Only B\n" +
                    "5|LOOP NAMES - Loops through all names infinitely\n" +
                    "6|SIMPLE PRINT - Prints all bidling names\n" +
                    "7|MOST USED LETTERS - Prints all letters in the order they are used\n" +
                    "8|ALPHABETICAL WORDS - Prints all words in alphabetical order\n" +
                    "9|FAMILY CREST - Prints the Bidling Family Crest\n"
                    );

                string input = Console.ReadLine().ToUpper().Replace(" ", "").Trim();

                Console.Clear();

                bool userReset = true;
                #endregion

                switch (input)
                {
                    case "0":
                    case "STOP":
                    case "EXIT":
                    case "CLOSE":
                        running = false;
                        break;
                    case "1":
                    case "MOSTUSEDWORDS":
                        FindMostUsedWords();
                        break;
                    case "2":
                    case "LONGESTNAME":
                        FindLongestName();
                        break;

                    case "3":
                    case "ALPHABETICAL":
                    case "ALPHABETICALSORT":
                        AlphabeticalSort();
                        break;
                    case "3.1":
                        AlphabeticalSortFirstOnly();
                        break;

                    /*case "4":
                    case "SYLLABLE":
                    case "SYLLABLECOUNT":
                    case "SYLLABLECOUNTSORT":
                        SyllableSort();
                        break;*/

                    case "4":
                    case "B":
                        PrintB();
                        break;

                    case "5":
                    case "LOOPNAMES":
                        while (true)
                        {
                            PrintNames();
                        }


                    case "6":
                    case "PRINT":
                    case "PRINTALL":
                    case "SIMPLEPRINT":
                        PrintNames();
                        break;

                    case "7":
                    case "MOSTUSEDLETTERS":
                        FindMostUsedLetters();
                        break;

                    case "8":
                    case "ALPHABETICALWORDS":
                        AlphabeticalWords();
                        break;

                    case "9":
                    case "FAMILYCREST":
                    case "PRINTFAMILYCREST":
                        PrintFamilyCrest();
                        break;

                    default:
                        userReset = false;
                        break;
                }

                #region Reset
                if (userReset)
                {
                    Console.ReadLine();
                    Console.Clear();
                }
                #endregion

            }
        }

        #region Analysis Methods
        static void PrintFamilyCrest()
        {
            string name1 = "";
            string name2 = "";
            
            foreach (string i in LoadFromFile())
            {
                //string[] splitLine = i.Split(' ');
                string stringToUse = i.ToLower().Trim();

                string[] splitted;
                if (stringToUse.Contains("mc"))
                {
                    splitted = stringToUse.Split(new[] { "mc" }, StringSplitOptions.None);
                } else
                {
                    splitted = stringToUse.Split(new[] { " " }, StringSplitOptions.None);
                }
                name1 += splitted[0].Trim() + " ";
                name2 += splitted[1].Trim() + " ";
            }

            string fullname = name1 + " mc " + name2;

            Console.WriteLine(fullname);
        }

        static void PrintNames()
        {
            foreach (string i in LoadFromFile())
            {
                Console.WriteLine(i.Trim());
            }
        }

        static void FindLongestName()
        {
            foreach (string i in (from x in LoadFromFile()

                                  orderby x.Length * -1

                                  select x).ToList())
            {
                Console.WriteLine(i.Trim() + ": " + i.Length);
            }
        }

        static void AlphabeticalSort()
        {
            foreach (string i in (from x in LoadFromFile()

                                  orderby x.Trim().ToLower()

                                  select x).ToList())
            {
                Console.WriteLine(i.Trim());
            }
        }

        static void AlphabeticalWords()
        {
            List<string> words = new List<string>();
            foreach (string i in LoadFromFile())
            {
                foreach (string n in i.Split(' '))
                {
                    words.Add(n.Trim());
                }
            }

            foreach (string i in (from x in words

                                  orderby x.Trim().ToLower()

                                  select x).ToList())
            {
                Console.WriteLine(i.Trim());
            }
        }

        static void AlphabeticalSortFirstOnly()
        {
            char lastLetter = '0';

            foreach (string i in (from x in LoadFromFile()

                                  orderby x.Trim().ToLower()

                                  select x).ToList())
            {
                if (i.ToUpper().Trim()[0] != lastLetter)
                {
                    lastLetter = i.ToUpper().Trim()[0];
                    Console.WriteLine(i.Trim());
                }
            }
        }

        static void FindMostUsedWords()
        {
            List<WordObject> words = new List<WordObject>();

            foreach (string i in LoadFromFile())
            {
                string[] wordsToTest = i.Split(' ');

                foreach (string n in wordsToTest)
                {

                    bool newWord = true;
                    foreach (WordObject t in words)
                    {
                        if (n.ToLower() == t.word)
                        {
                            t.usage++;
                            newWord = false;
                        }
                    }

                    if (newWord)
                    {
                        words.Add(new WordObject(n.ToLower()));
                    }

                }

                //Console.WriteLine(i.Trim());
            }

            //words = words.OrderBy(p => p.Substring(0)).ToList();
            words = (from x in words

                     orderby x.usage * -1

                     select x).ToList();

            foreach (WordObject i in words)
            {
                Console.WriteLine(i.word + ": " + i.usage);
            }
        }

        static void FindMostUsedLetters()
        {
            List<WordObject> words = new List<WordObject>();

            foreach (string i in LoadFromFile())
            {

                foreach (char t in i.ToLower())
                {
                    bool newWord = true;
                    foreach (WordObject e in words)
                    {
                        if (t == e.word[0])
                        {
                            e.usage++;
                            newWord = false;
                        }
                    }

                    if (newWord)
                    {
                        words.Add(new WordObject("" + t));
                    }


                }

                //Console.WriteLine(i.Trim());
            }

            //words = words.OrderBy(p => p.Substring(0)).ToList();
            words = (from x in words

                     orderby x.usage * -1

                     select x).ToList();

            foreach (WordObject i in words)
            {
                Console.WriteLine(i.word + ": " + i.usage);
            }
        }

        static void PrintB()
        {
            foreach (string i in (from x in LoadFromFile()

                                  orderby x.Trim().ToLower()

                                  select x).ToList())
            {
                string output = i.ToUpper().Trim();

                Dictionary<string, string> replacements = new Dictionary<string, string>()
                {
                    {"A", " " },
                    //{"B", " " },
                    {"C", " " },
                    {"D", " " },
                    {"E", " " },
                    {"F", " " },
                    {"G", " " },
                    {"H", " " },
                    {"I", " " },
                    {"J", " " },
                    {"K", " " },
                    {"L", " " },
                    {"M", " " },
                    {"N", " " },
                    {"O", " " },
                    {"P", " " },
                    {"Q", " " },
                    {"R", " " },
                    {"S", " " },
                    {"T", " " },
                    {"U", " " },
                    {"V", " " },
                    {"W", " " },
                    {"X", " " },
                    {"Y", " " },
                    {"Z", " " },

                    {"1", " " },
                    {"2", " " },
                    {"3", " " },
                    {"4", " " },
                    {"5", " " },
                    {"6", " " },
                    {"7", " " },
                    {"8", " " },
                    {"9", " " },
                    {"0", " " },

                    {"!", " " },
                    {"@", " " },
                    {"#", " " },
                    {"$", " " },
                    {"%", " " },
                    {"^", " " },
                    {"&", " " },
                    {"*", " " },
                    {"(", " " },
                    {")", " " },

                    {"/", " " },
                    {"{", " " },
                    {"}", " " },
                    {"[", " " },
                    {"]", " " },
                    {"-", " " },
                    {"_", " " },
                    {"+", " " },
                    {"=", " " },
                    {".", " " },
                    {",", " " },
                    {"<", " " },
                    {">", " " },
                    {"?", " " },
                };

                foreach (string n in replacements.Keys)
                {
                    output = output.Replace(n, replacements[n]);
                }

                Console.WriteLine(output);
            }
        }

        static void SyllableSort()
        {
            

            foreach (string i in (from x in LoadFromFile()

                                  orderby x.Trim().ToLower()

                                  select x).ToList())
            {
                Console.WriteLine(i.Trim());
            }
        }
        #endregion

        static List<string> LoadFromFile()
        {
            string fullText = "";
            string line = "";
            List<string> fullLine = new List<string>();
            StreamReader sr = new StreamReader("C:\\Users\\22GaddieR\\Bidlings.txt");
            //Read the first line of text
            line = sr.ReadLine();
            //Continue to read until you reach end of file
            while (line != null)
            {
                fullText += line + "\n";

                //write the line to console window
                //Console.WriteLine(line);
                fullLine.Add(line);
                //Read the next line
                line = sr.ReadLine();

            }
            //close the file
            sr.Close();

            return fullLine;

        }
    }

    class WordObject
    {
        public string word;
        public int usage;

        public WordObject(string word)
        {
            this.word = word;
            usage = 1;
        }
    }
}
