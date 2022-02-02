using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Sweets_Project_Server_Side
{
    public partial class Form1 : Form
    {
        class Sweets
        {//class to hold sweet information which includes a unique id, the time that sweet is posted,
        	//the user who posted the sweet, and the sweet itself
            public int sweetId;
            public string now;
            public string sweetBody;
            public string username;
            public Sweets()
            {
                this.sweetId = -1;
                this.now = "";
                this.sweetBody = "";
                this.username = "";
            }
            public Sweets(int id, string time, string body, string name)
            {
                this.sweetId = id;
                this.now = time;
                this.sweetBody = body;
                this.username = name;
            }
       
        }
        struct clientSocket 
        {//Struct to identify the username corresponding to the client socket 
            public Socket socket;
            public string name;
            public clientSocket(Socket s, string n)
            {
                this.socket = s;
                this.name = n;
            }
            public void changeName(string s) 
            {
                this.name = s;
            }
        }

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<clientSocket> clientSockets = new List<clientSocket>();
        private static Mutex operationOfAClient = new Mutex(); //To securely delete sweets and block users Newly added

        bool listening = false; 
        bool isServerconnected = false; 
        bool terminating = false;

        HashSet<string> userNames = new HashSet<string>();
        HashSet<string> connectedUsers = new HashSet<string>();

        Dictionary<string,HashSet<string>> XfollowsYs = new Dictionary<string,HashSet<string>>();
        Dictionary<string,HashSet<string>> YfollowedByXs = new Dictionary<string,HashSet<string>>();
        Dictionary<string,HashSet<string>> XblockedByYs = new Dictionary<string,HashSet<string>>();

        List<Sweets> listOfSweets = new List<Sweets>();
        int lastSeenId = 0;
        

        private void txt_reading()
        {
            if(File.Exists(@"./user-db.txt")){
                foreach (string line in System.IO.File.ReadLines(@"./user-db.txt"))
                { //Retrieve and store all the usernames from the txt file
                    userNames.Add(line);
                    XfollowsYs.Add(line,new HashSet<string>()); //Create following set for each user
                    YfollowedByXs.Add(line,new HashSet<string>());//Create followed set for each user
                    XblockedByYs.Add(line,new HashSet<string>());//Create blocked set for each user

                }
            }
            else{
                richTextBox1.AppendText("Username txt file could not be found...\n");
            }
            if(File.Exists(@"./sweets.txt")){
                foreach (string line in System.IO.File.ReadLines(@"./sweets.txt")) 
                {
                    //Retrieve and store all the previous Sweets from the txt file
                    string[] newline = line.Split(' ');
                    int id = Int32.Parse(newline[0]);
                    string username = newline[1];
                    string now = newline[2] + " " + newline[3];
                    string body = "";
                    for(int i = 4; i< newline.Length; i++)
                    {
                        if(i == newline.Length-1)
                        {
                            body += newline[i];
                        }
                        else
                        {
                            body += newline[i] + " ";
                        }
                        
                    }

                    listOfSweets.Add(new Sweets(id, now, body, username));
                    lastSeenId=id;
                }
            }

            if(File.Exists(@"./following.txt")){ //Retrieve all following relationships among users
                foreach (string line in System.IO.File.ReadLines(@"./following.txt")) 
                {
                    string[] newline = line.Split(' ');
                    string usernameX = newline[0];
                    string usernameY = newline[1];
                    XfollowsYs[usernameX].Add(usernameY);
                    YfollowedByXs[usernameY].Add(usernameX);
                }
            }

            if(File.Exists(@"./blockedBy.txt")){ //Retrieve all block relationships among users
                foreach (string line in System.IO.File.ReadLines(@"./blockedBy.txt")) 
                {
                    string[] newline = line.Split(' ');
                    string usernameX = newline[0];
                    string usernameY = newline[1];
                    XblockedByYs[usernameX].Add(usernameY);
                    
                }
            }
          
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            richTextBox1.AppendText("Disconnecting...\n");
            isServerconnected = false;
            foreach (clientSocket client in clientSockets)
            {
                //Send disconnection message to all connected clients
                string message = "MCDZD!!DISCONNECT ";
                Byte[] buffer3 = Encoding.Default.GetBytes(message);
                connectedUsers.Remove(client.name);
                try
                {
                    client.socket.Send(buffer3);
                    
                }
                catch
                {
                    richTextBox1.AppendText("There is a problem! I couldn't send disconnection message to a cilent...\n");
                    
                }

            }
            clientSockets.Clear();
            serverSocket.Close();
            terminating = true;
            Environment.Exit(0);
        }

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            InitializeComponent();
            txt_reading();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
                int serverPort;
                
                if (Int32.TryParse(textBox1.Text, out serverPort))
                { //If valid port number, try to connect
                    
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                    serverSocket.Bind(endPoint);
                    serverSocket.Listen(3);

                    listening = true;
                    isServerconnected = true;
                    button1.Enabled= false;
                    
                    Thread acceptThread = new Thread(Accept);
                    acceptThread.Start();

                    richTextBox1.AppendText("Started listening on port: " + serverPort + "\n");

                }
                else
                {
                    richTextBox1.AppendText("Couldn't connect to the port: "+ serverPort + "\n");
                }
           

        }
        private void Accept()
        {
            while (listening)
            {
                try
                {
                    
                    Socket newClient = serverSocket.Accept();
                    string s = "";
                    clientSocket newClientSocket = new clientSocket(newClient, s);

                    if(validUsername(ref newClientSocket)){ //If the username is valid(i.e. existing and not already connected)
                    	//Start to receive information from the that user
                        clientSockets.Add(newClientSocket);

                        Thread receiveThread = new Thread(() => Receive(newClientSocket));
                        receiveThread.Start();

                    }

                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        richTextBox1.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }

        private bool validUsername(ref clientSocket thisClient) {
            try
            {
                Byte[] buffer = new Byte[128];
                thisClient.socket.Receive(buffer);

                string username = Encoding.Default.GetString(buffer);
                username = username.Substring(0, username.IndexOf("\0"));
                if (userNames.Contains(username)) //If username exists
                {
                    if (connectedUsers.Contains(username)) //If user with that username already connected.
                    {
                        richTextBox1.AppendText("Clinet tried to connect with username: " + username + "\n");
                        richTextBox1.AppendText("A user with this username is already connected.\n");
                        string message = "A user with this username is already connected...\n";
                        Byte[] buffer4 =  Encoding.Default.GetBytes(message);
                        try
                        {
                            thisClient.socket.Send(buffer4);
                        }
                        catch
                        {
                            richTextBox1.AppendText("There is a problem! I couldn't send message to a cilent...\n");
                            serverSocket.Close();
                        }
                        return false;
                    }
                    else
                    {	//Username is valid, inform the client that the connection is successful
                        thisClient.changeName(username);
                        connectedUsers.Add(username);
                        richTextBox1.AppendText("Clinet tried to connect with username: " + username + "\n");
                        richTextBox1.AppendText("Connection is sucessful. \n");
                        string message = "valid name";
                        Byte[] buffer5 =  Encoding.Default.GetBytes(message);
                        try
                        {
                            thisClient.socket.Send(buffer5);
                        }
                        catch
                        {
                            richTextBox1.AppendText("There is a problem! I couldn't send message to a cilent...\n");
                            serverSocket.Close();
                        }
                        return true;
                    }
                }
                else
                {//Username does not exist in the provided txt file
                    richTextBox1.AppendText("Clinet tried to connect with username: " + username + "\n");
                    richTextBox1.AppendText("Username does not exist, connection unsuccessful!\n");
                    string message = "Username does not exist, connection unsuccessful!\n";
                    Byte[] buffer6 =  Encoding.Default.GetBytes(message);
                    try
                    {
                        thisClient.socket.Send(buffer6);
                    }
                    catch
                    {
                        richTextBox1.AppendText("There is a problem! I couldn't send message to a cilent...\n");
                        serverSocket.Close();
                    }
                    return false;
                }
               
            }
            catch
            {
                richTextBox1.AppendText("An error occured whıle parsıng username!\n");
                thisClient.socket.Close();
                return false;
            }

        }
        private void Receive(clientSocket thisClient) //Receive messages from thisCliet
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[2048];
                    thisClient.socket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Trim('\0');

                    if (!String.Equals(incomingMessage, ""))
                    {
                        operationOfAClient.WaitOne();  //mutex
                        string hint = incomingMessage.Substring(0, incomingMessage.IndexOf(" "));
                        string actual = incomingMessage.Substring(incomingMessage.IndexOf(" ") + 1);

                        string responseMessage = "";
                        if (String.Equals(hint, "MCDZD!!")) //Client requested all existing sweets
                        {
                            
                            richTextBox1.AppendText(thisClient.name + " " + "requested all sweets.\n");

                            foreach (Sweets sweets in listOfSweets)
                            { //Take each sweet that does not belong to the requesting username
                                
                                HashSet<string> listOfBlockedBy = XblockedByYs[thisClient.name]; 
                                if ((!String.Equals(thisClient.name, sweets.username))&&(!listOfBlockedBy.Contains(sweets.username)))
                                {
                                    responseMessage += sweets.sweetId.ToString() + " " + sweets.username + " " + sweets.now + ": " + sweets.sweetBody + "\n";

                                }
                            }
                            if (String.IsNullOrEmpty(responseMessage))
                            { //If there are no sweets excluding the clients own sweets 
                                responseMessage = "MCDZD!!NO ";
                            }
                            else
                            { //There are sweets excluding the clients own sweets 
                                responseMessage = "MCDZD!! " + responseMessage;
                            }


                        }
                        else if (String.Equals(hint, "MCDZD!!FOLLOWING")) //Client requested all sweets from followed users
                        {//No need to check blocked because if blocked, cannot follow

                            richTextBox1.AppendText(thisClient.name + " " + "requested all sweets from followed users.\n");

                            HashSet<string> listOfFollowing = XfollowsYs[thisClient.name]; //All the users that client is following

                            foreach (Sweets sweets in listOfSweets)
                            { //Take each sweet of the followed users

                                if ((!String.Equals(thisClient.name, sweets.username)) && listOfFollowing.Contains(sweets.username))
                                {
                                    responseMessage += sweets.sweetId.ToString() + " " + sweets.username + " " + sweets.now + ": " + sweets.sweetBody + "\n";

                                }
                            }
                            if (String.IsNullOrEmpty(responseMessage)) //There are no sweets from followed users
                            {
                                responseMessage = "MCDZD!!FOLLOWINGNO ";
                            }
                            else //There are sweets from followed users
                            {
                                responseMessage = "MCDZD!!FOLLOWING " + responseMessage;
                            }

                            
                        }
                        else if (String.Equals(hint, "MCDZD!!ALLUSERS")) //Client requested the list of all users
                        {

                            richTextBox1.AppendText(thisClient.name + " " + "requested all usernames.\n");
                            HashSet<string> listOfBlockedBy = XblockedByYs[thisClient.name];
                            foreach (String user in userNames)
                            { //Take each username except the client and the client is not blocked by

                                if ((!String.Equals(thisClient.name, user)) && (!listOfBlockedBy.Contains(user))) 
                                {
                                    responseMessage += user + "\n";

                                }
                            }
                            if (String.IsNullOrEmpty(responseMessage)) //There are no other users except the client
                            {
                                responseMessage = "MCDZD!!ALLUSERSNO ";
                            }
                            else //There are other users except the client
                            {
                                responseMessage = "MCDZD!!ALLUSERS " + responseMessage;
                            }
                        }
                        else if (String.Equals(hint, "MCDZD!!FOLLOW")){ //Client requested to follow someone
                            
                            HashSet<string> listOfBlockedBy = XblockedByYs[thisClient.name]; 
                            if (String.Equals(thisClient.name, actual)) //Client tried to follow themselves
                            {
                                richTextBox1.AppendText(thisClient.name + " tried to follow themselves, not permmited!" + ".\n");
                                responseMessage = "MCDZD!!YOU ";
                            }
                            else if (!userNames.Contains(actual)) //Client tried to follow non existing user
                            {
                                richTextBox1.AppendText(thisClient.name + " tried to follow " + actual + " ,username does not exist. Not permmited!" + ".\n");
                                responseMessage = "MCDZD!!NOTEXIST ";
                            }
                            else if (XfollowsYs[thisClient.name].Contains(actual)) //Client tried to follow already followed user
                            {
                                richTextBox1.AppendText(thisClient.name + " tried to follow " + actual + " ,username that is already followed. Not permmited!" + ".\n");
                                responseMessage = "MCDZD!!ALREADY ";
                            }
                            else if(listOfBlockedBy.Contains(actual)){ //Client tried to follow someone who blocked them
                                richTextBox1.AppendText(thisClient.name + " tried to follow " + actual + " ,user blocked you. Not permmited!" + ".\n");
                                responseMessage = "MCDZD!!BLOCKED ";

                            }
                            
                            else //Follow operation is successfull
                            {
                                XfollowsYs[thisClient.name].Add(actual);
                                YfollowedByXs[actual].Add(thisClient.name);
                                using (StreamWriter sw = File.AppendText(@"./following.txt"))
                                {
                                    sw.WriteLine(thisClient.name + " " + actual);
                                }
                                richTextBox1.AppendText(thisClient.name + " followed " + actual + ".\n");
                                responseMessage = "MCDZD!!FOLLOW ";
                            }
                        }
                        else if (String.Equals(hint, "MCDZD!!SWEET"))
                        {
                            //User posted a sweet, add that to the sweets
                            string realSweet = actual.Replace("\n", "");
                            richTextBox1.AppendText(thisClient.name + " posted a sweet: " + realSweet + ".\n");
                            lastSeenId++;
                            string nowTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            listOfSweets.Add(new Sweets(lastSeenId, nowTime, realSweet, thisClient.name));

                            using (StreamWriter sw = File.AppendText(@"./sweets.txt"))
                            {
                                sw.WriteLine(lastSeenId.ToString() + " " + thisClient.name + " " + nowTime + " " + realSweet);
                            }
                        }
                        else if (String.Equals(hint, "MCDZD!!BLOCK"))
                        {
                            
                            if (!userNames.Contains(actual)) //Client tried to block non existing user
                            {
                                
                                richTextBox1.AppendText(thisClient.name + " tried to block " + actual + " ,username does not exist. Not permmited!" + ".\n");
                                responseMessage = "MCDZD!!BLOCKNOTEXIST ";
                            }
                            
                            else if (String.Equals(thisClient.name, actual)) //Client tried to block themselves
                            {
                                richTextBox1.AppendText(thisClient.name + " tried to block themselves, not permmited!" + ".\n");
                                responseMessage = "MCDZD!!BLOCKYOU ";
                            }
                            else if(XblockedByYs[actual].Contains(thisClient.name)){//Client tried to block already blocked existing user
                                richTextBox1.AppendText(thisClient.name + " tried to block " + actual + " ,user already blocked by you!" + ".\n");
                                responseMessage = "MCDZD!!BLOCKALREADYBYYOU ";
                            }
                            
                            else
                            {
                                using (StreamWriter sw = File.AppendText(@"./blockedBy.txt"))
                                {
                                    sw.WriteLine(actual + " " + thisClient.name);
                                }
                                XblockedByYs[actual].Add(thisClient.name);
                                if(XfollowsYs[actual].Contains(thisClient.name)){ //if blocked user followed current
                                    XfollowsYs[actual].Remove(thisClient.name);
                                }
                                if(YfollowedByXs[thisClient.name].Contains(actual)){
                                    YfollowedByXs[thisClient.name].Remove(actual);
                                }
                                
                                responseMessage = "MCDZD!!BLOCK ";
                                richTextBox1.AppendText(thisClient.name+" blocked "+actual+"\n");
                                File.WriteAllText(@"./following.txt", String.Empty);
                                foreach(KeyValuePair<String,HashSet<String>> X in XfollowsYs){
                                    foreach(String Y in X.Value ){
                                        using (StreamWriter sw = File.AppendText(@"./following.txt"))
                                        {
                                            sw.WriteLine(X.Key + " " + Y);
                                        }
                                    }
                                }
                            }
                            
                        }
                        else if (String.Equals(hint, "MCDZD!!LISTFOLLOWED"))
                        {
                            //User requested list of users he is following
                            richTextBox1.AppendText(thisClient.name + " " + "requested list of followings.\n");
                            HashSet<string> listOfFollowing = XfollowsYs[thisClient.name];

                            foreach(String u in listOfFollowing){
                                responseMessage+= u + "\n";
                            }
                            if (String.IsNullOrEmpty(responseMessage)) //There are no users user is following
                            {
                                responseMessage = "MCDZD!!LISTFOLLOWEDNO ";
                            }
                            else //There are users user is following
                            {
                                responseMessage = "MCDZD!!LISTFOLLOWED " + responseMessage;
                            }
                            
                        }
                        else if (String.Equals(hint, "MCDZD!!LISTFOLLOWER"))
                        {
                            //User requested list of users following him
                            richTextBox1.AppendText(thisClient.name + " " + "requested list of followers.\n");
                            HashSet<string> listOfFollower = YfollowedByXs[thisClient.name];
                            
                            foreach(String u in listOfFollower){
                                responseMessage+= u + "\n";
                            }
                            if (String.IsNullOrEmpty(responseMessage)) //There are no users user is following
                            {
                                responseMessage = "MCDZD!!LISTFOLLOWERNO ";
                            }
                            else //There are users user is following
                            {
                                responseMessage = "MCDZD!!LISTFOLLOWER " + responseMessage;
                            }
                            
                        }
                        else if (String.Equals(hint, "MCDZD!!DELETESWEET"))
                        {
                            int sId;
                            richTextBox1.AppendText(thisClient.name + " " + "requested to delete sweet with id"+actual+".\n");
                            if(Int32.TryParse(actual, out sId)){
                                var itemToRemove = listOfSweets.FirstOrDefault(r => r.sweetId == sId);
                                if (itemToRemove==null){
                                    responseMessage = "MCDZD!!DELETESWEETNO ";
                                    richTextBox1.AppendText("Sweet with this id does not exist.\n");
                                }
                                else if(!String.Equals(itemToRemove.username, thisClient.name)){
                                    responseMessage = "MCDZD!!DELETESWEETOFOTHER ";
                                    richTextBox1.AppendText("Sweet with this id belongs to another user.\n");
                                }
                                else{
                                    listOfSweets.Remove(itemToRemove);
                                    responseMessage = "MCDZD!!DELETESWEET ";
                                    richTextBox1.AppendText("Deleted successfully.\n");
                                    
                                    File.WriteAllText(@"./sweets.txt", String.Empty);
                                    foreach(Sweets s in listOfSweets){
                                        String text = s.sweetId.ToString()+" "+s.username+" "+s.now+" "+s.sweetBody;
                                        using (StreamWriter sw = File.AppendText(@"./sweets.txt"))
                                        {
                                            sw.WriteLine(text);
                                        }
                                    }
                                }
                            }
                            
                        }
                        else if (String.Equals(hint, "MCDZD!!MY")) //Client requested all their sweets
                        {

                            richTextBox1.AppendText(thisClient.name + " " + "requested their sweets.\n");

                            foreach (Sweets sweets in listOfSweets)
                            { //Take each sweet that does not belong to the requesting username

                                if (String.Equals(thisClient.name, sweets.username))
                                {
                                    responseMessage += sweets.sweetId.ToString() + " " + sweets.username + " " + sweets.now + ": " + sweets.sweetBody + "\n";

                                }
                            }
                            if (String.IsNullOrEmpty(responseMessage))
                            { //If there are no sweets excluding the clients own sweets 
                                responseMessage = "MCDZD!!MYNO ";
                            }
                            else
                            { //There are sweets excluding the clients own sweets 
                                responseMessage = "MCDZD!!MY " + responseMessage;
                            }

                        }
                       
                        Byte[] buffer2 = Encoding.Default.GetBytes(responseMessage); 
                        //Send the client an appropriate answer of the requested operation
                        try
                        {
                            thisClient.socket.Send(buffer2);
                        }
                        catch
                        {
                            richTextBox1.AppendText("There is a problem! Check the connection...\n");
                            serverSocket.Close();
                        }
                        operationOfAClient.ReleaseMutex();
                    }

                }
                catch
                { //Client is disconnected
                    operationOfAClient.ReleaseMutex();
                    if (!terminating && isServerconnected)
                    {
                        richTextBox1.AppendText(thisClient.name + " is disconnected\n");
                    }
                    connectedUsers.Remove(thisClient.name);
                    thisClient.socket.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
        }

    }
}
