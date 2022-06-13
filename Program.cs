using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an ID of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room ID: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an ID of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an ID of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore ID: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an ID of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetAll();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Name} has an ID of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Assign a chore"):
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach (Chore c in allChores)
                        {
                            Console.WriteLine($"{c.Name} has an ID of {c.Id}");
                        }
                        Console.Write("Enter the ID of the chore you want to assign ");
                        int chosenChoreId = int.Parse(Console.ReadLine());

                        List<Roommate> allRoommates = roommateRepo.GetAll();
                        foreach (Roommate r in allRoommates)
                        {
                            Console.WriteLine($"{r.FirstName} {r.LastName} has an ID of {r.Id}");
                        }
                        Console.Write($"Enter the ID of the roommate you want to assign to the chore ");
                        int chosenRoommateId = int.Parse(Console.ReadLine());
                        int roommateChoreAssignment = choreRepo.AssignChore(chosenRoommateId, chosenChoreId);

                        Console.WriteLine($"The roommate with the ID of {chosenRoommateId} has been assigned the chore with the ID of {chosenChoreId}.");
                        Console.ReadKey();
                        break;

                    //case ("Show all roommates"):
                    //    List<Roommate> roommates = roommateRepo.GetAll();
                    //    foreach (Roommate r in roommates)
                    //    {
                    //        Console.WriteLine($"{r.FirstName} {r.LastName} moved in on {r.MoveInDate}, has an Id of {r.Id}, and rent portion of {r.RentPortion}");
                    //    }
                    //    Console.Write("Press any key to continue");
                    //    Console.ReadKey();
                    //    break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int rmId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(rmId);

                        Console.WriteLine($"{roommate.FirstName} {roommate.LastName} lives in {roommate.Room.Name} - Rent Portion: {roommate.RentPortion}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    //case ("Add a roommate"):
                    //    Console.Write("Roommate First Name: ");
                    //    string firstName = Console.ReadLine();

                    //    Console.Write("Roommate Last Name: ");
                    //    string lastName = Console.ReadLine();

                    //    Console.Write("Rent Portion: ");
                    //    int rent = int.Parse(Console.ReadLine());

                    //    DateTime moveIn = DateTime.Now;

                    //    Roommate roommateToAdd = new Roommate()
                    //    {
                    //        FirstName = firstName,
                    //        LastName = lastName,
                    //        MoveInDate = moveIn,
                    //        RentPortion = rent
                    //    };

                        //roommateRepo.Insert(roommateToAdd);

                        //Console.WriteLine($"{roommateToAdd.FirstName} {roommateToAdd.LastName} has been added and assigned an Id of {roommateToAdd.Id}");
                        //Console.Write("Press any key to continue");
                        //Console.ReadKey();
                        //break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Show unassigned chores",
                "Assign a chore",
                //"Show all roommates",
                "Search for roommate",
                //"Add a roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}