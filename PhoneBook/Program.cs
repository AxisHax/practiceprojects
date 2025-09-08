namespace PhoneBook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Make a phone book where you can add entries and search for people using their name or phone number.
            PhoneEntry p1 = new("Sonic", "Hedgehog", "006-035-1991");
            PhoneEntry p2 = new("Luigi", "Player 2", "001-060-1989");
            PhoneEntry p3 = new("Mairo", "Player 1", "001-060-1989");

            EntryList entryList = new EntryList();

            entryList.Append(p1);
            entryList.Append(p2);
            entryList.Append(p3);

            entryList.PrintEntries();

            PhoneEntry? value = entryList.Search("Sonic");
            entryList.Remove("Luigi");
            entryList.PrintEntries();

            entryList.RemoveAll();
        }
    }
}
