var express = require("express");
var app = express();

var server = require("http").createServer(app);
var io = require("socket.io").listen(server);

app.set("port", process.env.PORT || 3000);

var clients = [];
var id = 0;
var OnlinePlayerNum = 0;

//For optimisation / debugging
var movements = 0;
var healthChanges = 0;
var lastMoves, lastHealths, lastOnline;
setInterval(function() {
  if (
    movements != lastMoves ||
    healthChanges != lastHealths ||
    OnlinePlayerNum != lastOnline
  ) {
    lastMoves = movements;
    lastHealths = healthChanges;
    lastOnline = OnlinePlayerNum;
    return console.log(
      "Players Online: " + OnlinePlayerNum,
      " Server Uptime: " +
        process
          .uptime()
          .toString()
          .toHHMMSS(),
      "\r\nMovements: " + movements,
      " | Health Updates: " + healthChanges,
      "\r\n"
    );
  }
}, 10000);

io.on("connection", function(socket) {
  //var currentUser;
  //var AllReadyOnline = [];

  /* --------------- Connection --------------- */
  socket.on("USER_CONNECT", function() {
    OnlinePlayerNum++;
    id++;
    socket.emit("PLAYER_ID", {
      id: id,
      name: "Player " + id,
      num: OnlinePlayerNum,
      health: 100
    });
    console.log("client connected - sessionid: " + id);
  });

  socket.on("USER_INITIATED", userData => {
    clients.forEach(player => {
      socket.emit("GET_EXISTING_PLAYER", player); //we should consider looping on client instead to avoid emitting N times
    });
    clients.push(userData);
    console.log("- sending already online player data to id: " + userData.id);

    socket.broadcast.emit("A_USER_INITIATED", userData);
    console.log("- broadcasting new player data \r\n");
  });

  /* --------------- Actions --------------- */
  socket.on("CLIENT_MOVE", function(movementData) {
    movements++;
    for (var i = 0; i < clients.length; i++)
      if (clients[i].id == movementData.id) {
        clients[i].position = movementData.position;
        clients[i].rotation = movementData.rotation;
        socket.broadcast.emit("OTHER_PLAYER_MOVED", movementData);
        //console.log("Client moved: " + movementData.id+ " x: "+ movementData.position.x );
        return;
      }
  });

  socket.on("UPDATE_HEALTH", function(healthData) {
    healthChanges++;
    for (var i = 0; i < clients.length; i++)
      if (clients[i].id == healthData.id) {
        clients[i].health += healthData.health;
        healthData.health = clients[i].health;
        io.emit("PLAYER_HEALTHCHANGE", healthData);
        return;
      }
  });

  // socket.on("REPLAY_TO_CONNECT",(userData)=>{
  // 	AllReadyOnline.push(userData);
  // 	if(AllReadyOnline.length == OnlinePlayerNum -1){
  // 		socket.emit("ALL_USERS_INFO", AllReadyOnline);
  // 	}
  // });

  // socket.on("PLAY", function( data ){
  // 	currentUser = {
  // 		name:data.name,
  // 		position:data.position
  // 	}
  // clients.push(currentUser);
  // socket.emit("PLAY", currentUser);
  // socket.broadcast.emit("USER_CONNECTED", currentUser)

  // });

  // socket.on("disconnect", function(){
  // 	socket.broadcast.emit("USER_DISCONNECTED", currentUser);
  // 	for (var i = 0; i < clients.length; i++){
  // 		if (client[i].name = currentUser.name){
  // 			console.log( "User " + clients[i].name + " Disconnected");
  // 			client.splice(i,1);
  // 		}
  // 	}
  // });
});

server.listen(app.get("port"), function() {
  console.log("---SERVER IS RUNNING AT " + app.get("port") + "--- \r\n");
});

String.prototype.toHHMMSS = function() {
  var sec_num = parseInt(this, 10); // don't forget the second param
  var hours = Math.floor(sec_num / 3600);
  var minutes = Math.floor((sec_num - hours * 3600) / 60);
  var seconds = sec_num - hours * 3600 - minutes * 60;

  if (hours < 10) {
    hours = "0" + hours;
  }
  if (minutes < 10) {
    minutes = "0" + minutes;
  }
  if (seconds < 10) {
    seconds = "0" + seconds;
  }
  return hours + ":" + minutes + ":" + seconds;
};
