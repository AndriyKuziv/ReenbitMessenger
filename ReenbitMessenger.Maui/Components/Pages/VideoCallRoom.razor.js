let userId = null;
let localStream = null;
const videoGrid = document.getElementById('video-grid');

const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7051/callhub").build();

const myPeer = new Peer();
myPeer.on('open', id =>{
    userId = id;
    
    const startSignalR = async () =>{
        await connection.start();
        connection.on('ReceiveJoinedUser', id => {
            console.log(`User connected: ${id}`)
        });
        await connection.invoke('JoinRoom', 'roomId');
    }
    startSignalR();
})

const myVideo = document.createElement('video');
myVideo.id = "myVideo";
myVideo.autoplay = true;
myVideo.muted = true;

navigator.mediaDevices.getUserMedia({
    video: true,
    audio: true
}).then(function (stream) {
    addVideoStream(myVideo, stream)
    localStream = stream;
}).catch(function (error) {
    console.error(error.name + ': ' + error.message);
});

const addVideoStream = (video, stream) => {
    video.scrObject = stream;
    video.addEventListener('loadedmetadata', () => {
        video.play()
    });
    videoGrid.appendChild(video);
}

function playStream(element, stream) {
    var handleLoaded = function() {
        element.removeEventListener('loadedmetadata', handleLoaded);
        element.play();
    };
    element.addEventListener('loadedmetadata', handleLoaded);
    element.srcObject = stream;
}

function playCamera(element, preferedWidth, preferedHeight) {
    var devices = navigator.mediaDevices;
    if (devices && 'getUserMedia' in devices) {
        var constraints = {
            video: true,
            audio: true
        };
        var promise = devices.getUserMedia(constraints);
        promise
            .then(function(stream) {
                playStream(element, stream)
            })
            .catch(function(error) {
                console.error(error.name + ': ' + error.message);
            });
    } else {
        console.error('Camera API is not supported.');
    }
}

export function Start(token, roomId){
    var element = document.querySelector('#my-video');
    playCamera(element, 640, 480);

    console.log("Finished start");
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
