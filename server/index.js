var express = require('express');
var app = express();

var server = require('http').createServer(app);
var io = require('socket.io').listen(server);

app.set('port', process.env.PORT || 3000);

var clients = [];
var OnlinePlayerNum = 0;
io.on("connection", function(socket){
	// var currentUser;
	var AllReadyOnline = [];

	socket.on("USER_CONNECT", function (){
	OnlinePlayerNum++;
	socket.broadcast.emit("SOMEONE_JOINED");
	});

	socket.on("REPLAY_TO_CONNECT",(userData)=>{
		AllReadyOnline.push(userData);
		if(AllReadyOnline.length == OnlinePlayerNum -1){
			socket.emit("ALL_USERS_INFO", AllReadyOnline);
		}
	});










	socket.on("PLAY", function( data ){
		currentUser = {
			name:data.name,
			position:data.position
		}
	clients.push(currentUser);
	socket.emit("PLAY", currentUser);
	socket.broadcast.emit("USER_CONNECTED", currentUser)
	
	});
	
	socket.on("MOVE", function(data){
		currentUser.position = data.position;
		socket.emit("MOVE", currentUser);
		
	});

	socket.on("disconnect", function(){
		socket.broadcast.emit("USER_DISCONNECTED", currentUser);
		for (var i = 0; i < clients.length; i++){
			if (client[i].name = currentUser.name){
				console.log( "User " + clients[i].name + " Disconnected");
				client.splice(i,1);
			}
		}
	});
});

server.listen(app.get('port'), function() {
	console.log('---SERVER IS RUNNING AT '+ app.get('port')+'---')
});
