namespace NexPay.Login.Api.Models
{
    public class User
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        public string? LastName { get; set; }   

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string? Password {get;set;}

        /// <summary>
        /// Gets or sets IsAdmin.
        /// </summary>
        public bool? IsAdmin { get; set; }
    }
}
