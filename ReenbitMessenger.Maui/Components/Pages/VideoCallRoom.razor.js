
const myPeer = new Peer(undefined, {
    host: '/',
    port: '3001'
});

export function AddJoinedUser(userConnectionId){
    var videoGrid = document.getElementById("video-grid");

    var element = document.createElement("p");
    element.id = userConnectionId;
    element.innerHTML = userConnectionId;
    videoGrid.appendChild(element);
}

export function RemoveLeavingUser(userConnectionId){
    var element = document.getElementById(userConnectionId);

    element.remove();
}
