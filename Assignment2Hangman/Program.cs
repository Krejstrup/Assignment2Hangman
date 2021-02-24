
/*********************************************************
*   ____  __.                  __                        *
*  |    |/ _|___________      |__|____    ____           *
*  |      < \_  __ \__  \     |  \__  \  /    \          *
*  |    |  \ |  | \// __ \_   |  |/ __ \|   |  \         *
*  |____|__ \|__|  (____  /\__|  (____  /___|  /         *
*          \/           \/\______|    \/     \/          *
*  _________    _  _    _________            .___        *
*  \_   ___ \__| || |__ \_   ___ \  ____   __| _/____    *
*  /    \  \/\   __   / /    \  \/ /  _ \ / __ |/ __ \   *
*  \     \____|  ||  |  \     \___(  <_> ) /_/ \  ___/   *
*   \______  /_  ~~  _\  \______  /\____/\____ |\___  >  *
*          \/  |_||_|           \/            \/    \/   *
*                                                        *
*********** 2021-02-25 ** Richard Krejstrup **************/

/* Assignment 2 - Hangman.
 * Rules :
 * Player has 10 times to guess on the word.
 * o Player can have two types of guesses:
 *  - Guess a letter: If player guess a letter that occurs
 *    in the word, the program should update by inserting
 *    the letter in the correct position(s).
 *  - Guess a word: The player type in a word he/she thinks 
 *    is the word. If the guess is correct player wins the
 *    game and the whole word is revealed. If the word is
 *    incorrect nothing should get revealed.
 * o If the player guesses the same letter twice, the program
 *   will not consume a guess.
 *   
 * Must have:
 *  o The secret word should be randomly chosen from an array   | Check
 *    of Strings.
 *  o The incorrect letters the player has guessed, should be   | ?
 *    put inside a StringBuilder and presented to the player
 *    after each guess.
 *  o The correct letters should be put inside a char array.    | ?
 *    Unrevealed letters need to be represented by a lower 
 *    dash ( _ ).
 *    
 * Optional:
 *  o You unit tests need to have at least 50% coverage.        | ?
 *  o Read in the words from a text file with Comma-separated   | kind of (file)
 *    values and then store them in an array or list of Strings.
 * 
 *===| 
 *   O
 *  /|\
 *  / \
 *  
 */

using System;
using System.IO;
using System.Text;

namespace Assignment2Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfGuesses = 0;
            int numberOfMisses  = 0;
            int numberOfRights  = 0;
 
            string[] setOfDifferentWords = null;
            Random aRandomObject = new Random();
            char userGameTypeChar;
            char[] userGuessedChars = new char[12]; // Just a start, will build up if nessesary
            GetWordsFromFile(ref setOfDifferentWords);


            // If loopin the game restart from here===============================
            DrawMenu(true);
            string gameWord = setOfDifferentWords[aRandomObject.Next(0, setOfDifferentWords.Length)];   // pick one of all words
            StringBuilder sbGameWord = new StringBuilder(gameWord);
            StringBuilder sbHiddenWord = new StringBuilder();

            //Console.WriteLine(gameWord); // Test-cheater!! :O

            do  // let the user decide what type of a game we are to play letter or word
            {
                userGameTypeChar = UserInputChar("Enter type of game [l/w]: ");

            } while ((userGameTypeChar != 'l') && (userGameTypeChar != 'w'));

            //============== Below here is the game of pick a letter ======= (Word further down)

            if (userGameTypeChar == 'l')
            {
                bool isThisTypedBefore = false;
                char userGuess;
                Console.WriteLine("Ok - we will play for the letter then.");

                try
                {
                    do      // Kepp the game alive in this loop
                    {
                        do  // A lot has been removed and this is left - ask for a guess.
                        {
                            isThisTypedBefore = false;
                            userGuess = UserInputChar("\nPick a letter: "); //========< Stops For User Input]
                            if (Array.Exists(userGuessedChars, element => element == userGuess))// Ok, I walked into a Lamda expression here
                            {
                                isThisTypedBefore = true;
                                Console.WriteLine("No, you allready typed {0} before. ", userGuess);
                            }

                        } while (isThisTypedBefore);
                        if (userGuessedChars.Length == numberOfGuesses)
                            Array.Resize(ref userGuessedChars, userGuessedChars.Length + 1);
                        userGuessedChars[numberOfGuesses++] = userGuess;

                        // --- Guess input ended. Rewrite menu ithems ---------------
                        Console.Clear();
                        DrawMenu();

                        Console.Write("Number of guessed so far: {0} \nAnd letters you have tried so far: ", numberOfGuesses.ToString());
                        foreach (char buppEliBupp in userGuessedChars)
                        {
                            Console.Write(" " + buppEliBupp);
                        }


                        // ---------- Building underscore word: ---------------
                        bool jackPot = false;
                        numberOfRights = 0;
                        sbHiddenWord.Clear();   // erase the text to restart
                        for ( int myLoop = 0; myLoop < sbGameWord.Length; myLoop++) // === Start making hidden fields
                        {
                            jackPot = false;
                            foreach (char inputChars in userGuessedChars)
                            {
                                if (gameWord[myLoop] == inputChars)
                                {
                                    // this position in string has a good letter
                                    sbHiddenWord.Append(inputChars);
                                    sbHiddenWord.Append(' ');
                                    jackPot = true;
                                    numberOfRights++;
                                }
                            }
                            if (!jackPot) sbHiddenWord.Append("_ ");
                        }
                        //blippedGameWord = babbelByxa.ToString();


                        Console.WriteLine("\nWill you solve this: [{0}] \n", sbHiddenWord.ToString());

                        if (!  sbGameWord.ToString().Contains(userGuess.ToString()))
                            numberOfMisses++;
                        PlayerHangMan(numberOfMisses); // have no idea if it was right or not (yet)

                    } while ((numberOfMisses < 10)&&(numberOfRights!=sbGameWord.Length));

                    Console.Clear();
                    DrawMenu();
                    Console.WriteLine("Okej GG, the word was: " + sbGameWord.ToString());
                    if (numberOfMisses > 9) Console.WriteLine("\nBUT YOU TOTALLY DIED DUDE!");

                } catch (Exception eh)
                {
                    Console.WriteLine(eh.ToString());
                }
                

            }
            else // ======== Alternative game with Words starts here ================
            {
                Console.Clear();
                DrawMenu();
                Console.WriteLine("\nOk - we will play for the whole word then.");
                Console.WriteLine("I have totally picked a new word.");

                do // Do the tricks in here
                {
                    Console.Write("The picked word is {0} letter long. Take a guess: ", gameWord.Length);
                    StringBuilder userInputLine = new StringBuilder(Console.ReadLine());
                    if (userInputLine.Length > sbGameWord.Length)
                    {
                        Console.Write("What? No - that's to big.");
                    } else if (userInputLine.Length < sbGameWord.Length) 
                    {
                        Console.Write("What? No - that's to small.");
                    } else
                    {
                        Console.Write("Ok, good guess. ");
                    }

                    if (sbGameWord.Equals(userInputLine))
                    {
                        Console.WriteLine("WHAT? Are you kidding me? That's the right word!");
                        Console.WriteLine("Now - Fuck off!");
                        Environment.Exit(0);
                    }








                } while (true);

            }

            Console.WriteLine("Do you you want to play again?");
            Console.ReadLine();
            Console.WriteLine("Nah, just kidding - you'r to bad\n\n\n\n");
        }   // == End of Main() ------------------------------------------------


 

        static void DrawMenu(bool startUp=false)
        {
            // Just draw the headline of the game.
            Console.WriteLine(             "**************************************");
            Console.WriteLine(             "*    Welcome to the hangman show!    *");
            Console.WriteLine(             "*  I'll pick a word, and you'll      *");
            Console.WriteLine(             "*    guess what it is.               *");
            if (startUp) Console.WriteLine("*  Do you want to guess a (w)ord     *");
            if (startUp) Console.WriteLine("*    or (l)etters?                   *");
            Console.WriteLine(             "****   You'll hang by 10 misses!  ****\n");

        } // End of Method DrawMenu() -------------------------------------------


        /// <summary>
        /// GetWordsFromFile() fetches a list of game words from a GameWords.txt-file
        /// </summary>
        /// <param name="theWordArray">The string array can be empty or prefilled. Pass it as a ref and this method will fill the array up.</param>
        static void GetWordsFromFile(ref string[] theWordArray)
        {
            string filePathAndName = Environment.CurrentDirectory + "\\GameWords.txt";

            try
            {
                using (StreamReader myFilestream = File.OpenText(filePathAndName))
                {
                    // Console.WriteLine("Opening the file: {0}.", filePathAndName);
                    string myStreamReaderString = "";
                    int lengtOfArray;
                    
                    while ((myStreamReaderString = myFilestream.ReadLine()) != null)    // I sometimes forget that the '=' operator returns the same value as the others.
                    {
                        myStreamReaderString = myStreamReaderString.ToLower();  // only use low key characters
                        // Insert myStreamReaderString into end of theWordArray[]
                        if (theWordArray == null)
                        {
                            theWordArray = new string[1];
                            theWordArray[0] = myStreamReaderString;
                        } else
                        { 
                            lengtOfArray = theWordArray.Length;
                            Array.Resize(ref theWordArray, lengtOfArray + 1);
                            //ToDo: check for numbers and other unwanted characters
                            theWordArray[lengtOfArray] = myStreamReaderString;
                        }
                    }
                    myFilestream.Close();
                }
            }
            catch (Exception anDangerousException)
            {
                Console.WriteLine("\nUh, something happened: {0}", anDangerousException.ToString());
            }

        } // End of Method GetWordsFromFile() -----------------------------


        static char UserInputChar(string textMessage)
        {
            // just get a valid character from user
            
            while (true)
            {
                Console.Write(textMessage);
                try
                {
                    string userInput = Console.ReadLine();
                    char userInputChar = userInput[0];  // I'll just snatch the first character!
                    if (Char.IsLetter(userInputChar)) return userInputChar;
                }
                catch (Exception theException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exception: UserInputChar()");
                    Console.WriteLine(theException.ToString());
                    Console.ResetColor();
                    return '0';
                }
            }

        }// End of Method UserInputChar() -------------------------------------


        static void PlayerHangMan(int nrWrongGuess)
        {
            if (nrWrongGuess>0)
            {
                string screenMan = "===|\n   O\n  /|\\\n  / \\ \n";  // 1,2,3,4,(5,6,7,8,9),(10,11,12,13),14,15,(16,17,18,19),(20,21,22)
                int[] screenManPositions = { 1, 2, 3, 4, 9, 13, 14, 15, 19, 22 };
                nrWrongGuess--;

                Console.WriteLine(screenMan.Remove(screenManPositions[nrWrongGuess]));
            }

        }// End of Method PlayerHangMan() -------------------------------------


    }   // == End of Program
}   // == End of Namespace
