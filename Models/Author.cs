using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Author
    {

        public int AuthorId {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public virtual ICollection<AuthorBook> Books{get; set;}
        public virtual ApplicationUser User { get; set; }
        [NotMapped]
        public string Name =>$"{FirstName} {LastName}";
        
        public Author()
        {
            this.Books = new HashSet<AuthorBook>();
        }
    }
}