using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SweetProjectClientSide
{
    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;

        Socket clientSocket;
     
        string myUsername; //stores name of current client

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;
            int portNum;

            if (Int32.TryParse(textBox_port.Text, out portNum))
            {//If the port number is valid
                try
                {
                    string username = textBox_name.Text;
                    richTextBox_feed.Clear();
                    richTextBox_notification.Clear();
                    if (username != "")
                    {
                        clientSocket.Connect(IP, portNum);
                        button_connect.Enabled = false;
                        button_request.Enabled = true;
                        button_post.Enabled = true;
                        textBox_sweet.Enabled = true;
                        button_disconnect.Enabled = true;
                        button_followedFeed.Enabled = true;
                        button_follow.Enabled = true;
                        textBox_follow.Enabled = true;
                        button_listAll.Enabled = true;
    
                        button_block.Enabled = true;
                        button_delete.Enabled = true;
                        button_followers.Enabled = true;
                        button_followed.Enabled = true;
                        button_mysweet.Enabled = true;
                        button_delete.Enabled = true;
                        textBox_block.Enabled = true;
                        textBox_deleteSweet.Enabled = true;

                        connected = true;

                        //username is sent
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(username);
                        clientSocket.Send(buffer);

                        //receive response of the server
                        Byte[] buffer_Server = new Byte[64];
                        clientSocket.Receive(buffer_Server);
                        string incomingResponse = Encoding.Default.GetString(buffer_Server);
                        incomingResponse = incomingResponse.Substring(0, incomingResponse.IndexOf("\0"));

                        if (incomingResponse == "valid name")
                        {//Connection is successful
                            richTextBox_notification.AppendText("Connected to the server!\n");
                            myUsername = username;
                            Thread receiveThread = new Thread(Receive);
                            receiveThread.Start();
                        }
                        else if(incomingResponse== "A user with this username is already connected...\n")
                        {//Connection is unsuccessful because a user already connected with that username 
                            button_request.Enabled = false;
                            button_connect.Enabled = true;
                            button_post.Enabled = false;
                            textBox_sweet.Enabled = false;
                            button_disconnect.Enabled = false;
                            textBox_follow.Enabled = false;
                            button_follow.Enabled = false;
                            button_followedFeed.Enabled = false;
                            button_listAll.Enabled = false;
                            
                            button_block.Enabled = false;
                            button_delete.Enabled = false;
                            button_followers.Enabled = false;
                            button_followed.Enabled = false;
                            button_mysweet.Enabled = false;
                            button_delete.Enabled = false;
                            textBox_block.Enabled = false;
                            textBox_deleteSweet.Enabled = false;

                            richTextBox_notification.AppendText(incomingResponse);
                        }
                        else
                        {//Connection is unsuccessful because username does not exist
                            button_request.Enabled = false;
                            button_connect.Enabled = true;
                            button_post.Enabled = false;
                            textBox_sweet.Enabled = false;
                            button_disconnect.Enabled = false;
                            textBox_follow.Enabled = false;
                            button_follow.Enabled = false;
                            button_listAll.Enabled = false;
                            button_followedFeed.Enabled = false;
                           
                            button_block.Enabled = false;
                            button_delete.Enabled = false;
                            button_followers.Enabled = false;
                            button_followed.Enabled = false;
                            button_mysweet.Enabled = false;
                            button_delete.Enabled = false;
                            textBox_block.Enabled = false;
                            textBox_deleteSweet.Enabled = false;

                            richTextBox_notification.AppendText("Username does not exist,try with another!\n");
                        }
                    }
                    else
                    {
                        richTextBox_notification.AppendText("Please enter a username to connect!\n");
                    }
                }
                catch
                {
                    richTextBox_notification.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                richTextBox_notification.AppendText("Enter valid port number!\n");
            }
        }
        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[16384]; //the messages are taken as bytes.
                    clientSocket.Receive(buffer);
                    string incomingMessage = Encoding.Default.GetString(buffer); //bytes converted to string
                    incomingMessage = incomingMessage.Trim('\0');

                    if (incomingMessage != "")
                    {
                        string hint = incomingMessage.Substring(0, incomingMessage.IndexOf(" "));
                        string actual = incomingMessage.Substring(incomingMessage.IndexOf(" ") + 1);

                        if (String.Equals(hint, "MCDZD!!DISCONNECT"))
                        {//Server is disconnected, inform the user
                            connected = false;
                            terminating = true;
                            button_connect.Enabled = true;
                            button_post.Enabled = false;
                            textBox_sweet.Enabled = false;
                            button_request.Enabled = false;
                            button_disconnect.Enabled = false;
                            button_listAll.Enabled = false;
                            textBox_follow.Enabled = false;
                            button_follow.Enabled = false;
                            button_followedFeed.Enabled = false;
                            
                            button_block.Enabled = false;
                            button_delete.Enabled = false;
                            button_followers.Enabled = false;
                            button_followed.Enabled = false;
                            button_mysweet.Enabled = false;
                            button_delete.Enabled = false;
                            textBox_block.Enabled = false;
                            textBox_deleteSweet.Enabled = false;
                            button_listAll.Enabled = false;
                            button_followedFeed.Enabled = false;

                            richTextBox_notification.AppendText("Server disconnected\n");
                            textBox_ip.Clear();
                            textBox_port.Clear();
                            textBox_name.Clear();

                            clientSocket.Close();
                        }
                        
                        else if (String.Equals(hint, "MCDZD!!")) //Server sent the feed from all users
                        {
                            richTextBox_feed.Text = "Feed from all users: \n";
                            richTextBox_feed.AppendText(actual);
                        }
                        else if (String.Equals(hint, "MCDZD!!NO")) //Server sent an empty feed from all users
                        {
                            richTextBox_feed.Text = "There are no sweets from other users... \n";
                        }

                        else if (String.Equals(hint, "MCDZD!!FOLLOWING")) //Server sent the feed from followed users
                        {
                            richTextBox_feed.Text = "Feed from followed users: \n";
                            richTextBox_feed.AppendText(actual);
                        }
                        else if (String.Equals(hint, "MCDZD!!FOLLOWINGNO")) //Server sent an empty feed from followed users
                        {
                            richTextBox_feed.Text = "There are no sweets from followed users... \n";
                        }

                        else if (String.Equals(hint, "MCDZD!!ALLUSERS")) //Server sent the list of all users
                        {
                            richTextBox_notification.AppendText("List of all users: \n");
                            richTextBox_notification.AppendText(actual);
                        }
                        else if (String.Equals(hint, "MCDZD!!ALLUSERSNO")) //Server sent an empty list of all users
                        {
                            richTextBox_notification.AppendText("There are no users in db other than you!\n");
                        }

                        else if (String.Equals(hint, "MCDZD!!FOLLOW")) //Follow operation is successful
                        {
                            richTextBox_notification.AppendText("The user is followed.\n");

                        }
                        else if (String.Equals(hint, "MCDZD!!ALREADY")) //Follow operation is unsuccessful because user is already followed
                        {
                            richTextBox_notification.AppendText("You are already following this user.\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!YOU")) //Follow operation is unsuccessful because client tried to follow themselves
                        {
                            richTextBox_notification.AppendText("You can not follow yourself!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!NOTEXIST")) //Follow operation is unsuccessful because user does not exist
                        {
                            richTextBox_notification.AppendText("User does not exist!\n");
                        }
                        //newly added
                        else if (String.Equals(hint, "MCDZD!!LISTFOLLOWED")) //Server sent the list of followed users
                        {
                            richTextBox_notification.AppendText("List of followed users: \n");
                            richTextBox_notification.AppendText(actual);
                        }
                        else if (String.Equals(hint, "MCDZD!!LISTFOLLOWEDNO")) //No user is followed- empty followed list
                        {
                            richTextBox_notification.AppendText("No user is followed!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!LISTFOLLOWER")) //Server sent the list of followers
                        {
                            richTextBox_notification.AppendText("List of followers: \n");
                            richTextBox_notification.AppendText(actual);
                        }
                        else if (String.Equals(hint, "MCDZD!!LISTFOLLOWERNO")) //No followers - empty follower list
                        {
                            richTextBox_notification.AppendText("No follower exist!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!BLOCK")) //successful block
                        {
                            richTextBox_notification.AppendText("User is successfully blocked!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!BLOCKED")) //this user blocked you
                        {
                            richTextBox_notification.AppendText("User blocked you, you are not permitted to follow!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!BLOCKNOTEXIST")) //this user not exist
                        {
                            richTextBox_notification.AppendText("Username does not exist!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!BLOCKYOU")) //blocking himself/herself
                        {
                            richTextBox_notification.AppendText("You can not block yourself!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!BLOCKALREADYBYYOU")) //Client tried to block already blocked existing user
                        {
                            richTextBox_notification.AppendText("User is already blocked by you, you can not block again!\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!DELETESWEETNO")) //not exist sweet ID
                        {
                            richTextBox_notification.AppendText("Sweet with this id does not exist.\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!DELETESWEETOFOTHER")) //other user's sweet
                        {
                            richTextBox_notification.AppendText("Sweet with this id  belongs to another user.\n");
                        }
                        
                        else if (String.Equals(hint, "MCDZD!!DELETESWEET")) //successfully deleted
                        {
                            richTextBox_notification.AppendText("Deleted successfully.\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!MYNO")) //no sweets of user
                        {
                            richTextBox_notification.AppendText("You did not post any sweet..\n");
                        }
                        else if (String.Equals(hint, "MCDZD!!MY")) //there are sweets of user
                        {
                            richTextBox_notification.AppendText("List of your sweets: \n");
                            richTextBox_notification.AppendText(actual);
                        }
                       

                    }
                  

                }
                catch
                {
                    if (!terminating)
                    {
                        richTextBox_notification.AppendText("The server has disconnected\n");
                        button_connect.Enabled = true;
                        textBox_sweet.Enabled = false;
                        button_post.Enabled = false;
                        button_request.Enabled = false;
                        button_disconnect.Enabled = false;
                        button_listAll.Enabled = false;
                        textBox_follow.Enabled = false;
                        button_follow.Enabled = false;
                        button_followedFeed.Enabled = false;
                        
                        button_block.Enabled = false;
                        button_delete.Enabled = false;
                        button_followers.Enabled = false;
                        button_followed.Enabled = false;
                        button_mysweet.Enabled = false;
                        button_delete.Enabled = false;
                        textBox_block.Enabled = false;
                        textBox_deleteSweet.Enabled = false;
                        button_listAll.Enabled = false;
                        button_followedFeed.Enabled = false;
                    }
                    clientSocket.Close();
                    connected = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            button_connect.Enabled = true;
            button_post.Enabled = false;
            textBox_sweet.Enabled = false;
            button_request.Enabled = false;
            button_disconnect.Enabled = false;

            textBox_follow.Enabled = false;
            button_follow.Enabled = false;
            button_followedFeed.Enabled = false;

            button_listAll.Enabled = false;
            
            button_block.Enabled = false;
            button_delete.Enabled = false;
            button_followers.Enabled = false;
            button_followed.Enabled = false;
            button_mysweet.Enabled = false;
            button_delete.Enabled = false;
            textBox_block.Enabled = false;
            textBox_deleteSweet.Enabled = false;
            button_listAll.Enabled = false;
            button_followedFeed.Enabled = false;

            textBox_ip.Clear();
            textBox_port.Clear();
            textBox_name.Clear();
            richTextBox_feed.Clear();
            textBox_follow.Clear();
            //socket is closed
            clientSocket.Close();
            Environment.Exit(0);
        }

        private void button_request_Click(object sender, EventArgs e)
        {

            try
            {
                //Send message to the server requesting all the sweets.
               
                richTextBox_notification.AppendText("Request for feed is sent!\n");

                Byte[] buffer_request = new Byte[64];
                buffer_request = Encoding.Default.GetBytes("MCDZD!! ");
                clientSocket.Send(buffer_request);

                richTextBox_notification.AppendText("Successful request!\n");
                               
            }
            catch
            {
                richTextBox_notification.AppendText("Unsuccessful request!\n");
            }
        }

        private void button_post_Click(object sender, EventArgs e)
        {
            //Post a new sweet, send that to the server
            string sweet = textBox_sweet.Text;
            if (sweet != "" && sweet.Length <= 500)
            {

                Byte[] buffer_sweet = new Byte[500];
                buffer_sweet = Encoding.Default.GetBytes("MCDZD!!SWEET "+sweet);
                clientSocket.Send(buffer_sweet);
                richTextBox_notification.AppendText("Sent: " + sweet + "\n");
                textBox_sweet.Clear();
            }
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {//Disconnect the client from the server
            connected = false;
            terminating = true;
            button_connect.Enabled = true;
            button_post.Enabled = false;
            textBox_sweet.Enabled = false;
            button_request.Enabled = false;
            button_disconnect.Enabled = false;
            
            button_block.Enabled = false;
            button_delete.Enabled = false;
            button_followers.Enabled = false;
            button_followed.Enabled = false;
            button_mysweet.Enabled = false;
            button_listAll.Enabled = false;
            button_followedFeed.Enabled = false;
            richTextBox_notification.AppendText("Disconnected \n");
            textBox_ip.Clear();
            textBox_port.Clear();
            textBox_name.Clear();
            
            richTextBox_feed.Clear();
            
            //socket is closed
            clientSocket.Close();
        }

        private void button_followedFeed_Click(object sender, EventArgs e)
        {
            try
            {
                //Send message to the server requesting all the sweets of followed users.
               
                richTextBox_notification.AppendText("Request for feed for followed ones is sent!\n");

                Byte[] buffer_request_following = new Byte[64];
                buffer_request_following = Encoding.Default.GetBytes("MCDZD!!FOLLOWING ");
                clientSocket.Send(buffer_request_following);

                richTextBox_notification.AppendText("Successful request!\n");
                
            }
            catch
            {
                richTextBox_notification.AppendText("Unsuccessful request!\n");
            }
        }

        private void button_follow_Click(object sender, EventArgs e)
        {
            try
            {
                string follow_request_name = textBox_follow.Text;
               
                if (follow_request_name != "")
                {

                    //follow request name is sent
                    Byte[] buffer_follow = new Byte[2000];
                    buffer_follow = Encoding.Default.GetBytes("MCDZD!!FOLLOW "+follow_request_name);
                    clientSocket.Send(buffer_follow);
                    textBox_follow.Clear();
                }
                else
                {
                    richTextBox_notification.AppendText("You should enter the username...\n");
                }
            }
            catch
            {
                richTextBox_notification.AppendText("There is a problem to follow the user\n");
            }
           
        }

        private void button_listAll_Click(object sender, EventArgs e)
        {
           try
           {
               //Send message to the server requesting all list of users.

               richTextBox_notification.AppendText("Request for list of all users..\n");

               Byte[] buffer_request_followingList = new Byte[64];
               buffer_request_followingList = Encoding.Default.GetBytes("MCDZD!!ALLUSERS ");
               clientSocket.Send(buffer_request_followingList);

               richTextBox_notification.AppendText("Successful request for list of all users!\n");
           }
           catch
           {
               richTextBox_notification.AppendText("Unsuccessful request for list of all users!\n");
           }
        }

        private void button_followers_Click(object sender, EventArgs e)
        {
            try
            {
                //Send message to the server requesting list of followers.

                richTextBox_notification.AppendText("Request for list of followers..\n");

                Byte[] buffer_request_followers = new Byte[64];
                buffer_request_followers = Encoding.Default.GetBytes("MCDZD!!LISTFOLLOWER ");
                clientSocket.Send(buffer_request_followers);

                richTextBox_notification.AppendText("Successful request for list of followers!\n");
            }
            catch
            {
                richTextBox_notification.AppendText("Unsuccessful request for list of followers!\n");
            }
        }

        private void button_followed_Click(object sender, EventArgs e)
        {
            try
            {
                //Send message to the server requesting list of followed.

                richTextBox_notification.AppendText("Request for list of followed users..\n");

                Byte[] buffer_request_followed = new Byte[64];
                buffer_request_followed = Encoding.Default.GetBytes("MCDZD!!LISTFOLLOWED ");
                clientSocket.Send(buffer_request_followed);

                richTextBox_notification.AppendText("Successful request for list of followed users!\n");
            }
            catch
            {
                richTextBox_notification.AppendText("Unsuccessful request for list of followed users!\n");
            }
        }

        private void button_block_Click(object sender, EventArgs e)
        {
            try
            {
                string block_request_name = textBox_block.Text;
              
                if (block_request_name != "")
                {

                    //block request name is sent
                    Byte[] buffer_block = new Byte[2000];
                    buffer_block = Encoding.Default.GetBytes("MCDZD!!BLOCK " + block_request_name);
                    clientSocket.Send(buffer_block);
                    
                    textBox_block.Clear();
                    richTextBox_notification.AppendText("you try to block "+block_request_name+"\n");
                }
                else
                {
                    richTextBox_notification.AppendText("You should enter the username...\n");
                }
            }
            catch
            {
                richTextBox_notification.AppendText("There is a problem to block the user\n");
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                string sweetID_tobedeleted = textBox_deleteSweet.Text;

                if (sweetID_tobedeleted != "")
                {

                    //delete request sweet id is sent
                    Byte[] buffer_delete = new Byte[2000];
                    buffer_delete = Encoding.Default.GetBytes("MCDZD!!DELETESWEET " + sweetID_tobedeleted);
                    clientSocket.Send(buffer_delete);
                    textBox_deleteSweet.Clear();
                }
                else
                {
                    richTextBox_notification.AppendText("You should enter the sweet Id...\n");
                }
            }
            catch
            {
                richTextBox_notification.AppendText("There is a problem to send sweet Id\n");
            }
        }

        private void button_mysweet_Click(object sender, EventArgs e)
        {
            try
            {
                //Send message to the server requesting sweets of himself/herself.

                richTextBox_notification.AppendText("Request for my sweets\n");

                Byte[] buffer_request_mysweets = new Byte[64];
                buffer_request_mysweets = Encoding.Default.GetBytes("MCDZD!!MY ");
                clientSocket.Send(buffer_request_mysweets);

                richTextBox_notification.AppendText("Successful request for my sweets!\n");
            }
            catch
            {
                richTextBox_notification.AppendText("Unsuccessful request for my sweets!\n");
            }
        }
    }
}