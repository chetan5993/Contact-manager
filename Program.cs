using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContactManager
{
    class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({EmailAddress}, {PhoneNumber})";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Contact> contacts = new List<Contact>();

            while (true)
            {
                Console.WriteLine("Welcome to the Contact Manager!");
                Console.WriteLine("1. Display all contacts");
                Console.WriteLine("2. Add a new contact");
                Console.WriteLine("3. Remove an existing contact");
                Console.WriteLine("4. Update an existing contact");
                Console.WriteLine("5. Search for a contact by name");
                Console.WriteLine("6. Save contacts to file");
                Console.WriteLine("7. Load contacts from file");
                Console.WriteLine("8. Quit");

                Console.Write("Enter your choice (1-8): ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayContacts(contacts);
                        break;
                    case "2":
                        AddContact(contacts);
                        break;
                    case "3":
                        RemoveContact(contacts);
                        break;
                    case "4":
                        UpdateContact(contacts);
                        break;
                    case "5":
                        SearchContacts(contacts);
                        break;
                    case "6":
                        SaveContacts(contacts);
                        break;
                    case "7":
                        contacts = LoadContacts();
                        break;
                    case "8":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 8.");
                        break;
                }
            }
        }

        static void DisplayContacts(List<Contact> contacts)
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("There are no contacts to display.");
            }
            else
            {
                foreach (Contact contact in contacts)
                {
                    Console.WriteLine(contact);
                }
            }
        }

        static void AddContact(List<Contact> contacts)
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter email address: ");
            string emailAddress = Console.ReadLine();

            while (!IsValidEmail(emailAddress))
            {
                Console.WriteLine("Invalid email address. Please enter a valid email address.");
                Console.Write("Enter email address: ");
                emailAddress = Console.ReadLine();
            }

            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();

            while (!IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Invalid phone number. Please enter a valid phone number.");
                Console.Write("Enter phone number: ");
                phoneNumber = Console.ReadLine();
            }

            contacts.Add(new Contact { FirstName = firstName, LastName = lastName, EmailAddress = emailAddress, PhoneNumber = phoneNumber });
            Console.WriteLine("Contact added successfully.");
        }

        static void RemoveContact(List<Contact> contacts)
        {
            Console.Write("Enter the index of the contact to remove: ");
            int index = int.Parse(Console.ReadLine());

            if (index >= 0 && index < contacts.Count)
            {
                contacts.RemoveAt(index);
                Console.WriteLine("Contact removed successfully.");
            }
            else
            {
                Console.WriteLine("Invalid index. Please enter a valid index.");
            }
        }

        static void UpdateContact(List<Contact> contacts)
        {
            Console.Write("Enter the index of the contact to update: ");
            int index = int.Parse(Console.ReadLine());

            if (index >= 0 && index < contacts.Count)
            {
                Contact contact = contacts[index];

                Console.Write($"Enter new first name ({contact.FirstName}): ");
                string firstName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    contact.FirstName = firstName;
                }

                Console.Write($"Enter new last name ({contact.LastName}): ");
                string lastName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    contact.LastName = lastName;
                }

                Console.Write($"Enter new email address ({contact.EmailAddress}): ");
                string emailAddress = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(emailAddress) && IsValidEmail(emailAddress))
                {
                    contact.EmailAddress = emailAddress;
                }
                else if (!IsValidEmail(emailAddress))
                {
                    Console.WriteLine("Invalid email address. Email address not updated.");
                }

                Console.Write($"Enter new phone number ({contact.PhoneNumber}): ");
                string phoneNumber = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(phoneNumber) && IsValidPhoneNumber(phoneNumber))
                {
                    contact.PhoneNumber = phoneNumber;
                }
                else if (!IsValidPhoneNumber(phoneNumber))
                {
                    Console.WriteLine("Invalid phone number. Phone number not updated.");
                }

                Console.WriteLine("Contact updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid index. Please enter a valid index.");
            }
        }

        static void SearchContacts(List<Contact> contacts)
        {
            Console.Write("Enter a name to search for: ");
            string name = Console.ReadLine();

            List<Contact> matchingContacts = contacts.Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name)).ToList();

            if (matchingContacts.Count == 0)
            {
                Console.WriteLine($"No contacts found with name '{name}'.");
            }
            else
            {
                Console.WriteLine($"Contacts found with name '{name}':");
                foreach (Contact contact in matchingContacts)
                {
                    Console.WriteLine(contact);
                }
            }
        }

        static void SaveContacts(List<Contact> contacts)
        {
            using (StreamWriter writer = new StreamWriter("contacts.txt"))
            {
                foreach (Contact contact in contacts)
                {
                    writer.WriteLine($"{contact.FirstName},{contact.LastName},{contact.EmailAddress},{contact.PhoneNumber}");
                }
            }

            Console.WriteLine("Contacts saved to file.");
        }

        static List<Contact> LoadContacts()
        {
            List<Contact> contacts = new List<Contact>();

            if (File.Exists("contacts.txt"))
            {
                using (StreamReader reader = new StreamReader("contacts.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        contacts.Add(new Contact { FirstName = parts[0], LastName = parts[1], EmailAddress = parts[2], PhoneNumber = parts[3] });
                    }
                }

                Console.WriteLine("Contacts loaded from file.");
            }
            else
            {
                Console.WriteLine("No contacts file found.");
            }

            return contacts;
        }

        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        static bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit);
        }
    }
}

