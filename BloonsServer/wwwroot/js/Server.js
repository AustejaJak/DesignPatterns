const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(() => {
    console.log("Connected to the server.");
}).catch(err => console.error(err.toString()));

connection.on("SendUsername", (message) => {
    const messagesDiv = document.getElementById("messages");
    const newMessage = document.createElement("div");
    newMessage.textContent = `Connected user: ${message}`;
    messagesDiv.appendChild(newMessage);
});

