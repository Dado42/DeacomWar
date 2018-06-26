using System;
using System.Collections.Generic;

/*Deacom_War is currently a text based version of the card game 'War'. It is played entirely in the console.
  Once running, the user can enter the 'help' command for game rules, as well as a list of all other commands.*/
namespace Deacom_War
{
    class Program
    {
        /*The main method of the program. Waits for the user to input a command and either prints an appropriate message
          or runs the correspoding method from the War class. */
        static void Main(string[] args)
        {
            War game = new War();
            Console.WriteLine("Welcome to War! Type 'help' for rules and commands");
            while(true){
                //Parse user input
                string input = Console.ReadLine();
                if (input == "deal"){
                    game.deal();
                }
                else if(input == "quit"){
                    Environment.Exit(1);
                }
                else if(input == "help"){
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"+
                    "\nEach player turns up a card at the same time and the player with the higher card takes both cards and puts them, face down, on the bottom of his stack."
                    +"\nIf the cards are the same rank, it is War. Each player turns up one card face down and one card face up."
                    +"\nThe player with the higher cards takes both piles (six cards)."
                    +"\nIf the turned-up cards are again the same rank, each player places another card face down and turns another card face up."
                    +"\nThe player with the higher card takes all 10 cards, and so on."
                    +"\nThe game ends when one player runs out of cards to play."
                    +"\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine("COMMANDS:\n- 'deal': Both players deal the top card from their hand"
                    +"\n- 'p1': Shows the number of cards in Player 1's hand"
                    +"\n- 'p2': Shows the number of cards in Player 2's hand"
                    +"\n- 'quit': Exit the game");
                }
                else if(input == "p1"){
                    Console.WriteLine("Player 1 has " + game.player1.Count.ToString() + " cards remaining");
                }
                else if(input == "p2"){
                    Console.WriteLine("Player 2 has " + game.player2.Count.ToString() + " cards remaining");
                }
                else{
                    Console.WriteLine("Unkown command. Type 'help' for list of available commands.");
                }
            }
        }
    }

    /*The War class holds all relevant methods to run a card game of War. It also holds each 
      player's hand as a list of integers, the pile of cards played, and the initial deck of cards
      that are dealt to the players at the beginning of the game.
      Each integer 0..51 represents one card found in a standard card deck. */
    public class War{
        public List<int> player1 = new List<int>();
        public List<int> player2 = new List<int>();
        public List<int> pile = new List<int>();
        public List<int> deck = new List<int>();

        /*Class costructor. Shuffles and splits the deck evenly among the two players */
        public War(){
            //Add 52 cards to the deck
            for(int i = 0; i<52; i++){
                deck.Add(i);
            }

            //Randomly populate player1's and player2's hand
            Random rnd = new Random();
            int cardIndex;
            for(int i = 0; i < 26; i++){
                cardIndex = rnd.Next(deck.Count);
                player1.Add(deck[cardIndex]);
                deck.RemoveAt(cardIndex);

                cardIndex = rnd.Next(deck.Count);
                player2.Add(deck[cardIndex]);
                deck.RemoveAt(cardIndex);
            }
        }

        /*The core method of the game. It removes the top card from each player's hand
          and places it in the pile. It determines which card has a higher value. The player that put down
          the higher value card then takes the pile into their hand. If players play cards of equal value,
          then 'War' occurs. 'War' is covered in the playWar() method. */
        public void deal(){
            int player1Top = player1[player1.Count-1];
            int player2Top = player2[player2.Count-1];
            if(player1.Count > 0 && player2.Count > 0){
                Console.WriteLine("Player 1: " + getName(player1Top) + " VS Player 2: " + getName(player2Top));
                if ((player1Top % 13) > (player2Top % 13)){
                    Console.WriteLine("Player 1 takes the pile!");
                    pile.Add(player1Top);
                    pile.Add(player2Top);
                    player1.Remove(player1Top);
                    player2.Remove(player2Top);
                    
                    player1.InsertRange(0, pile);
                    pile.Clear();

                }
                else if ((player1Top % 13) == (player2Top % 13)){
                    pile.Add(player1Top);
                    pile.Add(player2Top);
                    player1.Remove(player1Top);
                    player2.Remove(player2Top);

                    Console.WriteLine("%%%%% WAR! %%%%%");
                    playWar();
                }
                else{
                    Console.WriteLine("Player 2 takes the pile!");
                    pile.Add(player1Top);
                    pile.Add(player2Top);
                    player1.Remove(player1Top);
                    player2.Remove(player2Top);
                    
                    player2.InsertRange(0, pile);
                    pile.Clear();
                }
            }
            else{
                if (player1.Count == 0){
                    Console.WriteLine("Player 2 Wins!");
                    Environment.Exit(1);
                }
                else{
                    Console.WriteLine("Player 1 Wins!");
                    Environment.Exit(1);
                }
            }
        }

        /*getName takes in an integer 0..51 and returns a string describing the name
          and suit of the corresponding card. */
        public string getName(int card){
            string suit;
            string face;
            if (card >=0 && card <= 12){
                suit = "Hearts";
            }
            else if(card >=13 && card <=25){
                suit = "Clubs";
            }
            else if(card >=26 && card <=38){
                suit = "Spades";
            }
            else{
                suit = "Diamonds";
            }

            int value = card % 13;
            if(value > 8){
                if(value == 9){
                    face = "Jack";
                }
                else if(value == 10){
                    face = "Queen";
                }
                else if(value == 11){
                    face = "King";
                }
                else{
                    face = "Ace";
                }
            }
            else{
                    face = (value + 2).ToString();
            }

            return "[" + face + " of " + suit + "]";

        }

        /*playWar() method is called when both players play equalvalue cards. Each player places a card
          face down in the pile and then deal once again. */
        public void playWar(){

            if (player1.Count > 1 && player2.Count > 1){
                //Put card facedown to pile
                pile.Add(player1[player1.Count-1]);
                pile.Add(player2[player2.Count-1]);
                player1.RemoveAt(player1.Count-1);
                player2.RemoveAt(player2.Count-1);

                deal();
            }
            else if(player1.Count < 2){
                Console.WriteLine("Player 2 Wins!");
                Environment.Exit(1);
            }
            else{
                Console.WriteLine("Player 1 Wins!");
                Environment.Exit(1);
            }
        }
    }
}
