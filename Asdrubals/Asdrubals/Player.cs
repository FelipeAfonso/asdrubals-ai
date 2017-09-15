using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asdrubals {
    class Player {

        public string Name { get; set; }

        public List<int> movement = new List<int>();

        public TcpClient Client { get; set; }

        public Asdrubal asdrubal { get; set; }

    }
}
