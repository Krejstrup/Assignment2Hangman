
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

/* Assignment 2 - Hangman. And with somewhat of sarcasm ;)
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
 *  o The incorrect letters the player has guessed, should be   | Check
 *    put inside a StringBuilder and presented to the player
 *    after each guess.
 *  o The correct letters should be put inside a char array.    | Check
 *    Unrevealed letters need to be represented by a lower 
 *    dash ( _ ).
 *
 * Optional:
 *  o Your unit tests need to have at least 50% coverage.       | Naaahh
 *  o Read in the words from a text file with Comma-separated   | Kind of:
 *    values and then store them in an array or list of Strings. Loads from file
 *
 *===| 
 *   O  Die allready!
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
            string[] setOfDifferentWords = null;
            Random aRandomObject = new Random();
            StringBuilder sbUserFailGuessedString = new StringBuilder(" ", 15);
            GetWordsFromFile(ref setOfDifferentWords);
            StringBuilder sbGameWord = null;
            string inputUserGuessString;
            bool userShotForWord = false;
            char inputUserGuessChar = 'A';
            bool keepGameAlive = true;
            bool userWonGame = false;
            bool newGame = true;
            char[] caHiddenWord = null;
            bool isThisTypedBefore;

            // If loopin the game restart from here===============================
            do  //======KeepGameAlive
            {
                int numberOfGuesses = 0;
                int numberOfMisses  = 0;
                int numberOfRights  = 0;
                sbUserFailGuessedString.Clear();
                userWonGame = false;
                userShotForWord = false;

                do  //=== DID I WIN 
                {


                    Console.Clear();
                    DrawMenu();

                    if (newGame)    // Prepping for a new game. Use this newGame-bool to show or hide texts and others.
                    {
                        // ToCome:== Pick a gameWord from >word pool< then claim and fill memory of array for hidden representation

                        sbGameWord = new StringBuilder(setOfDifferentWords[aRandomObject.Next(0, setOfDifferentWords.Length)]);
                        caHiddenWord = new char[sbGameWord.Length];
                        for (int myLoop = 0; myLoop < caHiddenWord.Length; myLoop++) caHiddenWord[myLoop] = '_';
                        Console.WriteLine("Allright kids > new game of Hang That Man!");
                    }
                        // ToCome:== Set up game area

                    Console.Write(  "The word length is   : " + sbGameWord.Length);
                    Console.Write("\nThe guess word a.t.m : "); // and the "___" text will come here.

                    foreach (char aChar in caHiddenWord)    
                    {
                        Console.Write(aChar.ToString());
                    }
                    
                    if (!newGame) Console.Write("\nYour previous guesses are: {0}\n", sbUserFailGuessedString.ToString());

                    PlayerHangMan(numberOfGuesses);
                    if (!newGame) Console.Write("\nSee - you'r soon dead as a rock! {0}/10", numberOfGuesses);


                    // ToCome:======= User Guesses and inputs Word or Letter ========

                    do //reapeat this input-section if [isThisTypedBefore] :
                    {
                        isThisTypedBefore = false;
                        inputUserGuessString = UserInput("\nTake a guess for full word, or a letter: ", sbGameWord.Length); //======< Stops For User Input]

                        // the first character from the user input string if not a try for the whole word
                        if (inputUserGuessString.Length != sbGameWord.Length)
                        {
                            inputUserGuessChar = inputUserGuessString[0];

                            //TEST HERE - IF PREVIOUSLY GUESSED SAME LETTER
                            if ( GuessedThisBefore(caHiddenWord, sbUserFailGuessedString, inputUserGuessChar) ) isThisTypedBefore = true;
                            if (isThisTypedBefore) Console.Write("What? Noo... you've tried that before");


                        }
                        else// If User choose to take a shot for the whole word - this has to be handled separately
                        {   // None of the characters can be taken to account for the FailGuess-list or correct word (rules!)
                            userShotForWord = true;

                            if (sbGameWord.Equals(inputUserGuessString))
                            {
                                userWonGame = true;
                                numberOfRights++;
                            }
                            else // if test fails its a miss and we put the first letter to 
                            {
                                numberOfMisses++;
                            }

                        }
                    } while (isThisTypedBefore);


                    // ToCome:======= Test the letter-guess for Good or Bad Guess. NOT IF WORD WAS GUESSED!!
                    if (!userShotForWord)
                    {

                        if (sbGameWord.ToString().Contains(inputUserGuessChar))
                        {
                            //We got at hit
                            PutInRight(ref caHiddenWord, inputUserGuessChar, sbGameWord.ToString());
                            numberOfRights++;
                        }
                        else
                        {
                            PutInWrong(ref sbUserFailGuessedString, inputUserGuessChar);
                            numberOfMisses++;
                        }
                    }
                    else userShotForWord = false;

                    numberOfGuesses++;



                    if (newGame) newGame = false;

                } while ((numberOfGuesses < 10) && (!userWonGame));
                // ===DID I WIN - End of Game-while ------------------------------------------


                Console.Clear();
                Console.WriteLine("*********** Statistics ***********");
                Console.WriteLine("* Number of tries: {0}      \t *", numberOfGuesses);
                Console.WriteLine("* Number of miss : {0}         \t *", numberOfMisses);
                Console.WriteLine("* Number of hits : {0}         \t *", numberOfRights);
                Console.WriteLine("* Random Word :{0}       \t *", sbGameWord.ToString());
                Console.WriteLine("* {0}         ", userWonGame ? "And You Won!!\t\t\t *\n" : "And You LOST!!\t\t *\n");
                Console.WriteLine("***********************************");
                if (!userWonGame) PlayerHangMan(10);

                string userEndAnswer=null;
                do
                {
                    userEndAnswer = UserInput("\nDo you you want to play again? [y/n]", 1);
                } while (!userEndAnswer.Contains('Y') && !userEndAnswer.Contains('N'));
                if (userEndAnswer.Contains('N')) keepGameAlive = false;
                if (userEndAnswer.Contains('Y')) newGame = true;


            } while (keepGameAlive);    //======KeepGameAlive

            Console.Clear();
            Console.WriteLine("\nAllright then - You sore looser - BYE!!\n\n\n\n\n\n");

        }   // == End of Main() ------------------------------------------------


        /// <summary>
        /// PutInWrong just inserts a wrong letter last in the string.
        /// </summary>
        /// <param name="theWrongString">The string that contains the bad guesses.</param>
        /// <param name="userInput">The letter that the user took a guess on.</param>
        static void PutInWrong(ref StringBuilder theWrongString, char userInput)
        {
            // Vi kollar inte om tecknet redan finns
            // kolla om det ska in i StringBuilder först
            theWrongString.Append(userInput);
        }
        /// <summary>
        /// PutInRight is using parameters to put the right letter in the right spot. Only 4 of same letters are put in.
        /// </summary>
        /// <param name="theRightArray">Input a ref til a char[] array. With a lot of '_'.</param>
        /// <param name="userInput">The character that was input from user, aka the guess.</param>
        /// <param name="gameRightWord">the string of the right word.</param>
        /// <returns>If all letters are at place we got a win and return true.</returns>
        static bool PutInRight(ref char[] theRightArray, char userInput, string gameRightWord)
        {
            // Vi kollar inte om tecknet redan finns
            // kolla om det ska in i StringBuilder först
            int rightPosition = gameRightWord.IndexOf(userInput);
            theRightArray[rightPosition] = userInput;

            //--- There could be more of this letter in the word, check for that----------
            rightPosition = gameRightWord.IndexOf(userInput, rightPosition + 1);
            if (rightPosition > -1)
            {
                theRightArray[rightPosition] = userInput;
                rightPosition = gameRightWord.IndexOf(userInput, rightPosition + 1);
                if (rightPosition > -1)  // get in here if there's a third match
                {
                    rightPosition = gameRightWord.IndexOf(userInput, rightPosition + 1);
                    if (rightPosition > -1) theRightArray[rightPosition] = userInput;
                }
            }

            int countTheEmty = 0;
            foreach (char loopingCharacter in theRightArray)
            {
                if (loopingCharacter == '_') countTheEmty++;
            }
            if (countTheEmty == 0) return true;   // returnera true om vi vann! För helt ord testar vi hela ordet direkt!
            return false;
        }


        static void DrawMenu()
        {
            // Just draw the headline of the game.
            Console.WriteLine("__________Krajan greets you___________");
            Console.WriteLine("|    Welcome to the Hang Man Show!   |");
            Console.WriteLine("|  I'll pick a word, and you'll      |");
            Console.WriteLine("|    guess what it is.               |");
            Console.WriteLine("|  Write a whole word or just guess  |");
            Console.WriteLine("| a letters.  You'll hang after      |");
            Console.WriteLine("|  just 10 chances!                  |");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

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
                            myStreamReaderString = myStreamReaderString.ToUpper();  // only use low key characters
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


        static string UserInput(string textMessage, int wordLetterExact)
        {
            // just get a string or valid character from user
            
            while (true)
            {
                Console.Write(textMessage);
                try
                {
                    string userInput;
                    do
                    {
                        userInput = Console.ReadLine();
                    } while (userInput.Length < 0);
                    if (userInput.Length != wordLetterExact)
                    {
                        char userInputChar = userInput[0];  // I'll just snatch the first character!
                        if (Char.IsLetter(userInputChar)) return (userInputChar.ToString()).ToUpper();
                    } else
                    {
                        return userInput.ToUpper();
                    }
                }
                catch (Exception theException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exception: UserInputChar()");
                    Console.WriteLine(theException.ToString());
                    Console.ResetColor();
                    return null;
                }
            }

        }// End of Method UserInputChar() -------------------------------------

        /// <summary>
        /// GuessedThisBefore() just checkes if the user has tried this guess before.
        /// </summary>
        /// <param name="theRightOnes">The Array of chars that we should test through.</param>
        /// <param name="theWrongOnes">The StringBuilder of what we should test through.</param>
        /// <param name="theUserGuess">The guessing letter.</param>
        /// <returns></returns>
        static bool GuessedThisBefore(char[] theRightOnes, StringBuilder theWrongOnes, char theUserGuess)
        {

            foreach (char aCharSelect in theRightOnes)
            {
                if (aCharSelect == theUserGuess) return true;
            }

            if (theWrongOnes.ToString().Contains(theUserGuess)) return true;

            return false;
        }


        static void PlayerHangMan(int nrWrongGuess)
        {
            if (nrWrongGuess>0)
            {
                string screenMan = "===|\n   O\n  /|\\\n  / \\ \n";  // 1,2,3,4,(5,6,7,8,9),(10,11,12,13),14,15,(16,17,18,19),(20,21,22)
                int[] screenManPositions = { 1, 2, 3, 4, 9, 13, 14, 15, 19, 22 };
                nrWrongGuess--;

                Console.WriteLine("\n" + screenMan.Remove(screenManPositions[nrWrongGuess]));
            }
            else
            {
                Console.Write("\n\n\n");    // leave area for Batman!
            }
           
        }// End of Method PlayerHangMan() -------------------------------------


    }   // == End of Program
}   // == End of Namespace
