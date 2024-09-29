const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub")
    .build();

connection.start().then(() => {
    console.log("Connected to the server.");
}).catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {
    const messagesDiv = document.getElementById("messages");
    const newMessage = document.createElement("div");
    newMessage.textContent = `Message from client: ${message}`;
    messagesDiv.appendChild(newMessage);
});

connection.on("TowerPlaced", (towerInfo) => {
    const messagesDiv = document.getElementById("messages");
    const newMessage = document.createElement("div");
    newMessage.textContent = `Tower placed: ${towerInfo}`;
    messagesDiv.appendChild(newMessage);
});