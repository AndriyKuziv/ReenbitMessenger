const videoGrid = document.getElementById('video-grid');

const lVideo = document.createElement('video');
lVideo.id = 'local-video';
lVideo.autoplay = true;
lVideo.muted = true;
lVideo.width = 640;
lVideo.height = 480;

const rVideo = document.createElement('video');
rVideo.id = 'remote-video';
rVideo.autoplay = true;
rVideo.width = 640;
rVideo.height = 480;

videoGrid.appendChild(lVideo);
videoGrid.appendChild(rVideo);

const peer = new Peer();

const localVideo = document.getElementById(lVideo.id);
const remoteVideo = document.getElementById(rVideo.id);
const callButton = document.getElementById('call-btn');
const callIdInput = document.getElementById('call-id');
const userIdField = document.getElementById('user-id');

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
        });
    })
    .catch(error => {
        console.error('Error accessing media devices.', error);
});
