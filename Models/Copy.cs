using System.Collections.Generic;

namespace Library.Models
{
    public class Copy
    {
        public int CopyId {get; set;}
        public int BookId {get; set;}
        public bool CheckedOut {get; set; } = false;
        public virtual ApplicationUser User { get; set; }
        public virtual Book Book {get;set;}
    }
}