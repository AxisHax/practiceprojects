using System.Xml.Linq;

namespace PhoneBook
{
    /// <summary>
    /// A node that represents a phonebook entry in a linked list.
    /// </summary>
    public sealed class EntryNode
    {
        /// <summary>
        /// The phonebook data.
        /// </summary>
        public PhoneEntry? Data { get; set; }

        /// <summary>
        /// Reference to the previous node in the list.
        /// </summary>
        public EntryNode? Previous { get; set; }

        /// <summary>
        /// Reference to the next node in the list.
        /// </summary>
        public EntryNode? Next { get; set; }

        /// <summary>
        /// Create's an instance of the <see cref="EntryNode"/> class. Initializes data to null.
        /// </summary>
        public EntryNode()
        {
            this.Data = null;
            this.Previous = null;
            this.Next = null;
        }

        /// <summary>
        /// Create's an instance of the <see cref="EntryNode"/> class using a phonebook entry.
        /// </summary>
        /// <param name="entry"></param>
        public EntryNode(PhoneEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);

            this.Data = entry;
            this.Previous = null;
            this.Next = null;
        }

        public override string ToString()
        {
            if (this.Data is null)
            {
                return string.Empty;
            }

            return $"{this.Data.FirstName}, {this.Data.LastName}, {this.Data.PhoneNumber}";
        }
    }

    /// <summary>
    /// A linked list that stores phone entries.
    /// </summary>
    public sealed class EntryList
    {
        /// <summary>
        /// The amount of items in the list.
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// The current node in the list.
        /// </summary>
        private EntryNode? _currentNode;

        /// <summary>
        /// The first node in the list.
        /// </summary>
        private EntryNode? _head;

        /// <summary>
        /// The last node in the list.
        /// </summary>
        private EntryNode? _tail;

        /// <summary>
        /// The amount of items contained in this list.
        /// </summary>
        public int Count { get => this._count; }

        /// <summary>
        /// Create's a new instance of the <see cref="EntryList"/> class. Initializes data to null and count to 0.
        /// </summary>
        public EntryList()
        {
            this._currentNode = null;
            this._head = null;
            this._tail = null;
            this._count = 0;
        }

        /// <summary>
        /// Create's a new instance of the <see cref="EntryList"/> class. Initializes data using the provided <see cref="PhoneEntry"/>.
        /// </summary>
        /// <param name="entry">A phone book entry.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided entry is null.</exception>
        public EntryList(PhoneEntry entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException($"{nameof(entry)} is null.");
            }

            InitializeNewList(entry);
        }

        public EntryList(IEnumerable<PhoneEntry> entries)
        {
            ArgumentNullException.ThrowIfNull(entries, nameof(entries));

            foreach(PhoneEntry entry in entries)
            {
                this.Append(entry);
            }
        }

        /// <summary>
        /// Append a <see cref="PhoneEntry"/> to the end of the list.
        /// </summary>
        /// <param name="entry">A phone book entry.</param>
        public void Append(PhoneEntry entry)
        {
            if (IsEmpty())
            {
                InitializeNewList(entry);
                return;
            }
            else if (ContainsOnlyOneElement())
            {
                // We only have a single node in the list. Append it after the first node.
                EntryNode newEntryNode = new(entry);
                this._currentNode = this._tail;
                this._currentNode?.Next = newEntryNode;

                newEntryNode.Previous = this._currentNode;
                newEntryNode.Next = null;

                // Update the end node.
                this._tail = newEntryNode;
            }
            else
            {
                // We have multiple items in this list. Traverse to the end and then append it.
                TraverseToEnd();

                EntryNode newEntryNode = new(entry);

                if (this._currentNode is not null)
                {
                    this._currentNode.Next = newEntryNode;
                }

                newEntryNode.Previous = this._currentNode;
                newEntryNode.Next = null;

                this._tail = newEntryNode;
            }

            this._count++;
        }

        /// <summary>
        /// Append multiple entries to the end of this list.
        /// </summary>
        /// <param name="entries">A list of phone entries to be added to the list.</param>
        public void Append(IEnumerable<PhoneEntry> entries)
        {
            ArgumentNullException.ThrowIfNull(entries, nameof(entries));

            foreach(PhoneEntry entry in entries)
            {
                this.Append(entry);
            }
        }

        /// <summary>
        /// Search for an element by name and remove it.
        /// </summary>
        /// <param name="name">The name of the person you want to remove from the entry list.</param>
        public void Remove(string name)
        {
            // Remove an element from the list using the name.
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            if (IsEmpty())
            {
                return;
            }

            if (ContainsOnlyOneElement())
            {
                // There's only a single node in the list, so remove it.
                this._head?.Previous = null;
                this._head?.Next = null;
                this._head = null;

                this._tail?.Previous = null;
                this._tail?.Next = null;
                this._tail = null;

            }
            else
            {
                // There are multiple elements in this list. Search for the one to remove by name.
                this._currentNode = this._head;

                while(this._currentNode is not null)
                {
                    // Check if name matches.
                    if (string.Equals(this._currentNode?.Data?.FirstName, name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Remove the node from the list.
                        this._currentNode?.Previous?.Next = this._currentNode.Next;
                        this._currentNode?.Next?.Previous = this._currentNode.Previous;

                        this._currentNode?.Data = null;
                        this._currentNode?.Previous = null;
                        this._currentNode?.Next = null;
                        this._currentNode = null;

                        break;
                    }

                    this._currentNode = this._currentNode?.Next;
                }
            }

            this._count--;
        }

        /// <summary>
        /// Remove an <see cref="EntryNode"/> from the end of the list.
        /// </summary>
        public void RemoveFromEnd()
        {
            if (IsEmpty())
            {
                return;
            }

            if (ContainsOnlyOneElement())
            {
                // There's only a single element in the list.
                this._head?.Data = null;
                this._head?.Previous = null;
                this._head?.Next = null;
                this._head = null;

                this._tail?.Data = null;
                this._tail?.Previous = null;
                this._tail?.Next = null;
                this._tail = null;
            }
            else
            {
                // There's multiple elements in the list. Go to the end and delete the final node.
                TraverseToEnd();

                if (this._currentNode is not null)
                {
                    this._tail = this._currentNode.Previous;

                    this._currentNode?.Data = null;
                    this._currentNode?.Previous = null;
                    this._currentNode?.Next = null;
                    this._currentNode = null;
                }
            }

            this._count--;
        }

        /// <summary>
        /// Remove all entries from the list.
        /// </summary>
        public void RemoveAll()
        {
            if (IsEmpty())
            {
                return;
            }

            // Traverse the list and remove each element.
            if (ContainsOnlyOneElement())
            {
                // There's only a single node in the list, so remove it.
                this._head?.Data = null;
                this._head?.Previous = null;
                this._head?.Next = null;
                this._head = null;

                this._tail?.Data = null;
                this._tail?.Previous = null;
                this._tail?.Next = null;
                this._tail = null;

            }
            else
            {
                // There's multiple elements in the list, remove all of them one at a time.
                this._currentNode = this._head;

                while (this._currentNode is not null)
                {
                    EntryNode? nextNode = this._currentNode.Next;

                    // Remove the current node from the list.
                    this._currentNode.Previous?.Next = this._currentNode.Next;
                    this._currentNode.Next?.Previous = this._currentNode.Previous;

                    this._currentNode?.Data = null;
                    this._currentNode?.Previous = null;
                    this._currentNode?.Next = null;
                    this._currentNode = null;

                    this._currentNode = nextNode;
                    this._count--;
                }

                // Remove head and tail elements.
                this._head?.Data = null;
                this._head?.Previous = null;
                this._head?.Next = null;
                this._head = null;

                this._tail?.Data = null;
                this._tail?.Previous = null;
                this._tail?.Next = null;
                this._tail = null;
            }
        }

        /// <summary>
        /// Search for a phone entry by first name.
        /// </summary>
        /// <param name="firstName">Name being searched for in the list.</param>
        /// <returns>A <see cref="PhoneEntry"/> object.</returns>
        public PhoneEntry? Search(string firstName)
        {
            ArgumentException.ThrowIfNullOrEmpty(firstName, nameof(firstName));

            if (IsEmpty())
            {
                return null;
            }

            this._currentNode = this._head;

            while (_currentNode is not null)
            {
                if (string.Equals(this._currentNode?.Data?.FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    return this._currentNode?.Data;
                }

                this._currentNode = this._currentNode?.Next;
            }

            return null;
        }

        /// <summary>
        /// Sort the list by first name.
        /// </summary>
        public void Sort()
        {
            // Sort by first and last name.
        }

        /// <summary>
        /// Print all entries in the list.
        /// </summary>
        public void PrintEntries()
        {
            this._currentNode = this._head;

            while (this._currentNode is not null)
            {
                Console.WriteLine(this._currentNode.ToString());
                this._currentNode = this._currentNode.Next;
            }
        }

        /// <summary>
        /// Check if the list is currently empty.
        /// </summary>
        /// <returns><c>true</c> if the list is empty, <c>false</c> otherwise.</returns>
        private bool IsEmpty()
        {
            return (this._head is null || this._tail is null);
        }

        /// <summary>
        /// Check if the list contains just a single element.
        /// </summary>
        /// <returns><c>true</c> if the list has only a single element, <c>false</c> otherwise.</returns>
        private bool ContainsOnlyOneElement()
        {
            return this._head == this._tail && (this._head is not null && this._tail is not null);
        }

        /// <summary>
        /// Initialize the list with a single phone book entry.
        /// </summary>
        /// <param name="entry">A phone book entry.</param>
        private void InitializeNewList(PhoneEntry entry)
        {
            EntryNode newEntry = new(entry);
            this._currentNode = newEntry;
            this._head = newEntry;
            this._tail = newEntry;
            this._count++;
        }

        /// <summary>
        /// Traverse to the end of the list, starting at the beginning.
        /// </summary>
        private void TraverseToEnd()
        {
            this._currentNode = this._head;

            while ((this._currentNode is not null) && (this._currentNode != this._tail))
            {
                this._currentNode = this._currentNode.Next;
            }
        }
    }
}
