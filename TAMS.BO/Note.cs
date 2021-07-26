using System;
namespace TAMS.BO
{
    public class Note
    {
        public Note()
        {
           

        }
       
        public string value { get; set; }

        public string name { get; set; }

        public override string ToString()
        {
            return name + " : " + value;
        }
    }
}
