namespace PhoneBook
{
    /// <summary>
    /// This class describes the data present in a phone entry.
    /// </summary>
    public sealed class PhoneEntry
    {
        /// <summary>
        /// First name of the added person.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the added person.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Phone number of the added person..
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Add a new phone book entry.
        /// </summary>
        /// <param name="fName">First name of the person being added.</param>
        /// <param name="lName">Last name of the person being added.</param>
        /// <param name="pNumber">Phone number of the person being added.</param>
        public PhoneEntry(string fName, string lName, string pNumber)
        {
            // Throw if any of these are invalid entries.
            ArgumentException.ThrowIfNullOrEmpty(fName, nameof(fName));
            ArgumentException.ThrowIfNullOrEmpty(lName, nameof(lName));
            ArgumentException.ThrowIfNullOrEmpty(pNumber, nameof(pNumber));

            this.FirstName = fName;
            this.LastName = lName;
            
            // Setup phone number verification later.
            this.PhoneNumber = pNumber;
        }
    }
}
