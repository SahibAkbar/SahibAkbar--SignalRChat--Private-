"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
///document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    // We can assign user-supplied strings to an element's textContent because it
//    // is not interpreted as markup. If you're assigning in any other way, you 
//    // should be aware of possible script injection concerns.
//    li.textContent = `${user} says ${message}`;
//});

connection.start().then(function () {
    document.getElementById("messengerButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

document.getElementById("messengerButton").addEventListener("click", function (event) {
    var message = document.getElementById("messengerInput").value;
    var receiverId = document.getElementById("receiverId").value;
    connection.invoke("SendPrivateMessage", receiverId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();

    var p = document.createElement("p");
    p.textContent = `${message}`;
    p.classList.add("sender")
    document.getElementsByClassName("messageCustom")[0].appendChild(p);
    document.getElementById("messengerInput").value = ""
});

connection.on("ReceiveMessage", function (senderName, message) {
    var p = document.createElement("p");
    p.textContent = `${message}`;
    p.classList.add("receiver")
    document.getElementsByClassName("messageCustom")[0].appendChild(p);
});

document.getElementById("messengerInput").addEventListener("focus", function (event) {
    var receiverId = document.getElementById("receiverId").value;

    connection.invoke("ShowTyping", receiverId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("ShowTyping", function () {
    var p = document.createElement("p");
    p.textContent = `Typing...`;
    p.classList.add("showtyping")
    document.getElementsByClassName("messageCustom")[0].appendChild(p);
});

document.getElementById("messengerInput").addEventListener("blur", function (event) {
    var receiverId = document.getElementById("receiverId").value;

    connection.invoke("HideTyping", receiverId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("HideTyping", function () {
    document.getElementsByClassName("showtyping")[0].remove();
});