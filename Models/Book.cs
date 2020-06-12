using System.Collections.Generic;

namespace Library.Models
{
    public class Book
    {

        public int BookId {get; set;}
        public string Title {get; set;}
        public ICollection<AuthorBook> Authors{get;set;}
        public ICollection<Copy> Copies{get;set;}

        
        public Book(){
            this.Authors = new HashSet<AuthorBook>();
        }
    }
}