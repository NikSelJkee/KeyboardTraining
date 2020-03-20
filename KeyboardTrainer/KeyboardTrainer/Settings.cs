using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardTrainer
{
    public class Settings
    {
        public string UserName { get; set; }
        public string Difficult { get; set; }
        public int Time { get; set; }
        public int Mistakes { get; set; }
        public int KeyboardSpeed { get; set; }

        public List<string> testcases;
        public Settings() => testcases = new List<string>();
    }
}
