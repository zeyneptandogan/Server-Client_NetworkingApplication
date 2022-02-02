# Server-Client_NetworkingApplication

In this project, a social networking application called Switter (SU 
version of Twitter) is  implemented with client and server modules.

(i) The Server module manages storage of messages (called sweets; SU version of tweets), sweet feeds, and follower
relationships among the users 

(ii) The Client module acts as a user who posts sweets,follows and blocks other users, and view sweets of other users.
The server listens on a predefined port and accepts incoming client connections. There might
be one or more clients connected to the server at the same time. Each client knows the IP 
address and the listening port of the server (to be entered through the Graphical User Interface 
(GUI)). 

Clients connect to the server on a corresponding port and identify themselves with 
their usernames. Server keeps the usernames of currently connected clients in order to 
avoid the same name to be connected more than once at a given time to the server. 

### Details:
- Each sweet has three attributes: (i) the username of the sweet owner, (ii) a unique sweet ID 
assigned by the server, and (iii) the timestamp (date and time) that the sweet is posted. These 
attributes are shown, in addition to the sweet itself, at the client side.
- The user can perform the following:

  1. Requesting and getting the list users
  2. Following other users
  3. Requesting sweet feed posted by the followed users only.
  4. Blocking (banning) a user 
  5. Requesting the list of followers and the users followed
  6. Deleting a sweet

### Client Side:
![image](https://user-images.githubusercontent.com/76870399/152242338-1554de75-3437-4d7d-b3a9-e8398c1afc60.png)

### Server Side:
![image](https://user-images.githubusercontent.com/76870399/152242262-bca430fd-6c9d-4c12-b7dc-1cdaf770a26b.png)
