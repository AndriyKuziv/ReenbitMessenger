const ROOM_ID = '@ViewBag.RoomId'
let userId = null;
let localStream = null;
const videoGrid = document.getElementById('video-grid');
const userIdField = document.getElementById('user-id');
const callButton = document.getElementById('call-btn');

const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7051/callhub").build();

const myPeer = new Peer();
myPeer.on('open', id =>{
    userId = id;
    userIdField.innerHTML = id;
    
    const startSignalR = async () =>{
        await connection.start();
        connection.on('ReceiveJoinedUser', id => {
            console.log(`User joined: ${id}`);
            AddJoinedUser(id);
            connectNewUser(id, localStream);
        });
        connection.on('RemoveLeavingUser', id => {
            console.log(`User left: ${id}`);
            RemoveLeavingUser(id);
        })
    }
    startSignalR();
})

myPeer.on('call', call => {
    call.answer(localStream);

    const userVideo = document.createElement('video');
    call.on('stream', userVideoStream => {
        addVideoStream(userVideo, userVideoStream);
    })
})

const myVideo = document.createElement('video');
myVideo.id = "my-video";
myVideo.autoplay = true;
myVideo.muted = true;
myVideo.width = 320;
myVideo.height = 240;

videoGrid.appendChild(myVideo);

const localVideo = document.getElementById(myVideo.id);
const remoteVideo = document.getElementById('remote-video');

navigator.mediaDevices.getUserMedia({ video: true, audio: true })
    .then(stream => {
        localVideo.srcObject = stream;

        myPeer.on('call', call => {
            call.answer(stream);
            call.on('stream', remoteStream => {
                remoteVideo.srcObject = remoteStream;
            });
        });

        callButton.addEventListener('click', () => {
            const call = myPeer.call(userIdField.innerHTML, stream);
            call.on('stream', remoteStream => {
                remoteVideo.srcObject = remoteStream;
            });
        });
    })
    .catch(error => {
        console.error('Error accessing media devices.', error);
});

export function startCamera(videoElementId) {
    const videoElement = document.getElementById(videoElementId);
    
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        navigator.mediaDevices.getUserMedia({ video: true }).then(function(stream) {
            localStream = stream;
            videoElement.srcObject = stream;
            videoElement.play();
        }).catch(function(error) {
            console.error("Error accessing the camera: ", error);
        });
    } else {
        console.error("getUserMedia not supported by this browser.");
    }
}

export function stopCamera(videoElementId) {
    const videoElement = document.getElementById(videoElementId);
    const stream = videoElement.srcObject;
    const tracks = stream.getTracks();

    tracks.forEach(function(track) {
        track.stop();
    });

    videoElement.srcObject = null;
    localStream = null;
}

export async function joinRoom(roomId){
    startCamera(myVideo.id);
    await connection.invoke('JoinRoom', roomId);
}

export async function leaveRoom(roomId){
    stopCamera(myVideo.id);
    await connection.invoke('LeaveRoom', roomId);
}

export async function Start(token, roomId){
    try{

    console.log("Finished start");
    }
    catch(err){
        console.error(err);
    }
}

export function End(){
    if (connection) {
        connection.stop();
        console.log("Finished end");
    }
}

export function AddJoinedUser(userConnectionId){
    var element = document.createElement("p");
    element.id = userConnectionId;
    element.innerHTML = userConnectionId;
    videoGrid.appendChild(element);
}

export function RemoveLeavingUser(userConnectionId){
    var element = document.getElementById(userConnectionId);

    element.remove();
}

const addVideoStream = (video, stream) =>{
    video.srcObject = stream;
    video.autoplay = true;
    videoGrid.appendChild(video);
}

const connectNewUser = (userId, localStream) =>{
    const userVideo = document.createElement('video');
    const call = myPeer.call(userId, localStream);

    call.on('stream', userVideoStream =>{
        addVideoStream(userVideo, userVideoStream);
    })
}
