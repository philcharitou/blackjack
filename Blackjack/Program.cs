using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Program
    {
        const int WINDOW_WIDTH = 110;
        const int WINDOW_HEIGHT = 30;
        const int HIGH_ACE_VALUE = 11;
        const int LOW_ACE_VALUE = 1;
        const int NO_HAND_NUMBER = 0;
        const int FIRST_HAND_INDEX = 0;
        const int SECOND_HAND_INDEX = 1;
        const int THIRD_HAND_INDEX = 2;
        const int FOURTH_HAND_INDEX = 3;
        const int BLACKJACK = 21;
        const int TWO_CARDS = 2;
        const string NO_HAND = "";
        const int FIRST_CARD_INDEX = 0;
        const int SECOND_CARD_INDEX = 1;
        const int FOURTH_CARD_INDEX = 3;
        const int ONE_CARD = 1;
        const int NO_MONEY = 0;
        const int FIRST_MOVE_INDEX = 0;
        const int SECOND_MOVE_INDEX = 1;
        const int THIRD_MOVE_INDEX = 2;
        const int FOURTH_MOVE_INDEX = 3;
        const int FIFTH_MOVE_INDEX = 4;
        const int SIXTH_MOVE_INDEX = 5;
        const int FOUR_MOVES = 4;
        const int FIVE_MOVES = 5;
        const int SIX_MOVES = 6;
        const int NO_MOVES_POSSIBLE = 0;
        const int GEN_FIRST_CARD = 0;
        const int GEN_LAST_CARD = 52;
        const int REGULAR = 0;
        const int DOUBLE_DOWN = 1;
        const int SHUFFLE_START = 0;
        const int SHUFFLE_AMOUNT = 750;
        const int INTRO = 0;
        const int INSTRUCTIONS = 1;
        const int BET_AMOUNT = 2;
        const int INVALID_BET = 3;
        const int USER_HAND_CHOICE = 4;
        const int INSURANCE = 5;
        const int PLAYER_BLACKJACK = 6;
        const int DEALER_BLACKJACK = 7;
        const int BLACKJACK_TIE = 8;
        const int OVER_21 = 9;
        const int DEALER_OVER = 10;
        const int HAND_TIE = 11;
        const int PLAYER_HIGHER_HAND = 12;
        const int PLAYER_LOWER_HAND = 13;
        const int NO_SPLIT_MONEY = 14;
        const int DOUBLE_DOWN_DISPLAY = 15;

        static string[] shuffleCards = new string[52] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A", "2", "3", "4", "5",
            "6", "7", "8", "9", "10", "J", "Q", "K", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K","A","2","3","4","5","6","7",
            "8","9","10","J","Q","K"};

        static string[,] playerCards = new string[4, 12];
        static string[] dealerCards = new string[10];

        static Random randInt = new Random();

        static int[] playerCardCount = new int[4] { 0, 0, 0, 0 };
        static int dealerCardsCount;

        static int[] playerHandsValues = new int[4];
        static int dealerHandValue;

        static int playerHandsCount = 1;

        static int nextCardInDeck;

        static int playerMoney = 200;

        static bool dealerBlackJack;
        static bool playerBlackJack;

        static bool[] splitsPossible = new bool[4];
        static bool doubleDownPossible;
        static bool insurancePossible;

        static int insuranceBet;

        static string[] possibleMoves = new string[6];
        static int possibleMovesCount;

        static bool[] handsStanding = new bool[4];

        static bool gameOver = false;
        static bool newRound = true;
        static bool handOver = true;

        static int handsDoneWith;

        static string userResponse;
        static int userBet;

        static void Main(string[] args)
        {
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            Console.Title = "BlackJack!";

            bool validBet;

            Display(INTRO, NO_HAND_NUMBER);

            userResponse = Console.ReadLine();

            if (userResponse.ToLower() == "h")
            {
                Display(INSTRUCTIONS, NO_HAND_NUMBER);
            }
            else
            {
                while (gameOver == false)
                {
                    if (playerMoney == NO_MONEY)
                    {
                        Console.WriteLine("You have no money left. Would you like to restart with 200 chips or quit?\nAnswer with either \"restart\" or \"quit\".");

                        userResponse = Console.ReadLine();

                        if (userResponse.ToLower() == "restart")
                        {
                            playerMoney = 200;
                        }
                        else if (userResponse.ToLower() == "quit")
                        {
                            Environment.Exit(0);
                        }
                        else
                        {
                            Console.WriteLine("\nPlease type in either \"restart\" to restart the game or \"quit\" to stop playing.\n");
                        }
                    }
                    else
                    {
                        if (newRound)
                        {
                            for (int i = NO_MOVES_POSSIBLE; i < possibleMoves.Length; i++)
                            {
                                possibleMoves[i] = "";
                            }

                            possibleMovesCount = 0;

                            while (true)
                            {
                                Display(BET_AMOUNT, NO_HAND_NUMBER);

                                validBet = int.TryParse(Console.ReadLine(), out userBet);

                                if (validBet == true && userBet == NO_MONEY)
                                {
                                    Environment.Exit(0);
                                }
                                else if (validBet == true && userBet <= playerMoney)
                                {
                                    playerMoney -= userBet;
                                    break;
                                }
                                else
                                {
                                    Display(INVALID_BET, NO_HAND_NUMBER);
                                    Console.ReadLine();
                                }
                            }

                            ShuffleCards();
                            DealingCards();

                            if (playerCards[FIRST_HAND_INDEX, FIRST_CARD_INDEX] == playerCards[FIRST_HAND_INDEX, SECOND_CARD_INDEX] &&
                                playerMoney - userBet >= NO_MONEY)
                            {
                                splitsPossible[FIRST_HAND_INDEX] = true;
                            }

                            if (dealerCards[FIRST_CARD_INDEX] == "A")
                            {
                                insurancePossible = true;
                            }

                            playerHandsValues = PlayerHandsValues(playerCards);
                            dealerHandValue = DealerHandValue(dealerCards);

                            if (playerHandsValues[FIRST_HAND_INDEX] == BLACKJACK)
                            {
                                playerBlackJack = true;
                            }
                            else
                            {
                                playerBlackJack = false;
                            }

                            if (dealerHandValue == BLACKJACK)
                            {
                                dealerBlackJack = true;
                            }
                            else
                            {
                                dealerBlackJack = false;
                            }

                            if (playerMoney - userBet < NO_MONEY)
                            {
                                doubleDownPossible = false;
                            }
                            else
                            {
                                doubleDownPossible = true;
                            }

                            newRound = false;

                            handsDoneWith = 0;
                        }

                        for (int i = FIRST_HAND_INDEX; i < playerHandsCount; i++)
                        {
                            handOver = false;

                            UserChoice(i);
                        }
                    }
                }
            }
        }

        static private void ShuffleCards()
        {
            int cardOne;
            int cardTwo;

            string oldCard;

            for (int i = SHUFFLE_START; i < SHUFFLE_AMOUNT; i++)
            {
                cardOne = randInt.Next(GEN_FIRST_CARD, GEN_LAST_CARD);
                cardTwo = randInt.Next(GEN_FIRST_CARD, GEN_LAST_CARD);

                oldCard = shuffleCards[cardOne];

                shuffleCards[cardOne] = shuffleCards[cardTwo];

                shuffleCards[cardTwo] = oldCard;
            }
        }

        static private void Display(int currentDisplay, int currentHand)
        {
            string[] displayPlayerHands = new string[4];
            string[] displayHandValues = new string[4];

            string displayDealerHand;

            string displayChoices;

            string displayHandNum = "";

            Console.Clear();

            switch (currentDisplay)
            {
                case INTRO:
                    Console.WriteLine("Welcome to Blackjack! Press the H key on your keyboard followed by ENTER to see how to play"
                        + "\nor press any other key followed by the ENTER key to begin the game.");
                    break;
                case INSTRUCTIONS:
                    // TODO - Write instructions display case.
                    break;
                case BET_AMOUNT:
                    Console.WriteLine("You currently have a total of " + playerMoney + " chips.\n\nEnter the amount which you would like to bet" +
                        " followed by the ENTER key or press the 0 key followed by ENTER\nto quit the game.");
                    break;
                case INVALID_BET:
                    Console.WriteLine("Invalid bet amount. Press enter to try again.");
                    break;
                case INSURANCE:
                    Console.WriteLine("The dealer's current cards are: " + dealerCards[0] + ", *");

                    Console.WriteLine("\nThe dealer has an A as their first card, would you like to make an insurance?");
                    Console.WriteLine("Type in yes if you do and no if you don't.");
                    break;
                case USER_HAND_CHOICE:
                case BLACKJACK_TIE:
                case DEALER_BLACKJACK:
                case PLAYER_BLACKJACK:
                case OVER_21:
                case HAND_TIE:
                case DEALER_OVER:
                case PLAYER_HIGHER_HAND:
                case NO_SPLIT_MONEY:
                case PLAYER_LOWER_HAND:
                    Console.WriteLine("You currently have a total of " + playerMoney + " chips.");
                    Console.WriteLine("\nYour current cards are:");

                    for (int i = FIRST_HAND_INDEX; i < playerHandsCount; i++)
                    {
                        for (int j = FIRST_CARD_INDEX; j < playerCardCount[i]; j++)
                        {
                            if (j == FIRST_CARD_INDEX)
                            {
                                switch (i)
                                {
                                    case FIRST_HAND_INDEX:
                                        displayPlayerHands[i] = "\tFirst hand: " + playerCards[i, j];
                                        displayHandValues[i] = "\tValue for First hand: " + playerHandsValues[i];
                                        break;
                                    case SECOND_HAND_INDEX:
                                        displayPlayerHands[i] = "\tSecond hand: " + playerCards[i, j];
                                        displayHandValues[i] = "\tValue for Second hand: " + playerHandsValues[i];
                                        break;
                                    case THIRD_HAND_INDEX:
                                        displayPlayerHands[i] = "\tThird hand: " + playerCards[i, j];
                                        displayHandValues[i] = "\tValue for Third hand: " + playerHandsValues[i];
                                        break;
                                    case FOURTH_HAND_INDEX:
                                        displayPlayerHands[i] = "\tFourth hand: " + playerCards[i, j];
                                        displayHandValues[i] = "\tValue for Fourth hand: " + playerHandsValues[i];
                                        break;
                                }
                            }
                            else
                            {
                                displayPlayerHands[i] += ", " + playerCards[i, j];
                            }
                        }
                    }

                    for (int i = FIRST_HAND_INDEX; i < playerHandsCount; i++)
                    {
                        Console.WriteLine(displayPlayerHands[i]);
                        Console.WriteLine(displayHandValues[i]);
                    }

                    if (currentDisplay == HAND_TIE 
                        || currentDisplay == DEALER_OVER 
                        || currentDisplay == PLAYER_HIGHER_HAND
                        || currentDisplay == PLAYER_LOWER_HAND)
                    {
                        displayDealerHand = "\nThe dealer's cards are: " + dealerCards[FIRST_CARD_INDEX];

                        for (int i = SECOND_CARD_INDEX; i < dealerCardsCount; i++)
                        {
                            displayDealerHand += ", " + dealerCards[i];
                        }

                        Console.WriteLine(displayDealerHand);
                        Console.WriteLine("The dealer's hand value is " + dealerHandValue + ".");
                    }
                    else
                    {
                        Console.WriteLine("\nThe dealer's current cards are: " + dealerCards[0] + ", *");
                    }

                    switch (currentHand)
                    {
                        case FIRST_HAND_INDEX:
                            displayHandNum = "FIRST";
                            break;
                        case SECOND_HAND_INDEX:
                            displayHandNum = "SECOND";
                            break;
                        case THIRD_HAND_INDEX:
                            displayHandNum = "THIRD";
                            break;
                        case FOURTH_HAND_INDEX:
                            displayHandNum = "FOURTH";
                            break;
                    }

                    if (currentDisplay == BLACKJACK_TIE)
                    {
                        Console.WriteLine("\nBoth you and the dealer got Blackjack! You don't lose or gain any money this round.");
                        Console.WriteLine("You now have a total of " + playerMoney + " chips, Good job!");
                        Console.WriteLine("Press ENTER to continue your game of Blackjack");
                    }
                    else if (currentDisplay == DEALER_BLACKJACK)
                    {
                        Console.WriteLine("\nThe dealer had a Blackjack, and you automatically lose.");
                        Console.WriteLine("You now have a total of " + playerMoney + " chips, better luck next time.");
                        Console.WriteLine("Press ENTER to continue your game of Blackjack");
                    }
                    else if (currentDisplay == PLAYER_BLACKJACK)
                    {
                        Console.WriteLine("\nYou have a Blackjack! You win 1.5 times your original bet.");
                        Console.WriteLine("You now have a total of " + playerMoney + " chips, Congrajulations!");
                        Console.WriteLine("Press ENTER to continue your game of Blackjack.");
                    }
                    else if (currentDisplay == OVER_21)
                    {
                        if (currentHand != DOUBLE_DOWN_DISPLAY)
                        {
                            switch (currentHand)
                            {
                                case FIRST_CARD_INDEX:
                                    Console.WriteLine("\nYou have went over 21 for your FIRST hand, press enter to continue.");
                                    break;
                                case SECOND_CARD_INDEX:
                                    Console.WriteLine("\nYou have went over 21 for your SECOND hand, press enter to continue.");
                                    break;
                                case THIRD_HAND_INDEX:
                                    Console.WriteLine("\nYou have went over 21 for your THIRD hand, press enter to continue.");
                                    break;
                                case FOURTH_HAND_INDEX:
                                    Console.WriteLine("\nYou have went over 21 for your FOURTH hand, press enter to continue.");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nYou have went over 21 for your DOUBLE DOWN hand and lost your bet, press enter to continue to your game.");
                        }
                    }
                    else if (currentDisplay == HAND_TIE)
                    {
                        if (currentHand != DOUBLE_DOWN_DISPLAY)
                        {
                            Console.WriteLine("\nThe value for your " + displayHandNum + " hand is equal to the one of the dealer. You did not lose or "
                            + "gain any money.");
                            Console.WriteLine("Press the ENTER key to continue.");

                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("\nThe value for your DOUBLE DOWN hand is equal to the one of the dealer. You did not lose or gain any money.");
                            Console.WriteLine("Press the ENTER key to continue.");

                            Console.ReadLine();
                        }
                    }
                    else if (currentDisplay == DEALER_OVER)
                    {
                        if (currentHand != DOUBLE_DOWN_DISPLAY)
                        {
                            Console.WriteLine("\nThe dealer has gone over 21! Congrajulations, you have won " + userBet + " chips for your " + displayHandNum
                                + " hand.");
                            Console.WriteLine("Press the ENTER key to continue playing!");

                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("\nThe dealer has gone over 21! Congrajulations, you have won " + (userBet + userBet) + " chips for your DOUBLE DOWN hand.");
                            Console.WriteLine("Press the ENTER key to continue playing!");

                            Console.ReadLine();
                        }
                    }
                    else if (currentDisplay == PLAYER_HIGHER_HAND)
                    {
                        if (currentHand != DOUBLE_DOWN_DISPLAY)
                        {
                            Console.WriteLine("\nCongrajulations, You have a higher hand value for your " + displayHandNum + " hand then the dealer "
                            + "resulting in a win!");
                            Console.WriteLine("You have won a total of " + userBet + " chips, good job!");
                            Console.WriteLine("Press the ENTER key to continue playing!");

                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("\nCongrajulations, You have a higher hand value for your DOUBLE DOWN hand then the dealer resulting in a win!");
                            Console.WriteLine("You have won a total of " + (userBet + userBet) + " chips, good job!");
                            Console.WriteLine("Press the ENTER key to continue playing!");

                            Console.ReadLine();
                        }
                    }
                    else if (currentDisplay == PLAYER_LOWER_HAND)
                    {
                        if (currentHand != DOUBLE_DOWN_DISPLAY)
                        {
                            Console.WriteLine("\nYou have a lower hand value then the dealer for your " + displayHandNum + " hand. Sadly you lost.");
                            Console.WriteLine("You have lost a total of " + userBet + " chips, better luck next time!");
                            Console.WriteLine("Press the ENTER key to continue playing.");

                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("\nYou have a lower hand value then the dealer for your DOUBLE DOWN hand. Sadly you lost.");
                            Console.WriteLine("You have lost a total of " + (userBet + userBet) + " chips, better luck next time!");
                            Console.WriteLine("Press the ENTER key to continue playing.");

                            Console.ReadLine();
                        }
                    }
                    else if (currentDisplay == NO_SPLIT_MONEY)
                    {
                        Console.WriteLine("\nI am sorry, you don't have enough money to split.");
                        Console.WriteLine("Press the ENTER key to try doing something else.");

                        Console.ReadLine();
                    }
                    else
                    {
                        switch (currentHand)
                        {
                            case FIRST_HAND_INDEX:
                                Console.WriteLine("\nWhat would you like to do with your FIRST hand?");
                                break;
                            case SECOND_HAND_INDEX:
                                Console.WriteLine("\nWhat would you like to do with your SECOND hand?");
                                break;
                            case THIRD_HAND_INDEX:
                                Console.WriteLine("\nWhat would you like to do with your THIRD hand?");
                                break;
                            case FOURTH_HAND_INDEX:
                                Console.WriteLine("\nWhat would you like to do with your FOURTH hand?");
                                break;
                        }

                        Console.WriteLine("\nPlease type in one of the options below or enter the number which corresponds to that option.");

                        possibleMoves[FIRST_MOVE_INDEX] = "Quit";
                        possibleMoves[SECOND_MOVE_INDEX] = "Stand";
                        possibleMoves[THIRD_MOVE_INDEX] = "Hit";

                        possibleMovesCount = 3;

                        displayChoices = "\t1) Quit\n\t2) Stand\n\t3) Hit\n";

                        if (doubleDownPossible == true)
                        {
                            possibleMoves[possibleMovesCount] = "Double Down";

                            possibleMovesCount++;

                            displayChoices += "\t" + possibleMovesCount + ") Double Down\n";
                        }

                        if (splitsPossible[currentHand] == true)
                        {
                            possibleMoves[possibleMovesCount] = "Split Cards";

                            possibleMovesCount++;

                            displayChoices += "\t" + possibleMovesCount + ") Split Cards\n";
                        }

                        if (insurancePossible == true)
                        {
                            possibleMoves[possibleMovesCount] = "Insurance";

                            possibleMovesCount++;

                            displayChoices += "\t" + possibleMovesCount + ") Insurance\n";
                        }

                        Console.WriteLine(displayChoices);
                    }
                    break;
            }
        }

        static private int CardValue(string currentCard)
        {
            int cardValue;

            switch (currentCard)
            {
                case "K":
                case "Q":
                case "J":
                    cardValue = 10;
                    break;
                case "A":
                    cardValue = 11;
                    break;
                default:
                    cardValue = Convert.ToInt16(currentCard);
                    break;
            }

            return cardValue;
        }

        static private void SplitCards(int handNumber)
        {
            if (playerMoney - userBet >= NO_MONEY)
            {
                playerCards[playerHandsCount, FIRST_CARD_INDEX] = playerCards[handNumber, SECOND_CARD_INDEX];

                playerCards[handNumber, SECOND_CARD_INDEX] = NO_HAND;

                playerCardCount[handNumber] = ONE_CARD;
                playerCardCount[playerHandsCount] = ONE_CARD;

                playerHandsCount++;

                playerMoney -= userBet;

                doubleDownPossible = false;
                insurancePossible = false;
                splitsPossible[handNumber] = false;
            }
            else
            {
                Display(NO_SPLIT_MONEY, handNumber);
            }
        }

        static private void DoubleDown(int handNum)
        {
            playerMoney -= userBet;

            playerCards[handNum, playerCardCount[handNum]] = shuffleCards[nextCardInDeck];

            playerCardCount[handNum]++;

            nextCardInDeck++;

            doubleDownPossible = false;
            insurancePossible = false;

            playerHandsValues = PlayerHandsValues(playerCards);

            CompareHands(handNum, DOUBLE_DOWN);
        }
        static private void Insurance()
        {
            playerMoney -= (int)(userBet / 2);

            if (dealerBlackJack == true)
            {
                playerMoney += userBet;

                Console.WriteLine("The dealer had a blackjack and so you won back your insurance money! Press ENTER to continue.");

                Console.ReadLine();

                Display(DEALER_BLACKJACK, NO_HAND_NUMBER);

                Console.ReadLine();

                dealerBlackJack = false;

                handOver = true;
                newRound = true;
            }
            else
            {
                Console.WriteLine("The dealer did not have a blackjack and so you lost your insurance money! Press ENTER to continue.");

                Console.ReadLine();
            }

            insurancePossible = false;
        }

        static private void DealingCards()
        {
            playerCards[FIRST_HAND_INDEX, FIRST_CARD_INDEX] = shuffleCards[FIRST_CARD_INDEX];
            playerCards[FIRST_HAND_INDEX, SECOND_CARD_INDEX] = shuffleCards[THIRD_HAND_INDEX];

            dealerCards[FIRST_CARD_INDEX] = shuffleCards[SECOND_CARD_INDEX];
            dealerCards[SECOND_CARD_INDEX] = shuffleCards[FOURTH_CARD_INDEX];

            nextCardInDeck = 4;

            playerCardCount[FIRST_HAND_INDEX] = 2;
            dealerCardsCount = 2;
        }

        static private int DealerHandValue(string[] cardsToCheck)
        {
            int aceCount = 0;
            int handValue = 0;

            for (int i = FIRST_CARD_INDEX; i < dealerCardsCount; i++)
            {
                if (CardValue(cardsToCheck[i]) == HIGH_ACE_VALUE)
                {
                    aceCount++;
                }
                else
                {
                    handValue += CardValue(cardsToCheck[i]);
                }
            }

            if (aceCount > 0)
            {
                if (aceCount > 1)
                {
                    handValue += aceCount - 1;
                }

                if (handValue + HIGH_ACE_VALUE > BLACKJACK)
                {
                    handValue += LOW_ACE_VALUE;
                }
                else
                {
                    handValue += HIGH_ACE_VALUE;
                }
            }

            return handValue;
        }

        static private void CompareHands(int handNum, int state)
        {
            while (dealerHandValue < 17)
            {
                dealerCards[dealerCardsCount] = shuffleCards[nextCardInDeck];

                dealerCardsCount++;
                nextCardInDeck++;

                dealerHandValue = DealerHandValue(dealerCards);
            }

            if (state == REGULAR)
            {
                if (dealerHandValue <= BLACKJACK)
                {
                    if (playerHandsValues[handNum] == dealerHandValue)
                    {
                        playerMoney += userBet;

                        Display(HAND_TIE, handNum);
                    }
                    else if (playerHandsValues[handNum] > dealerHandValue)
                    {
                        playerMoney += userBet + userBet;

                        Display(PLAYER_HIGHER_HAND, handNum);
                    }
                    else if (playerHandsValues[handNum] < dealerHandValue)
                    {
                        Display(PLAYER_LOWER_HAND, handNum);
                    }
                }
                else
                {
                    playerMoney += userBet + userBet;

                    Display(DEALER_OVER, handNum);
                }
            }
            else if (state == DOUBLE_DOWN)
            {
                handsDoneWith++;

                if (playerHandsValues[handNum] > BLACKJACK)
                {
                    Display(OVER_21, DOUBLE_DOWN_DISPLAY);

                    Console.ReadLine();
                }
                else
                {
                    if (dealerHandValue <= BLACKJACK)
                    {
                        if (playerHandsValues[handNum] == dealerHandValue)
                        {
                            playerMoney += userBet + userBet;

                            Display(HAND_TIE, DOUBLE_DOWN_DISPLAY);
                        }
                        else if (playerHandsValues[handNum] > dealerHandValue)
                        {
                            playerMoney += userBet + userBet + userBet + userBet;

                            Display(PLAYER_HIGHER_HAND, DOUBLE_DOWN_DISPLAY);
                        }
                        else if (playerHandsValues[handNum] < dealerHandValue)
                        {
                            Display(PLAYER_LOWER_HAND, DOUBLE_DOWN_DISPLAY);
                        }
                    }
                    else
                    {
                        playerMoney += userBet + userBet + userBet + userBet;

                        Display(DEALER_OVER, DOUBLE_DOWN_DISPLAY);
                    }
                }
            }
        }

        static private int[] PlayerHandsValues(string[,] cardsToCheck)
        {
            int aceCount = 0;

            int[] handValue = new int[4] { 0, 0, 0, 0 };

            for (int i = FIRST_HAND_INDEX; i < playerHandsCount; i++)
            {
                aceCount = 0;

                for (int j = FIRST_CARD_INDEX; j < playerCardCount[i]; j++)
                {
                    if (CardValue(cardsToCheck[i, j]) == HIGH_ACE_VALUE)
                    {
                        aceCount++;
                    }
                    else
                    {
                        handValue[i] += CardValue(cardsToCheck[i, j]);
                    }
                }

                if (aceCount > 0)
                {
                    if (aceCount > 1)
                    {
                        handValue[i] += aceCount - 1;
                    }

                    if (handValue[i] + HIGH_ACE_VALUE > BLACKJACK)
                    {
                        handValue[i] += LOW_ACE_VALUE;
                    }
                    else
                    {
                        handValue[i] += HIGH_ACE_VALUE;
                    }
                }
            }

            return handValue;
        }

        static private void HitCard(int playerHandNumber)
        {
            playerCards[playerHandNumber, playerCardCount[playerHandNumber]] = shuffleCards[nextCardInDeck];

            playerCardCount[playerHandNumber]++;

            nextCardInDeck++;

            doubleDownPossible = false;
            insurancePossible = false;

            if (playerCardCount[playerHandNumber] == TWO_CARDS)
            {
                if (playerCards[playerHandNumber, FIRST_CARD_INDEX] == playerCards[playerHandNumber, SECOND_CARD_INDEX] &&
                    playerMoney - userBet >= NO_MONEY)
                {
                    splitsPossible[playerHandNumber] = true;
                }
                else
                {
                    splitsPossible[playerHandNumber] = false;
                }
            }
            else
            {
                splitsPossible[playerHandNumber] = false;
            }
        }

        static private void UserChoice(int handNum)
        {
            while (handOver == false)
            {
                if (handsDoneWith == playerHandsCount)
                {
                    for (int i = FIRST_HAND_INDEX; i < playerHandsCount; i++)
                    {
                        if (handsStanding[i] == true)
                        {
                            CompareHands(i, REGULAR);

                            handsStanding[i] = false;
                        }
                    }

                    handOver = true;
                    newRound = true;
                }
                else if (handsDoneWith > handNum)
                {
                    handOver = true;
                }

                playerHandsValues = PlayerHandsValues(playerCards);
                dealerHandValue = DealerHandValue(dealerCards);

                if (newRound == false && handsDoneWith <= handNum)
                {
                    if (dealerBlackJack == true && playerBlackJack == true)
                    {
                        Display(BLACKJACK_TIE, NO_HAND_NUMBER);

                        Console.ReadLine();

                        playerMoney += userBet;

                        dealerBlackJack = false;
                        playerBlackJack = false;

                        handOver = true;
                        newRound = true;
                    }
                    else if (dealerBlackJack == true && playerBlackJack == false || insurancePossible == true)
                    {
                        if (insurancePossible == true && playerMoney - (int)(userBet / 2) >= 0)
                        {
                            Display(INSURANCE, handNum);

                            userResponse = Console.ReadLine();

                            if (userResponse.ToLower() == "yes")
                            {
                                Insurance();
                            }
                            else
                            {
                                Display(DEALER_BLACKJACK, NO_HAND_NUMBER);

                                Console.ReadLine();

                                dealerBlackJack = false;

                                handOver = true;
                                newRound = true;
                            }
                        }
                        else
                        {
                            Display(DEALER_BLACKJACK, NO_HAND_NUMBER);

                            Console.ReadLine();

                            dealerBlackJack = false;

                            handOver = true;
                            newRound = true;
                        }
                    }
                    else if (dealerBlackJack == false && playerBlackJack == true)
                    {
                        Display(PLAYER_BLACKJACK, NO_HAND_NUMBER);

                        Console.ReadLine();

                        playerMoney += Convert.ToInt32(userBet * 2.5);

                        playerBlackJack = false;

                        handOver = true;
                        newRound = true;
                    }
                    else if (playerHandsValues[handNum] > BLACKJACK)
                    {
                        Display(OVER_21, handNum);

                        Console.ReadLine();

                        handsDoneWith++;
                    }
                    else
                    {
                        Display(USER_HAND_CHOICE, handNum);

                        userResponse = Console.ReadLine();

                        userResponse = userResponse.ToLower();

                        switch (userResponse)
                        {
                            case "quit":
                            case "1":
                                Environment.Exit(0);
                                break;
                            case "stand":
                            case "2":
                                handsDoneWith++;
                                handsStanding[handNum] = true;
                                break;
                            case "hit":
                            case "3":
                                HitCard(handNum);
                                break;
                            case "4":
                                if (possibleMovesCount >= FOUR_MOVES)
                                {
                                    switch (possibleMoves[FOURTH_MOVE_INDEX])
                                    {
                                        case "Double Down":
                                            handOver = true;

                                            DoubleDown(handNum);
                                            break;
                                        case "Split Cards":
                                            SplitCards(handNum);
                                            break;
                                        case "Insurance":
                                            Insurance();
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option, please press enter to try again.");
                                    Console.ReadLine();
                                }
                                break;
                            case "5":
                                if (possibleMovesCount >= FIVE_MOVES)
                                {
                                    switch (possibleMoves[FIFTH_MOVE_INDEX])
                                    {
                                        case "Split Cards":
                                            SplitCards(handNum);
                                            break;
                                        case "Insurance":
                                            Insurance();
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option, please press enter to try again.");
                                    Console.ReadLine();
                                }
                                break;
                            case "6":
                                if (possibleMovesCount >= SIX_MOVES)
                                {
                                    if (possibleMoves[SIXTH_MOVE_INDEX] == "Insurance")
                                    {
                                        Insurance();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option, please press enter to try again.");
                                    Console.ReadLine();
                                }
                                break;
                            case "double down":
                                if (possibleMoves[FOUR_MOVES] == "Double Down")
                                {
                                    DoubleDown(handNum);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option, please press enter to try again.");
                                    Console.ReadLine();
                                }
                                break;
                            case "split cards":
                                if (possibleMoves[4] == "Split Cards" || possibleMoves[5] == "Split Cards")
                                {
                                    SplitCards(handNum);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option, please press enter to try again.");
                                    Console.ReadLine();
                                }
                                break;
                            case "insurance":
                                if (possibleMoves[4] == "Insurance" || possibleMoves[5] == "Insurance" || possibleMoves[6] == "Insurance")
                                {
                                    Insurance();
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option, please press enter to try again.");
                                    Console.ReadLine();
                                }
                                break;
                            default:
                                Console.WriteLine("Invalid option, please press enter to try again.");
                                Console.ReadLine();
                                break;
                        }
                    }
                }
            }
        }
    }
}