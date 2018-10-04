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

var spawnPositionsA = [{ x: -42, y: 2, z: -42 }];
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
      "Players Online: " + clients.length,
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
  //Properties for the current connected player
  var currentPlayerId;

  /* --------------- Connection --------------- */
  socket.on("USER_CONNECT", function() {
    OnlinePlayerNum++;
    id++;
    currentPlayerId = id;
    socket.emit("PLAYER_ID", {
      id: currentPlayerId,
      name: "Player " + id,
      num: OnlinePlayerNum,
      health: 100
    });
    console.log("client connected - sessionid: " + id);
  });

  socket.on("USER_INITIATED", userData => {
    console.log(userData.weapon);
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
        /*if (clients[i].health <= 0) {
          io.emit("PLAYER_DEAD", { id: healthData.id });
          //clients.slice(clients[i]);
        } else {*/
        healthData.health = clients[i].health;
        io.emit("PLAYER_HEALTHCHANGE", healthData);
        //}
        return;
      }
  });

  socket.on("PLAYER_DEAD", function(data) {
    //console.log("length: " + clients.length);
    removePlayerFromList(data);
    //console.log("length: " + clients.length);

    console.log("- remove and broadcast player dead ID: " + data + "\r\n");
    socket.broadcast.emit("OTHER_PLAYER_DEAD", { id: data });
  });

  /* --------------- Disconnection --------------- */

  socket.on("disconnect", function() {
    console.log("Player with id " + currentPlayerId + " has disconncted");
    io.emit("USER_DISCONNECTED", { id: currentPlayerId });
    removePlayerFromList(currentPlayerId);
  });
});

server.listen(app.get("port"), function() {
  console.log("---SERVER IS RUNNING AT " + app.get("port") + "--- \r\n");
});

/* --------------- Helpers --------------- */

var removePlayerFromList = data => {
  for (var i = 0; i < clients.length; i++) {
    if (clients[i].id == data) {
      clients.splice(i, 1);
      console.log("Player with id " + data + " has been removed from the list");
    }
  }
};

String.prototype.toHHMMSS = function() {
  var sec_num = parseInt(this, 10);
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
