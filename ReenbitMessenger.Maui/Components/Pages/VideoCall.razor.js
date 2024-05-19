const videoGrid = document.getElementById('video-grid');

const lVideo = document.createElement('video');
lVideo.id = 'local-video';
lVideo.autoplay = true;
lVideo.muted = true;
lVideo.width = 320;
lVideo.height = 240;

const rVideo = document.createElement('video');
rVideo.id = 'remote-video';
rVideo.autoplay = true;
rVideo.width = 320;
rVideo.height = 240;

videoGrid.appendChild(lVideo);
videoGrid.appendChild(rVideo);

const localVideo = document.getElementById(lVideo.id);
const remoteVideo = document.getElementById(rVideo.id);
const callButton = document.getElementById('call-btn');
const callIdInput = document.getElementById('call-id');
const userIdField = document.getElementById('user-id');
const leaveButton = document.getElementById('leave-btn');

let peer = null;

Setup();

function Setup(){
    peer = new Peer();
    peer.on('open', id => {
        userIdField.innerHTML = id;
    });
    
    navigator.mediaDevices.getUserMedia({ video: true, audio: true })
    .then(stream => {
        localVideo.srcObject = stream;

        peer.on('call', call => {
            call.answer(stream);
            call.on('stream', remoteStream => {
                remoteVideo.srcObject = remoteStream;
            });
        });

        callButton.addEventListener('click', () => {
            const call = peer.call(callIdInput.value, stream);
            call.on('stream', remoteStream => {
                remoteVideo.srcObject = remoteStream;
            });
            call.on('disconnected', () => {
                remoteVideo.srcObject = null;
            });
            call.on('close', () => {
                remoteVideo.srcObject = null;
            });
        });

        leaveButton.addEventListener('click', () =>{
            endCall();
        });
    })
    .catch(error => {
        console.error('Error accessing media devices.', error);
    });
}

export function leave(){
    peer.destroy();
}

function endCall(){
    peer.destroy();

    Setup();
}
